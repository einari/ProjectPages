using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;

namespace XmlDocToMarkDown
{
    public static class XElementExtensions
    {
        public static bool IsType(this XElement element)
        {
            return element.Attribute("name").Value.StartsWith("T:");
        }

        public static bool IsMethod(this XElement element)
        {
            return element.Attribute("name").Value.StartsWith("M:");
        }

        public static string GetPath(this XElement element)
        {
            var name = GetTypeName(element);
            return name.Replace(".","\\");
        }

        public static string GetTypeName(this XElement element)
        {
            var name = GetFullName(element);
            if (element.IsMethod())
                name = name.Substring(0, name.LastIndexOf("."));
            return name;
        }

        public static string GetName(this XElement element)
        {
            var fullName = element.GetFullName();
            

            return fullName.Substring(fullName.LastIndexOf(".")+1);
        }

        public static string GetFileName(this XElement element)
        {
            return string.Format("{0}.md", element.GetName());
        }

        public static string GetFullName(this XElement element)
        {
            var nameValue = element.Attribute("name").Value;
            nameValue = nameValue.Replace("#", "");

            var stop = nameValue.IndexOf("(");
            if (stop <= 0)
                stop = nameValue.IndexOf("'");


            stop -= 2;

            var name = stop > 0 ? nameValue.Substring(2, stop) : nameValue.Substring(2);
            return name;
        }

        public static string GetDescription(this XElement element)
        {
            var description = new StringBuilder();
            foreach (var node in element.Nodes())
            {
                if (node is XText)
                    description.Append(((XText)node).Value.Trim());

                if (node is XElement && ((XElement)node).Attribute("cref") != null)
                {
                    var reference = ((XElement)node).Attribute("cref").Value.Substring(2);
                    description.AppendFormat(" [{0}]({0}) ", reference);
                }
            }

            return description.ToString().Trim();
        }

        public static XElement GetSummary(this XElement element)
        {
            return (XElement)element.Nodes().FirstOrDefault(n => ((XElement)n).Name == "summary");
        }

        public static XElement GetReturns(this XElement element)
        {
            return (XElement)element.Nodes().FirstOrDefault(n => ((XElement)n).Name == "returns");
        }

        public static IEnumerable<XElement> GetParameters(this XElement element)
        {
            var elements = element.Nodes().Where(n => n is XElement && ((XElement)n).Name == "param").Select(n => (XElement)n);
            return elements;
        }

        public static string GetParameterName(this XElement element)
        {
            return element.Attribute("name").Value;
        }

    }


    class Program
    {
        static Assembly _assembly;
        static Dictionary<Type, List<string>> _baseTypesWithImplementations = new Dictionary<Type, List<string>>();
        static Dictionary<string, bool> _methodsOutputted = new Dictionary<string, bool>();

        static void Main(string[] args)
        {
            var path = @"c:\projects\bifrost\source\bifrost\bin\debug\bifrost.xml";
            var dllPath = @"c:\projects\bifrost\source\bifrost\bin\debug\bifrost.dll";

            _assembly = Assembly.LoadFile(dllPath);
            
            var document = XDocument.Load(path);
            var root = document.Root;

            var assemblyName = ((XElement)((XElement)root.Nodes().First()).Nodes().First()).Value;


            var outputPath = @"C:\Projects\Bifrost-Site\API\" + assemblyName;
            var members = (XElement)root.Nodes().FirstOrDefault(n => ((XElement)n).Name == "members");

            foreach (XElement member in members.Nodes())
            {
                var memberPath = string.Format("{0}\\{1}",outputPath, member.GetPath());
                Directory.CreateDirectory(memberPath);
                Directory.SetCurrentDirectory(memberPath);

                if (member.IsType()) OutputTypeSummary(member);
                if (member.IsMethod()) OutputMethodSummary(member);
            }
        }

        static void OutputMethodSummary(XElement member)
        {
            var fileName = member.GetFileName();
            var paths = new List<string>();
            var currentDirectory = Directory.GetCurrentDirectory();
            paths.Add(currentDirectory);

            var name = member.GetName();
            var fullName = member.GetFullName();
            
            var typeName = member.GetTypeName();
            var type = _assembly.GetType(typeName);

            if (type != null)
            {
                if (_baseTypesWithImplementations.ContainsKey(type))
                {
                    foreach (var path in _baseTypesWithImplementations[type])
                        paths.Add(path);
                }
            }

            var summary = member.GetSummary();
            var description = summary.GetDescription();
            var parameters = member.GetParameters();

            var builder = new StringBuilder();
            builder.AppendFormat("**{0}**\n",name);
            builder.AppendLine();
            builder.AppendLine(description);
            builder.AppendLine();

            if (parameters.Count() > 0)
            {
                builder.AppendLine("#Parameters#");
                builder.AppendLine();
                foreach (var parameter in parameters)
                {
                    builder.AppendFormat("\n##{0}##\n", parameter.GetParameterName());
                    builder.AppendLine(parameter.GetDescription());
                }
            }

            foreach (var path in paths)
            {
                Directory.SetCurrentDirectory(path);

                var writer = File.CreateText(fileName);
                writer.Write(builder.ToString());
                writer.Close();

                builder = new StringBuilder();
                builder.AppendLine();
                builder.AppendLine();
                if (!_methodsOutputted.ContainsKey(path))
                {
                    builder.AppendLine("**Methods**\n");
                    _methodsOutputted[path] = true;
                }
                builder.AppendFormat("[{0}]({1})\n", name, fullName);
                var dir = new DirectoryInfo(path);
                var typeSummaryFileName = Path.Combine(dir.FullName, string.Format("{0}.md", dir.Name));
                File.AppendAllText(typeSummaryFileName, builder.ToString());
            }


            Directory.SetCurrentDirectory(currentDirectory);
        }

        static void OutputTypeSummary(XElement member)
        {
            var name = member.GetFullName();
            var type = _assembly.GetType(name);

            if (type != null)
            {
                foreach (var baseType in type.GetInterfaces())
                {
                    List<string> paths = null;
                    if (_baseTypesWithImplementations.ContainsKey(baseType))
                        paths = _baseTypesWithImplementations[baseType];

                    if (paths == null)
                    {
                        paths = new List<string>();
                        _baseTypesWithImplementations[baseType] = paths;
                    }
                }
            }

            var fileName = member.GetFileName();
            var writer = File.CreateText(fileName);

            var summaryElement = (XElement)member.Nodes().First();
            var description = summaryElement.GetDescription();
            writer.Write(description);

            writer.Close();
        }
    }
}
