#region Namespaces

using SievoParser.Domain.AbstractFactories;
using SievoParser.Domain.AbstractProducts;
using SievoParser.Infrastructure.ConcreateProducts;
using static SievoParser.Domain.Utilities.Constants;

#endregion

namespace SievoParser.Infrastructure.ConcreteFactories
{
    /// <summary>
    /// The 'ConcreteFactory' TsvFileParser class.
    /// </summary>
    /// <seealso cref="SievoParser.Domain.AbstractFactories.IFileParser{SievoParser.Domain.Entities.Record}" />
    /// <seealso cref="SievoParser.Domain.AbstractFactories.IFileParser" />
    public class TsvFileParser : IFileParser
    {
        #region Properties

        /// <summary>
        /// Gets or sets the file path.
        /// </summary>
        /// <value>
        /// The file path.
        /// </value>
        public string FilePath { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TsvFileParser"/> class.
        /// </summary>
        public TsvFileParser()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TsvFileParser" /> class.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        public TsvFileParser(string filePath)
        {
            this.FilePath = filePath;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the file parser extractor.
        /// </summary>
        /// <returns>
        /// Returns an abstract FileParserExtractor.
        /// </returns>
        public IFileParserExtractor GetFileParserExtractor()
        {
            return new TsvFileParserExtractor(FilePath);
        }

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return ParserType.Tsv.ToString();
        }

        #endregion
    }
}
