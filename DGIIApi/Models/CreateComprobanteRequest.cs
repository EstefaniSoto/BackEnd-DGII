using System.ComponentModel.DataAnnotations;

namespace DGIIApi.Models
{
    public class CreateComprobanteRequest
    {
        [Required]
        [StringLength(13)]
        public string RncCedula { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Monto { get; set; }
    }
}
