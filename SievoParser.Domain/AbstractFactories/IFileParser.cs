#region Namespaces

using SievoParser.Domain.AbstractProducts;

#endregion

namespace SievoParser.Domain.AbstractFactories
{
    /// <summary>
    /// The 'AbstractFactory' FileParser interface.
    /// </summary>
    public interface IFileParser
    {
        #region Properties

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
        /// Gets the file parser extractor.
        /// </summary>
        /// <returns>
        /// Returns the parse extractor which will parse the file.
        /// </returns>
        IFileParserExtractor GetFileParserExtractor();

        #endregion
    }
}
