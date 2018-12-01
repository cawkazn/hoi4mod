using System;
using System.Collections.Generic;
using System.Text;

namespace TagConverter.HelperModels
{
    public class IdeologyRules
    {
        public string name { get; set; }
        public string canDeclareOnSame { get; set; }
        public string canForceGovernment { get; set; }
        public string canGuaranteeOther { get; set; }
        public string canLowerTension { get; set; }
        public string canOnlyJustifyOnThreat { get; set; }
        public string canPuppet { get; set; }
        public string canSendVolunteers { get; set; }

        public IdeologyRules()
        {

        }

        public IdeologyRules(string name, string canDeclareOnSame, string canForceGovernment, string canGuaranteeOther, string canLowerTension, string canOnlyJustifyOnThreat, string canPuppet,
            string canSendVolunteers)
        {
            this.name = name;
            this.canDeclareOnSame = canDeclareOnSame;
            this.canGuaranteeOther = canGuaranteeOther;
            this.canLowerTension = canLowerTension;
            this.canOnlyJustifyOnThreat = canOnlyJustifyOnThreat;
            this.canPuppet = canPuppet;
            this.canSendVolunteers = canSendVolunteers;
        }
    }
}
