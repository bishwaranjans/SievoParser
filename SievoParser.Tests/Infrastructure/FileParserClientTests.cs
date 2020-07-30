#region Namespaces

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SievoParser.Domain.AbstractFactories;

#endregion

namespace SievoParser.Infrastructure.Tests.Infrastructure
{
    [TestClass()]
    public class FileParserClientTests
    {
        #region Fields

        FileParserClient fileParserClient;
        IFileParser iFileParser;
        string fileName;

        #endregion

        #region Tests SetUp

        [TestInitialize]
        public void Setup()
        {
            fileName = "ExampleData.tsv";

            fileParserClient = new FileParserClient();
        }

        #endregion

        #region Test Methods

        [TestMethod()]
        public void GetFileParserFromFileExtensionTest()
        {
            // Arrange
            string expectedFileParserName = "TsvFileParser";

            // Act
            iFileParser = fileParserClient.GetFileParserFromFileExtension(fileName);

            // Assert
            Assert.IsNotNull(iFileParser);
            Assert.AreEqual(iFileParser.GetType().Name, expectedFileParserName);
        }

        #endregion

        #region Test CleanUp

        [TestCleanup]
        public void CleanUp()
        {
            fileParserClient.Dispose();
        }

        #endregion
    }
}