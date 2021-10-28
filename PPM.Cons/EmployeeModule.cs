using PPM1.Domain;
using PPM1.Model;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PPM1.Cons
{
    public class EmployeeModule
    {
        public void AddEmployee()
        {
            Employee employee = new Employee();
            try
            {
                Regex regx = new Regex(@"^[a-zA-Z ]+$");
                Console.Write("Enter Employee Id: ");
                employee.Id = Convert.ToUInt32(Console.ReadLine());
                Console.Write("Enter Employee FirstName: ");
                employee.FirstName = Console.ReadLine();
                Console.Write("Enter Employee LastName: ");
                employee.LastName = Console.ReadLine();
                while (!regx.IsMatch(employee.FirstName) || !regx.IsMatch(employee.LastName))
                {
                    Console.WriteLine("Invalid type FirstName or Last Name! Please Try Again!");
                    Console.Write("Enter Employee FirstName: ");
                    employee.FirstName = Console.ReadLine();
                    Console.Write("Enter Employee LastName: ");
                    employee.LastName = Console.ReadLine();
                }
                employee.EmployeeName = String.Concat(employee.FirstName.ToUpper(), " ", employee.LastName.ToUpper());
                Console.Write("Enter Employee DOB: ");
                employee.DOB = Convert.ToDateTime(Console.ReadLine());
                Console.Write("Enter Employee Contact: ");
                employee.Contact = Convert.ToUInt64(Console.ReadLine());
            }
            catch (Exception)
            {
                Console.WriteLine("OOPs!Error Occoured, Try Again!");
                Console.WriteLine("-----------------------------------------------------");
                CommandInterface.EmployeeModule();
            }
            RoleManager role = new RoleManager();
            Console.WriteLine("Choose the Role From below options:");
            var resRole = role.ViewListAll();
            if (resRole.IsSuccess)
            {
                foreach (Role r in resRole.Results)
                {
                    Console.WriteLine(r.RoleName);
                }
            }
            else
            {
                Console.WriteLine(resRole.Status);
                Console.WriteLine("Please Add Role First!");
                Console.WriteLine("--------------------------------------------");
                CommandInterface.EmployeeModule();
            }
            Console.WriteLine("Enter the Role of the Employee: ");
            employee.RoleName = Console.ReadLine().ToUpper();
            var validRole = role.ValidRoleByName(employee.RoleName);
            if (validRole.IsSuccess)
            {
                EmployeeManager employeeManager = new EmployeeManager();

                var resultEmployee = employeeManager.Add(employee);
                if (!resultEmployee.IsSuccess)
                {
                    Console.WriteLine("Employee Failed to add!");
                    Console.WriteLine(resultEmployee.Status);
                }
                else
                {
                    Console.WriteLine(resultEmployee.Status);
                }
            }
            else
            {
                Console.WriteLine(validRole.Status);
            }

        }

        public void ViewAllEmployeeDetails()
        {
            EmployeeManager employeeManager = new EmployeeManager();
            Console.WriteLine("Employee Details:");
            var employeeResult = employeeManager.ViewListAll();
            if (employeeResult.IsSuccess)
            {
                foreach (Employee e in employeeResult.Results)
                {
                    Console.WriteLine("Employee Id: " + e.Id + "\nEmployee Name: " + e.EmployeeName + "\nDOB: " + e.DOB.ToShortDateString() + "\nContact Number: " + e.Contact + "\nRole: " + e.RoleName);
                    Console.WriteLine("-----------------------------------------------------");
                }
            }
            else
            {
                Console.WriteLine(employeeResult.Status);
            }
        }

        public void ViewEmployeeDetailsById()
        {
            EmployeeManager employeeManager = new EmployeeManager();
            Console.Write("Enter the Employee Id: ");
            uint id = Convert.ToUInt32(Console.ReadLine());
            var employeeResult = employeeManager.ValidEmployee(id);
            if (employeeResult.IsSuccess)
            {
                var e = employeeManager.ViewListById(id);
                Console.WriteLine("Employee Id: " + e.Id + "\nEmployee Name: " + e.EmployeeName + "\nDOB: " + e.DOB.ToShortDateString() + "\nContact Number: " + e.Contact + "\nRole: " + e.RoleName);
                Console.WriteLine("-----------------------------------------------------");
            }
        }

        public void DeleteEmployee()
        {
            EmployeeManager employeeManager = new EmployeeManager();
            ProjectManager projectManager = new ProjectManager();

            Console.WriteLine("Choose From below list of Employee --> Id : Employee Name ");
            var empList = employeeManager.ViewListAll();
            if (empList.IsSuccess)
            {
                foreach (Employee e in empList.Results)
                {
                    Console.WriteLine(e.Id + " : " + e.EmployeeName);
                    Console.WriteLine("-----------------------------------------------------");
                }
            }
            else
            {
                Console.WriteLine(empList.Status);
            }

            Console.Write("Enter the Employee Id: ");
            uint id = Convert.ToUInt32(Console.ReadLine());
            var validEmp = employeeManager.ValidEmployee(id);
            if (validEmp.IsSuccess)
            {
                var empAssigned = projectManager.EmployeeDeleteFromProject(id);
                if (empAssigned.IsSuccess)
                {
                    Console.WriteLine(empAssigned.Status);
                    var delEmployee = employeeManager.Delete(id);
                    if (delEmployee.IsSuccess)
                    {
                        Console.WriteLine("Delete Employee Successful!");
                        Console.WriteLine(delEmployee.Status);
                    }
                    else
                    {
                        Console.WriteLine(delEmployee.Status);
                    }
                }
                else
                {
                    Console.WriteLine(empAssigned.Status);
                }
            }
            else
            {
                Console.WriteLine(validEmp.Status);
            }
        }

        public void RestoreRoleToEmployee()
        {
            EmployeeManager employeeManager = new EmployeeManager();
            RoleManager roleManager = new RoleManager();
            ProjectManager projectManager = new ProjectManager();
            Console.WriteLine("Below is the list of Employees with No Role Assigned:");
            var resEmp = employeeManager.ViewListAll();
            if (resEmp.IsSuccess)
            {
                int count = 0;
                foreach (Employee e in resEmp.Results)
                {
                    if (e.RoleName == null)
                    {
                        Console.WriteLine(e.Id + " : " + e.EmployeeName);
                        count++;
                    }
                }
                if (count == 0)
                {
                    Console.WriteLine("All Employees are assigned to a specific Role!");
                    CommandInterface.EmployeeModule();
                }
            }
            else
            {
                Console.WriteLine(resEmp.Status);
                Console.WriteLine("Please Add Employee First!");
                Console.WriteLine("-----------------------------------------------------");
                CommandInterface.StartProgram();
            }
            Console.Write("Enter the Employee Id: ");
            uint EmpId = Convert.ToUInt32(Console.ReadLine());
            var validEmp = employeeManager.ValidEmployee(EmpId);
            if (validEmp.IsSuccess)
            {
                Console.WriteLine("Choose the Role From below options:");
                var resRole = roleManager.ViewListAll();
                if (resRole.IsSuccess)
                {
                    foreach (Role r in resRole.Results)
                    {
                        Console.WriteLine("Role Name: " + r.RoleName);
                    }
                }
                else
                {
                    Console.WriteLine(resRole.Status);
                    Console.WriteLine("Please Add Role First!");
                    Console.WriteLine("--------------------------------------------");
                    CommandInterface.EmployeeModule();
                }
                Console.Write("Enter the Role of the Employee: ");
                string role = Console.ReadLine().ToUpper();
                var validRole = roleManager.ValidRoleByName(role);
                if (validRole.IsSuccess)
                {
                    var roleToEmp = employeeManager.RestoreRoleToEmployee(EmpId, role);
                    if (roleToEmp.IsSuccess)
                    {
                        var empPresentInProject = projectManager.IsEmployeePresentInProject(EmpId);
                        if (empPresentInProject.IsSuccess)
                        {
                            var roleToProject = projectManager.RestoreRoleToProject(role, EmpId);
                            if (roleToProject.IsSuccess)
                            {
                                Console.WriteLine(roleToProject.Status);
                                Console.WriteLine("Successful");
                            }
                            else
                            {
                                Console.WriteLine(roleToProject.Status);
                            }
                        }
                        Console.WriteLine(roleToEmp.Status);
                    }
                    else
                    {
                        Console.WriteLine(roleToEmp.Status);
                    }
                }
                else
                {
                    Console.WriteLine(validRole.Status);
                }
            }
            else
            {
                Console.WriteLine(validEmp.Status);
            }

        }
    }
}
