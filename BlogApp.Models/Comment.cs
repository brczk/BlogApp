using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

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
        [JsonIgnore]
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

        public override string ToString()
        {
            StringBuilder stars = new StringBuilder();
            stars.Append('*', this.PostRating);
            return $"ID: {this.Id} - Post:{this.PostId} - Rating: {stars}\nContent: {this.Content}\n";
        }
    }
}