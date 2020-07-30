#region Namespaces

using CsvHelper;
using CsvHelper.Configuration;
using SievoParser.Domain.Utilities;
using System;
using System.Globalization;
using System.Text;

#endregion

namespace SievoParser.Domain.Entities
{
    /// <summary>
    /// Record entity which holds the individual row data with it's properties. It also hold any parsing error or validation fail details.
    /// </summary>
    /// <seealso cref="System.IEquatable{SievoParser.Domain.Entities.Record}" />
    public class Record : IEquatable<Record>
    {
        #region Properties

        /// <summary>
        /// Gets or sets the project Id.
        /// </summary>
        /// <value>
        /// The project.
        /// </value>
        public int Project { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the start date.
        /// </summary>
        /// <value>
        /// The start date.
        /// </value>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        /// <value>
        /// The category.
        /// </value>
        public string Category { get; set; }

        /// <summary>
        /// Gets or sets the person who is responsible.
        /// </summary>
        /// <value>
        /// The responsible person.
        /// </value>
        public string Responsible { get; set; }

        /// <summary>
        /// Gets or sets the savings amount.
        /// </summary>
        /// <value>
        /// The savings amount.
        /// </value>
        public decimal? SavingsAmount { get; set; }

        /// <summary>
        /// Gets or sets the currency.
        /// </summary>
        /// <value>
        /// The currency.
        /// </value>
        public string Currency { get; set; }

        /// <summary>
        /// Gets or sets the complexity.
        /// </summary>
        /// <value>
        /// The complexity.
        /// </value>
        public string Complexity { get; set; }

        /// <summary>
        /// Gets or sets the errors.
        /// </summary>
        /// <value>
        /// The errors.
        /// </value>
        public string Error { get; set; }

        #endregion

        #region IEqutable Implementation

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.
        /// </returns>
        public bool Equals(Record other)
        {
            if (other == null) return false;
            return Project == other.Project &&
                   StartDate == other.StartDate &&
                   string.Equals(Description, other.Description) &&
                   string.Equals(Category, other.Category) &&
                   string.Equals(Responsible, other.Responsible) &&
                   SavingsAmount == other.SavingsAmount &&
                   string.Equals(Currency, other.Currency) &&
                   string.Equals(Complexity, other.Complexity);
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>
        /// true if the specified object  is equal to the current object; otherwise, false.
        /// </returns>
        public override bool Equals(Object obj)
        {
            if (obj == null)
                return false;

            Record projectDetails = obj as Record;
            if (projectDetails == null)
                return false;
            else
                return Equals(projectDetails);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            unchecked // only needed if you're compiling with arithmetic checks enabled
            { // (the default compiler behaviour is *disabled*, so most folks won't need this)
                int hash = 13;
                hash = (hash * 7) + Project.GetHashCode();
                hash = (hash * 7) + Description.GetHashCode();
                hash = (hash * 7) + StartDate.GetHashCode();
                hash = (hash * 7) + Category.GetHashCode();
                hash = (hash * 7) + Responsible.GetHashCode();
                hash = (hash * 7) + SavingsAmount.GetHashCode();
                hash = (hash * 7) + Currency.GetHashCode();
                hash = (hash * 7) + Complexity.GetHashCode();

                return hash;
            }
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="projectDetails1">The project details1.</param>
        /// <param name="projectDetails2">The project details2.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(Record projectDetails1, Record projectDetails2)
        {
            if (((object)projectDetails1) == null || ((object)projectDetails2) == null)
                return Object.Equals(projectDetails1, projectDetails2);

            return projectDetails1.Equals(projectDetails2);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="record1">The record1.</param>
        /// <param name="record2">The record2.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(Record record1, Record record2)
        {
            if (((object)record1) == null || ((object)record2) == null)
                return !Object.Equals(record1, record2);

            return !(record1.Equals(record2));
        }

        #endregion
    }

    /// <summary>
    /// The mapper class for Record
    /// </summary>
    /// <seealso cref="CsvHelper.Configuration.ClassMap{SievoParser.Domain.Entities.Record}" />
    public sealed class RecordMap : ClassMap<Record>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RecordMap"/> class.
        /// </summary>
        public RecordMap()
        {
            AutoMap(CultureInfo.InvariantCulture);
            Map(m => m.SavingsAmount).Name(Constants.SavingsAmountHeaderText);
            Map(m => m.StartDate).Name(Constants.StartDateHeaderText);
            Map(m => m.Error).ConvertUsing((IReaderRow row) =>
            {
                StringBuilder sb = new StringBuilder();

                // Validate Complexity
                string complexity = row.GetField(nameof(Record.Complexity));
                if (string.IsNullOrWhiteSpace(complexity) || !Bootstrapper.Instance.Config.AllowedComplexities.Contains(complexity))
                {
                    sb.AppendLine($"Complexity value: '{complexity}' is different than the allowed values i.e. {string.Join(Constants.CommaDelimiter.ToString(), Bootstrapper.Instance.Config.AllowedComplexities)}");
                }

                // Validate StartDate
                var startDate = row.GetField(Constants.StartDateHeaderText);
                if (!DateTime.TryParseExact(startDate, Constants.FileDateTimeFormat, CultureInfo.InvariantCulture.DateTimeFormat, DateTimeStyles.AllowWhiteSpaces, out DateTime validatedStartDate))
                {
                    sb.AppendLine($"Start date value: '{startDate}' is different than the allowed format i.e. {Constants.FileDateTimeFormat}");
                }

                // If some error, then append the raw record for more clarity
                if (sb.Length > 0)
                {
                    sb.Append($"RawRecord: {row.Context.RawRecord}");
                }

                return sb.ToString();
            });
        }

        #endregion
    }
}
