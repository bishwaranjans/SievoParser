#region Namespaces

using CsvHelper;
using CsvHelper.Configuration;
using SievoParser.Domain.AbstractProducts;
using SievoParser.Domain.Entities;
using SievoParser.Domain.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

#endregion

namespace SievoParser.Infrastructure.ConcreteProducts
{
    /// <summary>
    /// The Concrete 'Product' TsvFileParserExtractor class
    /// </summary>
    /// <seealso cref="SievoParser.Domain.AbstractProducts.IFileParserExtractor" />
    public class TsvFileParserExtractor : IFileParserExtractor
    {
        #region Properties

        /// <summary>
        /// Gets the file header columns.
        /// </summary>
        /// <value>
        /// The file header columns.
        /// </value>
        public IList<string> FileHeaderColumns { get; private set; }

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
        /// Initializes a new instance of the <see cref="TsvFileParserExtractor"/> class.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        public TsvFileParserExtractor(string filePath)
        {
            this.FilePath = filePath;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the record list by project.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <returns>
        /// Returns the filtered records by project.
        /// </returns>
        public IEnumerable<Record> GetRecordListByProject(int project)
        {
            var records = GetRecordList();
            return records.Where(s => s.Project == project);
        }

        /// <summary>
        /// Gets the record list.
        /// </summary>
        /// <returns>
        /// Returns the parsed records list.
        /// </returns>
        public IEnumerable<Record> GetRecordList()
        {
            using (var reader = new StreamReader(FilePath))
            using (var csv = new CsvReader(reader, GetCsvReaderConfiguration()))
            {
                csv.Read();
                csv.ReadHeader();
                FileHeaderColumns = csv.Context.HeaderRecord.ToList();

                while (csv.Read())
                {
                    Record record;
                    try
                    {
                        record = csv.GetRecord<Record>();
                    }
                    catch (Exception ex)
                    {
                        // Add record with error state
                        record = new Record() { Error = StringUtilities.GetMessageFromException(ex) };
                    }

                    yield return record;
                }
            }
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Gets the CSV reader configuration.
        /// </summary>
        /// <returns></returns>
        private CsvConfiguration GetCsvReaderConfiguration()
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                IgnoreBlankLines = true,
                Delimiter = Constants.TabDelimiter,
                HasHeaderRecord = true,
                AllowComments = true,
                TrimOptions = TrimOptions.Trim
            };

            config.RegisterClassMap<RecordMap>();
            config.TypeConverterOptionsCache.GetOptions<decimal?>().NullValues.AddRange(new[] { Constants.FileNullDepiction });
            config.TypeConverterOptionsCache.GetOptions<string>().NullValues.AddRange(new[] { Constants.FileNullDepiction });

            return config;
        }

        #endregion
    }
}
