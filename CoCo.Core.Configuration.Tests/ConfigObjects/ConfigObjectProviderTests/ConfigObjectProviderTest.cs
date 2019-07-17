using DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Configuration.ConfigObjects;
using DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Contract.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Configuration.Tests.ConfigObjects.ConfigObjectProviderTests
{
    [TestClass]
    public partial class ConfigObjectProviderTest
    {
        private ConfigObjectProvider _provider;
        private Mock<IConfigurator> _configMock;

        [TestInitialize]
        public void Initialize()
        {
            _configMock = new Mock<IConfigurator>();
            _provider = new ConfigObjectProvider(_configMock.Object);
        }
    }
}
