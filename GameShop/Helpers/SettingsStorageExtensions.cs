using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;

namespace GameShop.Helpers
{
    /// <summary>
    /// Use these extension methods to store and retrieve local and roaming app data
    /// <para>More details regarding storing and retrieving app data <a href="https://docs.microsoft.com/windows/uwp/app-settings/store-and-retrieve-app-data">here</a>.</para>
    /// </summary>
    public static class SettingsStorageExtensions
    {
        private const string FileExtension = ".json";

        /// <summary>
        /// Checks if storage is available
        /// </summary>
        /// <returns>True, if available</returns>
        public static bool IsRoamingStorageAvailable(this ApplicationData appData) => appData.RoamingStorageQuota == 0;

        /// <summary>
        /// Concat file name and default file extension
        /// </summary>
        /// <param name="name">File name</param>
        /// <returns>Concated string</returns>
        private static string GetFileName(string name) => String.Concat(name, FileExtension);

        /// <summary>
        /// Serialize object <paramref name="content"/> and save it into: 
        /// <para>Folder: <see cref="StorageFolder"/></para>
        /// <para>File name: <paramref name="name"/></para>
        /// </summary>
        /// <typeparam name="T">Type of object ot serialize</typeparam>
        /// <param name="folder">Current app folder</param>
        /// <param name="name">File name</param>
        /// <param name="content">Object to serialize</param>
        /// <returns></returns>
        public static async Task SaveAsync<T>(this StorageFolder folder, string name, T content)
        {
            var file = await folder.CreateFileAsync(GetFileName(name), CreationCollisionOption.ReplaceExisting);
            var fileContent = await Json.StringifyAsync(content);

            await FileIO.WriteTextAsync(file, fileContent);
        }

        /// <summary>
        /// Deserialize objcet as type <typeparamref name="T"/> from:
        /// <para>Folder: <see cref="StorageFolder"/></para>
        /// <para>File name: <paramref name="name"/></para>
        /// </summary>
        /// <typeparam name="T">Type of object ot deserialize</typeparam>
        /// <param name="folder">Current app folder</param>
        /// <param name="name">File name</param>
        /// <returns>Deserialized object of type: <typeparamref name="T"/></returns>
        public static async Task<T> ReadAsync<T>(this StorageFolder folder, string name)
        {
            if (System.IO.File.Exists(Path.Combine(folder.Path, GetFileName(name))))
            {
                var file = await folder.GetFileAsync(GetFileName(name));
                var fileContent = await FileIO.ReadTextAsync(file);

                return await Json.ToObjectAsync<T>(fileContent);
            }
            return default;
        }

        /// <summary>
        /// Get value from: <see cref="ApplicationDataContainer"/> with key: <paramref name="key"/>
        /// </summary>
        /// <typeparam name="T">Type of object ot deserialize</typeparam>
        /// <param name="settings">Application internal storage</param>
        /// <param name="key">Key of value in app data container</param>
        /// <returns>Deserialized object of type: <typeparamref name="T"/></returns>
        public static async Task<T> ReadAsync<T>(this ApplicationDataContainer settings, string key)
        {
            object obj = null;

            if (settings.Values.TryGetValue(key, out obj))
            {
                return await Json.ToObjectAsync<T>((string)obj);
            }

            return default;
        }

        /// <summary>
        /// Add <paramref name="value"/> to <see cref="ApplicationDataContainer"/> with key: <paramref name="key"/>
        /// </summary>
        /// <param name="settings">Application internal storage</param>
        /// <param name="key">Key of value in app data container</param>
        /// <param name="value">Value ot save</param>
        public static void SaveString(this ApplicationDataContainer settings, string key, string value) => settings.Values[key] = value;

        /// <summary>
        /// Add <paramref name="value"/> as serialized type: <typeparamref name="T"/> to <see cref="ApplicationDataContainer"/> with key: <paramref name="key"/>
        /// </summary>
        /// <typeparam name="T">Type of object to serialize</typeparam>
        /// <param name="settings">Application internal storage</param>
        /// <param name="key">Key of value in app data container</param>
        /// <param name="value">Value ot save</param>
        /// <returns>Completed Task</returns>
        public static async Task SaveAsync<T>(this ApplicationDataContainer settings, string key, T value) => settings.SaveString(key, await Json.StringifyAsync(value));

        /// <summary>
        /// Create new <see cref="StorageFile"/> with name: <paramref name="fileName"/> and writes <paramref name="content"/> there 
        /// </summary>
        /// <param name="folder">Current app folder</param>
        /// <param name="content">Data to write into file</param>
        /// <param name="fileName">File name</param>
        /// <param name="option">Creation file option</param>
        /// <returns>Created storrage file</returns>
        public static async Task<StorageFile> SaveFileAsync(this StorageFolder folder, byte[] content, string fileName, CreationCollisionOption option = CreationCollisionOption.ReplaceExisting)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            if (String.IsNullOrEmpty(fileName))
            {
                throw new ArgumentException($"File name is null or empty. specify a valid name", nameof(fileName));
            }

            var storageFile = await folder.CreateFileAsync(fileName, option);
            await FileIO.WriteBytesAsync(storageFile, content);
            return storageFile;
        }

        /// <summary>
        /// Read file with name <paramref name="fileName"/> as byte array
        /// </summary>
        /// <param name="folder">Current app folder</param>
        /// <param name="fileName">File name</param>
        /// <returns>Byte array</returns>
        public static async Task<byte[]> ReadFileAsync(this StorageFolder folder, string fileName)
        {
            var item = await folder.TryGetItemAsync(fileName).AsTask().ConfigureAwait(false);

            if ((item != null) && item.IsOfType(StorageItemTypes.File))
            {
                var storageFile = await folder.GetFileAsync(fileName);
                byte[] content = await storageFile.ReadBytesAsync();
                return content;
            }

            return null;
        }

        /// <summary>
        /// Read file as byte array
        /// </summary>
        /// <param name="file">Current file</param>
        /// <returns>Byte array</returns>
        public static async Task<byte[]> ReadBytesAsync(this StorageFile file)
        {
            if (file == null)
            {
                throw new ArgumentNullException("File if null. Please, initialize the parameter", nameof(file));
            }

            using (IRandomAccessStream stream = await file.OpenReadAsync())
            {
                using (var reader = new DataReader(stream.GetInputStreamAt(0)))
                {
                    await reader.LoadAsync((uint)stream.Size);
                    var bytes = new byte[stream.Size];
                    reader.ReadBytes(bytes);
                    return bytes;
                }
            }
        }
    }
}
