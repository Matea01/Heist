using Heist.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Heist.Core.DTO
{
    public class UpdateMemberSkillDto
    {
        public int MemberId { get; set; } // Id of the member
        public List<SkillDto> Skills { get; set; } // The new list of skills to update
    }
    public class MemberDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public  string Email { get; set; }

        [Required]
        public MemberSex Sex { get; set; }

        [Required]
        public string Status { get; set; }

        [Required]
        public  List<SkillDto> Skills { get; set; }

        public int? MainSkillId { get; set; } // Optional reference to main skill


    }
}



