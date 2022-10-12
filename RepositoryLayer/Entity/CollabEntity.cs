using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RepositoryLayer.Entity
{
    public class CollabEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long CollabId { get; set; }
        public long Sender_UserId { get; set; }
        public long Receiver_UserId { get; set; }
        //public string Sender_Email { get; set; }
        public string Receiver_Email { get; set; }

        [ForeignKey("NoteEntity")]
        public long NoteId { get; set; }

    }
}
