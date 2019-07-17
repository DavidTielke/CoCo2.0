using System;
using DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Contract.EventBrokerage.Exceptions;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.EventBrokerage.Tests.EventBrokerTests
{
    public partial class EventBrokerTests
    {
        [TestMethod]
        public void Raise_MessageIsNull_ArgumentNullException()
        {
            _broker
                .Invoking(b => b.Raise(null))
                .ShouldThrow<ArgumentNullException>("null as a message is not allowed");
        }

        [TestMethod]
        public void Raise_MessageHasSubscriber_SubscriberWasCalled()
        {
            var isCalled = false;
            Action<TestMessage> handler = msg => isCalled = true;
            _broker.Subscribe(handler);
            
            _broker.Raise(new TestMessage());

            isCalled.Should().BeTrue("the raised message was subscribed for that handler");
        }

        [TestMethod]
        public void Raise_MessageHasTwoSubscribers_SubscribersWereCalled()
        {
            var isCalled1 = false;
            var isCalled2 = false;
            Action<TestMessage> handler1 = msg => isCalled1 = true;
            Action<TestMessage> handler2 = msg => isCalled2 = true;
            _broker.Subscribe(handler1);
            _broker.Subscribe(handler2);

            _broker.Raise(new TestMessage());

            isCalled1.Should().BeTrue("the raised message was subscribed for that handler");
            isCalled2.Should().BeTrue("the raised message was subscribed for that handler");
        }

        [TestMethod]
        public void Raise_SubscriptionOrderisOneTwo_HandlerOrderIsOneTwo()
        {
            var order = "";
            Action<TestMessage> handler1 = msg => order +="One";
            Action<TestMessage> handler2 = msg => order += "Two";
            _broker.Subscribe(handler1);
            _broker.Subscribe(handler2);

            _broker.Raise(new TestMessage());

            order.Should().Be("OneTwo", "the subscribe order was one two");
        }
        
        [TestMethod]
        public void Raise_SubscriberRaisesException_SubscribersWereCalled()
        {
            Action<TestMessage> handler = msg => throw new Exception();
            _broker.Subscribe(handler);

            _broker.Raise(new TestMessage());
        }

        [TestMethod]
        public void Raise_FirstSubscriberHandlerThrowsException_SecondSubscriberIsCalled()
        {
            var isCalled2 = false;
            Action<TestMessage> handler1 = msg => throw new Exception();
            Action<TestMessage> handler2 = msg => isCalled2 = true;
            _broker.Subscribe(handler1);
            _broker.Subscribe(handler2);

            _broker.Raise(new TestMessage());

            isCalled2.Should().BeTrue("the raised message was subscribed for that handler");
        }

        [TestMethod]
        public void Raise_FirstSubscriberFilterThrowsException_SecondSubscriberIsCalled()
        {
            var isCalled2 = false;
            Action<TestMessage> handler1 = msg => msg.Message = "Nothing";
            Func<TestMessage, bool> filter1 = msg => throw new Exception();
            Action<TestMessage> handler2 = msg => isCalled2 = true;
            _broker.Subscribe(filter1, handler1);
            _broker.Subscribe(handler2);

            _broker.Raise(new TestMessage());

            isCalled2.Should().BeTrue("the raised message was subscribed for that handler");
        }

        [TestMethod]
        public void Raise_MessageHasNoSubscriber_NoError()
        {
            _broker.Raise(new TestMessage());
        }

        [TestMethod]
        public void Raise_HandlerTypeRegWithoutResolveCallback_NoResolverCallbackException()
        {
            _broker.Subscribe<TestHandler, TestMessage>((h,m) => h.ToString());

            _broker
                .Invoking(b => b.Raise(new TestMessage()))
                .ShouldThrow<NoResolveCallbackException>();
        }

        [TestMethod]
        public void Raise_TestHandlerAsHandlerSet_ResolveCallbackWasCalled()
        {
            var wasCalled = false;
            _broker.SetResolverCallback(type =>
            {
                wasCalled = true;
                return new TestHandler();
            });
            _broker.Subscribe<TestHandler, TestMessage>((h, m) => h.ToString());

            _broker.Raise(new TestMessage());

            wasCalled
                .Should()
                .BeTrue("the resolve callback should be called.");
        }

        [TestMethod]
        public void Raise_TestHandlerCreated_SameInstanceIsPassedToHandler()
        {
            var handler = new TestHandler();
            TestHandler passedHandler = null;
            _broker.SetResolverCallback(type => handler);
            _broker.Subscribe<TestHandler, TestMessage>((h, m) => passedHandler = h);

            _broker.Raise(new TestMessage());

            handler
                .Should()
                .BeSameAs(passedHandler, "the created handler should be passed to the lambda");
        }
    }
}
