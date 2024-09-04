using Heist.Core.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heist.Core.Entities
{
    [Table("Member")]
    public class Member
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public required string Name { get; set; }

        [Required]
        public MemberSex Sex { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(255)]
        public required string Email { get; set; }

        public List<Skill> Skills { get; set; } = new List<Skill>();

        [MaxLength(100)]
        public string? MainSkill { get; set; }

        [Required]
        [MaxLength(20)]
        public required string Status { get; set; }

    }
}
