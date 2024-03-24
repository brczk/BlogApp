using BlogApp.Models;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BlogApp.WpfClient.ViewModels
{
    public class BlogEditorWindowViewModel : ObservableRecipient
    {
        public RestCollection<Blog> Blogs
        {
            get; set;
        }

        public ICommand CreateBlogCommand
        {
            get; set;
        }

        public ICommand DeleteBlogCommand
        {
            get; set;
        }

        public ICommand UpdateBlogCommand
        {
            get; set;
        }

        private Blog selectedBlog;

        public Blog SelectedBlog
        {
            get { return selectedBlog; }
            set {
                if (value != null)
                {
                    selectedBlog = new Blog()
                    {
                        Id = value.Id,
                        BlogName = value.BlogName,
                        Posts = value.Posts
                    };
                    OnPropertyChanged();
                    (DeleteBlogCommand as RelayCommand).NotifyCanExecuteChanged();
                }  
            }
        }


        public BlogEditorWindowViewModel()
        {
            Blogs = new RestCollection<Blog>("http://localhost:5828/", "blog", "hub");
            CreateBlogCommand = new RelayCommand(() =>
            {
                Blogs.Add(new Blog()
                {
                    Id = Blogs.Max(x => x.Id) + 1,
                    BlogName = SelectedBlog.BlogName
                });
            });

            DeleteBlogCommand = new RelayCommand(() =>
            {
                Blogs.Delete(SelectedBlog.Id);
            }, () => SelectedBlog != null);

            UpdateBlogCommand = new RelayCommand(() =>
            {
                Blogs.Update(SelectedBlog);
            });
            SelectedBlog = new Blog();
        }
    }
}
