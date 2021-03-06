﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArgHelper.CmdLine;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace ArgHelper.CmdLine.Tests
{
    [TestClass()]
    public class CmdLineArgTests
    {
        [TestMethod()]
        public void ParseTest()
        {
            string[] args = {"-Slot","1","-eden","zhong","eden","zhong","--hello","my friend","micro=soft"};


            CmdArgs a = CmdLineArg.Parse(args);
            Assert.AreEqual(a.ArgPairs.Count, 4);
            Assert.AreEqual(a.ArgPairs["Slot"], "1");
            Assert.AreEqual(a.ArgPairs["eden"], "zhong");
            Assert.AreEqual(a.ArgPairs["hello"], "my friend");
            Assert.AreEqual(a.ArgPairs["micro"], "soft");

            Assert.AreEqual(a.Args.Count, 2);
            Assert.AreEqual(a.Args[0], "eden");
            Assert.AreEqual(a.Args[1], "zhong");
        }
    }
}
