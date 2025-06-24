using System.ComponentModel.DataAnnotations;

namespace ItlaNetwork.Core.Application.ViewModels.Account
{
    // Ahora hereda de la clase base
    public class LoginViewModel : AuthenticationBaseViewModel
    {
        [Required(ErrorMessage = "Debe colocar el nombre de usuario")]
        [DataType(DataType.Text)]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Debe colocar una contraseña")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        // Las propiedades HasError y Error ahora vienen heredadas.
    }
}