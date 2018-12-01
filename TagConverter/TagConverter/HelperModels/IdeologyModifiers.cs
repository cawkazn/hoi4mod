using System;
using System.Collections.Generic;
using System.Text;

namespace TagConverter.HelperModels
{
    public class IdeologyModifiers
    {
        public string name { get; set; }
        public string annexCostFactor { get; set; }
        public string generateWarGoalTension { get; set; }
        public string guaranteeTension { get; set; }
        public string joinFactionTension { get; set; }
        public string lendLeaseTension { get; set; }
        public string sendVolunteersTension { get; set; }
        public string driftDefenseFactor { get; set; }
        public string takeStatesCostFactor { get; set; }
        public string justifyWarGoalWhenInMajorWar { get; set; }

        public IdeologyModifiers()
        {

        }

        public IdeologyModifiers(string name, string annexCostFactor, string generateWarGoalTension, string guaranteeTension, string joinFactionTension, string lendLeaseTension,
            string sendVolunteersTension, string driftDefenseFactor, string takeStatesCostFactor, string justifyWarGoalWhenInMajorWar)
        {
            this.name = name;
            this.annexCostFactor = annexCostFactor;
            this.generateWarGoalTension = generateWarGoalTension;
            this.guaranteeTension = guaranteeTension;
            this.joinFactionTension = joinFactionTension;
            this.lendLeaseTension = lendLeaseTension;
            this.sendVolunteersTension = sendVolunteersTension;
            this.driftDefenseFactor = driftDefenseFactor;
            this.takeStatesCostFactor = takeStatesCostFactor;
            this.justifyWarGoalWhenInMajorWar = justifyWarGoalWhenInMajorWar;
        }
    }
}
