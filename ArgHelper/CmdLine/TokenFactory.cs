﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace ArgHelper.CmdLine
{
    public class TokenFactory
    {
        public delegate int TokenCreator(out KeyValuePair<string, string> kv,string token, string value);
        List<TokenCreator> _creators = new List<TokenCreator>();
        
        public TokenFactory()
        {
            Register(DashTokenCreator);
            Register(DoubleDashTokenCreator);
            Register(SingleTokenCreator);
            Register(EqualTokenCreator);
        }
        public void Register(TokenCreator c)
        {
            if ( null != c)
            {
                _creators.Add(c);
            }
        }
        public int Create(out KeyValuePair<string,string> kv,string token, string value)
        {
            kv = new KeyValuePair<string, string>(null, null);
            foreach(TokenCreator c in _creators)
            {
                int cover = c(out kv,token, value);
                if ( cover > 0)
                {
                    return cover;
                }
            }

            return 0;
        }

        static Regex DashTokenRegex = new Regex("^-[_a-zA-Z0-9][-_a-zA-Z0-9]*$");
        public static int DashTokenCreator(out KeyValuePair<string,string> kv,string token, string value)
        {
            kv = new KeyValuePair<string, string>(null,null);
            if (( null == token ) || (null == value))
            {
                return 0;
            }

            if (!DashTokenRegex.IsMatch(token))
            {
                return 0;
            }
            kv = new KeyValuePair<string, string>(token.Substring(1), value);

            return 2;
        }

        static Regex DoubleDashTokenRegex = new Regex("^--[_a-zA-Z0-9][-_a-zA-Z0-9]*$");
        public static int DoubleDashTokenCreator(out KeyValuePair<string, string> kv, string token, string value)
        {
            kv = new KeyValuePair<string, string>(null, null);
            if ((null == token) || (null == value))
            {
                return 0;
            }

            if (!DoubleDashTokenRegex.IsMatch(token))
            {
                return 0;
            }
            kv = new KeyValuePair<string, string>(token.Substring(2), value);

            return 2;
        }

        static Regex EqualTokenRegex = new Regex("^[_a-zA-Z0-9][-_a-zA-Z0-9]*=[_a-zA-Z0-9][-_a-zA-Z0-9 ]*[_a-zA-Z0-9]$");
        public static int EqualTokenCreator(out KeyValuePair<string, string> kv, string token, string value)
        {
            kv = new KeyValuePair<string, string>(null, null);
            if ((null == token))
            {
                return 0;
            }

            if (!EqualTokenRegex.IsMatch(token))
            {
                return 0;
            }
            int idx = token.IndexOf('=');
            kv = new KeyValuePair<string, string>(token.Substring(0,idx), token.Substring(idx+1));

            return 1;
        }

        static Regex SingleTokenRegex = new Regex("^[_a-zA-Z0-9][-_a-zA-Z0-9]*$");
        public static int SingleTokenCreator(out KeyValuePair<string, string> kv, string token, string value)
        {
            kv = new KeyValuePair<string, string>(null, null);
            if (null == token)
            {
                return 0;
            }

            if (!SingleTokenRegex.IsMatch(token))
            {
                return 0;
            }
            kv = new KeyValuePair<string, string>(null, token);

            return 1;
        }
    }
}
