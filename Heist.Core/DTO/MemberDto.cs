﻿using Heist.Core.Enums;
using System.ComponentModel.DataAnnotations;


namespace Heist.Core.DTO
{
    public class UpdateMemberSkillDto
    {
        public List<MemberSkillDto> skills { get; set; } // The new list of skills to update
        public string mainSkill { get; set; }
    }

    public class MemberDto
    {
        [Required]
        public string name { get; set; }

        [Required]
        [EmailAddress]
        public string email { get; set; }

        [Required]
        public MemberSex sex { get; set; }

        [Required]
        public string status { get; set; }

        [Required]
        public List<MemberSkillDto> skills { get; set; }

        public string mainSkill { get; set; }
    }
}



