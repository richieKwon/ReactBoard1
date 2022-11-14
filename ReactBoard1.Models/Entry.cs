using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReactBoard1.Models
{
    public class EntryBase
    {
        [Key]
        public long Id { get; set; }
        
        [MaxLength(255)]
        [Required(ErrorMessage = "Enter your name")]
        [Column(TypeName = "NVarchar(255)")]
        public string Name { get; set; }
        
        [MaxLength(255)]
        [Required(ErrorMessage = "Enter the title")]
        [Column(TypeName = "NVarchar(255)")]
        public string Title { get; set; }
        
        public string Content { get; set; }

        public DateTime? Created { get; set; }
    }
    
    [Table("Entries")]
    public class Entry : EntryBase
    {
        
    }
}