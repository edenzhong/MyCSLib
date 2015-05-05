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
    public class CmdLineArgLoaderTests
    {
        class MyCmdLineArg:IArg
        {
            [ArgInf]
            public string abc{get;set;}
            [ArgInf]
            public int intVal{get;set;}
            [ArgInf]
            public string equalVal{get;set;}

            [ArgInf("","",ArgInfAttribute.ArgType.StandAlone)]
            public bool standaloneVal{get;set;}

            public bool ArgInited { get; set; }
        }

        [TestMethod()]
        public void LoadCmdLineTest()
        {
            string[] args = {"-abc","abcval","--intVal","1","standaloneVal","equalVal=Equal-value"};

            MyCmdLineArg myarg = new MyCmdLineArg();
            myarg.LoadCmdLine(args);
            
        }
    }
}
