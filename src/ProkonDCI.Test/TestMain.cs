using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ivento.Dci;

namespace ProkonDCI.Test
{
    [TestClass]
    public class TestMain
    {
        [AssemblyInitialize]
        public static void Init(TestContext context)
        {
            Context.Initialize.InStaticScope();
        }

        [AssemblyCleanup]
        public static void DeInit()
        {
        }
    }
}
