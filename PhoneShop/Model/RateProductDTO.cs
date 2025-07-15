using System.ComponentModel.DataAnnotations;

namespace PhoneShop.Model
{
    public class RateProductDTO
    {
        public int ProductId { get; set; }
        [Range(1, 5, ErrorMessage = "Rate must be between 1 and 5.")]
        public double Rate { get; set; }
    }
}
