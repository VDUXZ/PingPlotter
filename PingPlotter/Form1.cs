using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PingPlotter
{
    public partial class f1 : Form
    {
        Timer timer = new Timer();
        int time = 0;

        private void InitializeTimer()
        {
            timer.Interval = 1000;
            timer.Tick += new EventHandler(TimerTick);
            timer.Start();
        }

        private void TimerTick(object sender, EventArgs e)
        {
            Debug.WriteLine(time);
            time++;
        }

        public f1()
        {
            //This gets called when you Run(the form)
            InitializeTimer();
        }
    }
}
