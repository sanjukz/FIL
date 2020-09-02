using System;

namespace FIL.Configuration.Exceptions
{
    public class MissingSettingException : Exception
    {
        public MissingSettingException(string settingName)
            : base($"Could not find setting: {settingName}")
        {
        }
    }
}