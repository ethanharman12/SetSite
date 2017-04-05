using Microsoft.VisualStudio.TestTools.UnitTesting;
using SetSite.UIAutomation;

namespace SetSite.UITests
{
    [TestClass]
    public class BaseTest
    {
        [TestInitialize]
        public void Initialize()
        {
            Driver.Initialize();
        }

        [TestCleanup]
        public void Cleanup()
        {
            Driver.Close();
        }
    }
}
