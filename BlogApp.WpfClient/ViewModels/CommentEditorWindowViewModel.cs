using BlogApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.WpfClient.ViewModels
{
    public class CommentEditorWindowViewModel
    {
        public RestCollection<Comment> Comments
        {
            get; set;
        }

        public CommentEditorWindowViewModel()
        {
            Comments = new RestCollection<Comment>("http://localhost:5828/", "comment");
        }
    }
}
