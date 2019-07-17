using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Configuration.Tests.ConfigObjects.ConfigObjectProviderTests
{
    public partial class ConfigObjectProviderTest
    {
        [TestMethod]
        public void Get_TypeIsNull_ArgumentNullException()
        {
            Action del = () => _provider.Get(null);

            del.Should().ThrowExactly<ArgumentNullException>();
        }

        [TestMethod]
        public void Get_RequestingValidObject_ObjectReturned()
        {
            _configMock.Setup(c => c.Get<object>("Test", "Value", It.IsAny<object>())).Returns(23);

            var obj = _provider.Get(typeof(COValid)).As<COValid>();

            obj.Value.Should().Be(23, "because 23 was provided by the configurator");
        }

    }
}
