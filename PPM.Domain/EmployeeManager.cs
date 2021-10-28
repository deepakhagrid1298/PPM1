using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using System.IO;
using System.Data.SqlClient;
using System.Data;
using PPM1.Model.PPM1Interface;
using PPM1.Model;
using PPM.Model;

namespace PPM1.Domain
{
    public class EmployeeManager : EmployeeInterface
    {
        private static readonly List<Employee> _employeeList = new List<Employee>();
        public ActionResult Add(Employee employee)
        {
            ActionResult result = new ActionResult() { IsSuccess = true };
            try
            {
                if (_employeeList.Count > 0)
                {
                    if (_employeeList.Exists(em => em.Id == employee.Id))
                    {
                        result.IsSuccess = false;
                        result.Status = $"ID: { employee.Id} Already Exists";
                    }
                    else
                    {
                        _employeeList.Add(employee);
                        result.Status = "Employee Added";
                    }
                }
                else
                {
                    _employeeList.Add(employee);
                    result.Status = "New Employee Added";
                }
            }
            catch (Exception)
            {
                Console.WriteLine("error occured");
                result.IsSuccess = false;
            }
            return result;

        }
        public DataResult<Employee> ViewListAll()
        {
            DataResult<Employee> employeeResult = new DataResult<Employee>() { IsSuccess = true };
            if (_employeeList.Count > 0)
            {
                employeeResult.Results = _employeeList;
            }
            else
            {
                employeeResult.IsSuccess = false;
                employeeResult.Status = "No Employee in the list";
            }
            return employeeResult;
        }

        public ActionResult ValidEmployee(uint id)
        {
            ActionResult actionResult = new ActionResult() { IsSuccess = true };
            try
            {
                if (_employeeList.Count > 0)
                {
                    if (_employeeList.Exists(e => e.Id == id))
                    {
                        actionResult.Status = "Id Exists!";
                    }
                    else
                    {
                        actionResult.IsSuccess = false;
                        actionResult.Status = $"Employee Id: {id} is not Exist in the Employee List!";
                    }
                }
                else
                {
                    actionResult.IsSuccess = false;
                    actionResult.Status = "Employee List is empty!";
                }
            }
            catch (Exception)
            {
                actionResult.Status = "Error Occoured";
            }
            return actionResult;
        }

        public Employee ViewListById(uint id)
        {
            Employee employee = new Employee();
            employee = _employeeList.Single(e => e.Id == id);
            return employee;
        }

        public ActionResult Delete(uint id)
        {
            ActionResult actionResult = new ActionResult() { IsSuccess = true };
            try
            {
                var empToDelete = _employeeList.Single(e => e.Id == id);
                _employeeList.Remove(empToDelete);
                actionResult.Status = $"Employee with Id: {id} Deleted Successfully!";
            }
            catch (Exception)
            {
                actionResult.Status = "Oops!Error Ocooured";
                actionResult.IsSuccess = false;
            }
            return actionResult;
        }

        public ActionResult RestoreRoleToEmployee(uint id, string role)
        {
            ActionResult actionResult = new ActionResult() { IsSuccess = true };
            if (_employeeList.Single(e => e.Id == id).RoleName == null)
            {
                _employeeList.Single(e => e.Id == id).RoleName = role;
                actionResult.Status = $"Role is Restored to Employee with Id:{id} Successfully!";
            }
            else
            {
                actionResult.IsSuccess = false;
                actionResult.Status = $"Already a role is exist for the Employee with Id: {id}";
            }
            return actionResult;
        }

        public ActionResult DeleteRoleFromEmployee(string role)
        {
            ActionResult actionResult = new ActionResult() { IsSuccess = true };
            try
            {
                foreach (Employee emp in _employeeList)
                {
                    if (emp.RoleName == role)
                    {
                        emp.RoleName = null;
                    }
                }
                actionResult.Status = "Role is removed Successfully!";
            }
            catch (Exception)
            {
                actionResult.Status = "Opps!Error Occoured!";
                actionResult.IsSuccess = false;
            }
            return actionResult;
        }

        public ActionResult IsRolePresent(string role)
        {
            ActionResult actionResult = new ActionResult() { IsSuccess = true };
            int count = 0;
            try
            {
                if (_employeeList.Count > 0)
                {
                    foreach (Employee emp in _employeeList)
                    {
                        if (emp.RoleName == role)
                        {
                            count++;
                        }
                    }
                    if (count > 0)
                    {
                        actionResult.Status = $"Role: {role} is Present in the Employee List!";
                    }
                    else
                    {
                        actionResult.IsSuccess = false;
                        actionResult.Status = "Role is not present in any project!";
                    }
                }
                else
                {
                    actionResult.IsSuccess = false;
                    actionResult.Status = "Employee List Empty!";
                }
            }
            catch (Exception)
            {
                actionResult.Status = "Error Occoured!";
                actionResult.IsSuccess = false;
            }
            return actionResult;
        }

        public ActionResult ToXmlSerialization()
        {
            ActionResult actionResult = new ActionResult() { IsSuccess = true };
            try
            {
                if (_employeeList.Count > 0)
                {
                    /*string filePath = System.IO.Path.Combine(System.Web.HttpContext.Current.Server.MapPath("F:\\PPM\\PPM.Model"), "AppData", fileName)
                    var filePath = System.IO.File.Create(Environment.GetFolderPath(Environment.SpecialFolder.MyMusic) + fileName)
                    XmlSerializer serializer = new XmlSerializer(typeof(List<Employee>));
                    using (TextWriter tw = new StreamWriter(fileName))
                    {
                        serializer.Serialize(tw, _employeeList);
                        tw.Close();
                    }*/
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Employee>));
                    TextWriter Filestream = new StreamWriter(@"C:\Users\hp\source\repos\employees.xml");
                    xmlSerializer.Serialize(Filestream, EmployeeManager._employeeList);
                    Filestream.Close();

                }
                else
                {
                    actionResult.IsSuccess = false;
                    actionResult.Status = "Employee List is Empty!";
                }
            }
            catch (Exception e)
            {
                actionResult.IsSuccess = false;
                actionResult.Status = "Error Occoured!" + e.Message;
            }
            return actionResult;
        }

        public ActionResult ToTxtFile(string fileName)
        {
            ActionResult actionResult = new ActionResult() { IsSuccess = true };
            try
            {
                if (_employeeList.Count > 0)
                {
                    using (TextWriter sw = new StreamWriter(fileName))
                    {
                        foreach (Employee e in _employeeList)
                        {
                            sw.WriteLine("Employee Id: " + e.Id + "\nEmployee Name: " + e.EmployeeName + "\nDOB: " + e.DOB.ToShortDateString() + "\nContact Number: " + e.Contact + "\nRole: " + e.RoleName);
                            sw.WriteLine("--------------------------------------------------------------------------------------");
                            actionResult.Status = "Employee Is Saved in The Text File!";
                        }
                    }
                }
                else
                {
                    actionResult.IsSuccess = false;
                    actionResult.Status = "Employee list is empty!";
                }
            }
            catch (Exception e)
            {
                actionResult.IsSuccess = false;
                actionResult.Status = "Error Occoured" + "\n" + e.Message;
            }

            return actionResult;
        }

        public ActionResult ToAdoDB()
        {
            ActionResult actionResult = new ActionResult() { IsSuccess = true };
            string conn = "Server=DESKTOP-NNAFOET\\SQLEXPRESS; Database=PPM;Integrated security=true;TrustServerCertificate=true";
            SqlConnection myConn = new SqlConnection(conn);
            string str = "DROP TABLE IF EXISTS employee";
            try
            {
                myConn.Open();
                using (SqlCommand command = new SqlCommand(str, myConn))
                {
                    command.ExecuteNonQuery();
                    actionResult.Status = "Old Data of Employee Dropped Successfully!";
                }
            }
            catch (Exception e)
            {
                actionResult.IsSuccess = false;
                actionResult.Status = e.Message;
            }
            finally
            {
                if (myConn.State == ConnectionState.Open)
                {
                    myConn.Close();
                }
            }
            if (actionResult.IsSuccess)
            {
                try
                {
                    myConn.Open();
                    using (SqlCommand command = new SqlCommand("CREATE TABLE employee (Id int,EmployeeName varchar(50), DOB date, Contact bigint, RoleName varchar(50));", myConn))
                    {
                        command.ExecuteNonQuery();

                        foreach (Employee employee in _employeeList)
                        {
                            int id = (int)employee.Id;
                            long contact = (long)employee.Contact;
                            string insertQ = "INSERT INTO employee values(@Id,@EmployeeName,@DOB,@Contact,@RoleName)";
                            SqlCommand command1 = new SqlCommand(insertQ, myConn);
                            command1.Parameters.AddWithValue("@Id", id);
                            command1.Parameters.AddWithValue("@EmployeeName", employee.EmployeeName);
                            command1.Parameters.AddWithValue("@DOB", employee.DOB);
                            command1.Parameters.AddWithValue("@Contact", contact);
                            command1.Parameters.AddWithValue("@RoleName", employee.RoleName);
                            command1.ExecuteNonQuery();
                        }
                        actionResult.Status = actionResult.Status + "\n" + "Table employee Added SuccessFully!";
                    }

                }
                catch (Exception e)
                {
                    actionResult.IsSuccess = false;
                    actionResult.Status = e.Message;
                }
                finally
                {
                    if (myConn.State == ConnectionState.Open)
                    {
                        myConn.Close();
                    }
                }
            }
            return actionResult;
        }

        public ActionResult ToEFDB()
        {
            ActionResult actionResult = new ActionResult() { IsSuccess = true };
            try
            {
                using (var db = new ProgramDbContext())
                {
                    foreach (Employee employee in _employeeList)
                    {

                        db.Employees.Add(employee);
                        db.SaveChanges();
                    }
                }
                actionResult.Status = "Employee Saved to Database Successfully";
            }
            catch (Exception e)
            {
                actionResult.IsSuccess = true;
                actionResult.Status = e.Message;
            }
            return actionResult;
        }
    }
}
