using PPM1.Cons;
using PPM1.Domain;
using PPM1.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PPM.Cons
{
    public class RoleModule
    {
        public void AddRole()
        {
            Role role = new Role();
            try
            {
                Regex regx = new Regex(@"^[a-zA-Z ]+$");
                Console.Write("Enter Role Id: ");
                role.RoleId = Convert.ToUInt32(Console.ReadLine());
                Console.Write("Enter Role Name: ");
                role.RoleName = Console.ReadLine().ToUpper();
                if (!regx.IsMatch(role.RoleName))
                {
                    Console.WriteLine("Invalid type Role Name");
                    CommandInterface.StartProgram();
                }
            }
            catch (Exception)
            {
                Console.WriteLine("OOPs!Error Occoured, Try Again!");
                Console.WriteLine("-----------------------------------------------------");
                CommandInterface.RoleModule();
            }

            RoleManager roleManager = new RoleManager();
            var resultRole = roleManager.Add(role);
            if (!resultRole.IsSuccess)
            {
                Console.WriteLine("Role Failed to Add");
                Console.WriteLine(resultRole.Status);
            }
            else
            {
                Console.WriteLine(resultRole.Status);
            }
        }

        public void ViewAllRoleDetails()
        {
            RoleManager roleManager = new RoleManager();
            Console.WriteLine("Role Details:");
            var resRole = roleManager.ViewListAll();
            if (resRole.IsSuccess)
            {
                foreach (Role r in resRole.Results)
                {
                    Console.WriteLine("Role Id: " + r.RoleId + "\nRole Name: " + r.RoleName);
                    Console.WriteLine("-----------------------------------------------------");
                }
            }
            else
            {
                Console.WriteLine(resRole.Status);
            }
        }

        public void ViewRoleDetailsById()
        {
            RoleManager roleManager = new RoleManager();
            Console.Write("Enter the Role Id: ");
            uint id = Convert.ToUInt32(Console.ReadLine());
            var resRole = roleManager.ValidRole(id);
            if (resRole.IsSuccess)
            {
                var r = roleManager.ViewListById(id);
                Console.WriteLine("Role Id: " + r.RoleId + "\nRole Name: " + r.RoleName);
                Console.WriteLine("-----------------------------------------------------");
            }
        }

        public void DeleteRole()
        {
            RoleManager roleManager = new RoleManager();
            EmployeeManager employeeManager = new EmployeeManager();
            ProjectManager projectManager = new ProjectManager();
            Console.WriteLine("Choose From below list of Role --> Id : Role Name");
            var resRole = roleManager.ViewListAll();
            if (resRole.IsSuccess)
            {
                foreach (Role r in resRole.Results)
                {
                    Console.WriteLine(r.RoleId + " : " + r.RoleName);
                    Console.WriteLine("-----------------------------------------------------");
                }
            }
            else
            {
                Console.WriteLine(resRole.Status);
            }
            Console.Write("Enter the Role Id: ");
            uint id = Convert.ToUInt32(Console.ReadLine());
            var validRole = roleManager.ValidRole(id);
            if (validRole.IsSuccess)
            {
                Console.WriteLine(validRole.Status);
                var getRoleName = roleManager.ViewListById(id);
                var isRolePresent = employeeManager.IsRolePresent(getRoleName.RoleName);
                if (isRolePresent.IsSuccess)
                {
                    Console.WriteLine(isRolePresent.Status);
                    Console.Write("Are you sure You want to delete the role?(y/n): ");
                    char ch = Convert.ToChar(Console.ReadLine().ToLower());
                    switch (ch)
                    {
                        case 'y':
                            var deleteRole = roleManager.Delete(id);
                            if (deleteRole.IsSuccess)
                            {
                                Console.WriteLine(deleteRole.Status);
                                var roleDeleteFromEmployee = employeeManager.DeleteRoleFromEmployee(getRoleName.RoleName);
                                if (roleDeleteFromEmployee.IsSuccess)
                                {
                                    Console.WriteLine(roleDeleteFromEmployee.Status);
                                    var roleDeleteFromProject = projectManager.RoleDeleteFromProject(getRoleName.RoleName);
                                    if (roleDeleteFromProject.IsSuccess)
                                    {
                                        Console.WriteLine(roleDeleteFromProject.Status);
                                        Console.WriteLine("Role Deleted From Every Entity Successfully!");
                                    }
                                    else
                                    {
                                        Console.WriteLine(roleDeleteFromProject.Status);
                                    }
                                }
                                else
                                {
                                    Console.WriteLine(roleDeleteFromEmployee.Status);
                                }
                            }
                            else
                            {
                                Console.WriteLine(deleteRole.Status);
                            }
                            break;

                        case 'n':
                            Console.WriteLine("Role Delete Cancelled!");
                            Console.WriteLine("------------------------------------------------");
                            CommandInterface.RoleModule();
                            break;
                    }
                }
                else
                {
                    var deleteRole = roleManager.Delete(id);
                    if (deleteRole.IsSuccess)
                    {
                        Console.WriteLine(deleteRole.Status);
                        Console.WriteLine("Delete Role SuccessFully!");
                    }
                    else
                    {
                        Console.WriteLine(deleteRole.Status);
                    }
                }
            }
            else
            {
                Console.WriteLine(validRole.Status);
            }

        }
    }
}
