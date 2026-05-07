using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPP.InternalWebhookHost.Application.DTOs
{
    public class WebhookPayloadDto
    {
        public Guid Id { get; set; }
        public DateTime ReceivedAt { get; set; }
        public string Data { get; set; }
    }
}
