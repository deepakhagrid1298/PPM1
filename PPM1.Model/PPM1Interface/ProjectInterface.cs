using System;
using System.Collections.Generic;
using System.Text;

namespace PPM1.Model.PPM1Interface
{
    public interface ProjectInterface : ProgramInterface<Project>
    {
        ActionResult IsValidID(uint id);
        ActionResult AddEmpToProject(Employee employee, uint id);
        ActionResult RemoveEmpFromProject(uint id, Employee employee);
        ActionResult EmployeeDeleteFromProject(uint id);
        ActionResult RoleDeleteFromProject(string role);
        ActionResult IsEmployeePresentInProject(uint id);
        ActionResult RestoreRoleToProject(string role, uint id);
    }
}
