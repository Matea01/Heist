﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heist.Core.DTO
{
    public class HeistSkillDto
    {
        [Required(ErrorMessage = "Name is required")]
        public required string name { get; set; }

        [MaxLength(10)]
        [RegularExpression(@"^\*{0,10}$", ErrorMessage = "Level must consist of up to 10 asterisks.")]
        public string level { get; set; }
        public int members { get; set; }
    }
}
