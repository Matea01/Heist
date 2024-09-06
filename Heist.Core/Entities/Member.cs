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


        [Required] // The Name field is required
        [MaxLength(100)]
        public string Name { get; set; }


        [Required]
        [EnumDataType(typeof(MemberSex), ErrorMessage = "Sex must be either 'M' or 'F'.")]
        public MemberSex Sex { get; set; }


        [Required]
        [EmailAddress]
        [MaxLength(50)]
        public  string Email { get; set; }


        [Required]
        public List<Skill> Skills { get; set; } = new List<Skill>();// initialize to avoid null ref exception


        [MaxLength(100)]
        [RegularExpression(@"^\*{1,10}$", ErrorMessage = "Level must be between 1 and 10 asterisk characters.")]

        //public string? MainSkill { get; set; }

        // Optional reference to one of the skills from the skills list

        [ForeignKey("MainSkillId")]
        public Skill? MainSkill { get; set; }
        public int? MainSkillId { get; set; }


        [MaxLength(20)]

        [EnumDataType(typeof(MemberStatus), ErrorMessage = "Invalid status value.")]
        public  string Status { get; set; }

    }
}
