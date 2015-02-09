using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace ArgHelper
{
    /// <summary>
    /// Argument information attribute. A property with this attribute could be handled by ArgHelper.
    /// </summary>
    public class ArgInfAttribute : Attribute
    {
        /// <summary>
        /// The argument type.
        /// </summary>
        public enum ArgType
        {
            /// <summary>
            /// The argument will be presented as a key/value pair.
            /// for example : -Address 192.168.1.1
            /// </summary>
            KeyValue,

            /// <summary>
            /// The argument will be presented as a standalone tag. It is just present or not, no value for it, or only true/false for it.
            /// </summary>
            StandAlone,
        }
        /// <summary>
        /// the ArgInf contructor.
        /// </summary>
        /// <param name="inf">The help information of the property comes with this attribute.</param>
        /// <param name="defultVal">The default value of the property.</param>
        /// <param name="t">The ArgType of the property.</param>
        /// <param name="mandatory">Specify if the property value must be specified. Not implement yet.</param>
        public ArgInfAttribute(string inf = "", string defultVal = null, ArgType t = ArgType.KeyValue, bool mandatory = false)
        {
            HelpInf = inf;
            Default = defultVal;
            AType = t;
            Mandatory = mandatory;
        }
        /// <summary>
        /// The help information of the property comes with this attribute.
        /// </summary>
        public string HelpInf { get; set; }
        /// <summary>
        /// The default value of the property.
        /// </summary>
        public string Default { get; set; }
        /// <summary>
        /// The ArgType of the property.
        /// </summary>
        public ArgType AType { get; set; }
        /// <summary>
        /// Specify if the property value must be specified. Not implement yet.
        /// </summary>
        public bool Mandatory { get; set; }
    }
    /// <summary>
    /// Argument interface. All Argument class must implement this interface.
    /// </summary>
    public interface IArg
    {
        /// <summary>
        /// Check if the argument is initialized or not. internal use only.
        /// </summary>
        bool ArgInited { get; set; }
    }
    public static class ArgHelper
    {
        /// <summary>
        /// Set the property value of an IArg from a string. The following types are supported:
        /// string, Int64,Int32,bool, float, double
        /// Other types are not support yet.
        /// This function commonly not called by end user. It is used for internal purpose.
        /// </summary>
        /// <param name="arg">The IArg object.</param>
        /// <param name="p">The property to be set.</param>
        /// <param name="str">The string value.</param>
        /// <returns>True if set the value success or false if fail.</returns>
        public static bool SetValFromString(this IArg arg, PropertyInfo p, string str)
        {
            if (null != str)
            {
                try
                {
                    if (p.PropertyType == typeof(string))
                    {
                        p.SetValue(arg, str, null);
                    }
                    else if (p.PropertyType == typeof(Int64))
                    {
                        Int64 v = Int64.Parse(str);
                        p.SetValue(arg, v, null);
                    }
                    else if (p.PropertyType == typeof(Int32))
                    {
                        Int32 v = Int32.Parse(str);
                        p.SetValue(arg, v, null);
                    }
                    else if (p.PropertyType == typeof(bool))
                    {
                        bool v = bool.Parse(str);
                        p.SetValue(arg, v, null);
                    }
                    else if (p.PropertyType == typeof(float))
                    {
                        float v = float.Parse(str);
                        p.SetValue(arg, v, null);
                    }
                    else if (p.PropertyType == typeof(double))
                    {
                        double v = double.Parse(str);
                        p.SetValue(arg, v, null);
                    }
                    else
                    {
                        return false;
                    }
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            return false;
        }

        /// <summary>
        /// Load value of an IArg. This function commonly not called by end user. It is used for internal purpose.
        /// </summary>
        /// <param name="args">The IArg object.</param>
        /// <param name="getArgStr">The delegate of getting the argument value string from configuration source, say, command line, config file.
        /// The 1st string parameter is the property name. The 2nd string parameter is the default value. Return value is the string value.
        /// </param>
        /// <param name="checkStandAloneArg">The delegate of getting the argument value for the standalone argument. 
        /// The 1st string parameter is the property name. The return value indicates the arguement show up or not.
        /// </param>
        /// <returns>Indicate the loading argument success or not.</returns>
        public static bool LoadArg(this IArg args, Func<string, string, string> getArgStr, Func<string, bool> checkStandAloneArg = null)
        {
            if ((null == args) || (null == getArgStr))
            {
                return false;
            }

            Type t = args.GetType();
            var ps = t.GetProperties();
            foreach (var p in ps)
            {
                object[] attrs = p.GetCustomAttributes(typeof(ArgInfAttribute), false);
                if ((null == attrs) || (0 == attrs.Length))
                {
                    continue;
                }
                ArgInfAttribute ia = attrs[0] as ArgInfAttribute;

                string key;
                int index = p.Name.LastIndexOf('.');
                key = (index >= 0) ? p.Name.Substring(index) : p.Name;

                string argStr = null;
                if (ia.AType == ArgInfAttribute.ArgType.KeyValue)
                {
                    if ((args.ArgInited) || (ia.Mandatory))
                    {
                        argStr = getArgStr(key, null);
                    }
                    else
                    {
                        argStr = getArgStr(key, ia.Default);
                    }
                }
                else //if ( ia.AType == ArgInfAttribute.ArgType.StandAlone )
                {
                    try
                    {
                        argStr = checkStandAloneArg(key).ToString();
                    }
                    catch { }
                }
                if (null != argStr)
                {
                    if (!args.SetValFromString(p, argStr))
                    {
                        return false;
                    }
                    ia.Mandatory = false;
                }
            }
            args.ArgInited = true;
            return true;
        }

        /// <summary>
        /// Get the help information of an IArg.
        /// </summary>
        /// <param name="arg">The IArg object.</param>
        /// <returns></returns>
        public static string GetHelpInfo(this IArg arg)
        {
            StringBuilder helpInfo = new StringBuilder();
            Type t = arg.GetType();
            var ps = t.GetProperties();
            foreach (var p in ps)
            {
                Object[] atts = p.GetCustomAttributes(typeof(ArgInfAttribute), false);
                if (atts.Length > 0)
                {
                    string s;
                    int index = p.Name.LastIndexOf('.');
                    s = (index >= 0) ? p.Name.Substring(index) : p.Name;

                    foreach (object o in atts)
                    {
                        ArgInfAttribute attr = o as ArgInfAttribute;
                        if (null != attr)
                        {
                            if (attr.AType == ArgInfAttribute.ArgType.KeyValue)
                            {
                                helpInfo.Append("-");
                                helpInfo.Append(s);
                                helpInfo.Append(" :[Type ");
                                helpInfo.Append(p.PropertyType.Name);
                                helpInfo.Append("] \n    ");
                                helpInfo.Append(attr.HelpInf);
                            }
                            else
                            {
                                helpInfo.Append(s);
                                helpInfo.Append(":\n    ");
                                helpInfo.Append(attr.HelpInf);
                            }

                            helpInfo.Append("\n");
                        }
                    }
                }

            }
            return helpInfo.ToString();
        }
    }
}
