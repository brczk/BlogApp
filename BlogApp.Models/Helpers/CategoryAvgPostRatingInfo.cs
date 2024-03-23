using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Models.Helpers
{
    public class CategoryAvgPostRatingInfo
    {

        public string CategoryName { get; set; }
        public double CategoryAvgPostRating { get; set; }

        public override bool Equals(object obj)
        {
            return obj is CategoryAvgPostRatingInfo info &&
                   CategoryName == info.CategoryName &&
                   CategoryAvgPostRating == info.CategoryAvgPostRating;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(CategoryName, CategoryAvgPostRating);
        }
    }
}
