using System;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace CodeBase._Localization
{
    public sealed class Localization
    {
        public void ChangeLocale(LocalizationKey key)
            => LocalizationSettings.SelectedLocale = FindLocale(key.ToString());

        private Locale FindLocale(string key) 
            => LocalizationSettings.AvailableLocales.Locales
                .First(l => string.Equals(GetLocalTag(l.name), key, StringComparison.OrdinalIgnoreCase));
            
        
        private string GetLocalTag(string localName)
            => new Regex("(\\S+$)")
                .Match(localName).Value
                .Trim('(')
                .Trim(')');

        public enum LocalizationKey
        {
            En,
            Ru
        }
    }
}