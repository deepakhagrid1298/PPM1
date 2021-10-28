using System;
using System.Collections.Generic;
using System.Text;

namespace PPM1.Model.PPM1Interface
{
	public interface RoleInterface : ProgramInterface<Role>
	{
		ActionResult ValidRole(uint id);
		ActionResult ValidRoleByName(string role);
	}
}
