using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Text.RegularExpressions;

namespace IronPdf_Contest
{
    public class HtmlTemplateEngine()
    {
        public string Execute(string html, object model)
        {
            string projectDir = Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.Parent.FullName;
            string logoPath = Path.Combine(projectDir, @"templates\customer-logo.png");
            var logo = "<img src='data:image/png;base64," + Convert.ToBase64String(File.ReadAllBytes(logoPath)) + "'>";
            var template = html.Replace("{{logo}}", logo);

            template = ReplaceLabels(template);
            template = ProcessConditionals(template, model);
            template = ProcessLoops(template, model);
            template = ReplacePlaceholders(template, model);

            return template;
        }

        private string ReplaceLabels(string template)
        {
            if (string.IsNullOrWhiteSpace(template))
                return template;

            var labelRegex = new Regex(@"{{\s*label:([\w\.]+)\s*}}");

            return labelRegex.Replace(template, match =>
            {
                string key = match.Groups[1].Value;
                //string localized = Localizer.GetText(key); 
                //return WebUtility.HtmlEncode(localized);  

                return key;
            });
        }

        private string ReplacePlaceholders(string template, object model)
        {
            if (string.IsNullOrWhiteSpace(template) || model == null)
                return template;

            if (string.IsNullOrWhiteSpace(template) || model == null)
                return template;

            template = ProcessLoops(template, model); 
            template = ProcessProperties(template, model);

            return template;
        }

        private string ProcessConditionals(string template, object model)
        {
            var regex = new Regex(
                @"{{#if\s+([^}]+)}}(.*?)({{else}}(.*?))?{{\/if}}",
                RegexOptions.Singleline);

            return regex.Replace(template, match =>
            {
                string expression = match.Groups[1].Value.Trim();
                string trueBlock = match.Groups[2].Value;
                string elseBlock = match.Groups[4].Success ? match.Groups[4].Value : "";

                bool result = EvaluateExpression(expression, model);

                return result ? trueBlock : elseBlock;
            });
        }

        private bool EvaluateExpression(string expression, object model)
        {
            string[] operators = { ">=", "<=", "==", "!=", ">", "<" };

            foreach (var op in operators)
            {
                // Split on operator
                var parts = expression.Split(op);
                if (parts.Length == 2)
                {
                    string left = parts[0].Trim();
                    string right = parts[1].Trim();

                    // Evaluate left (from model)
                    object? leftValue = GetNestedPropertyValue(model, left, out var leftType);
                    if (leftValue == null)
                        return false;

                    // String literal → remove quotes
                    if (right.StartsWith("\"") && right.EndsWith("\""))
                        right = right.Substring(1, right.Length - 2);

                    // Convert right side to correct type
                    object rightValue;
                    try
                    {
                        rightValue = Convert.ChangeType(right, leftType!);
                    }
                    catch
                    {
                        return false;
                    }

                    // Compare
                    int comparison = Comparer.DefaultInvariant.Compare(leftValue, rightValue);

                    return op switch
                    {
                        ">" => comparison > 0,
                        "<" => comparison < 0,
                        ">=" => comparison >= 0,
                        "<=" => comparison <= 0,
                        "==" => comparison == 0,
                        "!=" => comparison != 0,
                        _ => false
                    };
                }
            }

            // FALLBACK → your old logic (truthiness check)
            object? result = GetNestedPropertyValue(model, expression, out _);

            return result switch
            {
                bool b => b,
                string s => !string.IsNullOrWhiteSpace(s),
                null => false,
                _ => true
            };
        }

        private string ProcessLoops(string template, object model)
        {
            var loopRegex = new Regex(@"{{#(\w+)}}(.*?){{/\1}}", RegexOptions.Singleline);

            return loopRegex.Replace(template, match =>
            {
                string collectionName = match.Groups[1].Value; 
                string loopContent = match.Groups[2].Value;    

                var prop = model.GetType().GetProperty(collectionName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (prop == null) return "";

                var list = prop.GetValue(model) as System.Collections.IEnumerable;
                if (list == null) return "";

                var result = new System.Text.StringBuilder();

                foreach (var item in list)
                {
                    string renderedRow = ProcessProperties(loopContent, item);
                    result.AppendLine(renderedRow);
                }

                return result.ToString();
            });
        }

        private  string ProcessProperties(string template, object model)
        {
            var matches = Regex.Matches(template, @"{{\s*([\w\.]+)\s*}}");

            foreach (Match match in matches)
            {
                string fullMatch = match.Value; // {{Customer.Name}}
                string propertyPath = match.Groups[1].Value;

                object? value = GetNestedPropertyValue(model, propertyPath, out Type? valueType);
                string replacement = FormatValue(value, valueType);

                template = template.Replace(fullMatch, replacement);
            }

            return template;
        }

        private  object? GetNestedPropertyValue(object obj, string path, out Type? type)
        {
            type = null;
            string[] parts = path.Split('.');
            object? current = obj;

            foreach (string part in parts)
            {
                if (current == null) return null;

                var prop = current.GetType().GetProperty(part, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (prop == null) return null;

                current = prop.GetValue(current);
                type = prop.PropertyType;
            }

            return current;
        }

        private  string FormatValue(object? value, Type? type)
        {
            if (value == null || type == null)
                return string.Empty;

            if (type == typeof(DateTime) || type == typeof(DateTime?))
            {
                return ((DateTime)value).ToString("dd/MM/yyyy");
            }

            if (type == typeof(decimal) || type == typeof(decimal?) ||
                type == typeof(double) || type == typeof(double?) ||
                type == typeof(float) || type == typeof(float?))
            {
                return Convert.ToDecimal(value).ToString("#,##0.00", CultureInfo.InvariantCulture);
            }

            return value.ToString() ?? string.Empty;
        }
    }
}
