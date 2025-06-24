using ItlaNetwork.Core.Application.ViewModels.Post;
using System.Collections.Generic;

namespace ItlaNetwork.Core.Application.ViewModels.Home
{
    public class HomeViewModel
    {
        // Lista de todas las publicaciones para mostrar en el muro
        public List<PostViewModel> Posts { get; set; }

        // Propiedad para el formulario de crear una nueva publicación
        public SavePostViewModel NewPost { get; set; }
    }
}