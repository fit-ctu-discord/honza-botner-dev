using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HonzaBotner.Database
{
    public class Warning
    {
        [Key]
        public int Id { get; set; }
        public ulong UserId { get; set; }
        public string Reason { get; set; }
        public DateTime IssuedAt { get; set; } = DateTime.Now;
    }
}
