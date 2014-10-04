using System.Collections.Generic;

namespace Web.Content.API
{
    public class RootAssembly
    {
        public string Type { get { return "Assembly"; } }
        public string Name { get; set; }

        public List<Namespace> Namespaces { get; set; }

        public RootAssembly()
        {
            Namespaces = new List<Namespace>();
        }
    }
}