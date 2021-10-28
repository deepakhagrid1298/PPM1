using System;
using NUnit.Framework;
using PPM1.Cons;
using PPM1.Domain;
using PPM1.Model;

namespace PPM1Project
{
    public class UnitTest1
    {

        [Test]
        public void AddProjectTest1()
        {
            ProjectManager Pro = new ProjectManager();
            Project P1 = new Project();
            P1.ProjectId = 1;
            P1.ProjectName = "Prince";
            P1.StartDate = Convert.ToDateTime("1-3-2021");
            P1.EndDate = Convert.ToDateTime("3-3-2021");
            P1.Budget = 2000;
            var V2 = Pro.Add(P1);
            if (V2.IsSuccess)
            {
                Assert.Pass();

            }
            else
            {
                Assert.Fail();

            }


        }

        [Test]
        public void AddEmployeeTest1()
        {
            EmployeeManager Emo = new EmployeeManager();
            Employee E1 = new Employee();
            E1.Id = 2;
            E1.FirstName = "ps";
            E1.LastName = "ku";
            E1.DOB = Convert.ToDateTime("09-09-2019");
            E1.Contact = 9123446828;
            E1.RoleName = "Engineer";
            var V3 = Emo.Add(E1);
            if (V3.IsSuccess)
            {
                Assert.Pass();
            }
            else
            {
                Assert.Fail();
            }
        }
        [Test]
        public void AddRoleTest1()
        {
            RoleManager role = new RoleManager();
            Role RR = new Role();
            RR.RoleId = 2;
            RR.RoleName = "SD-1";
            var v4 = role.Add(RR);
            if (v4.IsSuccess)
            {
                Assert.Pass();
            }
            else
            {
                Assert.Fail();
            }
        }
        [Test]
        public void AddEmployeetoProjectTest2()
        {
            Employee emp = new Employee();
            EmployeeManager em = new EmployeeManager();
            uint Pro_Id = 1;
            emp.FirstName = "ps";
            var v1 = em.ValidEmployee(Pro_Id);
            if (!v1.IsSuccess)
            {
                ProjectManager projectManager = new ProjectManager();
                var r1 = projectManager.AddEmpToProject(emp, Pro_Id);
                if (!r1.IsSuccess)
                {
                    Assert.Pass();
                }
                else
                {
                    Assert.Fail();
                }
            }
        }
        [Test]
        public void DELETE_employeefromProj()
        {
            Employee emp = new Employee();
            EmployeeManager em = new EmployeeManager();
            uint Pro_Id = 1;
            emp.FirstName = "ps";
            var v1 = em.ValidEmployee(Pro_Id);
            if (v1.IsSuccess)
            {
                ProjectManager projectManager = new ProjectManager();
                var r1 = projectManager.AddEmpToProject(emp, Pro_Id);
                if (r1.IsSuccess)
                {
                    Assert.Pass();
                }
                else
                {
                    Assert.Fail();
                }
            }
        }
    }
}