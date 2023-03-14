using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShopOnline.Models
{
    public class ResetPassword
    {
        [Key]
        public long ID { get; set; }
        [Display(Name = "Ten dang nhap")]
        [Required(ErrorMessage = "Required")]
        public string Username { get; set; }

        [Display(Name = "Mat khau")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Do dai password it nhat 6 ky tu")]
        [Required(ErrorMessage = "Required")]
        public string Password { get; set; }

        
        [Display(Name = "Mat khau moi")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Do dai password it nhat 6 ky tu")]
        [Required(ErrorMessage = "Required")]
        public string NewPassword { get; set; }

        [Display(Name = "Xac nhan mat khau")]
        [Required(ErrorMessage = "Required")]
        [Compare("NewPassword", ErrorMessage = "Mật khẩu xác nhận không đúng")]
        public string ConfirmPassword { get; set; }
    }
}