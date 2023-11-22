using System.Text;
using Confluent.Kafka;
using Core;
using Newtonsoft.Json.Linq;

namespace DataCaptureService
{
    class Program
    {
        static void Main(string[] args)
        {
            var clientConfig = new ClientConfig { BootstrapServers = "localhost:9092" };
            var topicName = GetInput("Please, enter topic name: ");
            var inputFolder = GetValidatedFolderPath("Please, enter a path for the input folder: ");

            ProduceMessages(topicName, inputFolder, clientConfig);
        }

        static string GetInput(string message)
        {
            Console.WriteLine(message);
            return Console.ReadLine();
        }

        static string GetValidatedFolderPath(string message)
        {
            string folderPath;
            do
            {
                folderPath = GetInput(message);
                if (!Directory.Exists(folderPath))
                {
                    Console.WriteLine("Directory does not exist. Try again.");
                }
            }
            while (!Directory.Exists(folderPath));

            return folderPath;
        }

        static IEnumerable<FileMessage> GetFileMessages(string file)
        {
            // const long PartLength = 100000;
            // Decrease the part length for testing purposes
            const long PartLength = 100;

            var sequenceId = Guid.NewGuid();

            using var fileStream = new FileStream(file, FileMode.Open, FileAccess.Read);
            long offset = 0;

            while (offset < fileStream.Length)
            {
                var length = Math.Min(PartLength, fileStream.Length - offset);
                var fileData = new byte[length];
                fileStream.Read(fileData, 0, (int)length);

                yield return new FileMessage
                {
                    SequenceId = sequenceId,
                    Content = Encoding.Default.GetString(fileData),
                    Extension = Path.GetExtension(file),
                    FileName = Path.GetFileNameWithoutExtension(file)
                };

                offset += length;
            }
        }

        static void ProduceMessages(string topic, string folder, ClientConfig config)
        {
            using var producer = new ProducerBuilder<string, string>(config).Build();
            var files = Directory.GetFiles(folder, "*.txt");
            var producedMessagesCount = 0;

            foreach (var file in files)
            {
                try
                {
                    producedMessagesCount += ProduceMessagesFromFile(producer, topic, file);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error while processing file {file}: {ex.Message}");
                }
            }

            Console.WriteLine($"{producedMessagesCount} messages were published to topic {topic}");
        }

        static int ProduceMessagesFromFile(IProducer<string, string> producer, string topic, string file)
        {
            var messages = GetFileMessages(file).ToList();
            var producedMessagesCount = 0;

            foreach (var message in messages)
            {
                try
                {
                    var messageValue = JObject.FromObject(message).ToString();
                    Console.WriteLine($"Producing record: {message.FileName} {messageValue}");

                    producer.Produce(topic, new Message<string, string> { Key = message.FileName, Value = messageValue },
                        deliveryReport =>
                        {
                            if (deliveryReport.Error.Code != ErrorCode.NoError)
                            {
                                Console.WriteLine($"Failed to deliver message: {deliveryReport.Error.Reason}");
                            }
                            else
                            {
                                Console.WriteLine($"Produced message to: {deliveryReport.TopicPartitionOffset}");
                                producedMessagesCount++;
                            }
                        });

                    producer.Flush(TimeSpan.FromSeconds(10));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error while producing message: {ex.Message}");
                }
            }

            return producedMessagesCount;
        }

    }
}
