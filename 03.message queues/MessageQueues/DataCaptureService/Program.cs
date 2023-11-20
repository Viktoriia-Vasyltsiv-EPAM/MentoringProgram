using Confluent.Kafka;
using Core;
using Newtonsoft.Json.Linq;
using System.Text;

namespace DataCaptureService
{
    internal class Program
    {
        private static string _topicName = string.Empty;
        private static string _inputFolder = string.Empty;
        static void Main(string[] args)
        {
            var clientConfigFactory = new ProducerClientConfigFactory();
            var clientConfig = clientConfigFactory.GetClientConfig();
            Console.WriteLine("Please, enter topic name: ");
            _topicName = Console.ReadLine();
            Console.WriteLine("Please, enter a path for the input folder: ");
            _inputFolder = Console.ReadLine();
            while (!Directory.Exists(_inputFolder))
            {
                Console.WriteLine("Try again, this one is not exist: ");
                _inputFolder = Console.ReadLine();
            }
            Produce(_topicName, clientConfig);
        }

        static IEnumerable<FileMessageModel> GetFileMessagesFromFile(string file)
        {
            const long partLength = 100000;
            var sequenceId = Guid.NewGuid();
            using var fileStream = new FileStream(file, FileMode.Open, FileAccess.Read);
            long offset = 0;
            while (offset < fileStream.Length)
            {
                var length = offset + partLength > fileStream.Length ? fileStream.Length - offset : partLength;
                var fileData = new byte[length];
                fileStream.Read(fileData, 0, Convert.ToInt32(length));
                yield return new FileMessageModel()
                {
                    SequenceId = sequenceId,
                    Content = Encoding.Default.GetString(fileData),
                    Extension = Path.GetExtension(file),
                    FileName = Path.GetFileNameWithoutExtension(file)
                };
                offset += partLength;
            }
            fileStream.Close();
        }

        static void Produce(string topic, ClientConfig config)
        {
            using (var producer = new ProducerBuilder<string, string>(config).Build())
            {
                const string key = "FileMessage";
                var files = Directory.GetFiles(_inputFolder).Where(f => Path.GetExtension(f) == ".txt");
                var numProduced = 0;
                foreach (var file in files)
                {
                    var messages = GetFileMessagesFromFile(file);
                    foreach (var message in messages)
                    {
                        var val = JObject.FromObject(message).ToString();
                        Console.WriteLine($"Producing record: {key} {val}");
                        producer.Produce(topic, new Message<string, string> { Key = key, Value = val },
                            (deliveryReport) =>
                            {
                                if (deliveryReport.Error.Code != ErrorCode.NoError)
                                {
                                    Console.WriteLine($"Failed to deliver message: {deliveryReport.Error.Reason}");
                                }
                                else
                                {
                                    Console.WriteLine($"Produced message to: {deliveryReport.TopicPartitionOffset}");
                                    numProduced += 1;
                                }
                            });
                        producer.Flush(TimeSpan.FromSeconds(10));
                    }
                };
                Console.WriteLine($"{numProduced} messages were produced to topic {topic}");
            }
        }

        ClientConfig GetClientConfig()
        {
            return new ClientConfig()
            {
                BootstrapServers = "pkc-lz6r3.northeurope.azure.confluent.cloud:9092",
                SecurityProtocol = SecurityProtocol.SaslSsl,
                SaslMechanism = SaslMechanism.Plain,
                SaslUsername = "IHAFX3QRWSBOTBS2",
                SaslPassword = "u/NBYtLWW51eDkIDAGlv4nTAYWmN6HqIIBobMjVSdR74+vDHENBclwGNlxty9zSZ"
            };
        }
    }
}