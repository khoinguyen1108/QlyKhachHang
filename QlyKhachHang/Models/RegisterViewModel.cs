using System.ComponentModel.DataAnnotations;

namespace QlyKhachHang.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Tên khách hàng là bắt buộc")]
        [StringLength(100, ErrorMessage = "Tên không được vượt quá 100 ký tự")]
        [Display(Name = "Tên Khách Hàng")]
        public string CustomerName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Tên đăng nhập là bắt buộc")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Tên đăng nhập phải từ 3 đến 50 ký tự")]
        [RegularExpression(@"^[a-zA-Z0-9_.-]+$", ErrorMessage = "Tên đăng nhập chỉ chứa chữ, số, dấu gạch, dấu chấm")]
        [Display(Name = "Tên Đăng Nhập")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email là bắt buộc")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Số điện thoại là bắt buộc")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        [Display(Name = "Số Điện Thoại")]
        public string Phone { get; set; } = string.Empty;

        [StringLength(200)]
        [Display(Name = "Địa Chỉ")]
        public string Address { get; set; } = string.Empty;

        [StringLength(50)]
        [Display(Name = "Thành Phố")]
        public string City { get; set; } = string.Empty;

        [StringLength(20)]
        [Display(Name = "Mã Bưu Điện")]
        public string PostalCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mật khẩu là bắt buộc")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Mật khẩu phải từ 6 đến 100 ký tự")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật Khẩu")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Xác nhận mật khẩu là bắt buộc")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Mật khẩu xác nhận không khớp")]
        [Display(Name = "Xác Nhận Mật Khẩu")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Bạn phải đồng ý với điều khoản")]
        [Display(Name = "Tôi Đồng Ý Với Điều Khoản")]
        public bool AgreeToTerms { get; set; }
    }
}
