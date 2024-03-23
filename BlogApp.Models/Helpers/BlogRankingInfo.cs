using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Models.Helpers
{
    public class BlogRankingInfo
    {
        public string BlogName { get; set; }
        public int TotalNumberOfComments { get; set; }

        public override bool Equals(object obj)
        {
            return obj is BlogRankingInfo info &&
                   BlogName == info.BlogName &&
                   TotalNumberOfComments == info.TotalNumberOfComments;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(BlogName, TotalNumberOfComments);
        }
    }
}
