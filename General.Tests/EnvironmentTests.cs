using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using General.Model;

namespace General.Tests
{
    [TestClass]
    public class EnvironmentTests
    {
        [TestMethod]
        public void TestWhereAmI()
        {
            Environment.EnvironmentContext where = Environment.Current.WhereAmI();
            Assert.IsTrue(where == Environment.EnvironmentContext.QA);

            Assert.AreEqual(Configuration.GlobalConfiguration.GlobalSettings.GetByEnviromnentModifers("MyKey"), "qa value");

            Assert.AreEqual(Data.DBConnection.GetConnectionString("DBConn"), "mydbstring");
        }

    }
}
