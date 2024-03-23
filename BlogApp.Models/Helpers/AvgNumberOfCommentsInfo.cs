using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Models.Helpers
{
    public class AvgNumberOfCommentsInfo
    {
        public string BlogName { get; set; }
        public double AvgNumberOfComments { get; set; }

        public override bool Equals(object obj)
        {
            return obj is AvgNumberOfCommentsInfo info &&
                   BlogName == info.BlogName &&
                   AvgNumberOfComments == info.AvgNumberOfComments;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(BlogName, AvgNumberOfComments);
        }
    }
}
