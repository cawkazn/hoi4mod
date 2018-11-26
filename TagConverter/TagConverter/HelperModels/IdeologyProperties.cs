using System;
using System.Collections.Generic;
using System.Text;

namespace TagConverter.HelperModels
{
    public class IdeologyProperties
    {
        public string name { get; set; }
        public string color { get; set; }
        public string warImpactOnTension { get; set; }
        public string factionImpactOnTension { get; set; }
        public string factionTradeOpinion { get; set; }
        public string aiType { get; set; }

        public IdeologyProperties()
        {

        }

        public IdeologyProperties(string name, string color, string warImpactOnTension, string factionImpactOnTension, string factiontTradeOpinion, string aiType)
        {
            this.name = name;
            this.color = color;
            this.warImpactOnTension = warImpactOnTension;
            this.factionImpactOnTension = factionImpactOnTension;
            this.factionTradeOpinion = factionTradeOpinion;
            this.aiType = aiType;
        }
    }
}
