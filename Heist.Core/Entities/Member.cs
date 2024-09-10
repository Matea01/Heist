using Heist.Core.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Heist.Core.Entities
{
    [Table("Member")]
    public class Member
    {
        [Key]
        public int Id { get; set; }


        [Required]
        [MaxLength(100)]
        public string Name { get; set; }


        [Required]
        [EnumDataType(typeof(MemberSex), ErrorMessage = "Sex must be either 'M' or 'F'.")]
        public MemberSex Sex { get; set; }


        [Required]
        [EmailAddress]
        [MaxLength(50)]
        public string Email { get; set; }


        [Required]
        public List<MemberSkill> MemberSkills { get; set; } = new List<MemberSkill>();

        public string? MainSkill { get; set; }


        [MaxLength(20)]
        [EnumDataType(typeof(MemberStatus), ErrorMessage = "Invalid status value.")]
        public string Status { get; set; }

    }
}
