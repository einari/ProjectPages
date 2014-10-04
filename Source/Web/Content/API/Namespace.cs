using System.Collections.Generic;

namespace Web.Content.API
{
    public class Namespace
    {
        public string Type { get { return "Namespace"; } }
        public string Name { get; set; }

        public List<TypeMember> Members { get; set; }
        public List<Namespace> Namespaces { get; set; }

        public Namespace()
        {
            Members = new List<TypeMember>();
            Namespaces = new List<Namespace>();
        }
    }
}