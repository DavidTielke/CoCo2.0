using System;
using DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Contract.Configuration.Exceptions;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Configuration.Tests.ConfiguratorTests
{
    public partial class ConfiguratorTest
    {
        [TestMethod]
        public void Get_CategoryIsNull_ArgumentNullException()
        {
            _config.Set("Test", "Value", 1);

            Action del = () => _config.Get<string>(null, "Value");

            del.Should().ThrowExactly<ArgumentNullException>("because null is not allowed as a category");
        }

        [TestMethod]
        public void Get_CategoryIsEmpty_ArgumentNullException()
        {
            _config.Set("Test", "Value", 1);

            Action del = () => _config.Get<string>("", "Value");

            del.Should().ThrowExactly<ArgumentNullException>("because empty is not allowed as a category");
        }

        [TestMethod]
        public void Get_CategoryIsWhitespace_ArgumentNullException()
        {
            _config.Set("Test", "Value", 1);

            Action del = () => _config.Get<string>(" ", "Value");

            del.Should().ThrowExactly<ArgumentNullException>("because whitespace is not allowed as a category");
        }

        [TestMethod]
        public void Get_KeyIsNull_ArgumentException()
        {
            _config.Set("Test", "Value", 1);

            Action del = () => _config.Get<string>("Test", null);

            del.Should().ThrowExactly<ArgumentNullException>("because null is not allowed as a key");
        }

        [TestMethod]
        public void Get_KeyIsEmpty_ArgumentNullException()
        {
            _config.Set("Test", "Value", 1);

            Action del = () => _config.Get<string>("Test", "");

            del.Should().ThrowExactly<ArgumentNullException>("because empty is not allowed as a key");
        }

        [TestMethod]
        public void Get_KeyIsWhitespace_ArgumentNullException()
        {
            _config.Set("Test", "Value", 1);

            Action del = () => _config.Get<string>("Test", " ");

            del.Should().ThrowExactly<ArgumentNullException>("because whitespace is not allowed as a key");
        }

        [TestMethod]
        public void Get_StoreIsEmptyGetWithDefault_DefaultIsReturned()
        {
            var result = _config.Get<int>("Test", "Value", 23);

            result.Should().Be(23);
        }

        [TestMethod]
        public void Get_StoreIsEmpty_CategoryOrKeyNotFoundException()
        {
            Action del = () => _config.Get<int>("Test", "Value");

            del.Should().ThrowExactly<KeyOrCategoryNoException>("because no value for category/key is set");
        }

        [TestMethod]
        public void Get_StoreWithOneItem_CorrectItemReturned()
        {
            _config.Set("Test","Value",1);

            var result = _config.Get<int>("Test", "Value");

            result.Should().Be(1);
        }

        [TestMethod]
        public void Get_StoreWithTwoItems_CorrectItemReturned()
        {
            _config.Set("Test","Value1",1);
            _config.Set("Test","Value2",2);

            var result = _config.Get<int>("Test", "Value2");

            result.Should().Be(2);
        }

        [TestMethod]
        public void Get_IntIsStoredStringRequested_InvalidCastException()
        {
            _config.Set("Test","Value1",1);

            Action del = () => _config.Get<string>("Test", "Value1");

            del.Should().ThrowExactly<InvalidCastException>("because a int was stored");
        }
    }
}
