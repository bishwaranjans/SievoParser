#region Namespaces

using System.Collections.Generic;
using System.Collections.ObjectModel;

#endregion

namespace SievoParser.Domain.Utilities
{
    /// <summary>
    /// Constants
    /// </summary>
    public class Constants
    {
        public const string TabDelimiter = "\t";
        public const char CommaDelimiter = ',';
        public const string FileNullDepiction = "NULL";
        public const string FileDateTimeFormat = "yyyy-MM-dd HH:mm:ss.fff";
        public static readonly IList<string> DefaultComplexities = new ReadOnlyCollection<string>(new List<string> { "Simple", "Moderate", "Hazardous" });

        public const string StartDateHeaderText = "Start date";
        public const string StartDateMappedPropertyText = "StartDate";
        public const string AppConfigAllowedComplexityText = "AllowedComplexities";

        public const string SavingsAmountHeaderText = "Savings amount";
        public const string SavingsAmountMappedPropertyText = "SavingsAmount";

        public const string ApplicationAliasText = "SievoParser.Console.exe";
        public const string FilePathText = "file";
        public const string FilePathHelpText = "Full path to the input file.";
        public const string ProjectText = "project";
        public const string ProjectHelpText = "Filter results by column 'Project'. If you specify '0' or not specified at all, then all records will be retrieved.";
        public const string SortByStartDateText = "sortByStartDate";
        public const string SortByStartDateHelpText = "Sort results by column 'Start date' in ascending order.";

        #region Enums

        /// <summary>
        /// Supported ParserType
        /// </summary>
        public enum ParserType
        {
            Tsv,
            Xml,
            // e.g. CSV or more depending upon the supported format       
        }

        #endregion
    }
}
