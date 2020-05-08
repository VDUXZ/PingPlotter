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
                if (times[i].Substring(6,7) == "00")
                {
                    Debug.WriteLine("time: " + times[i]);
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
                const string hostname = "-n 2 8.8.8.8";
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
                        //Console.Write(result);
                    }
                    bool success = result.Contains("Destination host unreachable.");
                    if (success) return -1;
                    int position1 = result.IndexOf("(");
                    string buf = result.Substring(position1 + 1);
                    position1 = buf.IndexOf("%");
                    string buf2 = buf.Substring(0, position1);
                    packetLoss = int.Parse(buf2);
                    //Console.WriteLine("packet loss = " + packetLoss);

                    position1 = buf.LastIndexOf("A");
                    buf2 = buf.Substring(position1 + 9);
                    position1 = buf2.IndexOf("m");
                    buf = buf2.Substring(0, position1);
                    average = int.Parse(buf);
                    //Console.WriteLine("Average = " + average);
                }
                return average;
            }
 
        private void TimerTick(object sender, EventArgs e)
        {
            Debug.WriteLine(time);
            time++;
            

            average = SendPing();
            latency[count] = average;
            times[count] = DateTime.Now.ToString("hh:mm:ss");

            count++;
            if (count >= 500) count = 0;
            DrawChart();
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
