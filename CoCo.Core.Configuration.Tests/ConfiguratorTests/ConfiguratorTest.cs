using DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Contract.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Configuration.Tests.ConfiguratorTests
{
    [TestClass]
    public partial class ConfiguratorTest
    {
        private Configurator _config;
        private Mock<IConfigurationRepository> _repoMock;

        [TestInitialize]
        public void Initialize()
        {
            _repoMock = new Mock<IConfigurationRepository>();
            _config = new Configurator(_repoMock.Object);
        }
    }
}
