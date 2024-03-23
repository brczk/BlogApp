using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Models.Helpers
{
    public class MostPopularPostInfo
    {
        public string BlogName { get; set; }
        public string MostPopularPostContent { get; set; }
        public int NumberOfComments { get; set; }

        public override bool Equals(object obj)
        {
            return obj is MostPopularPostInfo info &&
                   BlogName == info.BlogName &&
                   MostPopularPostContent == info.MostPopularPostContent &&
                   NumberOfComments == info.NumberOfComments;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(BlogName, MostPopularPostContent, NumberOfComments);
        }
    }
}
