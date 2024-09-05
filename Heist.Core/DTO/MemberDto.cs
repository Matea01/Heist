using Heist.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Heist.Core.DTO
{
    public class MemberDto
    {
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



