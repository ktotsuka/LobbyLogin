using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
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
        readonly System.Timers.Timer timer = new System.Timers.Timer();
        public List<Visit> WaitingVisits { get; set; }
        public List<VisitWrapper> WaitingVisitsWrap { get; set; }

        private static readonly ReadOnlyCollection<double> reminderMinutes = new ReadOnlyCollection<double>(new[]
        {
            //5.0,
            //10.0,
            //15.0
            1.0,
            2.0,
            3.0
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
            timer.Stop();

            using (var mutex = new Mutex(false, WaitListMutexName))
            {
                mutex.WaitOne();

                ReadVisitsFromFile();
                RemoveOutdatedVisits();
                AddNewWaitingVisits();
                SendReminders();
                RemoveRemindedVisits();
                UpdateWaitListFile(WaitingVisits);

                mutex.ReleaseMutex();
            }

            timer.Start();
        }

        private void ReadVisitsFromFile()
        {
            WaitingVisits = GetVisitFromFile(WaitListFileLocation);
        }

        private void RemoveOutdatedVisits()
        {
            WaitingVisitsWrap.RemoveAll(item => ContainInWaitingVisits(item.Visit.Id) == false);
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

        private bool ContainInWaitingVisits(string id)
        {
            foreach (Visit visit in WaitingVisits)
            {
                if (visit.Id == id)
                {
                    return true;
                }
            }
            return false;
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
                    WriteToLogFile($"Sending reminder for {visitW.Visit.Visitor.FirstName} {visitW.Visit.Visitor.LastName}");
                }
            }
        }

        private void SendReminder(VisitWrapper visit)
        {
            Visitor visitor = visit.Visit.Visitor;
            Employee employee = visit.Visit.Employee;
            string message = $"Reminder: {visitor.FirstName} {visitor.LastName} from {visitor.CompanyName} "
                           + $"is still waiting for {employee.FirstName} {employee.LastName}";

            List<Employee> employees_to_be_notified = new List<Employee>();

            foreach (Employee generalEmployee in GeneralEmployee.GeneralEmployees)
            {
                employees_to_be_notified.Add(generalEmployee);
            }

            if (ContainsEmployee(employees_to_be_notified, employee) == false)
            {
                employees_to_be_notified.Add(employee);
            }

            foreach (Employee e in employees_to_be_notified)
            {
                string numeric_phone_number = new String(e.CellPhoneNumber.Where(Char.IsDigit).ToArray());
                List<string> addresses = GetPhoneEmailAddresses(numeric_phone_number);
                addresses.Add(e.EmailAddress);

                WriteToLogFile(message);

                Mail.SendEmail(addresses, message);
            }
        }

        private bool ContainsEmployee(List<Employee> employees, Employee employee)
        {
            foreach (Employee e in employees)
            {
                if (e.EmailAddress == employee.EmailAddress)
                {
                    return true;
                }
            }
            return false;
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

        public void WriteToLogFile(string Message)
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
