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
using System.IO;
using System.Drawing.Drawing2D;

namespace PingPlotter
{
    public partial class f1 : Form
    {
        Timer timer = new Timer();
        int time = 0;
        int[] latency = new int[500];
        string[] times = new string[500];
        int count = 0;
        int average = 0;
        //Graphics g = f1.Graphics;

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
            int X1, X2, Y1, Y2;
            g.DrawString("Latency "+ count, new Font("Verdana", 20), new SolidBrush(Color.Tomato), 20, 10);

            // Draw chart boundaries
            pea.Graphics.DrawLine(blackPen, 40, 250, 900, 250);
            pea.Graphics.DrawLine(blackPen, 40, 40, 40, 250);
            pea.Graphics.DrawLine(blackPen, 900, 40, 900, 250);

            
            for (int i = 0; i < count; i++)
            {
                // check to see if time label should be written
                if (times[i].Substring(7,1) == "0")
                {
                    GraphicsState state = g.Save();
                    g.ResetTransform();
                    g.RotateTransform(270.0F);
                    g.TranslateTransform(35 + i * 3, 320, MatrixOrder.Append);
                    g.DrawString(times[i], new Font("Verdana", 10), new SolidBrush(Color.Black), 0, 0);
                    g.Restore(state);
                }

                if (latency[i] == -1)
                {
                    // pings failed, network destination unreachable
                    X1 = 10 + i * 3;
                    Y1 = 250;
                    X2 = X1;
                    Y2 = 100;
                    pea.Graphics.DrawLine(redPen, X1, Y1, X2, Y2);
                }
                else
                {
                    // Draw line using integer coordinates
                    X1 = 42 + i * 3;
                    Y1 = 250;
                    X2 = X1;
                    Y2 = Y1 - latency[i] * 2;
                    pea.Graphics.DrawLine(bluePen, X1, Y1, X2, Y2);
                }
            }
            

            //Dispose of objects
            redPen.Dispose();
            bluePen.Dispose();
            greenPen.Dispose();
            blackPen.Dispose();
    }


            public int SendPing()
            {
                // Get ping result
                string result;
                int average;
                int packetLoss;
                const string hostname = "-n 1 -w 600 8.8.8.8";
                ProcessStartInfo start = new ProcessStartInfo();
                start.FileName = "C:\\Windows\\System32\\PING.EXE";
                start.UseShellExecute = false;
                start.CreateNoWindow = true;
                start.Arguments = hostname;
                start.RedirectStandardOutput = true;
                using (Process process = Process.Start(start))
                {
                    using (StreamReader reader = process.StandardOutput)
                    {
                        result = reader.ReadToEnd();
                    }
                    bool success = result.Contains("Destination host unreachable.");
                    if (success) return -1;
                    int position1 = result.IndexOf("(");
                    string buf = result.Substring(position1 + 1);
                    position1 = buf.IndexOf("%");
                    string buf2 = buf.Substring(0, position1);
                    packetLoss = int.Parse(buf2);

                    position1 = buf.LastIndexOf("A");
                    buf2 = buf.Substring(position1 + 9);
                    position1 = buf2.IndexOf("m");
                    buf = buf2.Substring(0, position1);
                    average = int.Parse(buf);
                }
                return average;
            }
 
        private void TimerTick(object sender, EventArgs e)
        {
            time++;
            

            average = SendPing();
            latency[count] = average;
            times[count] = DateTime.Now.ToString("hh:mm:ss");
            Debug.WriteLine(count + " " + times[count] + " " + latency[count]);
            count++;
            if (count >= 500) count = 0;
            //DrawChart();
            this.Refresh();

        }

        private void DrawChart()
        {
            Debug.WriteLine("Count=" + count);

        }

        public f1()
        {
            //This gets called when you Run(the form)
            InitializeTimer();
        }
    }
}
