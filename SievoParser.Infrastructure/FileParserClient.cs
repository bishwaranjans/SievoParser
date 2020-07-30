#region Namespaces

using SievoParser.Domain.AbstractFactories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using static SievoParser.Domain.Utilities.Constants;

#endregion

namespace SievoParser.Infrastructure
{
    /// <summary>
    /// FileParserClient responsible for providing which parser to be used as per the file provided in the argument.
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public class FileParserClient : IDisposable
    {
        #region Fields 

        /// <summary>
        /// The parsers dictionary. This contains the all available parser in the application.
        /// </summary>
        private static readonly Dictionary<string, IFileParser> Parsers = new Dictionary<string, IFileParser>();

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes the <see cref="FileParserClient"/> class.
        /// </summary>
        static FileParserClient()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            Type[] types = assembly.GetTypes();
            foreach (Type type in types)
            {
                if (!typeof(IFileParser).IsAssignableFrom(type)) continue;
                if (Activator.CreateInstance(type) is IFileParser parser)
                {
                    Parsers.Add(parser.ToString(), parser);
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the file parser from file extension.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>Returns the specific parser which will be used for parsing the input file.</returns>
        public IFileParser GetFileParserFromFileExtension(string fileName)
        {
            ParserType type = GetExtension(fileName);
            IFileParser parser = Parsers[type.ToString()];
            parser.FilePath = fileName;

            return parser;
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Gets the extension.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>Returns the ParserType  as per the file extension.</returns>
        private ParserType GetExtension(string fileName)
        {
            string strFileType = Path.GetExtension(fileName).ToLower();
            switch (strFileType)
            {
                case ".tsv":
                    return ParserType.Tsv;
                case ".xml":
                    return ParserType.Xml;
                default:
                    return ParserType.Tsv;
            }
        }

        #endregion

        #region IDisposal Implementation

        // To detect redundant calls
        private bool _disposed;

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose() => Dispose(true);

        /// <summary>
        /// Protected implementation of Dispose pattern. Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                // Dispose managed state (managed objects).               
            }

            _disposed = true;
        }

        #endregion
    }
}
