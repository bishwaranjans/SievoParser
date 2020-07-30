#region Namespaces

using SievoParser.Domain.Entities;
using System.Collections.Generic;

#endregion

namespace SievoParser.Domain.AbstractProducts
{
    /// <summary>
    /// The 'AbstractProduct' FileParserExtractor interface
    /// </summary>
    public interface IFileParserExtractor
    {
        #region Properties

        /// <summary>
        /// Gets the file header columns.
        /// </summary>
        /// <value>
        /// The file header columns.
        /// </value>
        IList<string> FileHeaderColumns { get; }

        /// <summary>
        /// Gets or sets the file path.
        /// </summary>
        /// <value>
        /// The file path.
        /// </value>
        string FilePath { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the record list.
        /// </summary>
        /// <returns>
        /// Returns the parsed records list.
        /// </returns>
        IEnumerable<Record> GetRecordList();

        /// <summary>
        /// Gets the record list by project.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <returns>
        /// Returns the filtered records by project.
        /// </returns>
        IEnumerable<Record> GetRecordListByProject(int project);

        #endregion

    }
}
