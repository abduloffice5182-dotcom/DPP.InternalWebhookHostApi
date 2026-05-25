using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPP.InternalWebhookHost.Rabbitmq.Interface
{
	public interface IRabbitMqProducer
	{
		Task PublishAsync<T>(
		string routingKey,
		T message);
	}
}
