using System.ComponentModel.DataAnnotations;

namespace ServerApp.DTOs
{
    public class UserForLoginDTO
    {
        [Required(ErrorMessage = "Kullanıcı Adı gerekli bir alan")]
        public string UserName { get; set; }  

        [Required(ErrorMessage = "Şifre gerekli bir alan")]
        public string Password { get; set; }
    }
}