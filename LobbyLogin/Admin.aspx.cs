using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Diagnostics;
using System.Text;
using System.IO;
using VisitDataBase;
using static VisitDataBase.DataAccess;
using static SignInMail.Mail;

namespace LobbyLogin
{
    public partial class AdminTool : System.Web.UI.Page
    {
        //public const string correctPassword = "Georgetown@4321!";


        public const string correctPassword = "A";
        public const int MaxTextLength = 50;
        public List<EmployeeWrapper> Employees { get; set; }
        public List<VisitorWrapper> Visitors { get; set; }
        public List<Visit> Visits { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            Employees = new List<EmployeeWrapper>();
            Visitors = new List<VisitorWrapper>();
            Visits = new List<Visit>();
            UpdateEmployeeList();
            UpdateVisitorList();
            UpdateVisitList();
        }

        protected void AdminPasswordSubmitButton_Click(object sender, EventArgs e)
        {
            if (adminPassword.Text == correctPassword)
            {
                submitErrorMessage.Text = "";
                AdminPasswordTable.Visible = false;
                AddEmployeeTable.Visible = true;
                RemoveEmployeeTable.Visible = true;
                RemoveVisitorTable.Visible = true;
                RemoveVisitTable.Visible = true;
                ExportTable.Visible = true;
                ImportTable.Visible = true;

                UpdateEmployeeDropDownList();
                UpdateVisitorDropDownList();
                UpdateVisitDropDownList();
            }
            else
            {
                submitErrorMessage.Text = "password is incorrect";
            }
        }

        protected void AddEmployeeButton_Click(object sender, EventArgs e)
        {
            if (VerifyAddEmployeeInputs())
            {
                using (var db = new VisitContext())
                {
                    EmployeeWrapper new_employee = new EmployeeWrapper
                    {
                        Employee = new Employee
                        {
                            FirstName = firstName.Text.Trim(),
                            LastName = lastName.Text.Trim(),
                            EmailAddress = emailAddress.Text.ToLower().Trim(),
                            CellPhoneNumber = cellPhoneNumber.Text.Trim()
                        },                        
                        Id = firstName.Text.Trim() + lastName.Text.Trim() + emailAddress.Text.ToLower().Trim()
                    };

                    try
                    {
                        db.Employees.Add(new_employee);
                        db.SaveChanges();
                        addEmployeeErrorMessage.Text = 
                            $"Added: {new_employee.Employee.FirstName} {new_employee.Employee.LastName}, " +
                            $"{new_employee.Employee.EmailAddress}, {new_employee.Employee.CellPhoneNumber}";
                        UpdateEmployeeList();
                        UpdateEmployeeDropDownList();
                    }
                    catch
                    {
                        addEmployeeErrorMessage.Text = "The employee already exists in the system!";
                    }
                }
            };
        }

        protected void ExportEmployeesButton_Click(object sender, EventArgs e)
        {
            string employees_string = GetEmployeesString();

            // Download Here

            HttpContext context = HttpContext.Current;
            context.Response.Write(employees_string);
            context.Response.ContentType = "text/csv";
            context.Response.AddHeader("Content-Disposition", "attachment; filename=EmployeeData.csv");
            context.Response.End();
        }

        public static string GetEmployeesString()
        {
            List<EmployeeWrapper> employees = new List<EmployeeWrapper>();

            using (var db = new VisitContext())
            {
                employees = db.Employees.ToList();
            }

            string header = @"""Last name"",""First name"",""Email address"",""Cell phone number"",""ID""";
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(header);

            foreach (var i in employees)
            {
                sb.AppendLine(string.Join(",",
                    string.Format(@"""{0}""", i.Employee.LastName),
                    string.Format(@"""{0}""", i.Employee.FirstName),
                    string.Format(@"""{0}""", i.Employee.EmailAddress),
                    string.Format(@"""{0}""", i.Employee.CellPhoneNumber),
                    string.Format(@"""{0}""", i.Id)));
            }
            return sb.ToString();
        }

        protected void ExportVisitorsButton_Click(object sender, EventArgs e)
        {
            string visitors_string = GetVisitorsString();

            HttpContext context = HttpContext.Current;
            context.Response.Write(visitors_string);
            context.Response.ContentType = "text/csv";
            context.Response.AddHeader("Content-Disposition", "attachment; filename=VisitorData.csv");
            context.Response.End();
        }

        public static string GetVisitorsString()
        {
            List<VisitorWrapper> visitors = new List<VisitorWrapper>();

            using (var db = new VisitContext())
            {
                visitors = db.Visitors.ToList();
            }

            string header = @"""Last name"",""First name"",""Company name"",""Email address"",""Phone number"",""Host ID"",""ID""";
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(header);
            foreach (var i in visitors)
            {
                sb.AppendLine(string.Join(",",
                    string.Format(@"""{0}""", i.Visitor.LastName),
                    string.Format(@"""{0}""", i.Visitor.FirstName),
                    string.Format(@"""{0}""", i.Visitor.CompanyName),
                    string.Format(@"""{0}""", i.Visitor.EmailAddress),
                    string.Format(@"""{0}""", i.Visitor.PhoneNumber),
                    string.Format(@"""{0}""", i.Visitor.HostId),
                    string.Format(@"""{0}""", i.Id)));
            }
            return sb.ToString();
        }

        protected void ExportVisitsButton_Click(object sender, EventArgs e)
        {
            List<Visit> visits = new List<Visit>();

            using (var db = new VisitContext())
            {
                visits = db.Visits.ToList();
            }

            string visits_string = GetVisitsString(visits);

            HttpContext context = HttpContext.Current;
            context.Response.Write(visits_string);
            context.Response.ContentType = "text/csv";
            context.Response.AddHeader("Content-Disposition", "attachment; filename=VisitData.csv");
            context.Response.End();
        }        

        protected void ImportEmployeesButton_Click(object sender, EventArgs e)
        {
            if (ImportEmployeesFileUploadControl.PostedFile.ContentType == "text/csv" || ImportEmployeesFileUploadControl.PostedFile.ContentType == "application/octet-stream")
            {
                string fileName = Path.Combine(Server.MapPath("~/Uploaded"), "employees" + ".csv");
                try
                {
                    ImportEmployeesFileUploadControl.PostedFile.SaveAs(fileName);

                    string[] Lines = File.ReadAllLines(fileName);
                    string[] Fields;

                    //Remove Header line
                    Lines = Lines.Skip(1).ToArray();
                    List<EmployeeWrapper> employees = new List<EmployeeWrapper>();
                    foreach (var line in Lines)
                    {
                        Fields = line.Split(new char[] { ',' });
                        if (Fields.Count() != EmployeeNumOfFields)
                        {
                            throw new System.InvalidOperationException("Invalid number of fields");
                        }
                        employees.Add(
                            new EmployeeWrapper
                            {
                                Employee = new Employee
                                {
                                    LastName = Fields[0].Replace("\"", ""), // removed "" 
                                    FirstName = Fields[1].Replace("\"", ""),
                                    EmailAddress = Fields[2].Replace("\"", ""),
                                    CellPhoneNumber = Fields[3].Replace("\"", "")
                                },
                                Id = Fields[4].Replace("\"", "")
                            });
                    }

                    // Update database data
                    using (var dc = new VisitContext())
                    {
                        foreach (var i in employees)
                        {
                            var v = dc.Employees.Where(a => a.Id == i.Id).FirstOrDefault();
                            if (v == null)
                            {
                                dc.Employees.Add(i);
                            }
                        }
                        dc.SaveChanges();
                    }
                    UpdateEmployeeList();
                    UpdateEmployeeDropDownList();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        protected void ImportVisitorsButton_Click(object sender, EventArgs e)
        {
            if (ImportVisitorsFileUploadControl.PostedFile.ContentType == "text/csv" || ImportVisitorsFileUploadControl.PostedFile.ContentType == "application/vnd.ms-excel")
            {
                string fileName = Path.Combine(Server.MapPath("~/Uploaded"), "visitors" + ".csv");
                try
                {
                    ImportVisitorsFileUploadControl.PostedFile.SaveAs(fileName);

                    string[] Lines = File.ReadAllLines(fileName);
                    string[] Fields;

                    //Remove Header line
                    Lines = Lines.Skip(1).ToArray();
                    List<VisitorWrapper> visitors = new List<VisitorWrapper>();
                    foreach (var line in Lines)
                    {
                        Fields = line.Split(new char[] { ',' });
                        if (Fields.Count() != VisitorNumOfFields)
                        {
                            throw new System.InvalidOperationException("Invalid number of fields");
                        }
                        visitors.Add(
                            new VisitorWrapper
                            {
                                Visitor = new Visitor
                                {
                                    LastName = Fields[0].Replace("\"", ""), // removed "" 
                                    FirstName = Fields[1].Replace("\"", ""),
                                    CompanyName = Fields[2].Replace("\"", ""),
                                    EmailAddress = Fields[3].Replace("\"", ""),
                                    PhoneNumber = Fields[4].Replace("\"", ""),
                                    HostId = Fields[5].Replace("\"", "")
                                },
                                Id = Fields[6].Replace("\"", "")
                            });
                    }

                    // Update database data
                    using (var dc = new VisitContext())
                    {
                        foreach (var i in visitors)
                        {
                            var v = dc.Visitors.Where(a => a.Id == i.Id).FirstOrDefault();
                            if (v == null)
                            {
                                dc.Visitors.Add(i);
                            }
                        }
                        dc.SaveChanges();
                    }
                    UpdateVisitorList();
                    UpdateVisitorDropDownList();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        protected void ImportVisitsButton_Click(object sender, EventArgs e)
        {
            if (ImportVisitsFileUploadControl.PostedFile.ContentType == "text/csv" || ImportVisitsFileUploadControl.PostedFile.ContentType == "application/vnd.ms-excel")
            {
                try
                {
                    string fileName = Path.Combine(Server.MapPath("~/Uploaded"), "visits" + ".csv");

                    ImportVisitsFileUploadControl.PostedFile.SaveAs(fileName);
                    List<Visit> visits = GetVisitFromFile(fileName);                    

                    // Update database data
                    using (var dc = new VisitContext())
                    {
                        foreach (var i in visits)
                        {
                            var v = dc.Visits.Where(a => a.Id == i.Id).FirstOrDefault();
                            if (v == null)
                            {
                                dc.Visits.Add(i);
                            }
                        }
                        dc.SaveChanges();
                    }
                    UpdateVisitList();
                    UpdateVisitDropDownList();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        private void UpdateEmployeeList()
        {
            Employees.Clear();
            using (var db = new VisitContext())
            {
                var query = from b in db.Employees
                            orderby b.Employee.LastName, b.Employee.FirstName
                            select b;
                foreach (var b in query)
                {
                    Employees.Add(b);
                }
            }
        }

        private void UpdateVisitorList()
        {
            Visitors.Clear();
            using (var db = new VisitContext())
            {
                var query = from b in db.Visitors
                            orderby b.Visitor.LastName, b.Visitor.FirstName, b.Visitor.CompanyName
                            select b;
                foreach (var b in query)
                {
                    Visitors.Add(b);
                }
            }
        }

        private void UpdateVisitList()
        {
            Visits.Clear();
            using (var db = new VisitContext())
            {
                var query = from b in db.Visits
                            orderby b.Time descending, b.Employee.LastName, b.Employee.FirstName, b.Visitor.FirstName, b.Visitor.LastName, b.Visitor.CompanyName
                            select b;
                foreach (var b in query)
                {
                    Visits.Add(b);
                }
            }
        }

        private void UpdateEmployeeDropDownList()
        {
            EmployeesDropDownList.Items.Clear();
            foreach (var employee in Employees)
            {
                Employee emp = employee.Employee;
                string employee_info = $"{emp.FirstName} {emp.LastName}, {emp.EmailAddress}, {emp.CellPhoneNumber}";
                EmployeesDropDownList.Items.Add(employee_info);
            }
        }

        private void UpdateVisitorDropDownList()
        {
            VisitorsDropDownList.Items.Clear();
            foreach (var visitor in Visitors)
            {
                Visitor vis = visitor.Visitor;
                string visitor_info = $"{vis.FirstName} {vis.LastName} from {vis.CompanyName} ({vis.EmailAddress}, {vis.PhoneNumber})";
                VisitorsDropDownList.Items.Add(visitor_info);
            }
        }

        private void UpdateVisitDropDownList()
        {
            VisitsDropDownList.Items.Clear();
            foreach (var visit in Visits)
            {
                Employee emp = visit.Employee;
                Visitor vis = visit.Visitor;
                string visit_info = $"{emp.FirstName} {emp.LastName}: {vis.FirstName} {vis.LastName} from {vis.CompanyName} on {visit.Time}";
                VisitsDropDownList.Items.Add(visit_info);
            }
        }

        private bool VerifyAddEmployeeInputs()
        {
            if ((firstName.Text == "")
                ||
                (lastName.Text == "")
                ||
                (emailAddress.Text == "")
                ||
                (cellPhoneNumber.Text == ""))
            {
                addEmployeeErrorMessage.Text = "All required fields need to be filled";
                return false;
            }
            else if ((firstName.Text.Contains(","))
                ||
                (lastName.Text.Contains(","))
                ||
                (emailAddress.Text.Contains(","))
                ||
                (cellPhoneNumber.Text.Contains(",")))
            {
                addEmployeeErrorMessage.Text = "no comma allowed";
                return false;
            }
            else if (!IsValidEmail(emailAddress.Text))
            {
                addEmployeeErrorMessage.Text = "Invalid email address";
                return false;
            }
            else
            {
                submitErrorMessage.Text = "";
                return true;
            }
        }

        protected void RemoveEmployeeButton_Click(object sender, EventArgs e)
        {
            EmployeeWrapper selected_employee;

            try
            {
                selected_employee = Employees[EmployeesDropDownList.SelectedIndex];
            }
            catch
            {
                removeEmployeeMessage.Text = "No employee selected";
                return;
            }

            using (var db = new VisitContext())
            {
                EmployeeWrapper employee_to_remove = (EmployeeWrapper)db.Employees.Where(b => b.Id == selected_employee.Id).First();
                db.Employees.Remove(employee_to_remove);
                db.SaveChanges();
                Employee emp = employee_to_remove.Employee;
                removeEmployeeMessage.Text = $"Removed: {emp.FirstName} {emp.LastName}, {emp.EmailAddress}, {emp.CellPhoneNumber}";
            }
            UpdateEmployeeList();
            UpdateEmployeeDropDownList();
        }

        protected void RemoveVisitorButton_Click(object sender, EventArgs e)
        {
            VisitorWrapper selected_visitor;

            try
            {
                selected_visitor = Visitors[VisitorsDropDownList.SelectedIndex];
            }
            catch
            {
                removeVisitorMessage.Text = "No visitor selected";
                return;
            }

            using (var db = new VisitContext())
            {
                VisitorWrapper visitor_to_remove = (VisitorWrapper)db.Visitors.Where(b => b.Id == selected_visitor.Id).First();
                db.Visitors.Remove(visitor_to_remove);
                db.SaveChanges();
                Visitor vis = visitor_to_remove.Visitor;
                removeVisitorMessage.Text = $"Removed: {vis.FirstName} {vis.LastName} from {vis.CompanyName}";
            }
            UpdateVisitorList();
            UpdateVisitorDropDownList();
        }

        protected void RemoveVisitButton_Click(object sender, EventArgs e)
        {
            Visit selected_visit;

            try
            {
                selected_visit = Visits[VisitsDropDownList.SelectedIndex];
            }
            catch
            {
                removeVisitMessage.Text = "No visit selected";
                return;
            }

            using (var db = new VisitContext())
            {
                Visit visit_to_remove = (Visit)db.Visits.Where(b => b.Id == selected_visit.Id).First();
                db.Visits.Remove(visit_to_remove);
                db.SaveChanges();
                Employee emp = visit_to_remove.Employee;
                Visitor vis = visit_to_remove.Visitor;
                removeVisitMessage.Text = 
                    $"Removed: {emp.FirstName} {emp.LastName} was visited by {vis.FirstName} {vis.LastName} from {vis.CompanyName} on {visit_to_remove.Time}";
            }
            UpdateVisitList();
            UpdateVisitDropDownList();
        }
    }
}