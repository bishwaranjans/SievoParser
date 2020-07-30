#region Namespaces

using CsvHelper;
using System;
using System.Globalization;
using System.Text;

#endregion

namespace SievoParser.Domain.Utilities
{
    /// <summary>
    /// StringUtilities methods
    /// </summary>
    public static class StringUtilities
    {
        #region Methods

        /// <summary>
        /// Gets the property value.
        /// </summary>
        /// <param name="src">The source.</param>
        /// <param name="propName">Name of the property.</param>
        /// <returns></returns>
        public static object GetPropValue(object src, string propName)
        {
            string mappedPropName;
            bool isDate = false;

            switch (propName)
            {
                case Constants.StartDateHeaderText:
                    mappedPropName = Constants.StartDateMappedPropertyText;
                    isDate = true;
                    break;
                case Constants.SavingsAmountHeaderText:
                    mappedPropName = Constants.SavingsAmountMappedPropertyText;
                    break;
                default:
                    mappedPropName = propName;
                    break;
            }

            object propValue = src.GetType().GetProperty(mappedPropName)?.GetValue(src, null);
            return isDate ? Convert.ToDateTime(propValue, CultureInfo.InvariantCulture).ToString(Constants.FileDateTimeFormat) : propValue;
        }

        /// <summary>
        /// Displays the message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="isError">if set to <c>true</c> [is error].</param>
        public static void DisplayMessage(string message, bool isError = false)
        {
            if (isError)
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            Console.WriteLine(message);
            if (isError)
            {
                Console.ResetColor();
            }
        }

        /// <summary>
        /// Gets the message from exception.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <returns>Returns the exception message.</returns>
        public static string GetMessageFromException(Exception ex)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(ex.Message);

            if (ex.InnerException != null)
            {
                sb.Append(ex.InnerException.Message);
            }

            if (ex is CsvHelperException exception)
            {
                sb.Append($"{Environment.NewLine}RawRowValue: {exception.ReadingContext.RawRecord}");
            }

            return $"{sb}";
        }

        #endregion
    }
}
