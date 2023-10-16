using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogApp.Models
{
    public class Blog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BlogId { get; set; }
        [Required]
        [StringLength(240)]
        public string BlogName { get; set; }
        [Required]
        [StringLength(100)]
        public string URL { get; set;}
        public virtual ICollection<Post> Posts { get; set; }
    }
}
