#region Namespaces

using CommandLine;
using CommandLine.Text;
using SievoParser.Domain;
using SievoParser.Domain.AbstractFactories;
using SievoParser.Domain.Entities;
using SievoParser.Domain.Utilities;
using SievoParser.Infrastructure;
using SievoParser.Infrastructure.Facades;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using static SievoParser.Domain.Utilities.Constants;

#endregion

namespace SievoParser.Console
{
    /// <summary>
    /// Main entry program
    /// </summary>
    class Program
    {
        #region Arguments Options

        /// <summary>
        /// Argument options
        /// </summary>
        class Options
        {
            /// <summary>
            /// Gets or sets the filename.
            /// </summary>
            /// <value>
            /// The filename.
            /// </value>
            [Option(FilePathText, Required = true, HelpText = FilePathHelpText)]
            public string Filename { get; set; }

            /// <summary>
            /// Gets or sets the project.
            /// </summary>
            /// <value>
            /// The project.
            /// </value>
            [Option(ProjectText, Required = false, HelpText = ProjectHelpText)]
            public int Project { get; set; }

            /// <summary>
            /// Gets or sets the sort by start date.
            /// </summary>
            /// <value>
            /// The sort by start date.
            /// </value>
            [Option(SortByStartDateText, Required = false, Default = false, HelpText = SortByStartDateHelpText)]
            public bool IsSortByStartDate { get; set; }

            /// <summary>
            /// Gets the examples.
            /// </summary>
            /// <value>
            /// The examples.
            /// </value>
            [Usage(ApplicationAlias = ApplicationAliasText)]
            public static IEnumerable<Example> Examples
            {
                get
                {
                    return new List<Example>() {
                               new Example("SievoParser.Console Usage Example", new Options { Filename = @"E:\Projects\SievoParser\Documentation\ExampleData.tsv", Project = 2, IsSortByStartDate = false })
                               };
                }
            }
        }

        #endregion

        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        /// <param name="args">The arguments.</param>
        static void Main(string[] args)
        {
            // PreValidate arguments
            List<string> arguments = PreValidateArguments(args);

            // Try to parse the passed arguments. In-case of invalid one, it should show proper message with example
            var commandLineParser = new Parser(c =>
            {
                c.CaseSensitive = false;
                c.HelpWriter = System.Console.Error;
            });
            commandLineParser.ParseArguments<Options>(arguments)
               .WithParsed<Options>(o =>
               {
                   try
                   {
                       // In case of any invalid file path,
                       if (!File.Exists(o.Filename))
                       {
                           StringUtilities.DisplayMessage($"File : {o.Filename} does not exists! Please specify a valid file full name along with the path.");
                           Environment.Exit(0);
                       }

                       // Register services for dependency injection
                       Bootstrapper.Instance.RegisterServices();

                       #region Record Retrieval as per filter if any

                       FileParserClient fileParserClient = new FileParserClient();
                       IFileParser iFileParser = fileParserClient.GetFileParserFromFileExtension(o.Filename);
                       FileParserFacade fileParserFacade = new FileParserFacade(iFileParser);
                       List<Record> records = o.Project == 0 ?
                       (o.IsSortByStartDate ? fileParserFacade.GetRecordList().OrderBy(s => s.StartDate).ToList() : fileParserFacade.GetRecordList().ToList()) :
                       (o.IsSortByStartDate ? fileParserFacade.GetRecordListByProject(o.Project).OrderBy(s => s.StartDate).ToList() : fileParserFacade.GetRecordListByProject(o.Project).ToList());

                       #endregion

                       // Display header
                       DisplayHeader(fileParserFacade.FileHeaderColumns);

                       // Display records
                       DisplayRecords(fileParserFacade.FileHeaderColumns, records);

                       // Dispose services
                       fileParserClient.Dispose();
                       Bootstrapper.Instance.DisposeServices();
                   }
                   catch (Exception ex)
                   {
                       StringUtilities.DisplayMessage(StringUtilities.GetMessageFromException(ex));
                   }
               })
               .WithNotParsed<Options>(o =>
               {
                   // Automatically help section should display.
               });
        }

        #region Helper Methods

        /// <summary>
        /// Pres the validate arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <returns>Returns the arguments.</returns>
        private static List<string> PreValidateArguments(string[] args)
        {
            // Handling situation when no value specified for 'sortByStartDate'
            var arguments = new List<string>(args);
            bool sortByStartDateContained = args.Any(l => l.Contains(SortByStartDateText));
            if (sortByStartDateContained)
            {
                arguments.Add(true.ToString());
            }
            return arguments;
        }

        /// <summary>
        /// Displays the headers.
        /// </summary>
        /// <param name="headerColumns">The header columns.</param>
        private static void DisplayHeader(IList<string> headerColumns)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var headerColumn in headerColumns)
            {
                sb.Append($"{headerColumn}{TabDelimiter}");
            }

            StringUtilities.DisplayMessage(sb.ToString());
        }

        /// <summary>
        /// Displays the records with error details if any.
        /// </summary>
        /// <param name="headerColumns">The header columns.</param>
        /// <param name="records">The records.</param>
        private static void DisplayRecords(IList<string> headerColumns, List<Record> records)
        {
            records.ForEach(record =>
            {
                StringBuilder sb = new StringBuilder();
                bool isErrorRecord = false;

                if (!string.IsNullOrWhiteSpace(record.Error))
                {
                    isErrorRecord = true;
                    sb.Append(record.Error); //.TrimEnd('\n')
                }
                else
                {
                    foreach (var headerColumn in headerColumns)
                    {
                        sb.Append(StringUtilities.GetPropValue(record, headerColumn)).Append(TabDelimiter);
                    }
                }
                StringUtilities.DisplayMessage(sb.ToString(), isErrorRecord);
            });
        }

        #endregion
    }
}
