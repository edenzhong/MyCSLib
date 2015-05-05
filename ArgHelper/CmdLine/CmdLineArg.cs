using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgHelper.CmdLine
{
    public class CmdLineArg
    {
        public static CmdArgs Parse(string[] args)
        {
            CmdArgs carg = new CmdArgs();

            TokenFactory tf = new TokenFactory();

            KeyValuePair<string, string> kv;
            int i = 1;
            while(i <= args.Length)
            {
                int cover;
                if ( i == args.Length)
                {
                    cover = tf.Create(out kv, args[i - 1], null);
                }
                else
                {
                    cover = tf.Create(out kv, args[i - 1], args[i]);
                }
                
                if ( cover > 0) // valid
                {
                    carg.Add(kv);
                }
                i += cover;
            }
            return carg;
        }
        
    }

    public class CmdArgs
    {
        public bool Inited { get; set; }
        public Dictionary<string, object> ArgPairs = new Dictionary<string, object>();
        public List<string> Args = new List<string>();

        public void Add(KeyValuePair<string, string> kv)
        {
            if ( null != kv.Key)
            {
                if ( ArgPairs.Keys.Contains(kv.Key))
                {
                    object o = ArgPairs[kv.Key];
                    if ( o is string)
                    {
                        List<string> l = new List<string>();
                        l.Add(o as string);
                        l.Add(kv.Value);
                        ArgPairs[kv.Key] = l;
                    }
                    else if ( o is List<string>)
                    {
                        List<string> l = o as List<string>;
                        l.Add(kv.Value);
                    }
                    else
                    {
                        // ??
                        throw new Exception("Not implement");
                    }
                }
                else
                {
                    ArgPairs.Add(kv.Key, kv.Value);
                }
            }
            else
            {
                Args.Add(kv.Value);
            }
        }
    }
}
