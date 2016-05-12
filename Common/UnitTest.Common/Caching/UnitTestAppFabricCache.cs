using System;
using Common.Caching;
using Common.Caching.Microsoft;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest.Common.Caching
{
    /// <summary>
    /// The pre setups below should be done before run the tests.
    /// 1. Install and setup Appfabric caching feature in local machine.
    /// 2. Start cache cluster.
    /// 3. Create named cache: cache1 and cache2
    /// </summary>
    //[TestClass]
    public class UnitTestAppFabricCache : UnitTestCacheBase
    {
        protected override ICache CreateCacheInstance(string name)
        {
            return new AppFabricCache(name);
        }

        protected override ICache CreateCacheInstance()
        {
            return new AppFabricCache();
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public override void Cache_Set_SlidingExpiration_Success()
        {
            base.Cache_Set_SlidingExpiration_Success();
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public override void Cache_Add_SlidingExpiration_ArgumentException()
        {
            base.Cache_Add_SlidingExpiration_ArgumentException();
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public override void Cache_Set_SlidingExpiration_ArgumentException()
        {
            base.Cache_Set_SlidingExpiration_ArgumentException();
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public override void Cache_Add_SlidingExpiration_Success()
        {
            base.Cache_Add_SlidingExpiration_Success();
        }
    }
}
