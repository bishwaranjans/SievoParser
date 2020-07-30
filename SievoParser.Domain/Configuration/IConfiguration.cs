#region Namespaces

using System.Collections.Generic;

#endregion

namespace SievoParser.Domain.Configuration
{
    /// <summary>
    /// Configuration responsible for reading App.Config file settings data.
    /// </summary>
    public interface IConfiguration
    {
        #region Properties

        /// <summary>
        /// Gets the allowed complexities from App.Config. This field can be used to adding/removing any complexity type. Depending upon this value, validation is happening for a record while reading.
        /// </summary>
        /// <value>
        /// The allowed complexities.
        /// </value>
        IList<string> AllowedComplexities { get; }

        #endregion
    }
}
