using System;

namespace DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.EventBrokerage
{
    public class Subscription
    {
        public Delegate Filter { get; set; }
        public Delegate Handler { get; set; }
        public Type HandlerType { get; set; }

        public Subscription(Delegate handler)
        {
            Handler = handler;
        }
    }
}