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
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PostId { get; set; }
        [Required]
        [StringLength(50)]
        public string PostTitle { get; set; }
        [Required]
        [StringLength(50)]
        public string PostAuthor { get; set; }
        [Required]
        [StringLength(240)]
        public string PostBody { get; set; }


        [ForeignKey(nameof(Blog))]
        public int BlogId { get; set; }
        public virtual Blog Blog { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}
