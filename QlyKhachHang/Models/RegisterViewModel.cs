using System.ComponentModel.DataAnnotations;

namespace QlyKhachHang.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Tên khách hàng là b?t bu?c")]
        [StringLength(100, ErrorMessage = "Tên không ???c v??t quá 100 ký t?")]
        [Display(Name = "Tên Khách Hàng")]
        public string CustomerName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Tên ??ng nh?p là b?t bu?c")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Tên ??ng nh?p ph?i t? 3 ??n 50 ký t?")]
        [RegularExpression(@"^[a-zA-Z0-9_.-]+$", ErrorMessage = "Tên ??ng nh?p ch? ch?a ch?, s?, d?u g?ch, d?u ch?m")]
        [Display(Name = "Tên ??ng Nh?p")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email là b?t bu?c")]
        [EmailAddress(ErrorMessage = "Email không h?p l?")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "S? ?i?n tho?i là b?t bu?c")]
        [Phone(ErrorMessage = "S? ?i?n tho?i không h?p l?")]
        [Display(Name = "S? ?i?n Tho?i")]
        public string Phone { get; set; } = string.Empty;

        [StringLength(200)]
        [Display(Name = "??a Ch?")]
        public string Address { get; set; } = string.Empty;

        [StringLength(50)]
        [Display(Name = "Thành Ph?")]
        public string City { get; set; } = string.Empty;

        [StringLength(20)]
        [Display(Name = "Mã B?u ?i?n")]
        public string PostalCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "M?t kh?u là b?t bu?c")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "M?t kh?u ph?i t? 6 ??n 100 ký t?")]
        [DataType(DataType.Password)]
        [Display(Name = "M?t Kh?u")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Xác nh?n m?t kh?u là b?t bu?c")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "M?t kh?u xác nh?n không kh?p")]
        [Display(Name = "Xác Nh?n M?t Kh?u")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "B?n ph?i ??ng ý v?i ?i?u kho?n")]
        [Display(Name = "Tôi ??ng Ý V?i ?i?u Kho?n")]
        public bool AgreeToTerms { get; set; }
    }
}
