using System;
using System.Resources;
using Microsoft.CodeAnalysis;

namespace Harurow.Extensions.One.Analyzer.Commons
{
    public static class LocalizableResourceStringUtil
    {
        private static readonly ResourceManager ResourceManager = Resources.ResourceManager;
        private static readonly Type ResourcesType = typeof(Resources);

        public static LocalizableResourceString Get(string key)
            => new LocalizableResourceString(key, ResourceManager,ResourcesType);
    }
}