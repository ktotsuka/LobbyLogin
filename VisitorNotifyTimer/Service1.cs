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
using VisitDataBase;
using System.Collections.ObjectModel;
using SignInMail;
using static VisitDataBase.DataAccess;
using static SignInMail.Mail;

namespace VisitorNotifyTimer
{
    public class VisitWrapper
    {
        public Visit Visit { get; set; }
        public int ReminderCount { get; set; }
        public DateTime InitialTime { get; set; }
    }

    public partial class Service1 : ServiceBase
    {
        readonly Timer timer = new Timer();
        public List<Visit> WaitingVisits { get; set; }
        public List<VisitWrapper> WaitingVisitsWrap { get; set; }

        private static readonly ReadOnlyCollection<double> reminderMinutes = new ReadOnlyCollection<double>(new[]
        {
            5.0,
            8.0,
            11.0
        });

        public static ReadOnlyCollection<double> ReminderMinutes
        {
            get { return reminderMinutes; }
        }

        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            System.Diagnostics.Debugger.Launch();

            WaitingVisits = new List<Visit>();
            WaitingVisitsWrap = new List<VisitWrapper>();

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
            RemoveOutdatedVisits();
            AddNewWaitingVisits();
            SendReminders();
            RemoveRemindedVisits();
            WriteVisitsToFile();
        }

        private void ReadVisitsFromFile()
        {
            lock (waitingVisitFileLock)
            {
                WaitingVisits = GetVisitFromFile(WaitListFileLocation);
            }
        }

        private void WriteVisitsToFile()
        {
            lock (waitingVisitFileLock)
            {
                FileStream waitListFile = new FileStream(WaitListFileLocation, FileMode.Create);
                StreamWriter sw = new StreamWriter(waitListFile);

                string visit_str = GetVisitsString(WaitingVisits);

                sw.WriteLine(visit_str);
                sw.Close();
            }
        }

        private void RemoveOutdatedVisits()
        {
            WaitingVisitsWrap.RemoveAll(item => WaitingVisits.Contains(item.Visit) == false);
        }

        private void AddNewWaitingVisits()
        {
            foreach (Visit visit in WaitingVisits)
            {
                if (ContainInWaitingVisitsWrapper(visit.Id) == false)
                {
                    VisitWrapper visitW = new VisitWrapper()
                    {
                        Visit = visit,
                        ReminderCount = 0,
                        InitialTime = DateTime.Now
                    };
                    WaitingVisitsWrap.Add(visitW);
                }
            }
        }

        private bool ContainInWaitingVisitsWrapper(string id)
        {
            foreach (VisitWrapper visitW in WaitingVisitsWrap)
            {
                if (visitW.Visit.Id == id)
                {
                    return true;
                }
            }
            return false;
        }

        private void SendReminders()
        {
            DateTime currentTime = DateTime.Now;

            foreach (VisitWrapper visitW in WaitingVisitsWrap)
            {
                if (TimeToSendReminderr(visitW, currentTime))
                {
                    SendReminder(visitW);
                    visitW.ReminderCount++;
                }
            }
        }

        private void SendReminder(VisitWrapper visit)
        {
            Visitor visitor = visit.Visit.Visitor;
            Employee employee = visit.Visit.Employee;
            string message = $"Reminder: {visitor.FirstName} {visitor.LastName} from {visitor.CompanyName} is still waiting";

            List<Employee> employees_to_be_notified = new List<Employee>();

            foreach (Employee generalEmployee in GeneralEmployee.GeneralEmployees)
            {
                employees_to_be_notified.Add(generalEmployee);
            }
            if (employees_to_be_notified.Contains(employee) == false)
            {
                employees_to_be_notified.Add(employee);
            }

            foreach (Employee e in employees_to_be_notified)
            {
                string numeric_phone_number = new String(e.CellPhoneNumber.Where(Char.IsDigit).ToArray());
                List<string> addresses = GetPhoneEmailAddresses(numeric_phone_number);
                addresses.Add(employee.EmailAddress);
                Mail.SendEmail(addresses, message);
            }
        }

        private bool TimeToSendReminderr(VisitWrapper visit, DateTime currentTime)
        {
            int count = 0;

            foreach (double reminderMinute in ReminderMinutes)
            {
                double minutesPassed = (currentTime - visit.InitialTime).TotalMinutes;
                if ((minutesPassed > reminderMinute) && (visit.ReminderCount <= count))
                {
                    return true;
                }
                count++;
            }
            return false;
        }

        private void RemoveRemindedVisits()
        {
            WaitingVisitsWrap.RemoveAll(item => item.ReminderCount >= reminderMinutes.Count);
            WaitingVisits.Clear();
            foreach (VisitWrapper visit in WaitingVisitsWrap)
            {
                WaitingVisits.Add(visit.Visit);
            }
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
