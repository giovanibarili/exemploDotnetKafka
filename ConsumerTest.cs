using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace exemplo_dotnet_kafka
{
    public class ConsumerTest : AbstractConsumeKafka, IHostedService, IDisposable
    {
        private readonly ILogger loggerTest;

        public ConsumerTest(ConsumerConfig consumerConfig, ILogger<ConsumerTest> logger) : base(consumerConfig, logger)
        {
            this.loggerTest = logger;
        }

        public void Dispose()
        {
        }

        public override void Handler(ConsumeResult<Ignore, string> consume)
        {
            loggerTest.LogDebug($"Thread: [{Thread.CurrentThread.ManagedThreadId}] - Topic: [{consume.Topic}] - Partition: [{consume.Partition}] - Message: [{consume.Message.Value}]");
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            this.Run("testes", 10, cancellationToken);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
