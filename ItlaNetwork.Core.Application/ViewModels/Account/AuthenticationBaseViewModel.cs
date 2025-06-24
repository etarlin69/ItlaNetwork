namespace ItlaNetwork.Core.Application.ViewModels.Account
{
    // Esta clase contendrá las propiedades comunes para manejar errores.
    public class AuthenticationBaseViewModel
    {
        public bool HasError { get; set; }
        public string? Error { get; set; }
    }
}