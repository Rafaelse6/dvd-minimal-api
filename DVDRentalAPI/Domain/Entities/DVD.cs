using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DVDRentalAPI.Domain.Entities
{
    public class DVD
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } = default!;

        [StringLength(150)]
        public string Title { get; set; } = default!;

        [StringLength(20)]
        public string Genre { get; set; } = default!;

        public int Duration { get; set; } = default!;

        public int Year { get; set; } = default!;
    }
}
