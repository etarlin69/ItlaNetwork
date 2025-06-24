using System.Collections.Generic;

namespace ItlaNetwork.Core.Application.ViewModels.Friendship
{
    public class AddFriendViewModel
    {
        // Propiedad para el campo de búsqueda
        public string? UserName { get; set; }

        // Propiedad para guardar el ID del usuario seleccionado en el radio button
        public string? SelectedUserId { get; set; }

        public List<FriendViewModel> Users { get; set; }

        public AddFriendViewModel()
        {
            Users = new List<FriendViewModel>();
        }
    }
}