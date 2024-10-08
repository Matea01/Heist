﻿using System.ComponentModel.DataAnnotations;

namespace Heist.Core.DTO
{
    public class MemberSkillDto
    {
        [Required(ErrorMessage = "Name is required")]
        public required string name { get; set; }

        [MaxLength(10)]
        [RegularExpression(@"^\*{0,10}$", ErrorMessage = "Level must consist of up to 10 asterisks.")]
        public string? level { get; set; }
    }
}
