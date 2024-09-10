using System.ComponentModel.DataAnnotations;

namespace Heist.Core.Entities
{

    public class Skill
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public required string Name { get; set; }

    }
}
