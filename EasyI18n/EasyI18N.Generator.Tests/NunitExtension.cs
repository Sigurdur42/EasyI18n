using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyI18N.Generator.Tests;
internal static class NunitExtension
{
    public static void MultiLineAreEqual(string expected, string toVerify, string additionalHint)
    {
        var expectedResult = expected
                    .Replace("\r\n", "\n")
                    .Replace("\n", "\r\n");

        var left = expectedResult.Split(new[]
                                {
                                        '\r', '\n'
                                    }, StringSplitOptions.RemoveEmptyEntries);

        var right = toVerify.Split(new[]
                         {
                                 '\r', '\n'
                             }, StringSplitOptions.RemoveEmptyEntries);
        var numberOfLines = Math.Min(left.Length, right.Length);
        var errorLines = new StringBuilder();

        if (left.Length != right.Length)
        {
            errorLines.AppendFormat(CultureInfo.InvariantCulture, "Line count doesn't match: expected: {0}, found: {1}", left.Length, right.Length);
            errorLines.AppendLine();
        }

        for (var index = 0; index < numberOfLines; index++)
        {
            if (left[index] != right[index])
            {
                errorLines.Append("Line number " + (index + 1) + ":");
                errorLines.AppendLine();
                errorLines.Append("Expected: <");
                errorLines.Append(left[index]);
                errorLines.Append("> Actual: <");
                errorLines.Append(right[index]);
                errorLines.Append(">");
                errorLines.AppendLine();
            }
        }

        if (errorLines.Length > 0)
        {
            var message = string.Format(CultureInfo.InvariantCulture, "{1}: {0}", errorLines.ToString(), additionalHint);
            throw new InvalidOperationException(message);
        }
    }
}
