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
    public class TokenFactoryTests
    {
        [TestMethod()]
        public void TokenFactoryTest()
        {
            string key = "Slot_1";
            string value = "hello eden";
            TokenFactory tf = new TokenFactory();
            KeyValuePair<string,string> kv;
            int cover;
            cover = tf.Create(out kv, "-"+key, value);
            Assert.AreEqual(cover, 2);
            Assert.AreEqual(kv.Key, key);
            Assert.AreEqual(kv.Value, value);
        }

        
        [TestMethod()]
        public void DashTokenCreatorTest()
        {
            KeyValuePair<string,string> kv;
            int cover;
            cover = TokenFactory.DashTokenCreator(out kv, "-_Slot1", "hello eden");
            Assert.AreEqual(cover, 2);

            cover = TokenFactory.DashTokenCreator(out kv, "-Slot-1", "hello eden");
            Assert.AreEqual(cover, 2);

            cover = TokenFactory.DashTokenCreator(out kv, "-Slot1", null);
            Assert.AreEqual(cover, 0);

            cover = TokenFactory.DashTokenCreator(out kv, null, "hello eden");
            Assert.AreEqual(cover, 0);

            cover = TokenFactory.DashTokenCreator(out kv, "Slot1", "hello eden");
            Assert.AreEqual(cover, 0);

            cover = TokenFactory.DashTokenCreator(out kv, "-Slot 1", "hello eden");
            Assert.AreEqual(cover, 0);

            cover = TokenFactory.DashTokenCreator(out kv, "--Slot1", "hello eden");
            Assert.AreEqual(cover, 0);
        }

        [TestMethod()]
        public void SingleTokenCreatorTest()
        {
            KeyValuePair<string, string> kv;
            int cover;
            cover = TokenFactory.SingleTokenCreator(out kv, "eden", null);
            Assert.AreEqual(cover, 1);

            cover = TokenFactory.SingleTokenCreator(out kv, "_eden-zhong", null);
            Assert.AreEqual(cover, 1);

            cover = TokenFactory.SingleTokenCreator(out kv, "-eden", null);
            Assert.AreEqual(cover, 0);

            cover = TokenFactory.SingleTokenCreator(out kv, "eden zhong", null);
            Assert.AreEqual(cover, 0);
        }

        [TestMethod()]
        public void DoubleDashTokenCreatorTest()
        {
            KeyValuePair<string, string> kv;
            int cover;
            cover = TokenFactory.DoubleDashTokenCreator(out kv, "--_Slot1", "hello eden");
            Assert.AreEqual(cover, 2);

            cover = TokenFactory.DoubleDashTokenCreator(out kv, "--Slot-1", "hello eden");
            Assert.AreEqual(cover, 2);

            cover = TokenFactory.DoubleDashTokenCreator(out kv, "--Slot1", null);
            Assert.AreEqual(cover, 0);

            cover = TokenFactory.DoubleDashTokenCreator(out kv, null, "hello eden");
            Assert.AreEqual(cover, 0);

            cover = TokenFactory.DoubleDashTokenCreator(out kv, "Slot1", "hello eden");
            Assert.AreEqual(cover, 0);

            cover = TokenFactory.DoubleDashTokenCreator(out kv, "--Slot 1", "hello eden");
            Assert.AreEqual(cover, 0);

            cover = TokenFactory.DoubleDashTokenCreator(out kv, "-Slot1", "hello eden");
            Assert.AreEqual(cover, 0);
        }
    }
}
