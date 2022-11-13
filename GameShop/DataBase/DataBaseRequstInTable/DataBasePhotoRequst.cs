using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;

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
            adapter.Fill(table);

            if (table.Rows.Count == 1)
            {
                count = int.Parse(command.ExecuteScalar().ToString());
            }

            command.Parameters.Clear();

            return count;
        }

        public static List<byte[]> ReadPhoto(int idProduct)
        {
            DataBaseConnect db = new DataBaseConnect();
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand command = new MySqlCommand();

            int CountPhoto = DataBasePhotoRequst.CountPhotoProduct(idProduct);

            if (CountPhoto == 0)
            {
                return null;
            }

            command = new MySqlCommand($"SELECT `PhotoProduct` FROM `photoproduct` WHERE `idProduct` = @idProduct", db.IsConnection());

            command.Parameters.Add(new MySqlParameter("@idProduct", MySqlDbType.Int32));
            command.Parameters["@idProduct"].Value = idProduct;

            adapter.SelectCommand = command;
            adapter.Fill(table);

            List<byte[]> PhotoBytes = new List<byte[]>();

            if (table.Rows.Count > 0)
            {
                MySqlDataReader readerBy = command.ExecuteReader();
                for (int i = 0; readerBy.Read(); i++)
                {
                    byte[] Photo = readerBy["PhotoProduct"] as byte[];

                    PhotoBytes.Add(Photo);
                }

                command.Parameters.Clear();
            }

            return PhotoBytes;
        }

        public static void SavePhoto(byte[] Photo, int idProduct)
        {
            DataBaseConnect db = new DataBaseConnect();
            MySqlCommand command = new MySqlCommand();

            command = new MySqlCommand($"INSERT INTO `photoproduct` (`idProduct`, `PhotoProduct`) VALUES (@idProduct, @PhotoProduct)", db.IsConnection());

            command.Parameters.Add(new MySqlParameter("@idProduct", MySqlDbType.Int32));
            command.Parameters["@idProduct"].Value = idProduct;

            command.Parameters.Add(new MySqlParameter("@PhotoProduct", MySqlDbType.Blob));
            command.Parameters["@PhotoProduct"].Value = Photo;

            command.ExecuteNonQuery();

            command.Parameters.Clear();
        }

        public static bool SavePhoto(List<byte[]> Photo, int idProduct)
        {
            DataBaseConnect db = new DataBaseConnect();
            MySqlCommand command = new MySqlCommand();

            for (int i = 0; i < Photo.Count; i++)
            {
                command = new MySqlCommand($"INSERT INTO `photoproduct` (`idProduct`, `PhotoProduct`) VALUES (@idProduct, @PhotoProduct)", db.IsConnection());

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
    }
}

