using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eSim.Infrastructure.DTOs.Global;
using eSim.Infrastructure.DTOs.Subscribers;

namespace eSim.Infrastructure.Interfaces.Selfcare.Subscriber
{  
    public interface ISubscriber
    {
        Task<Result<SubscriberDTO>> SubscriberDetail(Subscriber)
    }
}
