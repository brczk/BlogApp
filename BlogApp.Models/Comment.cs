using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogApp.Models
{
    public class Comment
    {
        public Comment()
        {
        }

        public Comment(int id, string userName, string commentBody, int postId)
        {
            Id = id;
            UserName = userName;
            CommentBody = commentBody;
            PostId = postId;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string UserName { get; set; }
        [Required]
        [StringLength(240)]
        public string CommentBody { get; set; }

        [ForeignKey(nameof(Post))]
        public int PostId { get; set; }
        [Required]
        [Range(1, 10)]
        public int PostRating { get; set; }
        [NotMapped]
        public virtual Post Post { get; set; }
    }
}