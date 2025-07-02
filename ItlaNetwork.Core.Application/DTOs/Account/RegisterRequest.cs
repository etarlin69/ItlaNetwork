namespace ItlaNetwork.Core.Application.DTOs.Account
{
    public class RegisterRequest
    {
        // --- PROPIEDADES AÑADIDAS PARA SOLUCIONAR EL ERROR ---
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        // --- FIN DE LAS PROPIEDADES AÑADIDAS ---

        // Propiedades que ya tenías
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
