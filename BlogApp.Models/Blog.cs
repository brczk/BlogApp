using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogApp.Models
{
    public class Blog
    {
        public Blog() 
        {
            Posts = new HashSet<Post>();
        }

        public Blog(int id, string blogName)
        {
            Id = id;
            BlogName = blogName;
            Posts = new HashSet<Post>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [StringLength(240)]
        public string BlogName { get; set; }
        [NotMapped]
        public virtual ICollection<Post> Posts { get; set; }
    }
}
