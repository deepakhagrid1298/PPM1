using System;
using System.Collections.Generic;
using System.Text;

namespace PPM1.Model.PPM1Interface
{
	public interface EmployeeInterface : ProgramInterface<Employee>
	{
		ActionResult ValidEmployee(uint id);
		ActionResult RestoreRoleToEmployee(uint id, string role);
		ActionResult DeleteRoleFromEmployee(string role);
		ActionResult IsRolePresent(string role);
	}
}
