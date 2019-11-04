using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace General.Tests
{
    [TestClass]
    public class dBaseReaderTest
    {
        [TestMethod]
        public void TestDBFReader()
        {
            var testFile = "../../Resources/TestImport.dbf";

            string baseDir = System.Environment.CurrentDirectory;
            string filePath = System.IO.Path.GetFullPath(baseDir + "/" + testFile);

            Assert.IsTrue(System.IO.File.Exists(filePath));

            var table = General.Data.Conversion.dBaseReader.ReadDBF(testFile);
            Assert.IsNotNull(table);
            Assert.IsTrue(table.Rows.Count > 0);
        }
    }
}
