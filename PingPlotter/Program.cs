using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;

namespace PingPlotter
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Form f = new f1
            {
                BackColor = Color.White,
                FormBorderStyle = FormBorderStyle.FixedSingle
            };

            System.Drawing.Rectangle wSize = new Rectangle(0, 0, 1000, 400);

            f.Bounds = wSize;
            f.TopMost = true;
            Application.EnableVisualStyles();
            Application.Run(f);
            Debug.WriteLine(Screen.PrimaryScreen.Bounds);
            Console.WriteLine(Screen.PrimaryScreen.Bounds);
        }
    }
}
