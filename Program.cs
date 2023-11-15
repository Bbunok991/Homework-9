using System;
using System.Text.Json;
using System.Xml;

namespace Homework_9
{
    class Program
    {
        static void Main(string[] args)
        {
            var json = @"
            {
                ""Current"": {
                    ""Time"": ""2023-06-18T20:35:06.722127+04:00"",
                    ""Temperature"": 29,
                    ""Weathercode"": 1,
                    ""Windspeed"": 2.1,
                    ""Winddirection"": 1
                },
                ""History"": [
                    {
                        ""Time"": ""2023-06-17T20:35:06.77707+04:00"",
                        ""Temperature"": 29,
                        ""Weathercode"": 2,
                        ""Windspeed"": 2.4,
                        ""Winddirection"": 1
                    },
                    {
                        ""Time"": ""2023-06-16T20:35:06.777081+04:00"",
                        ""Temperature"": 22,
                        ""Weathercode"": 2,
                        ""Windspeed"": 2.4,
                        ""Winddirection"": 1
                    },
                    {
                        ""Time"": ""2023-06-15T20:35:06.777082+04:00"",
                        ""Temperature"": 21,
                        ""Weathercode"": 4,
                        ""Windspeed"": 2.2,
                        ""Winddirection"": 1
                    }
                ]
            }";

            var xml = ConvertJsonToXml(json);
            Console.WriteLine(xml);
        }

        static string ConvertJsonToXml(string json)
        {
            using (JsonDocument document = JsonDocument.Parse(json))
            {
                XmlDocument xmlDocument = new XmlDocument();
                XmlElement root = xmlDocument.CreateElement("root");
                xmlDocument.AppendChild(root);
                BuildXmlElement(document.RootElement, root);
                return xmlDocument.OuterXml;
            }
        }

        static void BuildXmlElement(JsonElement element, XmlElement parent)
        {
            foreach (JsonProperty property in element.EnumerateObject())
            {
                XmlElement xmlElement = parent.OwnerDocument.CreateElement(property.Name);
                parent.AppendChild(xmlElement);
                switch (property.Value.ValueKind)
                {
                    case JsonValueKind.Object:
                        BuildXmlElement(property.Value, xmlElement);
                        break;
                    case JsonValueKind.Array:
                        foreach (JsonElement arrayElement in property.Value.EnumerateArray())
                        {
                            BuildXmlElement(arrayElement, xmlElement);
                        }
                        break;
                    case JsonValueKind.String:
                        xmlElement.InnerText = property.Value.GetString();
                        break;
                    case JsonValueKind.Number:
                        xmlElement.InnerText = property.Value.GetRawText();
                        break;
                    case JsonValueKind.True:
                        xmlElement.InnerText = "true";
                        break;
                    case JsonValueKind.False:
                        xmlElement.InnerText = "false";
                        break;
                    case JsonValueKind.Null:
                        xmlElement.SetAttribute("null", "true");
                        break;
                }
            }
        }
    }
}