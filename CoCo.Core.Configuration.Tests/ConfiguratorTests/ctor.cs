using System;
using System.Collections.Generic;
using System.Linq;
using DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Contract.Configuration.DataClasses;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Configuration.Tests.ConfiguratorTests
{
    public partial class ConfiguratorTest
    {
        [TestMethod]
        public void Ctor_CreationPossible()
        {
            var config = new Configurator(_repoMock.Object);

            config.Should().NotBeNull();
        }

        [TestMethod]
        public void Ctor_ConfigRepoIsNull_ArgumentException()
        {
            Action del = () => new Configurator(null);

            del.Should().ThrowExactly<ArgumentNullException>("because a configurator cant work without a repository");
        }

        [TestMethod]
        public void Ctor_3ItemsFromRepo_3ItemsLoaded()
        {
            _repoMock.Setup(rm => rm.Load()).Returns(new List<ConfigEntry>
            {
                new ConfigEntry{Category = "Test",Key = "Value1", Persist = true, Value = 1},
                new ConfigEntry{Category = "Test",Key = "Value2", Persist = true, Value = 2},
                new ConfigEntry{Category = "Test",Key = "Value3", Persist = true, Value = 3},
            }.AsEnumerable());

            var conf = new Configurator(_repoMock.Object);

            conf.Count.Should().Be(3, "because three items should be loaded from repo");
        }
    }
}
