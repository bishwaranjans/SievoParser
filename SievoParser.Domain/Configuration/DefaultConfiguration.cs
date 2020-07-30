#region Namespaces

using SievoParser.Domain.Utilities;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

#endregion

namespace SievoParser.Domain.Configuration
{
    /// <summary>
    /// Default configuration responsible for reading App.Config file settings data.
    /// </summary>
    /// <seealso cref="SievoParser.Domain.Configuration.IConfiguration" />
    public class DefaultConfiguration : IConfiguration
    {
        #region Properties

        /// <summary>
        /// Gets the allowed complexities from App.Config. This field can be used to adding/removing any complexity type. Depending upon this value, validation is happening for a record while reading.
        /// </summary>
        /// <value>
        /// The allowed complexities.
        /// </value>
        public IList<string> AllowedComplexities
        {
            get
            {
                string allowedComplexity = ConfigurationManager.AppSettings[Constants.AppConfigAllowedComplexityText];
                if (!string.IsNullOrWhiteSpace(allowedComplexity))
                {
                    return allowedComplexity.Trim().Split(Constants.CommaDelimiter).Select(s => s.Trim()).ToList();
                }
                return Constants.DefaultComplexities;
            }
        }

        #endregion
    }
}
