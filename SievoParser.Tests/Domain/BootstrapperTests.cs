#region Namespaces

using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SievoParser.Domain;

#endregion

namespace SievoParser.Tests.Domain
{
    [TestClass()]
    public class BootstrapperTests
    {
        #region Fields

        Bootstrapper _instance;

        #endregion

        #region Tests SetUp

        [TestInitialize]
        public void Setup()
        {
            _instance = Bootstrapper.Instance;
            _instance.RegisterServices();
        }

        #endregion

        #region Test Methods

        [TestMethod()]
        public void GetAllowedComplexitiesTest()
        {
            // Act
            List<string> expectedAllowedComplexities = ConfigurationManager.AppSettings[SievoParser.Domain.Utilities.Constants.AppConfigAllowedComplexityText].Trim().Split(SievoParser.Domain.Utilities.Constants.CommaDelimiter).Select(s => s.Trim()).ToList();
            
            // Assert
            Assert.IsNotNull(_instance.Config);
            Assert.AreEqual(_instance.Config.AllowedComplexities.Count, expectedAllowedComplexities.Count);
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