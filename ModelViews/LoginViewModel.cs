using System;
using System.ComponentModel.DataAnnotations;

namespace BookShop.ModelViews
{
    public class LoginViewModel
    {
        [Key]

        public string Email2 { get; set; }
        public string Password { get; set; }
        public bool KeepLoggedIn { get; set; }
    }
}
