using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GDLauncher
{
    public partial class Form1 : Form
    {

        RegistryKey rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);



        public static cGameViewModel gvm = new cGameViewModel();
        public static String gamePath;
        public static TimeSpan playTime;
        public static string pathGamesList = @"C:\Gamer Desk Data\GamesList.txt";

        
      
        
        
        public Form1()
        {
            InitializeComponent();


            if (rkApp.GetValue("GDLauncher") == null)
            {
                chkAutoStart.Checked = false;

                this.Show();
                this.WindowState = FormWindowState.Normal;
                this.ShowInTaskbar = true;
            }
            else
            {
                chkAutoStart.Checked = true;

            }

            this.NotifyIconGD.Icon =
                ((System.Drawing.Icon)(GDLauncher.Properties.Resources.GD_Icon));


            //notify icon menus

            ToolStripMenuItem show = new ToolStripMenuItem();
            show.Text = "Show";
            show.Click += show_Click;

            cms.Items.Add(show);

            ToolStripMenuItem exit = new ToolStripMenuItem();
            exit.Text = "Exit";
            exit.Click += exit_Click;

            cms.Items.Add(exit);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Show();
            Thread th = new Thread(Start);
            th.IsBackground = true;
            th.Start();
        }


        public static void Start()
        {
            string fileToWatch = @"C:\Gamer Desk Data\";
            GetGames();
            FileSystemWatcher watcher = new FileSystemWatcher();

            watcher.Path = fileToWatch;

            watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite;

            //filter to look for files
            watcher.Filter = "GameLaunch.txt";

            //events
            watcher.Changed += watcher_Changed;

            watcher.EnableRaisingEvents = true;

            //Console.WriteLine("Enter 'q' to Exit");
            while (true) { Thread.Sleep(160000); }

            
        }

        private void chkAutoStart_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAutoStart.Checked)
            {
                rkApp.SetValue("GDLauncher", Application.ExecutablePath.ToString());
            }
            else
            {
                rkApp.DeleteValue("GDLauncher", false);
            }
        }











        static Process game = new Process();
        //static Process gameRe = new Process();
        static int count = 0;
        static void watcher_Changed(object sender, FileSystemEventArgs e)
        {
            if (++count == 4)
            {
                count = 0;

                if (e.ChangeType == WatcherChangeTypes.Changed)
                {
                    //Console.WriteLine("File: " + e.FullPath + " " + e.ChangeType);
                    //Console.WriteLine("Processing.... ");



                    Thread.Sleep(1000);

                    //Console.WriteLine("chk");

                    StreamReader sr = new StreamReader(@"C:\Gamer Desk Data\GameLaunch.txt");
                    try
                    {
                        gamePath = sr.ReadLine();
                        game.StartInfo = new ProcessStartInfo(gamePath);
                        game.StartInfo.WorkingDirectory = Path.GetDirectoryName(gamePath);
                        //var processStartInfo = new ProcessStartInfo(gamePath);
                        // processStartInfo.WorkingDirectory = Path.GetDirectoryName(gamePath);

                        //game.StartInfo.FileName = gamePath;
                        Console.WriteLine(gamePath);

                        GetGames();
                        //Process.Start(line);
                        game.Start();
                        // restart:
                        //Console.WriteLine("Start Time: " + game.StartTime);


                        game.WaitForExit();




                        //Console.WriteLine("Exit Time: " + game.ExitTime);

                        playTime = game.ExitTime.Subtract(game.StartTime);

                        int hours = playTime.Hours;
                        int minutes = playTime.Minutes;
                        int seconds = playTime.Seconds;
                        //Console.WriteLine("Total Time: " + playTime.ToString(@"hh\:mm" + " Hours"));

                        //Console.WriteLine("Hours: " + hours.ToString());
                        //Console.WriteLine("Minutes: " + minutes.ToString());
                        //Console.WriteLine("Seconds: " + seconds.ToString());


                        if (seconds < 3)
                        {
                            string exeName = Path.GetFileNameWithoutExtension(gamePath);
                            //Console.WriteLine("exe name: " + exeName);
                            /*
                            bool found = false;

                            //Console.WriteLine("Going to Wait for " + exeName);
                            while (!found)
                            {
                                Console.WriteLine("inside wait loop");
                                foreach (Process process in Process.GetProcesses())
                                {
                                    //Console.WriteLine(process.ProcessName);
                                    if (process.ProcessName.Equals(exeName))
                                    {
                                        found = true;
                                    }
                                }
                                Thread.Sleep(500);
                            }
                            Console.WriteLine(game.MainWindowTitle);
                            */

                            /*
                            Process[] lst = Process.GetProcessesByName(exeName);
                            //game.Refresh();
                            game = lst[0];
                            Console.WriteLine("Restart Process Name: " + game.ProcessName);
                            goto restart;
                            */
                            //Console.WriteLine("Process Re Name: " + gameRe.ProcessName);
                            //retry();
                        }


                        UpdateTime();


                    }
                    catch (Exception ed) { Console.WriteLine(ed.ToString()); }

                    finally
                    {
                        sr.Close();
                    }


                }
            }
        }

        /*public static void retry()
        {
            Console.WriteLine("Start Time: " + gameRe.StartTime);
            Console.WriteLine(gameRe.ToString());
            gameRe.WaitForExit();
            



            Console.WriteLine("Exit Time: " + gameRe.ExitTime);

            playTime = gameRe.ExitTime.Subtract(gameRe.StartTime);

            int hours = playTime.Hours;
            int minutes = playTime.Minutes;
            int seconds = playTime.Seconds;
            //Console.WriteLine("Total Time: " + playTime.ToString(@"hh\:mm" + " Hours"));

            Console.WriteLine("Hours: " + hours.ToString());
            Console.WriteLine("Minutes: " + minutes.ToString());
            Console.WriteLine("Seconds: " + seconds.ToString());
        }*/


        public static void UpdateTime()
        {
            int i = 0;
            gamePath = gamePath.TrimStart('"');
            gamePath = gamePath.TrimEnd('"');
            foreach (cGame game in Program.gvm.GamesCollection)
            {
                // Console.WriteLine("Saved Path: " + gamePath);
                //Console.WriteLine("New Path: " + game.Path);

                //---------

                //Console.WriteLine("Changed Path: " + gamePath);
                if (gamePath.Equals(game.Path, StringComparison.CurrentCulture))
                {
                    game.LastTime = playTime;
                    game.TotalTime += game.LastTime;
                    //Console.WriteLine("INN" + playTime.ToString());
                    Program.gvm.GamesCollection[i].LastTime = playTime;
                    Program.gvm.GamesCollection[i].TotalTime = game.TotalTime;
                    break;
                }
                i++;
            }
            UpdateGames();
        }

        public static void GetGames()
        {
            try
            {
                string json = File.ReadAllText(pathGamesList);
                var games = JsonConvert.DeserializeObject<List<cGame>>(json);

                Program.gvm.GamesCollection = new ObservableCollection<cGame>(games);

               // Console.WriteLine("Games Library Opened Successfully");
            }
            catch (Exception) { }

        }

        public static void UpdateGames()
        {
            //GetGames();
            try
            {
                string json = JsonConvert.SerializeObject(Program.gvm.GamesCollection, Formatting.Indented);
                File.WriteAllText(pathGamesList, json);

                //Console.WriteLine("Games Library Updated");
            }
            catch (Exception) { }
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            NotifyIconGD.BalloonTipTitle = "Gamer Desk Desktop App";
            NotifyIconGD.BalloonTipText = "GD Launcher Running in the Background";

            if (FormWindowState.Minimized == this.WindowState)
            {
                NotifyIconGD.Visible = true;
                NotifyIconGD.ShowBalloonTip(500);
                this.ShowInTaskbar = false;
                this.Hide();
            }
            //else if (FormWindowState.Normal == this.WindowState)
            //{
            //    NotifyIconGD.Visible = false;
            //}
        }

        private void NotifyIconGD_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;

            NotifyIconGD.Visible = true;
            NotifyIconGD.ShowBalloonTip(500);
            this.ShowInTaskbar = false;
            this.Hide();
        }

        private void NotifyIconGD_MouseClick(object sender, MouseEventArgs e)
        {
            
        }

        void exit_Click(object sender, EventArgs e)
        {
            
            if (MessageBox.Show("Are you sure you want to close Gamer Desk Desktop App?" + 
                " You won't be able play games from Gamer Desk", "Close Gamer Desk Desktop App", 
                MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                this.Hide();
                Application.Exit();
            }
            else
            {
                return;
            }

            
        }

        void show_Click(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true;
        }

       

        private void cmsExit_MouseClick(object sender, MouseEventArgs e)
        {

            Application.Exit();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.microsoft.com/en-us/store/apps/gamer-desk/9wzdncrdfkk7");
        }
    }
}
