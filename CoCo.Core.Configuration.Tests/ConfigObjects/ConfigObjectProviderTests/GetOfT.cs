using System;
using DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Contract.Configuration.DataClasses;
using DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Contract.Configuration.Exceptions;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Configuration.Tests.ConfigObjects.ConfigObjectProviderTests
{
    public class COWithoutAttribute
    {
        public virtual int Value { get; set; }
    }

    public class COWithoutVirtual
    {
        [ConfigMap("Test","Value")]
        public int Value { get; set; }
    }

    public class COValid
    {
        [ConfigMap("Test","Value")]
        public virtual int Value { get; set; }
    }

    public partial class ConfigObjectProviderTest
    {
        [TestMethod]
        public void GetOfT_RequestingObjWithoutAttributes_InvalidOperationException()
        {
            Action del = () => _provider.Get<COWithoutAttribute>();

            del.Should().ThrowExactly<InvalidOperationException>(
                "because the properties of the requested types have no ConfigMap-attributes");
        }

        [TestMethod]
        public void GetOfT_RequestingObjWithoutVirtual_InvalidOperationException()
        {
            Action del = () => _provider.Get<COWithoutVirtual>();

            del.Should().ThrowExactly<InvalidOperationException>(
                "because the properties of the requested types are not virtual");
        }

        [TestMethod]
        public void GetOfT_CatKeyNotFoundInConfig_KeyOrCategoryNotFoundException()
        {
            _configMock.Setup(c => c.Get<object>("Test", "Value", It.IsAny<object>()))
                .Throws<KeyOrCategoryNoException>();
            var obj = _provider.Get<COValid>();

            Action del = () =>
            {
                var foo = obj.Value;
                obj.Value = 5;
            };

            del.Should().ThrowExactly<KeyOrCategoryNoException>("because the defined key in the ConfigMap does not exist");
        }

        [TestMethod]
        public void GetOfT_RequestingValidObject_ObjectReturned()
        {
            _configMock.Setup(c => c.Get<object>("Test", "Value", It.IsAny<object>())).Returns(23);

            var obj = _provider.Get<COValid>();

            obj.Value.Should().Be(23, "because 23 was provided by the configurator");
        }

        [TestMethod]
        public void GetOfT_RequestingObjTwoTimes_SameObject()
        {
            _configMock.Setup(c => c.Get<object>("Test", "Value", It.IsAny<object>())).Returns(23);

            var obj1 = _provider.Get<COValid>();
            var obj2 = _provider.Get<COValid>();

            obj1.Should().BeSameAs(obj2, "because the provider returns singletons");
        }

        [TestMethod]
        public void GetOfT_RequestValidObject_ReturnTypeIsProxy()
        {
            _configMock.Setup(c => c.Get<object>("Test", "Value", It.IsAny<object>())).Returns(23);

            var obj1 = _provider.Get<COValid>();

            obj1.GetType().Name.Should().Contain("Proxy");
        }
    }
}
