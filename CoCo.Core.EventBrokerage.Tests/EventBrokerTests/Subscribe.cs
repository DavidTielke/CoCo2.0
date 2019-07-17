using System;
using DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Contract.EventBrokerage.Exceptions;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.EventBrokerage.Tests.EventBrokerTests
{
    public partial class EventBrokerTests
    {
        [TestMethod]
        public void Subscribe_NullAsHandler_ArgumentNullException()
        {
            _broker
                .Invoking(b => b.Subscribe<TestMessage>(null))
                .ShouldThrow<ArgumentNullException>();
        }

        [TestMethod]
        public void Subscribe_NoSubscriptionsButOneAdd_OneSubscription()
        {
            Action<TestMessage> handler = msg => msg.Message = "Test";

            _broker.Subscribe(handler);

            _broker.AmountSubscriptions
                .Should()
                .Be(1, "one subscription was added.");
        }

        [TestMethod]
        public void Subscribe_NoSubscriptionsButTwoAdds_TwoSubscription()
        {
            Action<TestMessage> handler1 = msg => msg.Message = "Test";
            Action<TestMessage> handler2 = msg => msg.Message = "Test";

            _broker.Subscribe(handler1);
            _broker.Subscribe(handler2);

            _broker.AmountSubscriptions
                .Should()
                .Be(2, "two subscription was added.");
        }

        [TestMethod]
        public void Subscribe_TwoEqualHandlerAdded_DuplicatedHandlerException()
        {
            Action<TestMessage> handler = msg => msg.Message = "Test";
            _broker.Subscribe(handler);

            _broker
                .Invoking(b => b.Subscribe(handler))
                .ShouldThrow<DuplicatedHandlerException>();
        }
    }
}
