using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace UACME.uac
{
    internal class Settings
    {
        [DllImport("shell32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsUserAnAdmin();

        public static void CheckA()
        {
            Console.WriteLine(IsUserAnAdmin());

            //Console.ReadKey(); //
        }
    }
}
