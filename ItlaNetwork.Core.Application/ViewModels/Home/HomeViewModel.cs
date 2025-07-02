using ItlaNetwork.Core.Application.ViewModels.Post;
using System.Collections.Generic;

namespace ItlaNetwork.Core.Application.ViewModels.Home
{
    public class HomeViewModel
    {
        public List<PostViewModel> Posts { get; set; }
        public SavePostViewModel NewPost { get; set; }

        // --- PROPIEDAD AÑADIDA ---
        // Usaremos esta propiedad para pasar el nombre de usuario a la vista del formulario.
        public string CurrentUserName { get; set; }
    }
}