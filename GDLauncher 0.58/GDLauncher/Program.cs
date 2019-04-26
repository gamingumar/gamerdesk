using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GDLauncher
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>

        public static cGameViewModel gvm = new cGameViewModel();
        public static TimeSpan playTime;
       
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());

            /*
            using (NotifyIcon icon = new NotifyIcon())
            {
                icon.Icon = new Icon("GD Icon.ico");
                icon.ContextMenu = new ContextMenu(new MenuItem[]{
                    new MenuItem("Open",(s,e) =>{new Form1().Show();}),
                    new MenuItem("Exit",(s,e) =>{Application.Exit();}),
                });

                icon.Visible = true;

                Application.Run();

                icon.Visible = false;
            }*/



            



        }









     









    }
}
