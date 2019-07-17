using System;
using DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Contract.Configuration.DataClasses;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Configuration.Tests.ConfiguratorTests
{
    public partial class ConfiguratorTest
    {
        [TestMethod]
        public void Set_CategoryIsNull_ArgumentException()
        {
            Action del = () => _config.Set(null, "Value", 1);

            del.Should().ThrowExactly<ArgumentNullException>("because null is not allowed as a category");
        }

        [TestMethod]
        public void Set_KeyIsNull_ArgumentException()
        {
            Action del = () => _config.Set("Test", null, 1);

            del.Should().ThrowExactly<ArgumentNullException>("because null is not allowed as a category");
        }

        [TestMethod]
        public void Set_CategoryIsEmpty_ArgumentNullException()
        {
            Action del = () => _config.Set("", "Value", 1);

            del.Should().ThrowExactly<ArgumentNullException>("because empty string is not allowed as a category");
        }

        [TestMethod]
        public void Set_KeyIsEmpty_ArgumentNullException()
        {
            Action del = () => _config.Set("Test", "", 1);

            del.Should().ThrowExactly<ArgumentNullException>("because empty string is not allowed as a key");
        }

        [TestMethod]
        public void Set_CategoryIsWhitespace_ArgumentNullException()
        {
            Action del = () => _config.Set(" ", "Value", 1);

            del.Should().ThrowExactly<ArgumentNullException>("because whitespace string is not allowed as a category");
        }

        [TestMethod]
        public void Set_KeyIsWhitespace_ArgumentNullException()
        {
            Action del = () => _config.Set("Test", "", 1);

            del.Should().ThrowExactly<ArgumentNullException>("because whitespace string is not allowed as a key");
        }

        [TestMethod]
        public void Set_EmptyStore_OneItemStored()
        {
            _config.Set("Test", "Value", 1);

            _config.Count.Should().Be(1, "because one item should be stored");
        }

        [TestMethod]
        public void Set_OneItemStored_TwoItemsStored()
        {
            _config.Set("Test","Value1",1);
            _config.Set("Test","Value2",2);

            _config.Count.Should().Be(2, "because two items should be stored");
        }

        [TestMethod]
        public void Set_ItemIsOverwritten_OneItemIsStored()
        {
            _config.Set("Test","Value",1);
            _config.Set("Test","Value",2);

            _config.Count.Should().Be(1, "because item was overwritten");
        }

        [TestMethod]
        public void Set_ItemWithPersistFlagIsStored_SaveOnRepoIsCalled()
        {
            _config.Set("Test","Value",1, true);
            
            _repoMock.Verify(r => r.SaveEntry(It.IsAny<ConfigEntry>()), Times.Once);
        }
    }
}
