using PPM1.Cons;
using PPM1.Domain;
using PPM1.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PPM.Cons
{
    public class ProjectModule
    {
        public void AddProject()
        {
            Project project = new Project();
            try
            {
                Regex regx = new Regex(@"^([a-zA-Z\s][a-zA-Z0-9]{1,50})$");
                Console.Write("Enter Project ID: ");
                project.ProjectId = Convert.ToUInt32(Console.ReadLine());
                Console.Write("Enter Project Name: ");
                project.ProjectName = Console.ReadLine().ToUpper();
                Console.Write("Enter Project Budget: ");
                project.Budget = Convert.ToDecimal(Console.ReadLine());
                while (project.Budget <= 0 || !regx.IsMatch(project.ProjectName))
                {
                    Console.WriteLine("UnAuthentic Value!");
                    Console.Write("Enter Project Name: ");
                    project.ProjectName = Console.ReadLine().ToUpper();
                    Console.Write("Enter Project Budget: ");
                    project.Budget = Convert.ToDecimal(Console.ReadLine());
                }
                Console.Write("Enter Project Starting Date: ");
                project.StartDate = Convert.ToDateTime(Console.ReadLine());
                Console.Write("Enter Project End date: ");
                project.EndDate = Convert.ToDateTime(Console.ReadLine());

            }
            catch (Exception)
            {
                Console.WriteLine("OOPs!Error Occoured, Try Again!");
                Console.WriteLine("-----------------------------------------------------");
                CommandInterface.ProjectModule();
            }
            ProjectManager projectManager = new ProjectManager();
            var resultProject = projectManager.Add(project);
            if (!resultProject.IsSuccess)
            {
                Console.WriteLine("Project failed to Add");
                Console.WriteLine(resultProject.Status);
            }
            else
            {
                Console.WriteLine(resultProject.Status);
            }
            while (true)
            {
                try
                {
                    Console.Write("Do you want to Add Employee To Project?(y/n): ");
                    string choice = Console.ReadLine().ToLower();
                    switch (choice)
                    {
                        case "y":
                            EmployeeManager employeeManager = new EmployeeManager();
                            Console.WriteLine("Below is the Employee ID and respective Name to choose:");
                            var resEmp = employeeManager.ViewListAll();
                            if (resEmp.IsSuccess)
                            {
                                foreach (Employee e in resEmp.Results)
                                {
                                    Console.WriteLine(e.Id + " : " + e.EmployeeName);
                                }
                            }
                            else
                            {
                                Console.WriteLine(resEmp.Status);
                                Console.WriteLine("Please Add Employee First!");
                                Console.WriteLine("-----------------------------------------------------");
                                CommandInterface.ProjectModule();
                            }
                            Console.Write("Enter the Id of the employee: ");
                            uint Id = Convert.ToUInt32(Console.ReadLine());
                            var valid = employeeManager.ValidEmployee(Id);
                            if (valid.IsSuccess)
                            {

                                var obj = employeeManager.ViewListById(Id);
                                var result = projectManager.AddEmpToProject(obj, project.ProjectId);

                                if (!result.IsSuccess)
                                {
                                    Console.WriteLine("Failed to Add Employee into project");
                                    Console.WriteLine(result.Status);
                                }
                                else
                                {
                                    Console.WriteLine(result.Status);
                                }
                            }
                            else
                            {
                                Console.WriteLine(valid.Status);
                            }

                            break;
                        case "n":
                            CommandInterface.ProjectModule();
                            break;
                        default:
                            Console.WriteLine("Invalid Option!");
                            break;
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Oops, Some Error Occoured in Add Employee to Project!");
                    ProjectModule projectModule = new ProjectModule();
                    projectModule.AddProject();
                }

            }

        }

        public void ViewAllProjectDetails()
        {
            ProjectManager Manager = new ProjectManager();
            Console.WriteLine("All Project Details:");
            var resultPro = Manager.ViewListAll();
            if (resultPro.IsSuccess)
            {
                foreach (Project result in resultPro.Results)
                {
                    Console.WriteLine("Project ID: " + result.ProjectId + "\nProject Name: " + result.ProjectName + "\nStarting Date: " + result.StartDate.ToShortDateString() + "\nEnding Date: " + result.EndDate.ToShortDateString() + "\nBudget: " + result.Budget);
                    Console.WriteLine("Employee Assigned: ");
                    if (result.EmpName != null)
                    {
                        foreach (Employee e in result.EmpName)
                        {
                            Console.WriteLine("Employee Id: " + e.Id + " " + "|" + "Employee Name : " + e.EmployeeName + " " + "|" + "Role : " + e.RoleName);
                        }
                    }
                    else
                    {
                        Console.WriteLine("No Employee Assigned!");
                    }
                    Console.WriteLine("-----------------------------------------------------");

                }
            }
            else
            {
                Console.WriteLine(resultPro.Status);
            }
        }

        public void ViewProjectDetailsById()
        {
            ProjectManager Manager = new ProjectManager();
            Console.Write("Enter the Project Id: ");
            uint i = Convert.ToUInt32(Console.ReadLine());
            var valid = Manager.IsValidID(i);
            if (valid.IsSuccess)
            {
                var result = Manager.ViewListById(i);
                Console.WriteLine("Project ID: " + result.ProjectId + "\nProject Name: " + result.ProjectName + "\nStarting Date: " + result.StartDate.ToShortDateString() + "\nEnding Date: " + result.EndDate.ToShortDateString() + "\nBudget: " + result.Budget);
                Console.WriteLine("Employee Assigned: ");
                if (result.EmpName != null)
                {
                    foreach (Employee e in result.EmpName)
                    {
                        Console.WriteLine("Employee Id: " + e.Id + " " + "|" + "Employee Name : " + e.EmployeeName + " " + "|" + "Role : " + e.RoleName);
                    }
                }
                else
                {
                    Console.WriteLine("No Employee Assigned!");
                }
                Console.WriteLine("--------------------------------------");
            }
            else
            {
                Console.WriteLine(valid.Status);
            }

        }

        public void DeleteProject()
        {
            ProjectManager projectManager = new ProjectManager();
            Console.WriteLine("Choose Project Id From Below List:");
            var resultPro = projectManager.ViewListAll();
            if (resultPro.IsSuccess)
            {
                foreach (Project result in resultPro.Results)
                {
                    Console.WriteLine(result.ProjectId + " : " + result.ProjectName);

                }
            }
            else
            {
                Console.WriteLine(resultPro.Status);
                Console.WriteLine("No Project is there to Delete!");
                Console.WriteLine("------------------------------------------------------");
                CommandInterface.ProjectModule();
            }
            Console.Write("Enter the Project Id: ");
            uint id = Convert.ToUInt32(Console.ReadLine());
            var valid = projectManager.IsValidID(id);
            if (valid.IsSuccess)
            {
                var result = projectManager.Delete(id);
                if (result.IsSuccess)
                {
                    Console.WriteLine(result.Status);
                    Console.WriteLine("Project Removed!");
                }
                else
                {
                    Console.WriteLine(result.Status);
                }
            }
            else
            {
                Console.WriteLine(valid.Status);
            }
        }

        public void AddEmpToProject()
        {
            ProjectManager projectManager = new ProjectManager();
            EmployeeManager employeeManager = new EmployeeManager();
            Console.WriteLine("Choose Project From Below Project List: Project ID : Project Name");
            var resPro = projectManager.ViewListAll();
            if (resPro.IsSuccess)
            {
                foreach (Project result in resPro.Results)
                {
                    Console.WriteLine(result.ProjectId + " : " + result.ProjectName);
                }
            }
            else
            {
                Console.WriteLine(resPro.Status);
                Console.WriteLine("Please Add Project First!");
                Console.WriteLine("---------------------------------------------------");
                CommandInterface.ProjectModule();
            }
            Console.Write("Provide the project Id: ");
            uint projectId = Convert.ToUInt32(Console.ReadLine());
            Console.WriteLine("Below is the Employee ID and respective Name to choose:");
            var resEmp = employeeManager.ViewListAll();
            if (resEmp.IsSuccess)
            {
                foreach (Employee e in resEmp.Results)
                {
                    Console.WriteLine(e.Id + " : " + e.EmployeeName);
                }
            }
            else
            {
                Console.WriteLine(resEmp.Status);
                Console.WriteLine("Please Add Employee First!");
                Console.WriteLine("-----------------------------------------------------");
                CommandInterface.ProjectModule();
            }
            Console.Write("Enter the Id of the employee: ");
            uint Id = Convert.ToUInt32(Console.ReadLine());
            var valid = employeeManager.ValidEmployee(Id);
            if (valid.IsSuccess)
            {

                var obj = employeeManager.ViewListById(Id);
                var result = projectManager.AddEmpToProject(obj, projectId);

                if (!result.IsSuccess)
                {
                    Console.WriteLine("Failed to Add Employee into project");
                    Console.WriteLine(result.Status);
                }
                else
                {
                    Console.WriteLine(result.Status);
                }
            }
            else
            {
                Console.WriteLine(valid.Status);
            }
        }

        public void RemoveEmpFromProject()
        {
            ProjectManager projectManager = new ProjectManager();
            Employee employee = new Employee();
            Console.WriteLine("Choose Project From Below Project List: Project ID : Project Name");
            var resProject = projectManager.ViewListAll();
            if (resProject.IsSuccess)
            {
                foreach (Project res in resProject.Results)
                {
                    Console.WriteLine(res.ProjectId + " : " + res.ProjectName);
                }
            }
            else
            {
                Console.WriteLine(resProject.Status);
            }

            Console.Write("Enter The project Id From Employee Should Be removed: ");
            uint projectId = Convert.ToUInt32(Console.ReadLine());
            Console.WriteLine($"Choose The Employee Id in Selected Project Id: {projectId} From the Following List: Employee ID : Employee Name");
            var empList = projectManager.ViewListById(projectId);
            foreach (Employee res in empList.EmpName)
            {
                Console.WriteLine(res.Id + " : " + res.EmployeeName);
            }
            Console.Write("Enter the ID of the Employee to remove: ");
            employee.Id = Convert.ToUInt32(Console.ReadLine());
            var result = projectManager.RemoveEmpFromProject(projectId, employee);
            if (!result.IsSuccess)
            {
                Console.WriteLine("Employee Removed");
                Console.WriteLine(result.Status);
            }
            else
            {
                Console.WriteLine(result.Status);
            }
        }
    }
}