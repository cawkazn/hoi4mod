using System;
using System.Collections.Generic;
using System.Text;

namespace TagConverter
{
    public class ProvinceHelper
    {
        public string stateFrom { get; set; }
        public string stateTo { get; set; }
        public string provinceId { get; set; }

        public ProvinceHelper()
        {

        }

        public ProvinceHelper(string stateFrom, string stateTo, string provinceId)
        {
            this.stateFrom = stateFrom;
            this.stateTo = stateTo;
            this.provinceId = provinceId;
        }
    }
}
