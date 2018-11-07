using System;
using System.Collections.Generic;
using System.Text;

namespace TagConverter
{
    public class TagHelper
    {
        public string oldTag { get; set; }
        public string newTag { get; set; }

        public TagHelper()
        {

        }

        public TagHelper(string oldTag, string newTag)
        {
            this.oldTag = oldTag;
            this.newTag = newTag;
        }
    }
}
