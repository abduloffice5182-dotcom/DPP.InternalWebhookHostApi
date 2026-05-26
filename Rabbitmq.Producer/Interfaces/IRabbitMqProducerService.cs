using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPP.InternalWebhookHost.Rabbitmq.Interface
{
	public interface IRabbitMqProducerService
	{
		Task PublishAsync<T>(
			string exchange,
				string queue,
			string routingKey,
			T message);
	}
}
