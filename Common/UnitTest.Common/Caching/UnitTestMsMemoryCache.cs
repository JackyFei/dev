using System;
using Common.Caching;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Common.Caching.Microsoft;

namespace UnitTest.Common.Caching
{
    [TestClass]
    public class UnitTestMsMemoryCache : UnitTestCacheBase
    {
        protected override ICache CreateCacheInstance(string name)
        {
            return new MsMemoryCache(name);
        }

        protected override ICache CreateCacheInstance()
        {
            return new MsMemoryCache();
        }

        [TestMethod]
        public void MsMemoryCache_Set_DifferentMemoryInstances()
        {
            var key = GenerateKey();
            CacheInstance1.Set(key, 1, DateTimeOffset.Now.AddMilliseconds(1000));
            CacheInstance2.Set(key, 2, DateTimeOffset.Now.AddMilliseconds(1000));
            //Assert.IsTrue(success1);
            var obj = CacheInstance1.Get(key);
            Assert.IsNotNull(obj);
            Assert.IsInstanceOfType(obj, typeof(int));
            Assert.AreEqual(1, (int)obj);

            //Assert.IsTrue(success2);
            var obj2 = CacheInstance2.Get(key);
            Assert.IsNotNull(obj2);
            Assert.IsInstanceOfType(obj2, typeof(int));
            Assert.AreEqual(2, (int)obj2);
        }
    }
}
