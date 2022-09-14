using GameShop.Activation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Controls;

namespace GameShop.File
{
    public class FileInputandOutput
    {
        //private static int count;
        //private static int MaxCount = 10;
        //private static bool auth;


        //public static bool LoginCheck(bool stat = false)
        //{
        //    return stat;
        //}
        //public static async void InputFile(StorageFolder storageFolder, StorageFile sampleFile, string Login, string Password, int count = 1)
        //{
        //    await FileIO.WriteTextAsync(sampleFile, $"{Login} {Password} {count}");
        //}

        //public static async void CheckDataFile(StorageFolder storageFolder, StorageFile sampleFile, string LoginLog, string PasswordLog)
        //{
        //    LoginActivationHandler loginhandler = new LoginActivationHandler();
        //    var Data = await FileIO.ReadLinesAsync(sampleFile);
        //    count = int.Parse(Data.ToString());

        //    if (count < MaxCount && count > 0)
        //    {
        //        auth = false;
        //        count++;
        //        InputFile(storageFolder, sampleFile, LoginLog, PasswordLog,  count);
        //        loginhandler.CanHandle(auth);
        //    }
        //    else if (count >= MaxCount)
        //    {
        //        auth = true;
        //        InputFile(storageFolder, sampleFile, LoginLog, PasswordLog, 0);
        //        loginhandler.CanHandle(auth);
        //    }
        //    else if (count == 0)
        //    {
        //        if (LoginCheck())
        //        {
        //            auth = false;
        //            InputFile(storageFolder, sampleFile, LoginLog, PasswordLog);
        //            loginhandler.CanHandle(auth);
        //        }
        //    }
        // }

    }
}
