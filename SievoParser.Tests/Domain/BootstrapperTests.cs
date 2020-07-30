#region Namespaces

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

#endregion

namespace SievoParser.Domain.Tests.Domain
{
    [TestClass()]
    public class BootstrapperTests
    {
        #region Fields

        Bootstrapper instance;

        #endregion

        #region Tests SetUp

        [TestInitialize]
        public void Setup()
        {
            instance = Bootstrapper.Instance;
            instance.RegisterServices();
        }

        #endregion

        #region Test Methods

        [TestMethod()]
        public void GetAllowedComplexitiesTest()
        {
            // Act
            List<string> expectedAllowedComplexities = ConfigurationManager.AppSettings[Utilities.Constants.AppConfigAllowedComplexityText].Trim().Split(Utilities.Constants.CommaDelimiter).Select(s => s.Trim()).ToList();
            
            // Assert
            Assert.IsNotNull(instance.Config);
            Assert.AreEqual(instance.Config.AllowedComplexities.Count, expectedAllowedComplexities.Count);
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