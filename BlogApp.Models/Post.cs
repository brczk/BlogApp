using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Models
{
    public class Post
    {
        public Post()
        {
            Comments = new HashSet<Comment>();
        }

        public Post(int id, string postTitle, string postAuthor, string postBody, int blogId)
        {
            Id = id;
            PostTitle = postTitle;
            PostAuthor = postAuthor;
            PostBody = postBody;
            BlogId = blogId;
            Comments = new HashSet<Comment>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string PostTitle { get; set; }
        [Required]
        [StringLength(50)]
        public string PostAuthor { get; set; }
        [Required]
        [StringLength(240)]
        public string PostBody { get; set; }
        [Required]
        [StringLength(240)]
        public string Category { get; set; }


        [ForeignKey(nameof(Blog))]
        public int BlogId { get; set; }
        [NotMapped]
        public virtual Blog Blog { get; set; }
        [NotMapped]
        public virtual ICollection<Comment> Comments { get; set; }
    }
}
