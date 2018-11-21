using System;
using System.Collections.Generic;
using System.Text;

namespace TagConverter
{
    public class CountryMergeHelper
    {
        public string stateId { get; set; }               
        public string countryFrom { get; set; }
        public string countryTo { get; set; }
        public string tagFrom { get; set; }
        public string tagTo { get; set; }
        public string addCore { get; set; }
        public string retainCore { get; set; }

        public CountryMergeHelper()
        {

        }

        public CountryMergeHelper(string stateId, string countryFrom, string countryTo, string tagFrom, string tagTo, string addCore, string retainCore)
        {
            this.stateId = stateId;
            this.countryFrom = countryFrom;
            this.countryTo = countryTo;
            this.tagFrom = tagFrom;
            this.tagTo = tagTo;
            this.addCore = addCore;
            this.retainCore = retainCore;
        }
    }
}
