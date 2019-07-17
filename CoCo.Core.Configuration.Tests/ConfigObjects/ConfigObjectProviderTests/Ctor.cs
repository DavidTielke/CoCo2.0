using System;
using DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Configuration.ConfigObjects;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Configuration.Tests.ConfigObjects.ConfigObjectProviderTests
{
    public partial class ConfigObjectProviderTest
    {
        [TestMethod]
        public void Ctor_ConfiguratorIsNull_ArgumentNullException()
        {
            Action del = () => new ConfigObjectProvider(null);

            del.Should().ThrowExactly<ArgumentNullException>();
        }
    }
}
