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
        int maxSamples = 300; // maximum number of samples in ring buffer
        int mostRecent = 0; // pointer to last value in ring buffer
        Timer timer = new Timer(); // set up event to ping every second
        int time = 0; // counter for number of pings
        int[] latency = new int[500]; // ring buffer space for latency values
        string[] times = new string[500]; // ring buffer storage for time strings
        int count = 0; // number of samples in the ring buffer
        int average = 0;
        int xsize = 0;  // store the size of the graph horizontally
        int ysize = 500; // store vertical size of graph window


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
            Pen redPen = new Pen(Color.Red, 2);
            Pen bluePen = new Pen(Color.Blue, 2);
            Pen greenPen = new Pen(Color.Green, 3);
            Pen blackPen = new Pen(Color.Black, 4);
            Graphics g = pea.Graphics;
            int X1, X2, Y1, Y2; // variables for plotting lines on graph
            int currentSample; // variable for iterating through ring buffer
            xsize = 100 + maxSamples * 3;
            g.DrawString("Latency", new Font("Verdana", 20), new SolidBrush(Color.Tomato), 20, 10);

            // Draw chart boundaries
            pea.Graphics.DrawLine(blackPen, 40, 250, 43+maxSamples*3, 250);
            pea.Graphics.DrawLine(blackPen, 40, 40, 40, 250);
            pea.Graphics.DrawLine(blackPen, 43+maxSamples*3, 40, 43+maxSamples*3, 250);

            currentSample = mostRecent; // set starting point in ringbuffer
            Debug.WriteLine("currentSample=" + currentSample + " mostRecent=" + mostRecent);
            for (int i = 0; i < count; i++)
            {
                if (currentSample >= count)
                {
                    currentSample = 0; // roll ring buffer
                    Debug.WriteLine("rolling currentSample");
                }
                //Debug.WriteLine("i=" + i + " currentSample=" + currentSample);
                // check to see if time label should be written

                if (times[currentSample] == null)
                {
                    Debug.WriteLine("time is null.  currentSample=" + currentSample + " i=" + i + " count=" + count);
                    return;
                }
                if (times[currentSample].Substring(7,1) == "0")
                {
                    GraphicsState state = g.Save();
                    g.ResetTransform();
                    g.RotateTransform(270.0F);
                    g.TranslateTransform(35 + i * 3, 320, MatrixOrder.Append);
                    g.DrawString(times[currentSample], new Font("Verdana", 10), new SolidBrush(Color.Black), 0, 0);
                    g.Restore(state);
                }

                if (latency[currentSample] == -1)
                {
                    // pings failed, network destination unreachable
                    X1 = 42 + i * 3;
                    Y1 = 250;
                    X2 = X1;
                    Y2 = 50;
                    pea.Graphics.DrawLine(redPen, X1, Y1, X2, Y2);
                }
                else
                {
                    // Draw line using integer coordinates
                    X1 = 42 + i * 3;
                    Y1 = 250;
                    X2 = X1;
                    Y2 = Y1 - latency[currentSample] * 2;
                    pea.Graphics.DrawLine(bluePen, X1, Y1, X2, Y2);
                }
                currentSample++;
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
                    bool success = result.Contains("(100% loss)");
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
            
            // perform ring buffer logic
            if ((count >= maxSamples)&&(mostRecent == count))
            {
                mostRecent = 0; // reset the ring buffer
                Debug.WriteLine("reset ring Buffer. count=" + count + " mostRecent=" + mostRecent);
            }

            average = SendPing();
            latency[mostRecent] = average;
            times[mostRecent] = DateTime.Now.ToString("hh:mm:ss");
            Debug.WriteLine(mostRecent + " " + times[mostRecent] + " " + latency[mostRecent]);
            if (count < maxSamples) count++;
            mostRecent++;
     
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
