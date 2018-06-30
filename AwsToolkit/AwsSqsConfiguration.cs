using Amazon.SQS;

namespace AwsToolkit
{
    internal class AwsSqsConfiguration
    {
        public AmazonSQSConfig AmazonSQSConfig { get; set; }

        public string QueueURL { get; set; }
    }
}