using System.Text;
using Confluent.Kafka;
using Core;
using Newtonsoft.Json;

namespace ProcessingService
{
    class Program
    {
        private static string _outputFolder;

        static async Task Main(string[] args)
        {
            var clientConfig = new ClientConfig { BootstrapServers = "localhost:9092" };
            var topicName = GetInput("Please, enter topic name: ");
            _outputFolder = GetValidatedFolderPath("Please, enter a path for the output folder: ");

            ConsumeMessages(topicName, clientConfig);
        }

        static string GetInput(string message)
        {
            Console.WriteLine(message);
            return Console.ReadLine();
        }

        static string GetValidatedFolderPath(string message)
        {
            string folderPath = GetInput(message);
            while (!Directory.Exists(folderPath))
            {
                Console.WriteLine("Directory does not exist. Try again: ");
                folderPath = Console.ReadLine();
            }
            return folderPath;
        }

        static void ConsumeMessages(string topic, ClientConfig config)
        {
            var consumerConfig = new ConsumerConfig(config)
            {
                GroupId = "test-group-1",
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false
            };

            using var consumer = new ConsumerBuilder<string, string>(consumerConfig).Build();
            consumer.Subscribe(topic);

            var cancellationTokenSource = new CancellationTokenSource();
            Console.CancelKeyPress += (sender, e) =>
            {
                e.Cancel = true;
                cancellationTokenSource.Cancel();
            };

            ProcessMessages(consumer, cancellationTokenSource.Token);
        }
        
        static void ProcessMessages(IConsumer<string, string> consumer, CancellationToken cancellationToken)
        {
            FileMessage currentMessage = null;

            try
            {
                while (true)
                {
                    var consumeResult = consumer.Consume(cancellationToken);
                    var fileMessage = JsonConvert.DeserializeObject<FileMessage>(consumeResult.Message.Value);

                    if (currentMessage == null || fileMessage.SequenceId != currentMessage.SequenceId)
                    {
                        // Save the message sequence
                        if (currentMessage != null)
                        {
                            SaveFileMessage(currentMessage);
                            consumer.Commit();
                        }
                        // Start a new sequence from the cluster of messsages
                        currentMessage = fileMessage; 
                    }
                    else
                    {
                        // Append the content of the current message with the new message
                        currentMessage.Content += fileMessage.Content;
                    }
                }
            }
            catch (OperationCanceledException ex)
            {
                Console.WriteLine($"{nameof(OperationCanceledException)} thrown with message: {ex.Message}");
            }
            finally
            {
                consumer.Close();
            }
        }


        static void SaveFileMessage(FileMessage fileMessage)
        {
            try
            {
                string fileName = $"{fileMessage.FileName}{fileMessage.SequenceId}{fileMessage.Extension}";
                var path = Path.Combine(_outputFolder, fileName);
                var byteArray = Encoding.Default.GetBytes(fileMessage.Content);

                File.WriteAllBytes(path, byteArray);

                Console.WriteLine($"Message saved as {fileName}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving message '{fileMessage.FileName}{fileMessage.SequenceId}{fileMessage.Extension}': {ex.Message}");
            }
        }
    }
}
