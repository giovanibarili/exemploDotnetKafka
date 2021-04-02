using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace exemplo_dotnet_kafka
{
    public abstract class AbstractConsumeKafka
    {
        private ILogger logger;
        private readonly ConsumerConfig consumerConfig;

        public AbstractConsumeKafka(ConsumerConfig consumerConfig, ILogger logger)
        {
            this.logger = logger;
            this.consumerConfig = consumerConfig;
        }

        public abstract void Handler(ConsumeResult<Ignore, string> consume);

        protected void Run(string topics, int concurrence, CancellationToken cancellationToken)
        {
            int nPart = -1;
            for (int i = 0; i < concurrence; i++) {

                var ts = new ThreadStart(() =>
                {
                    using (var consumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build())
                    {
                        consumer.Subscribe(topics);

                        while (!cancellationToken.IsCancellationRequested)
                        {
                            ConsumeResult<Ignore, string> consumeResult = null;
                            try
                            {
                                consumeResult = consumer.Consume(1000);

                                // Loggar partições conectadas
                                if (consumer.Assignment.Count != nPart)
                                {
                                    nPart = consumer.Assignment.Count;
                                    consumer.Assignment.ForEach((tp) =>
                                    {
                                        logger.LogDebug($"Assinado para partição [{tp.Partition}] para Thread [{Thread.CurrentThread.ManagedThreadId}]");
                                    });
                                }

                                if (consumeResult == null)
                                    continue;

                                this.Handler(consumeResult);
                            }
                            catch (Exception ex)
                            {
                                logger.LogError(ex.Message);

                                //Retry
                                //DLQ

                                Thread.Sleep(1000);
                            }
                            finally
                            {
                                if(consumeResult != null)
                                    consumer.Commit(consumeResult);
                            }
                        }

                        consumer.Close();
                    }
                });

                new Thread(ts).Start();
            }
        }
    }
}