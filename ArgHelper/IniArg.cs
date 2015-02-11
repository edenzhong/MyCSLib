using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;

namespace ArgHelper
{
    /// <summary>
    /// The ini file access helper.
    /// </summary>
    public class IniArg
    {
        [System.Runtime.InteropServices.DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        [System.Runtime.InteropServices.DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, System.Text.StringBuilder retVal, int size, string filePath);


        private string sPath = null;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path">The Ini file path.</param>
        public IniArg(string path)
        {
            this.sPath = path;
        }

        public void WriteValue(string section, string key, string value)
        {
            WritePrivateProfileString(section, key, value, sPath);
        }
        public string ReadValue(string section, string key,string defVal="")
        {
            System.Text.StringBuilder temp = new System.Text.StringBuilder(255);
            if (null == defVal)
            {
                defVal = "";
            }
            GetPrivateProfileString(section, key, defVal, temp, 255, sPath);
            return temp.ToString();
        }
    }

    /// <summary>
    /// Load configuration from ini file.
    /// </summary>
    public static class IniArgLoader
    {
        public static bool LoadIni(this IArg arg, string confFile, string section)
        {
            if ( null ==arg )
            {
                return false;
            }
            IniArg ini = new IniArg(confFile);
            return arg.LoadArg((key, defVal) => { return ini.ReadValue(section, key, defVal); });
        }
    }
}
