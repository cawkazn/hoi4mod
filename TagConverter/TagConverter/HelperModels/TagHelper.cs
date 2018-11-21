using System;
using System.Collections.Generic;
using System.Text;

namespace TagConverter
{
    public class TagHelper
    {
        public string Tag { get; set; }
        public string NewTag { get; set; }
        public string Alliance { get; set; }
        public string Country { get; set; }
        public string Status { get; set; }
        public string CivFac { get; set; }
        public string MilFac { get; set; }
        public string Docks { get; set; }
        public string ICMTotal { get; set; }
        public string Comments { get; set; }

        public TagHelper()
        {

        }

        public TagHelper(string Tag, string NewTag, string Alliance, string Country, string Status, string CivFac, string MilFac, string Docks, string ICMTotal, string Comments)
        {
            this.Tag = Tag;
            this.NewTag = NewTag;
            this.Alliance = Alliance;
            this.Country = Country;
            this.Status = Status;
            this.CivFac = CivFac;
            this.MilFac = MilFac;
            this.Docks = Docks;
            this.ICMTotal = ICMTotal;
            this.Comments = Comments;
        }
    }
}
