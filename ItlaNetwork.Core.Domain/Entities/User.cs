namespace ItlaNetwork.Core.Domain.Entities
{
    // Esta es nuestra entidad de dominio pura. No tiene dependencias externas.
    public class User
    {
        public string Id { get; set; } // Usamos string para compatibilidad con el Id de Identity
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string NombreUsuario { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public string? FotoDePerfilURL { get; set; }
        public bool EmailConfirmado { get; set; }
    }
}