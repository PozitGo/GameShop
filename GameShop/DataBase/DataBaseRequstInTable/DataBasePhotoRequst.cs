using GameShop.Convert;
using GameShop.Model;
using MySql.Data.MySqlClient;
using NPOI.SS.Formula.Functions;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace GameShop.DataBase.DataBaseRequstInTable
{
    public struct DataBasePhotoRequst
    {
        public static int CountPhotoProduct(int idProduct)
        {

            DataBaseConnect db = new DataBaseConnect();
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand command = new MySqlCommand();
            int count = 0;

            command = new MySqlCommand($"SELECT COUNT(*) FROM `photoproduct` WHERE `idProduct` = @idProduct", db.IsConnection());

            command.Parameters.Add(new MySqlParameter("@idProduct", MySqlDbType.Int32));
            command.Parameters["@idProduct"].Value = idProduct;

            adapter.SelectCommand = command;
            adapter.FillAsync(table);

            if (table.Rows.Count == 1)
            {
                count = int.Parse(command.ExecuteScalar().ToString());
            }

            command.Parameters.Clear();

            return count;
        }

        public static int MaxIdPhoto()
        {
            DataBaseConnect db = new DataBaseConnect();
            MySqlCommand command = new MySqlCommand();

            command = new MySqlCommand($"SELECT MAX(`idPhoto`) FROM `photoproduct`", db.IsConnection());

            try
            {
                return (int)command.ExecuteScalar();
            }
            catch (System.Exception)
            {

                return 0;
            }
        }

        public static List<PhotoProduct> ReadPhoto(int idProduct)
        {
            DataBaseConnect db = new DataBaseConnect();
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand command = new MySqlCommand();

            List<PhotoProduct> PhotoProduct = new List<PhotoProduct>();
            int CountPhoto = DataBasePhotoRequst.CountPhotoProduct(idProduct);

            if (CountPhoto == 0)
            {
                return null;
            }

            command = new MySqlCommand($"SELECT * FROM `photoproduct` WHERE `idProduct` = @idProduct", db.IsConnection());
            command.Parameters.Add(new MySqlParameter("@idProduct", MySqlDbType.Int32));
            command.Parameters["@idProduct"].Value = idProduct;

            adapter.SelectCommand = command;
            adapter.Fill(table);


            if (table.Rows.Count > 0)
            {
                MySqlDataReader readerBy = command.ExecuteReader();
                for (int i = 0; readerBy.Read(); i++)
                {
                    PhotoProduct photoProduct = new PhotoProduct();
                    byte[] tempByte = readerBy["PhotoProduct"] as byte[];
                    photoProduct.BitPhotoProducts = new List<Windows.UI.Xaml.Media.Imaging.BitmapImage>();
                    photoProduct.BitPhotoProducts.Add(ImageConverter.GetBitmapAsync(tempByte));
                    photoProduct.idPhoto = int.Parse(readerBy["idPhoto"].ToString());
                    photoProduct.idProduct = int.Parse(readerBy["idProduct"].ToString());

                    PhotoProduct.Add(photoProduct);
                }

                command.Parameters.Clear();
            }

            return PhotoProduct;
        }

        public static bool SavePhoto(byte[] Photo, int idProduct)
        {
            DataBaseConnect db = new DataBaseConnect();
            MySqlCommand command = new MySqlCommand();

            command = new MySqlCommand($"INSERT INTO `photoproduct` (`idPhoto`, `idProduct`, `PhotoProduct`) VALUES (@idPhoto, @idProduct, @PhotoProduct)", db.IsConnection());

            command.Parameters.Add(new MySqlParameter("@idPhoto", MySqlDbType.Int32));
            command.Parameters["@idPhoto"].Value = MaxIdPhoto() + 1;

            command.Parameters.Add(new MySqlParameter("@idProduct", MySqlDbType.Int32));
            command.Parameters["@idProduct"].Value = idProduct;

            command.Parameters.Add(new MySqlParameter("@PhotoProduct", MySqlDbType.Blob));
            command.Parameters["@PhotoProduct"].Value = Photo;

            try
            {
                command.ExecuteNonQuery();

                command.Parameters.Clear();
            }
            catch (System.Exception)
            {
                return true;
            }

            return false;
        }

        public static bool SavePhoto(List<byte[]> Photo, int idProduct)
        {
            DataBaseConnect db = new DataBaseConnect();
            MySqlCommand command = new MySqlCommand();

            for (int i = 0; i < Photo.Count; i++)
            {
                command = new MySqlCommand($"INSERT INTO `photoproduct` (`idPhoto`, `idProduct`, `PhotoProduct`) VALUES (@idPhoto, @idProduct, @PhotoProduct)", db.IsConnection());

                command.Parameters.Add(new MySqlParameter("@idPhoto", MySqlDbType.Int32));
                command.Parameters["@idPhoto"].Value = MaxIdPhoto() + 1;

                command.Parameters.Add(new MySqlParameter("@idProduct", MySqlDbType.Int32));
                command.Parameters["@idProduct"].Value = idProduct;

                command.Parameters.Add(new MySqlParameter("@PhotoProduct", MySqlDbType.Blob));
                command.Parameters["@PhotoProduct"].Value = Photo[i];

                try
                {
                    command.ExecuteNonQuery();
                    command.Parameters.Clear();
                }
                catch (System.Exception)
                {

                    return false;
                }

            }

            return true;
        }

        public static void DeletePhoto(int idPhoto)
        {
            DataBaseConnect db = new DataBaseConnect();
            MySqlCommand command = new MySqlCommand();

            command = new MySqlCommand($"DELETE FROM `photoproduct` WHERE `idPhoto` = @idPhoto", db.IsConnection());

            command.Parameters.Add(new MySqlParameter("@idPhoto", MySqlDbType.Int32));
            command.Parameters["@idPhoto"].Value = idPhoto;

            command.ExecuteNonQuery();

            command.Parameters.Clear();
        }
        
        public static bool CheckPhoto(int idPhoto)
        {
            DataBaseConnect db = new DataBaseConnect();
            MySqlCommand command = new MySqlCommand();
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            
            command = new MySqlCommand($"SELECT * FROM `photoproduct` WHERE `idPhoto` = @idPhoto", db.IsConnection());
            command.Parameters.Add(new MySqlParameter("@idPhoto", MySqlDbType.Int32));
            command.Parameters["@idPhoto"].Value = idPhoto;

            adapter.SelectCommand = command;
            adapter.Fill(table);

            if (table.Rows.Count > 0)
                return true;
            
            return false;
        }
    }
}

