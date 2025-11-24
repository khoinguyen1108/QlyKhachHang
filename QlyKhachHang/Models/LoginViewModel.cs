using System.ComponentModel.DataAnnotations;

namespace QlyKhachHang.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Tên ??ng nh?p ho?c email là b?t bu?c")]
        [StringLength(100)]
        [Display(Name = "Tên ??ng Nh?p ho?c Email")]
        public string UsernameOrEmail { get; set; } = string.Empty;

        [Required(ErrorMessage = "M?t kh?u là b?t bu?c")]
        [StringLength(100)]
        [DataType(DataType.Password)]
        [Display(Name = "M?t Kh?u")]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Nh? Tôi")]
        public bool RememberMe { get; set; }
    }
}
