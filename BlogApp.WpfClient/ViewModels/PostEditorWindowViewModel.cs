﻿using BlogApp.Models;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BlogApp.WpfClient.ViewModels
{
    public class PostEditorWindowViewModel : ObservableRecipient
    {
        public RestCollection<Post> Posts
        {
            get; set;
        }

        public ICommand CreatePostCommand
        {
            get; set;
        }

        public ICommand DeletePostCommand
        {
            get; set;
        }

        public ICommand UpdatePostCommand
        {
            get; set;
        }

        private Post selectedPost;

        public Post SelectedPost
        {
            get { return selectedPost; }
            set
            {
                if (value != null)
                {
                    selectedPost = new Post()
                    {
                        Id = value.Id,
                        PostAuthor = value.PostAuthor,
                        Content = value.Content,
                        Category = value.Category,
                        BlogId = value.BlogId
                    };
                    OnPropertyChanged();
                    (DeletePostCommand as RelayCommand).NotifyCanExecuteChanged();
                }
            }
        }


        public PostEditorWindowViewModel()
        {
            Posts = new RestCollection<Post>("http://localhost:5828/", "post", "hub");
            CreatePostCommand = new RelayCommand(() =>
            {
                Posts.Add(new Post()
                {
                    PostAuthor = SelectedPost.PostAuthor,
                    Content = SelectedPost.Content,
                    Category = SelectedPost.Category,
                    BlogId = SelectedPost.BlogId,
                });
            });

            DeletePostCommand = new RelayCommand(() =>
            {
                Posts.Delete(SelectedPost.Id);
                SelectedPost = new Post();
            }, () => SelectedPost != null);

            UpdatePostCommand = new RelayCommand(() =>
            {
                Posts.Update(SelectedPost);
            });
            SelectedPost = new Post();
        }
    }
}
