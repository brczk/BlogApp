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

        public Blog(int id, string blogName, string url)
        {
            Id = id;
            BlogName = blogName;
            URL = url;
            Posts = new HashSet<Post>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [StringLength(240)]
        public string BlogName { get; set; }
        [Required]
        [StringLength(100)]
        public string URL { get; set;}
        [NotMapped]
        public virtual ICollection<Post> Posts { get; set; }
    }
}
