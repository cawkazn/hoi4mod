using System;
using System.Collections.Generic;
using System.Text;

namespace TagConverter.HelperModels
{
    public class IdeologyType
    {
        public string name { get; set; }
        public string type { get; set; }

        public IdeologyType()
        {

        }

        public IdeologyType(string name, string type)
        {
            this.name = name;
            this.type = type;
        }
    }
}
