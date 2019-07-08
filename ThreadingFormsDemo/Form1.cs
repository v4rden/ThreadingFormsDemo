using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ThreadingFormsDemo
{
    using System.Threading;

    public partial class Form1 : Form
    {
        private readonly AutoResetEvent _are;
        private readonly SynchronizationContext _context;

        public Form1()
        {
            InitializeComponent();
            _context = SynchronizationContext.Current;
            _are = new AutoResetEvent(false);
        }

        private void UpdateButton_Click(object sender, EventArgs e)
        {
            StartWorkerThread();
            updateButton.Enabled = false;
            _are.Set();
        }

        private void StartWorkerThread()
        {
            var wt = new Thread(DoWork);
            wt.Start();
        }

        private void DoWork()
        {
            _are.WaitOne();
            Thread.Sleep(3000);
            _context.Post(status => timeLable.Text = DateTime.Now.ToString(), null);
            _context.Post(status => updateButton.Enabled = true, null);
        }
    }
}