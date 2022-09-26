using Microsoft.Toolkit.Helpers;
using System.Text.Json;

namespace GameShop.Services
{
    internal class JsonObjectSerializer : IObjectSerializer
    {
        public T Deserialize<T>(string value)
        {
            return JsonSerializer.Deserialize<T>(value);
        }

        public string Serialize<T>(T value)
        {
            return JsonSerializer.Serialize(value);
        }
    }
}
