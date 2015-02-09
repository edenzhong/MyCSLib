using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace ArgHelper
{
    /// <summary>
    /// Load configuration from app.config appConfig <appSettings> section.
    /// </summary>
    public static class ConfigAppSettingArgLoader
    {
        public static bool LoadConfigAppSetting(this IArg arg)
        {
            if (null != arg)
            {
                return arg.LoadArg((key, defVal) =>
                {
                    string confStr;
                    confStr = ConfigurationManager.AppSettings[key];
                    if (null == confStr)
                    {
                        confStr = defVal;
                    }
                    return confStr;
                },
                (key) =>
                {
                    return (null != ConfigurationManager.AppSettings[key]);
                });
            }
            return false;
        }
        public static T LoadConfigAppSetting<T>() where T : class,IArg, new()
        {
            T arg = new T();
            if ( LoadConfigAppSetting(arg) )
            {
                return arg;
            }
            else
            {
                return null;
            }
        }
    }
}
