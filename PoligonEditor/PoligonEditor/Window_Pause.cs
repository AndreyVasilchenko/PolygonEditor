using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//using System.Threading;

namespace PolygonEditor
{
    public partial class Window_Pause : Form
    {
        Timer timer;

        public Window_Pause( int pause_in_msec )
        {
            InitializeComponent();

            timer = new Timer();
            timer.Tick += new EventHandler(TimerProc);
            timer.Interval = pause_in_msec;
            timer.Start();
        }

        private void TimerProc(Object myObject, EventArgs myEventArgs)
        {
            timer.Stop();

            this.Close();
        }
    }
}
