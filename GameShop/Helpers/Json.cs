using System.Text.Json;
using System.Threading.Tasks;

namespace GameShop.Helpers
{
    /// <summary>
    /// Class to manage json object
    /// </summary>
    public static class Json
    {
        /// <summary>
        /// Deserialize object from json 
        /// </summary>
        /// <typeparam name="T">Type, that json will be converted back</typeparam>
        /// <param name="value">Obejct</param>
        /// <returns>Objcet of type T</returns>
        public static async Task<T> ToObjectAsync<T>(string value) => await Task.Run(() => JsonSerializer.Deserialize<T>(value));

        /// <summary>
        /// Serialize object to Json
        /// </summary>
        /// <param name="value">Object to serialize</param>
        /// <returns>Json as string</returns>
        public static async Task<string> StringifyAsync(object value) => await Task.Run(() => JsonSerializer.Serialize(value));
    }
}
