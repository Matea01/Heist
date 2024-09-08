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
    
    public class Skill
    {

        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public required string Name { get; set; }


        [MaxLength(10)]
        [RegularExpression(@"^\*{0,10}$", ErrorMessage = "Level must consist of up to 10 asterisks.")]
        public string Level { get; set; }
    }
}
