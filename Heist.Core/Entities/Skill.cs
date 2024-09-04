using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heist.Core.Entities
{
   
    public class Skill
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public required string Name { get; set; }

        [MaxLength(10)]
        public string Level { get; set; } = "*";

        [ForeignKey("HeistMemberId")]
        public required Member HeistMember { get; set; }
        public int HeistMemberId { get; set; }
    }
}
