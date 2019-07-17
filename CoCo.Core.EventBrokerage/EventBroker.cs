using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Contract.EventBrokerage;
using DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Contract.EventBrokerage.Exceptions;

[assembly: InternalsVisibleTo("CoCo.Core.EventBrokerage.Tests")]
namespace DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.EventBrokerage
{
    public class EventBroker : IEventBroker
    {
        private readonly Dictionary<Type, List<Subscription>> _messageSubscriptions;
        private Func<Type, object> _resolverCallback;

        public EventBroker()
        {
            _messageSubscriptions = new Dictionary<Type, List<Subscription>>();
        }

        public void Subscribe<THandler, TMessage>(Action<THandler, TMessage> handler)
        {
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            var subscription = new Subscription(handler)
            {
                HandlerType = typeof(THandler)
            };

            AddSubscription<TMessage>(subscription);
        }

        private void AddSubscription<TMessage>(Subscription subscription)
        {
            var messageType = typeof(TMessage);

            var messageAlreadyHasSubscribers = _messageSubscriptions.ContainsKey(messageType);
            if (!messageAlreadyHasSubscribers)
            {
                _messageSubscriptions[messageType] = new List<Subscription>();
            }

            var isHandlerAlreadyRegistered = _messageSubscriptions[messageType].Any(s => s.Handler == subscription.Handler);
            if (isHandlerAlreadyRegistered)
            {
                throw new DuplicatedHandlerException("Handler was already registered");
            }

            _messageSubscriptions[messageType].Add(subscription);
        }

        public void Subscribe<THandler, TMessage>(Func<TMessage, bool> filter, Action<THandler, TMessage> handler)
        {
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }

            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            var subscription = new Subscription(handler)
            {
                Filter = filter,
                HandlerType = typeof(THandler)
            };

            AddSubscription<TMessage>(subscription);
        }

        public void Subscribe<TMessage>(Func<TMessage, bool> filter, Action<TMessage> handler)
        {
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }

            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            Subscribe(handler);
            _messageSubscriptions[typeof(TMessage)]
                .Single(s => s.Handler == (Delegate)handler)
                .Filter = filter;
        }

        public void Subscribe<TMessage>(Action<TMessage> handler)
        {
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            var subscription = new Subscription(handler);

            AddSubscription<TMessage>(subscription);
        }

        internal int AmountSubscriptions => _messageSubscriptions.SelectMany(s => s.Value).Count();

        public void Raise(object message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            var messageType = message.GetType();
            var isSomeoneInterested = _messageSubscriptions.ContainsKey(messageType) && _messageSubscriptions[messageType].Count > 0;
            if (!isSomeoneInterested)
            {
                //_logger.DebugAsync("Try to raise msg, but no one interessted", false, message).Wait();
                return;
            }

            var subscriptions = _messageSubscriptions[messageType];

            EnsureResolveCallbackIsSetIfNeeded(subscriptions);

            foreach (var subscription in subscriptions)
            {
                RaiseForSubscription(message, subscription);
            }
        }

        private void EnsureResolveCallbackIsSetIfNeeded(List<Subscription> subscriptions)
        {
            var hasAnyActivationSubscription = subscriptions.Any(s => s.HandlerType != null);
            var hasResolveCallbackSet = _resolverCallback != null;
            if (hasAnyActivationSubscription && !hasResolveCallbackSet)
            {
                throw new NoResolveCallbackException("Can't activate handler, no resolve callback set.");
            }
        }

        private void RaiseForSubscription(object message, Subscription subscription)
        {
            try
            {
                var isFilterSet = subscription.Filter != null;
                if (isFilterSet)
                {
                    var isFilterMatched = (bool)subscription.Filter.DynamicInvoke(message);
                    if (!isFilterMatched)
                    {
                        return;
                    }
                }

                var shallHandlerTypeBeCreated = subscription.HandlerType != null;
                if (shallHandlerTypeBeCreated)
                {
                    var handlerType = subscription.HandlerType;
                    var handler = _resolverCallback(handlerType);

                    subscription.Handler.DynamicInvoke(handler, message);
                    //_logger.DebugAsync("Notified subscriber (activate)", false, message).Wait();

                }
                else
                {
                    subscription.Handler.DynamicInvoke(message);
                    //_logger.DebugAsync("Notified subscriber (activate)", false, message).Wait();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void SetResolverCallback(Func<Type, object> resolverCallback)
        {
            if (resolverCallback == null)
            {
                throw new ArgumentNullException(nameof(resolverCallback));
            }

            _resolverCallback = resolverCallback;
        }
    }
}
