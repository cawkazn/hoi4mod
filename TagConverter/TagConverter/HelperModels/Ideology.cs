using System;
using System.Collections.Generic;
using System.Text;

namespace TagConverter.HelperModels
{
    public class Ideology
    {
        public IdeologyProperties properties { get; set; }
        public List<IdeologyType> types { get; set; }
        public List<IdeologyFaction> factions { get; set; }
        public IdeologyRules rules { get; set; }
        public IdeologyModifiers modifiers { get; set; }

        public Ideology()
        {

        }

        public Ideology(IdeologyProperties properties, List<IdeologyType> types, List<IdeologyFaction> factions, IdeologyRules rules, IdeologyModifiers modifiers)
        {
            this.properties = properties;
            this.types = types;
            this.factions = factions;
            this.rules = rules;
            this.modifiers = modifiers;
        }
    }
}
