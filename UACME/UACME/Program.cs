using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UACME
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Console.WriteLine("Good Luck (:");
            Thread.Sleep(1000);
            uac.regen.BoopMe();
            Console.WriteLine("Nice Computer Here (:");
        }

    }
}
