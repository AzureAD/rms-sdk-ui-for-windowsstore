using System;

namespace Microsoft.RightsManagement.UILibrary
{
    using Windows.ApplicationModel.Resources;

    /// <summary>
    /// Provides access to string resources.
    /// </summary>
    public class LocalizedStrings
    {
        static ResourceLoader _resourceLoader = null;

        public static ResourceLoader LocalizedResources
        {
            get
            {
                if (LocalizedStrings._resourceLoader == null)
                {
                    _resourceLoader = ResourceLoader.GetForCurrentView("Microsoft.RightsManagement.UILibrary/LibResources");
                }

                return _resourceLoader;
            }
        }

        public static string Get(string key)
        {
            return LocalizedResources.GetString(key);
        }
    }
}