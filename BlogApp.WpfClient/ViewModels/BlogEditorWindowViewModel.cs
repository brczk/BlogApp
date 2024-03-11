using BlogApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.WpfClient.ViewModels
{
    public class BlogEditorWindowViewModel
    {
        public RestCollection<Blog> Blogs
        {
            get; set;
        }

        public BlogEditorWindowViewModel()
        {
            Blogs = new RestCollection<Blog>("http://localhost:5828/", "blog");
        }
    }
}
