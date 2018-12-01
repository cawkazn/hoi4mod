using System;
using System.Collections.Generic;
using System.Text;

namespace TagConverter.HelperModels
{
    public class IdeologyFaction
    {
        public string name { get; set; }
        public string factionName { get; set; }

        public IdeologyFaction()
        {

        }

        public IdeologyFaction(string name, string factionName)
        {
            this.name = name;
            this.factionName = factionName;
        }
    }
}
