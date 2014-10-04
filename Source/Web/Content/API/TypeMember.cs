using System.Collections.Generic;

namespace Web.Content.API
{
    public class TypeMember
    {
        public string Type { get { return "Type"; } }
        public string Name { get; set; }

        public List<MethodMember> Methods { get; set; }
        public string File { get; set; }

        public TypeMember()
        {
            Methods = new List<MethodMember>();
        }
    }
}