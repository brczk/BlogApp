using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Models.Helpers
{
    public class CategoryPostCountInfo
    {
        public string CategoryName { get; set; }
        public int CategoryCount { get; set; }

        public override bool Equals(object obj)
        {
            return obj is CategoryPostCountInfo info &&
                   CategoryName == info.CategoryName &&
                   CategoryCount == info.CategoryCount;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(CategoryName, CategoryCount);
        }
    }
}
