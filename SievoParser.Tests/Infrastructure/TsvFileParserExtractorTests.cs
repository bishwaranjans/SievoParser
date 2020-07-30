#region Namespaces

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SievoParser.Domain;
using SievoParser.Domain.AbstractFactories;
using SievoParser.Domain.Entities;
using SievoParser.Infrastructure.ConcreateFactories;
using SievoParser.Infrastructure.Facades;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

#endregion

namespace SievoParser.Infrastructure.ConcreateProducts.Tests.Infrastructure
{
    [TestClass()]
    public class TsvFileParserExtractorTests
    {
        #region Fields

        Bootstrapper instance;
        FileParserFacade fileParserFacade;
        IFileParser iFileParser;
        string applicatuionBase;

        #endregion

        #region Tests SetUp

        [TestInitialize]
        public void Setup()
        {
            instance = Bootstrapper.Instance;
            instance.RegisterServices();

            applicatuionBase = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
        }

        #endregion

        #region Test Methods

        [TestMethod()]
        public void GetRecordListTest_Success_ValidData()
        {
            // Arrange
            string fileFullPath = Path.Join(applicatuionBase, @"TestData\ExampleData.tsv");
            iFileParser = new TsvFileParser(fileFullPath);
            fileParserFacade = new FileParserFacade(iFileParser);

            // Act
            IList<Record> faultyRecords = fileParserFacade.GetRecordList().Where(s => !string.IsNullOrWhiteSpace(s.Error)).ToList();

            // Assert
            Assert.IsNotNull(faultyRecords);
            Assert.AreEqual(0, faultyRecords.Count);
        }

        [TestMethod()]
        public void GetRecordListByProjectTest_Success_ValidData()
        {
            // Arrange
            string fileFullPath = Path.Join(applicatuionBase, @"TestData\ExampleData.tsv");
            iFileParser = new TsvFileParser(fileFullPath);
            fileParserFacade = new FileParserFacade(iFileParser);
            int project = 5;
            int expectedRecordCount = 1;

            // Act
            IList<Record> records = fileParserFacade.GetRecordListByProject(project).ToList();

            // Assert
            Assert.IsNotNull(records);
            Assert.AreEqual(expectedRecordCount, records.Count);
        }

        [TestMethod()]
        public void GetRecordListTest_Success_SortByStartDate()
        {
            // Arrange
            string fileFullPath = Path.Join(applicatuionBase, @"TestData\ExampleData.tsv");
            iFileParser = new TsvFileParser(fileFullPath);
            fileParserFacade = new FileParserFacade(iFileParser);
            string expectedStartDate = "2009-06-01 00:00:00.000";

            // Act
            IList<Record> records = fileParserFacade.GetRecordList().OrderBy(s => s.StartDate).ToList();
            string startDate = DateTime.Parse(records.FirstOrDefault().StartDate.ToString()).ToString(Domain.Utilities.Constants.FileDateTimeFormat);

            // Assert
            Assert.IsNotNull(records);
            Assert.AreEqual(expectedStartDate, startDate);
        }

        [TestMethod()]
        public void GetRecordListTest_Success_ColumnsOrderChanged()
        {
            // Arrange
            string fileFullPath = Path.Join(applicatuionBase, @"TestData\ColumnsOrderChangeExampleData.tsv");
            iFileParser = new TsvFileParser(fileFullPath);
            fileParserFacade = new FileParserFacade(iFileParser);
            int expectedProject = 2;

            // Act
            IList<Record> records = fileParserFacade.GetRecordList().ToList();
            IList<Record> faultyRecords = records.Where(s => !string.IsNullOrWhiteSpace(s.Error)).ToList();
            Record record = records.FirstOrDefault();

            // Assert           
            Assert.AreEqual(2, records.Count);
            Assert.AreEqual(expectedProject, record.Project);
            Assert.IsNotNull(faultyRecords);
            Assert.AreEqual(0, faultyRecords.Count);
        }

        [TestMethod()]
        public void GetRecordListTest_Error_InvalidDateFormat()
        {
            // Arrange
            string fileFullPath = Path.Join(applicatuionBase, @"TestData\InvalidDateExampleData.tsv");
            iFileParser = new TsvFileParser(fileFullPath);
            fileParserFacade = new FileParserFacade(iFileParser);

            // Act
            IList<Record> faultyRecords = fileParserFacade.GetRecordList().Where(s => !string.IsNullOrWhiteSpace(s.Error)).ToList();
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
            string fileFullPath = Path.Join(applicatuionBase, @"TestData\InvalidSavingAmountExampleData.tsv");
            iFileParser = new TsvFileParser(fileFullPath);
            fileParserFacade = new FileParserFacade(iFileParser);

            // Act
            IList<Record> faultyRecords = fileParserFacade.GetRecordList().Where(s => !string.IsNullOrWhiteSpace(s.Error)).ToList();
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
            string fileFullPath = Path.Join(applicatuionBase, @"TestData\InvalidComplexityExampleData.tsv");
            iFileParser = new TsvFileParser(fileFullPath);
            fileParserFacade = new FileParserFacade(iFileParser);

            // Act
            IList<Record> faultyRecords = fileParserFacade.GetRecordList().Where(s => !string.IsNullOrWhiteSpace(s.Error)).ToList();
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
            string fileFullPath = Path.Join(applicatuionBase, @"TestData\InvalidLinesExampleData.tsv");
            iFileParser = new TsvFileParser(fileFullPath);
            fileParserFacade = new FileParserFacade(iFileParser);

            // Act
            IList<Record> faultyRecords = fileParserFacade.GetRecordList().Where(s => !string.IsNullOrWhiteSpace(s.Error)).ToList();
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
            bool isSortByStartDate = true;

            string fileFullPath = Path.Join(applicatuionBase, @"TestData\MixedExampleData.tsv");
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
            Assert.AreEqual(3, records.Count);
            Assert.IsNotNull(faultyRecords);
            Assert.AreEqual(1, faultyRecords.Count);
        }

        #endregion

        #region Test CleanUp

        [TestCleanup]
        public void CleanUp()
        {
            instance.DisposeServices();
        }

        #endregion
    }
}