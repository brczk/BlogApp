using BlogApp.Models;
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
    public class CommentEditorWindowViewModel : ObservableRecipient
    {
        public RestCollection<Comment> Comments
        {
            get; set;
        }

        public ICommand CreateCommentCommand
        {
            get; set;
        }

        public ICommand DeleteCommentCommand
        {
            get; set;
        }

        public ICommand UpdateCommentCommand
        {
            get; set;
        }

        private Comment selectedComment;

        public Comment SelectedComment
        {
            get { return selectedComment; }
            set
            {
                if (value != null)
                {
                    selectedComment = new Comment()
                    {
                        PostId = value.PostId,
                        UserName = value.UserName,
                        Content = value.Content,
                        Id = value.Id,
                        PostRating = value.PostRating
                    };
                    OnPropertyChanged();
                    (DeleteCommentCommand as RelayCommand).NotifyCanExecuteChanged();
                }
            }
        }


        public CommentEditorWindowViewModel()
        {
            Comments = new RestCollection<Comment>("http://localhost:5828/", "comment", "hub");
            CreateCommentCommand = new RelayCommand(() =>
            {
                Comments.Add(new Comment()
                {
                    PostId = SelectedComment.PostId,
                    UserName = SelectedComment.UserName,
                    Content = SelectedComment.Content,
                    Id = Comments.Max(x => x.Id) + 1,
                    PostRating = SelectedComment.PostRating
                });
            });

            DeleteCommentCommand = new RelayCommand(() =>
            {
                Comments.Delete(SelectedComment.Id);
            }, () => SelectedComment != null);

            UpdateCommentCommand = new RelayCommand(() =>
            {
                Comments.Update(SelectedComment);
            });
            SelectedComment = new Comment();
        }
    }
}
