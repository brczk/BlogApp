using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BlogApp.Models
{
    public class Post
    {
        public Post()
        {
            Comments = new HashSet<Comment>();
        }

        public Post(int id, string postAuthor, string content, int blogId)
        {
            Id = id;
            PostAuthor = postAuthor;
            Content = content;
            BlogId = blogId;
            Comments = new HashSet<Comment>();
        }

        #region Database
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey(nameof(Blog))]
        public int BlogId { get; set; }
        #endregion

        #region Navigation
        [NotMapped]
        [JsonIgnore]
        public virtual Blog Blog { get; set; }
        [NotMapped]
        public virtual ICollection<Comment> Comments { get; set; }
        #endregion

        [Required]
        [StringLength(50)]
        public string PostAuthor { get; set; }

        [Required]
        [StringLength(240)]
        public string Content { get; set; }

        [Required]
        [StringLength(50)]
        public string Category { get; set; }
    }
}
