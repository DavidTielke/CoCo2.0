using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.EventBrokerage.Tests.EventBrokerTests
{
    [TestClass]
    public partial class EventBrokerTests
    {
        private EventBroker _broker;

        [TestInitialize]
        public void TestInitialize()
        {
            _broker = new EventBroker();
        }
    }
}
