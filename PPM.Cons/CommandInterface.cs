using System;
using System.Collections.Generic;
using PPM1.Domain;
using System.Text.RegularExpressions;
using PPM.Cons;

namespace PPM1.Cons
{
    public static class CommandInterface
    {
        public static void StartProgram()
        {
            Console.WriteLine("---Main Menu---");
            Console.WriteLine("Press 1: Project Module");
            Console.WriteLine("Press 2: Employee Module");
            Console.WriteLine("Press 3: Role Module");
            Console.WriteLine("Press 4: Save");
            Console.WriteLine("Press 5: Exit");
            while (true)
            {
                try
                {
                    Console.Write("Choose from 1 to 5: ");
                    int i = Convert.ToInt32(Console.ReadLine());
                    switch (i)
                    {
                        case 1:
                            ProjectModule();
                            break;
                        case 2:
                            EmployeeModule();
                            break;
                        case 3:
                            RoleModule();
                            break;
                        case 4:
                            Save();
                            break;
                        case 5:
                            Environment.Exit(i);
                            break;
                        default:
                            Console.WriteLine("---Option is not in the list!---S");
                            break;
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("OOPS!Error Occoured!Try Again");
                    Console.WriteLine("-----------------------------------------------------");
                    StartProgram();
                }


            }
        }
        public static void ProjectModule()
        {
            Console.WriteLine("Choose option from below:");
            Console.WriteLine("Press 1: Add Project");
            Console.WriteLine("Press 2: View All Project Details");
            Console.WriteLine("press 3: View Project Details By Id");
            Console.WriteLine("Press 4: Delete Project");
            Console.WriteLine("Press 5: Add Employee to Project");
            Console.WriteLine("Press 6: Delete Employee From Project");
            Console.WriteLine("Press 7: Main Menu");
            bool k = true;
            while (k)
            {
                try
                {
                    ProjectModule projectModule = new ProjectModule();
                    Console.Write("Choose Your Option from 1 to 7: ");
                    int i = Convert.ToInt32(Console.ReadLine());
                    switch (i)
                    {
                        case 1:
                            projectModule.AddProject();
                            break;
                        case 2:
                            projectModule.ViewAllProjectDetails();
                            break;
                        case 3:
                            projectModule.ViewProjectDetailsById();
                            break;
                        case 4:
                            projectModule.DeleteProject();
                            break;
                        case 5:
                            projectModule.AddEmpToProject();
                            break;
                        case 6:
                            projectModule.RemoveEmpFromProject();
                            break;
                        case 7:
                            StartProgram();
                            break;
                        default:
                            Console.WriteLine("Option is not in the list!");
                            break;

                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Error Occoured at Project Module!");
                    Console.WriteLine("-----------------------------------------------------");
                    StartProgram();
                }

            }


        }
        public static void EmployeeModule()
        {
            Console.WriteLine("Choose the option you want to select:");
            Console.WriteLine("Press 1: Add Employee");
            Console.WriteLine("Press 2: View All Employee Details");
            Console.WriteLine("press 3: View Employee Details By Id");
            Console.WriteLine("Press 4: Delete Employee");
            Console.WriteLine("Press 5: Add Role to Employee");
            Console.WriteLine("Press 6: Main Menu");
            Console.WriteLine("----------------------------------------------------");

            while (true)
            {
                try
                {
                    EmployeeModule employeeModule = new EmployeeModule();
                    Console.Write("Choose Your Option from 1 to 6: ");
                    int i = Convert.ToInt32(Console.ReadLine());
                    switch (i)
                    {
                        case 1:
                            employeeModule.AddEmployee();
                            break;
                        case 2:
                            employeeModule.ViewAllEmployeeDetails();
                            break;
                        case 3:
                            employeeModule.ViewEmployeeDetailsById();
                            break;
                        case 4:
                            employeeModule.DeleteEmployee();
                            break;
                        case 5:
                            employeeModule.RestoreRoleToEmployee();
                            break;
                        case 6:
                            StartProgram();
                            break;
                        default:
                            Console.WriteLine("Option is not in the list!");
                            break;

                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Error Occoured at Employee Module!");
                    Console.WriteLine("-----------------------------------------------------");
                    StartProgram();
                }

            }
        }
        public static void RoleModule()
        {
            Console.WriteLine("Choose the option you want to select:");
            Console.WriteLine("Press 1: Add Role");
            Console.WriteLine("Press 2: View All Role Details");
            Console.WriteLine("press 3: View Role Details By Id");
            Console.WriteLine("Press 4: Delete Role");
            Console.WriteLine("Press 5: Main Menu");

            while (true)
            {
                try
                {
                    RoleModule roleModule = new RoleModule();
                    Console.Write("Choose Your Option from 1 to 5: ");
                    int i = Convert.ToInt32(Console.ReadLine());
                    switch (i)
                    {
                        case 1:
                            roleModule.AddRole();
                            break;
                        case 2:
                            roleModule.ViewAllRoleDetails();
                            break;
                        case 3:
                            roleModule.ViewRoleDetailsById();
                            break;
                        case 4:
                            roleModule.DeleteRole();
                            break;
                        case 5:
                            StartProgram();
                            break;
                        default:
                            Console.WriteLine("Option is not in the list!");
                            break;

                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Error Occoured at Role Module!");
                    Console.WriteLine("-----------------------------------------------------");
                    StartProgram();
                }
            }
        }

        public static void Save()
        {
            try
            {
                Console.WriteLine("Press 1: Save as XML file");
                Console.WriteLine("Press 2: Save as TXT File");
                Console.WriteLine("Press 3: Save as DB-Ado");
                Console.WriteLine("Press 4: Save as DB-EF");
                Console.Write("Please Choose from 1 to 4: ");
                int choice = Convert.ToInt32(Console.ReadLine());
                EmployeeManager employeeManager = new EmployeeManager();
                ProjectManager projectManager = new ProjectManager();
                RoleManager roleManager = new RoleManager();
                switch (choice)
                {

                    case 1:
                        var employeeSerialize = employeeManager.ToXmlSerialization();
                        var projectSerialize = projectManager.ToXmlSerialization();
                        var roleSerialize = roleManager.ToXmlSerialization();

                        if (employeeSerialize.IsSuccess || projectSerialize.IsSuccess || roleSerialize.IsSuccess)
                        {
                            Console.WriteLine("Save Data Successfully!");
                            Console.WriteLine(employeeSerialize.Status + "\n" + projectSerialize.Status + "\n" + roleSerialize.Status);
                        }
                        else
                        {
                            Console.WriteLine(employeeSerialize.Status + "\n" + projectSerialize.Status + "\n" + roleSerialize.Status);
                        }
                        break;
                    case 2:
                        var saveRoleToText = roleManager.ToTxtFile("role.txt");
                        var saveEmployeeToText = employeeManager.ToTxtFile("SaveEmployee.txt");
                        var saveProjectToText = projectManager.ToTxtFile("SaveProject.txt");
                        if (saveRoleToText.IsSuccess || saveEmployeeToText.IsSuccess || saveProjectToText.IsSuccess)
                        {
                            Console.WriteLine("Save Data To TEXT File Successfully!");
                            Console.WriteLine(saveRoleToText.Status + "\n" + saveProjectToText.Status + "\n" + saveEmployeeToText.Status);
                        }
                        else
                        {
                            Console.WriteLine(saveRoleToText.Status + "\n" + saveProjectToText.Status + "\n" + saveEmployeeToText.Status);
                        }
                        break;
                    case 3:
                        DBModule dB = new DBModule();
                        dB.DB_ADO();
                        break;
                    case 4:
                        DBModule dB1 = new DBModule();
                        dB1.DB_EF();
                        break;
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Oops, Error Occoured at Save State!");
                Console.WriteLine("-----------------------------------------------------");
                StartProgram();
            }

        }

    }
}
