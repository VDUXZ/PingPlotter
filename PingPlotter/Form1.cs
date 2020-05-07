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
        protected override void OnPaint(PaintEventArgs pea)
        {
            // Initialize graphics and then call pinger program

            // Defines pen 
            Pen pen = new Pen(ForeColor);
            Pen redPen = new Pen(Color.Red, 1);
            Pen bluePen = new Pen(Color.Blue, 2);
            Pen greenPen = new Pen(Color.Green, 3);
            Pen blackPen = new Pen(Color.Black, 4);
            Graphics g = pea.Graphics;
            int X1, X2, Y1, Y2, average = 0;
            g.DrawString("Latency", new Font("Verdana", 20), new SolidBrush(Color.Tomato), 20, 20);

            // Draw chart boundaries
            pea.Graphics.DrawLine(blackPen, 40, 250, 290, 250);
            pea.Graphics.DrawLine(blackPen, 40, 40, 40, 250);
            pea.Graphics.DrawLine(blackPen, 290, 40, 290, 250);

            /*
             *
            for (int i = 0; i < 30; i++)
            {
                average = SendPing();

                //g.Clear(Color.White);
                //g.DrawString("Latency " + average, new Font("Verdana", 20), new SolidBrush(Color.Tomato), 40, 40);

                if (average == -1)
                {
                    // pings failed, network destination unreachable
                    X1 = 10 + i * 5;
                    Y1 = 250;
                    X2 = X1;
                    Y2 = 200;
                    pea.Graphics.DrawLine(redPen, X1, Y1, X2, Y2);
                }
                else
                {
                    // Draw line using integer coordinates
                    X1 = 10 + i * 5;
                    Y1 = 250;
                    X2 = X1;
                    Y2 = Y1 - average * 3;
                    pea.Graphics.DrawLine(bluePen, X1, Y1, X2, Y2);
                }
            }
            */

            //Dispose of objects
            redPen.Dispose();
            bluePen.Dispose();
            greenPen.Dispose();
            blackPen.Dispose();
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
