using System.ComponentModel.DataAnnotations;

namespace ItlaNetwork.Core.Application.ViewModels.Account
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Debe colocar su nombre de usuario o correo")]
        [DataType(DataType.Text)]
        public string UserNameOrEmail { get; set; }

        [Required(ErrorMessage = "Debe colocar una contraseña")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        // Las propiedades HasError y Error han sido eliminadas para simplificar
        // y usar el sistema de validación estándar de ASP.NET Core.
    }
}