using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RepositoryLayer.Entity
{
    public class NoteEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NoteId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Reminder { get; set; }
        public string Colour { get; set; }
        public string Image { get; set; }
        public bool Archive { get; set; }
        public bool Pinned { get; set; }
        public bool Trash { get; set; }
        public DateTime Created { get; set; }
        public DateTime Edited { get; set; }

        [ForeignKey("User")]
        public long userId { get; set; }
    }
}
