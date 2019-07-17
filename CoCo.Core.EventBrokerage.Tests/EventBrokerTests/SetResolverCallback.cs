using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.EventBrokerage.Tests.EventBrokerTests
{
    public partial class EventBrokerTests
    {
        [TestMethod]
        public void SetResolverCallback_NullAsCallback_ArgumentNullException()
        {
            _broker
                .Invoking(b => b.SetResolverCallback(null))
                .ShouldThrow<ArgumentNullException>();
        }

        [TestMethod]
        public void SetResolverCallback_NormalCallback_CallbackIsStored()
        {
            var wasCalled = false;
            Func<Type, object> resolver = t =>
            {
                wasCalled = true;
                return new TestHandler();
            };
            _broker.Subscribe<TestHandler, TestMessage>((handler, msg) => msg.ToString());

            _broker.SetResolverCallback(resolver);
            _broker.Raise(new TestMessage());

            wasCalled.Should().BeTrue("the resolver callback was called");
        }
    }
}
