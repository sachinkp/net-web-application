using System;
using System.Collections.Generic;
using Amazon.SQS;
using Amazon.SQS.Model;
using Newtonsoft.Json;

namespace AwsToolkit
{
    internal class AwsSqsClient
    {
        private readonly AwsSqsConfiguration awsSqsConfiguration;
        private readonly AmazonSQSClient amazonSQSClient;

        public AwsSqsClient(AwsSqsConfiguration awsSqsConfiguration)
        {
            this.awsSqsConfiguration = awsSqsConfiguration;

            // AmazonSQSClient _client = new AmazonSQSClient("ACCESSKEY", "ACCESSSECRET", config);
            amazonSQSClient = new AmazonSQSClient(awsSqsConfiguration.AmazonSQSConfig);
        }

        public void SendMessage(TenantActionMessage message)
        {
            try
            {
                var sendMessageRequest = new SendMessageRequest();
                sendMessageRequest.QueueUrl = awsSqsConfiguration.QueueURL;
                sendMessageRequest.MessageBody = JsonConvert.SerializeObject(message);
                var sendMessageResponse = amazonSQSClient.SendMessage(sendMessageRequest);
                Console.WriteLine($"Send message successful : " + sendMessageResponse.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.ToString());
            }
        }

        public List<string> ReceiveMessage()
        {
            var receiptHandles = new List<string>();
            try
            {
                var receiveMessageRequest = new ReceiveMessageRequest();
                receiveMessageRequest.QueueUrl = awsSqsConfiguration.QueueURL;
                var receiveMessageResponse = amazonSQSClient.ReceiveMessage(receiveMessageRequest);
                if (receiveMessageResponse.Messages.Count != 0)
                {
                    for (int i = 0; i < receiveMessageResponse.Messages.Count; i++)
                    {
                        var message = JsonConvert.DeserializeObject<TenantActionMessage>(receiveMessageResponse.Messages[i].Body);
                        receiptHandles.Add(receiveMessageResponse.Messages[i].ReceiptHandle);
                        Console.WriteLine("Receive message successful : " + message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.ToString());
            }

            return receiptHandles;
        }

        public void DeleteMessage(string receiptHandle)
        {
            try
            {
                DeleteMessageRequest deleteMessageRequest = new DeleteMessageRequest();
                deleteMessageRequest.QueueUrl = awsSqsConfiguration.QueueURL;
                deleteMessageRequest.ReceiptHandle = receiptHandle;
                var deleteMessageResponse = amazonSQSClient.DeleteMessage(deleteMessageRequest);
                Console.WriteLine("Delete message successful : " + deleteMessageResponse.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.ToString());
            }
        }
    }
}