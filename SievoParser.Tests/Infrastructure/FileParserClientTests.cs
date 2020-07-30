#region Namespaces

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SievoParser.Domain.AbstractFactories;
using SievoParser.Infrastructure;

#endregion

namespace SievoParser.Tests.Infrastructure
{
    [TestClass()]
    public class FileParserClientTests
    {
        #region Fields

        FileParserClient _fileParserClient;
        IFileParser _iFileParser;
        string _fileName;

        #endregion

        #region Tests SetUp

        [TestInitialize]
        public void Setup()
        {
            _fileName = "ExampleData.tsv";

            _fileParserClient = new FileParserClient();
        }

        #endregion

        #region Test Methods

        [TestMethod()]
        public void GetFileParserFromFileExtensionTest()
        {
            // Arrange
            string expectedFileParserName = "TsvFileParser";

            // Act
            _iFileParser = _fileParserClient.GetFileParserFromFileExtension(_fileName);

            // Assert
            Assert.IsNotNull(_iFileParser);
            Assert.AreEqual(_iFileParser.GetType().Name, expectedFileParserName);
        }

        #endregion

        #region Test CleanUp

        [TestCleanup]
        public void CleanUp()
        {
            _fileParserClient.Dispose();
        }

        #endregion
    }
}