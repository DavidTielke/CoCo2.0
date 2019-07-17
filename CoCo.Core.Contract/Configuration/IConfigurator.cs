using System;
using DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Contract.Configuration.Exceptions;

namespace DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Contract.Configuration
{
    /// <summary>
    /// Contract to get or set config values from a store
    /// by a key/category pair
    /// </summary>
    public interface IConfigurator
    {
        /// <summary>
        /// Gets a config value from the configurator
        /// </summary>
        /// <typeparam name="T">The expected return type</typeparam>
        /// <param name="category">The category the value is stored in</param>
        /// <param name="key">The key under the value is stored</param>
        /// <param name="defaultValue">The default value, if category/key is not found.</param>
        /// <returns>The value from the store, or the default value if passed as a parameter</returns>
        /// <exception cref="KeyOrCategoryNoException">If the key or the category was not found</exception>
        /// <exception cref="ArgumentException">If the passed category or key is null, empty or whitespace</exception>
        /// <exception cref="InvalidCastException">If the expected return type differs from the stored type</exception>
        T Get<T>(string category, string key, T defaultValue = default(T));

        /// <summary>
        /// Set a config value to the configurator
        /// </summary>
        /// <typeparam name="T">The type of value to store</typeparam>
        /// <param name="category">The category the value is stored in</param>
        /// <param name="key">The key under the value will be stored</param>
        /// <param name="value">The value that will be stored</param>
        /// <param name="persist">If set to true, the value will be stored permanently</param>
        void Set<T>(string category, string key, T value, bool persist = false);
    }
}
