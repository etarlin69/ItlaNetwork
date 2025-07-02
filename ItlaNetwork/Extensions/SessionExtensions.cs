using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace ItlaNetwork.Extensions
{
    public static class SessionExtensions
    {
        // Método para guardar un objeto en la sesión (serializado como JSON)
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }

        // Método para obtener un objeto de la sesión (deserializado desde JSON)
        public static T Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : JsonSerializer.Deserialize<T>(value);
        }
    }
}