#region Namespaces

using SievoParser.Domain.AbstractFactories;
using SievoParser.Domain.AbstractProducts;
using SievoParser.Domain.Entities;
using System.Collections.Generic;

#endregion

namespace SievoParser.Infrastructure.Facades
{
    /// <summary>
    /// The 'Client' FileParser class  
    /// </summary>
    public class FileParserFacade
    {
        #region Fields 

        /// <summary>
        /// The file parser extractor
        /// </summary>
        private readonly IFileParserExtractor _fileParserExtractor;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the file header columns.
        /// </summary>
        /// <value>
        /// The file header columns.
        /// </value>
        public IList<string> FileHeaderColumns => _fileParserExtractor.FileHeaderColumns;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FileParserFacade"/> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        public FileParserFacade(IFileParser factory)
        {
            _fileParserExtractor = factory.GetFileParserExtractor();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the record list.
        /// </summary>
        /// <returns>
        /// Returns the records from file.
        /// </returns>
        public IEnumerable<Record> GetRecordList()
        {
            return _fileParserExtractor.GetRecordList();
        }

        /// <summary>
        /// Gets the record list by project.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <returns>
        /// Returns the filtered records by project.
        /// </returns>
        public IEnumerable<Record> GetRecordListByProject(int project)
        {
            return _fileParserExtractor.GetRecordListByProject(project);
        }

        #endregion       
    }
}
