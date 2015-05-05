using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArgHelper.CmdLine;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace ArgHelper.CmdLine.Tests
{
    [TestClass()]
    public class CmdArgsTests
    {
        [TestMethod()]
        public void AddTest()
        {
            string key = "eden";
            string value = "zhong";



            CmdArgs arg = new CmdArgs();
            KeyValuePair<string,string> kv = new KeyValuePair<string,string>(key,value);
            arg.Add(kv);
            Assert.AreEqual(arg.ArgPairs.Keys.Count, 1);
            Assert.IsTrue(arg.ArgPairs.Keys.Contains(key));
            Assert.AreEqual(arg.ArgPairs[key], value);
        }
    }
}
