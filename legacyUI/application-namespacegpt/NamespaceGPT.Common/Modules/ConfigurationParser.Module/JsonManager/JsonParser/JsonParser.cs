using System.Globalization;
using System.Text;

namespace NamespaceGPT.Common.Modules.ConfigurationParser.Module.JsonManager.JsonParser
{
    public static class JsonParser
    {
        public static object Parse(string jsonString)
        {
            int position = 0;

            SkipWhitespace(jsonString, ref position);

            char symbol = Peek(jsonString, 0);

            if (symbol == '{')
            {
                return ParseObject(jsonString, ref position);
            }
            else if (symbol == '[')
            {
                return ParseArray(jsonString, ref position);
            }

            throw new Exception("Unexpected symbol!");
        }

        private static JsonObject ParseObject(string jsonString, ref int position)
        {
            JsonObject jsonObject = new ();
            Consume(jsonString, ref position, '{');

            while (Peek(jsonString, position) != '}')
            {
                string key = ParseString(jsonString, ref position);
                Consume(jsonString, ref position, ':');

                object? value = ParseValue(jsonString, ref position);
                jsonObject.Properties[key] = value;

                if (Peek(jsonString, position) != '}')
                {
                    Consume(jsonString, ref position, ',');
                }
            }

            Consume(jsonString, ref position, '}');
            return jsonObject;
        }

        private static JsonArray ParseArray(string jsonString, ref int position)
        {
            JsonArray jsonArray = new ();
            Consume(jsonString, ref position, '[');

            while (Peek(jsonString, position) != ']')
            {
                jsonArray.Items.Add(ParseValue(jsonString, ref position));

                if (Peek(jsonString, position) != ']')
                {
                    Consume(jsonString, ref position, ',');
                }
            }

            Consume(jsonString, ref position, ']');
            return jsonArray;
        }

        private static object? ParseValue(string jsonString, ref int position)
        {
            SkipWhitespace(jsonString, ref position);

            char symbol = Peek(jsonString, position);

            if (symbol == '-' || (symbol >= '0' && symbol <= '9'))
            {
                return ParseNumber(jsonString, ref position);
            }

            return symbol switch
            {
                '{' => ParseObject(jsonString, ref position),
                '[' => ParseArray(jsonString, ref position),
                '"' => ParseString(jsonString, ref position),
                't' => ParseTrue(jsonString, ref position),
                'f' => ParseFalse(jsonString, ref position),
                'n' => ParseNull(jsonString, ref position),
                _ => throw new Exception("Unexpected token at position: " + position),
            };
        }

        private static object ParseNumber(string jsonString, ref int position)
        {
            StringBuilder stringBuilder = new ();

            while ((Peek(jsonString, position) >= '0' && Peek(jsonString, position) <= '9') ||
                Peek(jsonString, position) == '.' || Peek(jsonString, position) == '-')
            {
                stringBuilder.Append(Next(jsonString, ref position));
            }

            double result = double.Parse(stringBuilder.ToString(), CultureInfo.InvariantCulture);

            if (double.IsInteger(result))
            {
                return (int)result;
            }

            return result;
        }

        private static string ParseString(string jsonString, ref int position)
        {
            Consume(jsonString, ref position, '"');

            StringBuilder stringBuilder = new ();

            while (Peek(jsonString, position) != '"')
            {
                stringBuilder.Append(Next(jsonString, ref position));
            }

            Consume(jsonString, ref position, '"');

            return stringBuilder.ToString();
        }

        private static bool ParseTrue(string jsonString, ref int position)
        {
            Consume(jsonString, ref position, "true");
            return true;
        }

        private static bool ParseFalse(string jsonString, ref int position)
        {
            Consume(jsonString, ref position, "false");
            return false;
        }

        private static object? ParseNull(string jsonString, ref int position)
        {
            Consume(jsonString, ref position, "null");
            return null;
        }

        // Helper methods...
        private static void SkipWhitespace(string jsonString, ref int position)
        {
            while (position < jsonString.Length && char.IsWhiteSpace(jsonString[position]))
            {
                position++;
            }
        }

        private static char Peek(string jsonString, int position)
        {
            return jsonString[position];
        }

        private static char Next(string jsonString, ref int position)
        {
            return jsonString[position++];
        }

        private static void Consume(string jsonString, ref int position, char expected)
        {
            if (Peek(jsonString, position) != expected)
            {
                throw new Exception("Expected " + expected + " at position " + position);
            }

            position++;
        }

        private static void Consume(string jsonString, ref int position, string expected)
        {
            foreach (char character in expected)
            {
                Consume(jsonString, ref position, character);
            }
        }
    }
}
