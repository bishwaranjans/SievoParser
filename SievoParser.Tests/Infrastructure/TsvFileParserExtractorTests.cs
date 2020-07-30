#region Namespaces

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SievoParser.Domain;
using SievoParser.Domain.AbstractFactories;
using SievoParser.Domain.Entities;
using SievoParser.Domain.Utilities;
using SievoParser.Infrastructure;
using SievoParser.Infrastructure.ConcreteFactories;
using SievoParser.Infrastructure.Facades;

#endregion

namespace SievoParser.Tests.Infrastructure
{
    [TestClass()]
    public class TsvFileParserExtractorTests
    {
        #region Fields

        Bootstrapper _instance;
        FileParserFacade _fileParserFacade;
        IFileParser _iFileParser;
        string _applicationBase;

        #endregion

        #region Tests SetUp

        [TestInitialize]
        public void Setup()
        {
            _instance = Bootstrapper.Instance;
            _instance.RegisterServices();

            _applicationBase = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
        }

        #endregion

        #region Test Methods

        [TestMethod()]
        public void GetRecordListTest_Success_ValidData()
        {
            // Arrange
            string fileFullPath = Path.Join(_applicationBase, @"TestData\ExampleData.tsv");
            _iFileParser = new TsvFileParser(fileFullPath);
            _fileParserFacade = new FileParserFacade(_iFileParser);

            // Act
            IList<Record> faultyRecords = _fileParserFacade.GetRecordList().Where(s => !string.IsNullOrWhiteSpace(s.Error)).ToList();

            // Assert
            Assert.IsNotNull(faultyRecords);
            Assert.AreEqual(0, faultyRecords.Count);
        }

        [TestMethod()]
        public void GetRecordListByProjectTest_Success_ValidData()
        {
            // Arrange
            string fileFullPath = Path.Join(_applicationBase, @"TestData\ExampleData.tsv");
            _iFileParser = new TsvFileParser(fileFullPath);
            _fileParserFacade = new FileParserFacade(_iFileParser);
            int project = 5;
            int expectedRecordCount = 1;

            // Act
            IList<Record> records = _fileParserFacade.GetRecordListByProject(project).ToList();

            // Assert
            Assert.IsNotNull(records);
            Assert.AreEqual(expectedRecordCount, records.Count);
        }

        [TestMethod()]
        public void GetRecordListTest_Success_SortByStartDate()
        {
            // Arrange
            string fileFullPath = Path.Join(_applicationBase, @"TestData\ExampleData.tsv");
            _iFileParser = new TsvFileParser(fileFullPath);
            _fileParserFacade = new FileParserFacade(_iFileParser);
            string expectedStartDate = "2009-06-01 00:00:00.000";

            // Act
            IList<Record> records = _fileParserFacade.GetRecordList().OrderBy(s => s.StartDate).ToList();
            string startDate = DateTime.Parse(records.FirstOrDefault()?.StartDate.ToString() ?? string.Empty).ToString(Constants.FileDateTimeFormat);

            // Assert
            Assert.IsNotNull(records);
            Assert.AreEqual(expectedStartDate, startDate);
        }

        [TestMethod()]
        public void GetRecordListTest_Success_ColumnsOrderChanged()
        {
            // Arrange
            string fileFullPath = Path.Join(_applicationBase, @"TestData\ColumnsOrderChangeExampleData.tsv");
            _iFileParser = new TsvFileParser(fileFullPath);
            _fileParserFacade = new FileParserFacade(_iFileParser);
            int expectedProject = 2;

            // Act
            IList<Record> records = _fileParserFacade.GetRecordList().ToList();
            IList<Record> faultyRecords = records.Where(s => !string.IsNullOrWhiteSpace(s.Error)).ToList();
            Record record = records.FirstOrDefault();

            // Assert           
            Assert.AreEqual(2, records.Count);
            Assert.AreEqual(expectedProject, record?.Project);
            Assert.IsNotNull(faultyRecords);
            Assert.AreEqual(0, faultyRecords.Count);
        }

        [TestMethod()]
        public void GetRecordListTest_Error_InvalidDateFormat()
        {
            // Arrange
            string fileFullPath = Path.Join(_applicationBase, @"TestData\InvalidDateExampleData.tsv");
            _iFileParser = new TsvFileParser(fileFullPath);
            _fileParserFacade = new FileParserFacade(_iFileParser);

            // Act
            IList<Record> faultyRecords = _fileParserFacade.GetRecordList().Where(s => !string.IsNullOrWhiteSpace(s.Error)).ToList();
            string errorMessage1 = faultyRecords.Select(s => s.Error).FirstOrDefault();
            string errorMessage2 = faultyRecords.Select(s => s.Error).LastOrDefault();
            bool isContainExpectedErrorMessage1 = errorMessage1.Contains("The string '2014hghdkdk-01-01 00:00:00.000' was not recognized as a valid DateTime. There is an unknown word starting at index '4'.");
            bool isContainExpectedErrorMessage2 = errorMessage2.Contains("Start date value: '2013/01/01 00:00:00.000' is different than the allowed format i.e. yyyy-MM-dd HH:mm:ss.fff");

            // Assert
            Assert.IsNotNull(faultyRecords);
            Assert.AreEqual(2, faultyRecords.Count);
            Assert.AreEqual(isContainExpectedErrorMessage1, true);
            Assert.AreEqual(isContainExpectedErrorMessage2, true);
        }

        [TestMethod()]
        public void GetRecordListTest_Error_InvalidSavingAmount()
        {
            // Arrange
            string fileFullPath = Path.Join(_applicationBase, @"TestData\InvalidSavingAmountExampleData.tsv");
            _iFileParser = new TsvFileParser(fileFullPath);
            _fileParserFacade = new FileParserFacade(_iFileParser);

            // Act
            IList<Record> faultyRecords = _fileParserFacade.GetRecordList().Where(s => !string.IsNullOrWhiteSpace(s.Error)).ToList();
            string errorMessage = faultyRecords.Select(s => s.Error).FirstOrDefault();
            bool isContainExpectedErrorMessage = errorMessage.Contains("The conversion cannot be performed.\r\n    Text: 'invalidsavingvalue'");

            // Assert
            Assert.IsNotNull(faultyRecords);
            Assert.AreEqual(1, faultyRecords.Count);
            Assert.AreEqual(isContainExpectedErrorMessage, true);
        }

        [TestMethod()]
        public void GetRecordListTest_Error_InvalidComplexity()
        {
            // Arrange
            string fileFullPath = Path.Join(_applicationBase, @"TestData\InvalidComplexityExampleData.tsv");
            _iFileParser = new TsvFileParser(fileFullPath);
            _fileParserFacade = new FileParserFacade(_iFileParser);

            // Act
            IList<Record> faultyRecords = _fileParserFacade.GetRecordList().Where(s => !string.IsNullOrWhiteSpace(s.Error)).ToList();
            string errorMessage = faultyRecords.Select(s => s.Error).FirstOrDefault();
            bool isContainExpectedErrorMessage = errorMessage.Contains("Complexity value: 'VeryHigh' is different than the allowed values");

            // Assert
            Assert.IsNotNull(faultyRecords);
            Assert.AreEqual(1, faultyRecords.Count);
            Assert.AreEqual(isContainExpectedErrorMessage, true);
        }

        [TestMethod()]
        public void GetRecordListTest_Error_InvalidLines()
        {
            // Arrange
            string fileFullPath = Path.Join(_applicationBase, @"TestData\InvalidLinesExampleData.tsv");
            _iFileParser = new TsvFileParser(fileFullPath);
            _fileParserFacade = new FileParserFacade(_iFileParser);

            // Act
            IList<Record> faultyRecords = _fileParserFacade.GetRecordList().Where(s => !string.IsNullOrWhiteSpace(s.Error)).ToList();
            string errorMessage = faultyRecords.Select(s => s.Error).FirstOrDefault();
            bool isContainExpectedErrorMessage = errorMessage.Contains("The conversion cannot be performed.\r\n    Text: 'hkdhgkhgs'");

            // Assert
            Assert.IsNotNull(faultyRecords);
            Assert.AreEqual(1, faultyRecords.Count);
            Assert.AreEqual(isContainExpectedErrorMessage, true);
        }

        [TestMethod()]
        public void GetRecordListByProjectWithSort_IntegrationTest_Error_MixedValidInValidRecords()
        {
            // Arrange
            int project = 6;

            string fileFullPath = Path.Join(_applicationBase, @"TestData\MixedExampleData.tsv");
            FileParserClient fileParserClient = new FileParserClient();
            IFileParser iFileParser = fileParserClient.GetFileParserFromFileExtension(fileFullPath);
            FileParserFacade fileParserFacade = new FileParserFacade(iFileParser);

            // Act
            List<Record> records = fileParserFacade.GetRecordListByProject(project).OrderBy(s => s.StartDate).ToList();

            List<Record> faultyRecords = records.Where(s => !string.IsNullOrWhiteSpace(s.Error)).ToList();

            // Assert
            Assert.IsNotNull(records);
            Assert.AreEqual(3, records.Count);
            Assert.IsNotNull(faultyRecords);
            Assert.AreEqual(1, faultyRecords.Count);
        }

        [TestMethod()]
        public void GetRecordListByProjectWithSortAndNoProjectIdSpecification_IntegrationTest_Error_MixedValidInValidRecords()
        {
            // Arrange
            int project = 0; // This means no project i specified as per the CommandLine nuget package. Hence all records code is being invoked.
            bool isSortByStartDate = true;

            string fileFullPath = Path.Join(_applicationBase, @"TestData\MixedExampleData.tsv");
            FileParserClient fileParserClient = new FileParserClient();
            IFileParser iFileParser = fileParserClient.GetFileParserFromFileExtension(fileFullPath);
            FileParserFacade fileParserFacade = new FileParserFacade(iFileParser);

            // Act
            List<Record> records = project == 0 ?
                        (isSortByStartDate ? fileParserFacade.GetRecordList().OrderBy(s => s.StartDate).ToList() : fileParserFacade.GetRecordList().ToList()) :
                        (isSortByStartDate ? fileParserFacade.GetRecordListByProject(project).OrderBy(s => s.StartDate).ToList() : fileParserFacade.GetRecordListByProject(project).ToList());


            List<Record> faultyRecords = records.Where(s => !string.IsNullOrWhiteSpace(s.Error)).ToList();

            // Assert
            Assert.IsNotNull(records);
            Assert.AreEqual(9, records.Count);
            Assert.IsNotNull(faultyRecords);
            Assert.AreEqual(5, faultyRecords.Count);
        }

        #endregion

        #region Test CleanUp

        [TestCleanup]
        public void CleanUp()
        {
            _instance.DisposeServices();
        }

        #endregion
    }
}