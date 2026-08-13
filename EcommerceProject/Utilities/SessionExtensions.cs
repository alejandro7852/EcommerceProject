using System.Text.Json;

namespace EcommerceProject.Utilities
{
    public static class SessionExtensions
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key,JsonSerializer.Serialize(value));
        }
        public static T Get<T>(this ISession session, string key)
        {
            var valor = session.GetString(key);
            return valor == null ? default : JsonSerializer.Deserialize<T>(valor);
        }
    }
}
