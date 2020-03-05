using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.IO;

namespace VisitorNotifyTimer
{
    public partial class Service1 : ServiceBase
    {
        readonly Timer timer = new Timer();

        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            //System.Diagnostics.Debugger.Launch();
            timer.Elapsed += new ElapsedEventHandler(OnElapsedTime);
            timer.Interval = 10000; //number in milisecinds  
            timer.Enabled = true;
        }

        protected override void OnStop()
        {
        }
        private void OnElapsedTime(object source, ElapsedEventArgs e)
        {
            ReadVisitsFromFile();
            SendReminder();
            RemoveOutdatedVisits();
        }

        private void ReadVisitsFromFile()
        {


        }

        private void SendReminder()
        {

        }

        private void RemoveOutdatedVisits()
        {

        }

        public void WriteToFile(string Message)
        {
            try
            {
                string filepath = @"C:\Temp\test.txt";
                if (!File.Exists(filepath))
                {
                    // Create a file to write to.   
                    using (StreamWriter sw = File.CreateText(filepath))
                    {
                        sw.WriteLine(Message);
                    }
                }
                else
                {
                    using (StreamWriter sw = File.AppendText(filepath))
                    {
                        sw.WriteLine(Message);
                    }
                }
            }
            catch
            {

            }

        }
    }
}
