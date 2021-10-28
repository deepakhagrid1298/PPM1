using PPM.Model;
using PPM1.Model;
using PPM1.Model.PPM1Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PPM1.Domain
{
    public class ProjectManager : ProjectInterface
    {
        private static readonly List<Project> _projectList = new List<Project>();

        public ActionResult Add(Project project)
        {
            ActionResult result = new ActionResult() { IsSuccess = true };
            try
            {
                if (_projectList.Count > 0)
                {
                    if (_projectList.Exists(proj => proj.ProjectId == project.ProjectId))
                    {
                        result.IsSuccess = false;
                        result.Status = $"Validation Failed.ID: {project.ProjectId} is already Exists!";
                    }
                    else
                    {
                        _projectList.Add(project);
                        result.Status = "Project Added";
                    }
                }
                else
                {
                    _projectList.Add(project);
                    result.Status = "New Project Added";
                }
            }
            catch (Exception e)
            {
                result.IsSuccess = false;
                result.Status = "Exception Occured : " + e.ToString();
            }
            return result;
        }

        public DataResult<Project> ViewListAll()
        {
            DataResult<Project> projResult = new DataResult<Project>() { IsSuccess = true };
            if (_projectList.Count > 0)
            {
                projResult.Results = _projectList;
            }
            else
            {
                projResult.IsSuccess = false;
                projResult.Status = "No Projects in list";
            }
            return projResult;
        }

        public ActionResult IsValidID(uint id)
        {
            ActionResult actionResult = new ActionResult() { IsSuccess = true };
            if (_projectList.Count > 0)
            {
                if (_projectList.Exists(p => p.ProjectId == id))
                {
                    actionResult.IsSuccess = true;
                    actionResult.Status = $"Project Id: {id} Exists!";
                }
                else
                {
                    actionResult.Status = $"Project Id: {id} is not in the Project List!";
                }

            }
            else
            {
                actionResult.IsSuccess = false;
                actionResult.Status = "Project List is Empty!";
            }
            return actionResult;
        }
        public Project ViewListById(uint id)
        {
            Project project = new Project();

            project = _projectList.Single(p => p.ProjectId == id);
            return project;
        }

        public ActionResult Delete(uint id)
        {
            ActionResult actionResult = new ActionResult() { IsSuccess = true };
            try
            {
                var itemToRemove = _projectList.Single(p => p.ProjectId == id);
                _projectList.Remove(itemToRemove);
                actionResult.Status = $"Project with Id: {id} Deleted Successfully!";
            }
            catch (Exception)
            {
                actionResult.IsSuccess = false;
                actionResult.Status = "Error Occoured!";
            }
            return actionResult;

        }

        public ActionResult AddEmpToProject(Employee employee, uint id)
        {
            ActionResult result = new ActionResult() { IsSuccess = true };

            try
            {
                if (_projectList.Count > 0)
                {
                    if (_projectList.Exists(p => p.ProjectId == id))
                    {

                        if (_projectList.Single(p => p.ProjectId == id).EmpName == null)
                        {
                            _projectList.Single(p => p.ProjectId == id).EmpName = new List<Employee>();
                        }

                        if (_projectList.Single(p => p.ProjectId == id).EmpName.Exists(e => e.Id == employee.Id))
                        {
                            result.Status = $"Employee Id : {employee.Id} already exists in this project: {id}";
                            result.IsSuccess = false;
                        }
                        else
                        {
                            _projectList.Single(p => p.ProjectId == id).EmpName.Add(employee);
                            result.Status = "Employee is Added to project";

                        }

                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Status = "Project Id not found!" + id;
                    }
                }
                else
                {
                    result.IsSuccess = false;
                    result.Status = "Project list is Empty!";
                }

            }
            catch (Exception e)
            {
                result.IsSuccess = false;
                result.Status = "Exception Occured : " + e.ToString();
            }
            return result;

        }

        public ActionResult RemoveEmpFromProject(uint id, Employee employee)
        {
            ActionResult action = new ActionResult() { IsSuccess = true };
            try
            {
                if (_projectList.Exists(p => p.ProjectId == id))
                {
                    if (_projectList.Single(s => s.ProjectId == id).EmpName.Exists(n => n.Id == employee.Id))
                    {
                        var itemToRemove = _projectList.Single(s => s.ProjectId == id).EmpName.Single(e => e.Id == employee.Id);
                        _projectList.Single(s => s.ProjectId == id).EmpName.Remove(itemToRemove);
                        action.Status = $"Employee: { employee.Id} is Deleted Successfully";
                    }
                    else
                    {
                        action.IsSuccess = false;
                        action.Status = $"Given Employee ID: {employee.Id} is not Present in the particular Project";
                    }
                }
                else
                {
                    action.IsSuccess = false;
                    action.Status = $"Project Id: {id} is not in the List!";
                }
            }
            catch (Exception e)
            {
                action.IsSuccess = false;
                action.Status = "Exception Occured : " + e.ToString();
            }
            return action;
        }

        public ActionResult EmployeeDeleteFromProject(uint id)
        {
            ActionResult actionResult = new ActionResult() { IsSuccess = true };
            try
            {
                foreach (Project pro in _projectList)
                {
                    if (pro.EmpName.Exists(p => p.Id == id))
                    {
                        var empToRemove = pro.EmpName.Single(e => e.Id == id);
                        pro.EmpName.Remove(empToRemove);
                    }
                }
                actionResult.Status = $"Employee with Employee Id: {id} is Removed Successfully!";
            }
            catch (Exception)
            {
                actionResult.IsSuccess = false;
                actionResult.Status = "Eror Occoured!";
            }
            return actionResult;
        }

        public ActionResult RoleDeleteFromProject(string role)
        {
            ActionResult actionResult = new ActionResult() { IsSuccess = true };
            try
            {
                if (_projectList.Count > 0)
                {
                    foreach (Project pro in _projectList)
                    {
                        if (pro.EmpName.Exists(p => p.RoleName == role))
                        {
                            foreach (Employee emp in pro.EmpName)
                            {
                                if (emp.RoleName == role)
                                {
                                    emp.RoleName = null;
                                }
                            }
                        }
                    }
                    actionResult.Status = "Role is Removed Successfully from Projects!";
                }
                else
                {
                    actionResult.Status = "No project Present!";
                }
            }
            catch (Exception)
            {
                actionResult.IsSuccess = false;
                actionResult.Status = "Error occoured in Role Delete from Project";
            }

            return actionResult;
        }

        public ActionResult IsEmployeePresentInProject(uint id)
        {
            ActionResult actionResult = new ActionResult() { IsSuccess = true };
            int count = 0;
            if (_projectList.Count > 0)
            {
                foreach (Project pro in _projectList)
                {
                    if (pro.EmpName.Exists(p => p.Id == id))
                    {
                        count++;
                    }
                }
                if (count > 0)
                {
                    actionResult.Status = "Employee is Present!";
                }
                else
                {
                    actionResult.IsSuccess = false;
                    actionResult.Status = "Employee is not present in any project!";
                }
            }
            else
            {
                actionResult.IsSuccess = false;
            }
            return actionResult;
        }

        public ActionResult RestoreRoleToProject(string role, uint id)
        {
            ActionResult actionResult = new ActionResult() { IsSuccess = true };
            try
            {
                foreach (Project pro in _projectList)
                {
                    if (pro.EmpName.Exists(p => p.Id == id))
                    {
                        pro.EmpName.Single(p => p.Id == id).RoleName = role;
                    }
                }
                actionResult.Status = "Role Restored To Project!";
            }
            catch (Exception e)
            {
                actionResult.IsSuccess = false;
                actionResult.Status = "Role can not be restored to project!" + e.ToString();
            }

            return actionResult;
        }

        public ActionResult ToXmlSerialization()
        {
            ActionResult actionResult = new ActionResult() { IsSuccess = true };
            try
            {
                if (_projectList.Count > 0)
                {
                    /*string filePath = System.IO.Path.Combine(System.Web.HttpContext.Current.Server.MapPath("F:\\PPM\\PPM.Model"), "AppData", fileName)
                    var filePath = System.IO.File.Create(Environment.GetFolderPath(Environment.SpecialFolder.MyMusic) + fileName)
                    XmlSerializer serializer = new XmlSerializer(typeof(List<Project>));
                    using (TextWriter tw = new StreamWriter(fileName))
                    {
                        serializer.Serialize(tw, _projectList);
                        tw.Close();
                    }*/
                    XmlSerializer ProjectxmlSerializer = new XmlSerializer(typeof(List<Project>));
                    TextWriter ProjectFilestream = new StreamWriter(@"C:\Users\hp\source\repos\projectss.xml");
                    ProjectxmlSerializer.Serialize(ProjectFilestream, ProjectManager._projectList);
                    ProjectFilestream.Close();
                }
                else
                {
                    actionResult.IsSuccess = false;
                    actionResult.Status = "Project List is Empty!";
                }
            }
            catch (Exception e)
            {
                actionResult.IsSuccess = false;
                actionResult.Status = "Error Ocooured!" + e.Message;
            }
            return actionResult;
        }

        public ActionResult ToTxtFile(string fileName)
        {
            ActionResult actionResult = new ActionResult() { IsSuccess = true };
            try
            {
                if (_projectList.Count > 0)
                {
                    using (TextWriter sw = new StreamWriter(fileName))
                    {
                        foreach (Project pro in _projectList)
                        {
                            sw.WriteLine("Project ID: " + pro.ProjectId + "\nProject Name: " + pro.ProjectName + "\nStarting Date: " + pro.StartDate.ToShortDateString() + "\nEnding Date: " + pro.EndDate.ToShortDateString() + "\nBudget: " + pro.Budget);
                            sw.WriteLine("Employee Assigned:");
                            if (pro.EmpName != null)
                            {
                                foreach (Employee e in pro.EmpName)
                                {
                                    sw.WriteLine("Employee Id: " + e.Id + " " + "|" + "Employee Name : " + e.EmployeeName + " " + "|" + "Role : " + e.RoleName);
                                }
                            }
                            else
                            {
                                sw.WriteLine("No Employee Assigned!");
                            }
                            sw.WriteLine("-------------------------------------------------------");
                            actionResult.Status = "Project Is Saved in The Text File!";
                        }
                    }
                }
                else
                {
                    actionResult.IsSuccess = false;
                    actionResult.Status = "Project list is empty!";
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
            string str = "DROP TABLE IF EXISTS project";
            try
            {
                myConn.Open();
                using (SqlCommand command = new SqlCommand(str, myConn))
                {
                    command.ExecuteNonQuery();
                    actionResult.Status = "Old Data Dropped Successfully!";
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
                    using (SqlCommand command = new SqlCommand("CREATE TABLE project (ProjectId int,ProjectName varchar(50), StartDate date, EndDate date, Budget decimal, EmployeeAssigned Varchar(500));", myConn))
                    {
                        command.ExecuteNonQuery();

                        foreach (Project project in _projectList)
                        {
                            int id = (int)project.ProjectId;
                            string insertQ = "INSERT INTO project values(@ProjectId,@ProjectName,@StartDate, @EndDate, @Budget,@EmployeeAssigned)";
                            SqlCommand command1 = new SqlCommand(insertQ, myConn);
                            command1.Parameters.AddWithValue("@ProjectId", id);
                            command1.Parameters.AddWithValue("@ProjectName", project.ProjectName);
                            command1.Parameters.AddWithValue("@StartDate", project.StartDate);
                            command1.Parameters.AddWithValue("@EndDate", project.EndDate);
                            command1.Parameters.AddWithValue("@Budget", project.Budget);
                            StringBuilder sb = new StringBuilder();
                            foreach (Employee emp in project.EmpName)
                            {
                                string empDetails = emp.Id.ToString() + " : " + emp.RoleName;
                                sb.Append(empDetails);
                                sb.AppendLine(" | ");
                            }
                            command1.Parameters.AddWithValue("@EmployeeAssigned", sb.ToString());
                            command1.ExecuteNonQuery();
                        }
                        actionResult.Status = actionResult.Status + "\n" + "Table project Added SuccessFully!";
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

        public DataResult<Project> ToEFDB()
        {

            DataResult<Project> actionResult = new DataResult<Project>() { IsSuccess = true };
            try
            {

                if (_projectList.Count > 0)
                {

                    using (var db = new ProgramDbContext())
                    {
                        Project project1 = new Project();
                        List<Project> projectList = db.Projects.ToList();
                        foreach (Project project in _projectList)
                        {
                            project1.ProjectId = project.ProjectId;
                            project1.ProjectName = project.ProjectName;
                            project1.Budget = project.Budget;
                            project1.EndDate = Convert.ToDateTime(project.EndDate.ToShortDateString());
                            project1.StartDate = Convert.ToDateTime(project.StartDate.ToShortDateString());
                            if (projectList.Exists(p => p.ProjectId == project.ProjectId))
                            {
                                var p = projectList.Single(p => p.ProjectId == project.ProjectId);
                                db.Projects.Remove(p);
                                db.SaveChanges();

                                db.Projects.Add(project1);
                                db.SaveChanges();
                            }
                            else
                            {
                                db.Projects.Add(project1);
                                db.SaveChanges();
                            }

                        }
                    }
                    actionResult.Status = "Project Saved to Database Successfully";
                }
                else
                {
                    actionResult.IsSuccess = false;
                    actionResult.Status = "Project List is Empty!";
                }
            }
            catch (Exception e)
            {
                actionResult.IsSuccess = false;
                actionResult.Status = e.Message;
            }
            return actionResult;
        }
    }
}
