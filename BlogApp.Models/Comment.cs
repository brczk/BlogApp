﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogApp.Models
{
    public class Comment
    {
        public Comment()
        {
        }

        public Comment(int id, string userName, string content, int postId)
        {
            Id = id;
            UserName = userName;
            Content = content;
            PostId = postId;
        }

        #region Database
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey(nameof(Post))]
        public int PostId { get; set; }
        #endregion

        #region Navigation
        [NotMapped]
        public virtual Post Post { get; set; }
        #endregion

        [Required]
        [StringLength(50)]
        public string UserName { get; set; }

        [Required]
        [StringLength(240)]
        public string Content { get; set; }

        [Required]
        [Range(1, 10)]
        public int PostRating { get; set; }
    }
}