using System;
using StackExchange.Redis;

namespace PubSub.Subscriber
{
    static class RedisSubscriber
    {
        static void Main(string[] args)
        {
            // A quick note about clustering when sending Pub/Sub messages.
            // Pub/Sub doesn't support clustering like the cache does.
            // To send messages in Pub/Sub the consumers must all be connected to the same node, and the producers must also be connected to the same node.
            // To get around this you can use streams.
            //ConfigurationOptions options = new ConfigurationOptions
            //{
            //    EndPoints = {
            //        { "127.0.0.1", 2111 },
            //        { "127.0.0.1", 2112 },
            //        { "127.0.0.1", 2113 },
            //    },
            //};
            var redis = ConnectionMultiplexer.Connect("localhost");

            var sub = redis.GetSubscriber();

            sub.Subscribe(RedisChannel.Literal("test_channel"), (channel, message) => {
                var receivedTimestamp = DateTime.UtcNow.ToString("dd-MM-yyyyTHH:mm:ss.fffZ");
                Console.WriteLine($"Received: {message}");
                Console.WriteLine($"Received at: {receivedTimestamp}");
            });

            Console.WriteLine("Subscribed to channel. Press Enter to exit.");
            Console.ReadLine();
        }
    }

}
