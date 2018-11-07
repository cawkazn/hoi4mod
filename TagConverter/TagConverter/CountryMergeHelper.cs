using System;
using System.Collections.Generic;
using System.Text;

namespace TagConverter
{
    public class CountryMergeHelper
    {
        public string tagToGetRidOf { get; set; }
        public string tagToMergeTo { get; set; }
        public int stateId { get; set; }

        public CountryMergeHelper()
        {

        }

        public CountryMergeHelper(string tagToGetRidOf, string tagToMergeTo, int stateId)
        {
            this.tagToGetRidOf = tagToGetRidOf;
            this.tagToMergeTo = tagToMergeTo;
            this.stateId = stateId;
        }
    }
}
