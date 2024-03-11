using BlogApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.WpfClient.ViewModels
{
    public class PostEditorWindowViewModel
    {
        public RestCollection<Post> Posts
        {
            get; set;
        }

        public PostEditorWindowViewModel()
        {
            Posts = new RestCollection<Post>("http://localhost:5828/", "post");
        }
    }
}
