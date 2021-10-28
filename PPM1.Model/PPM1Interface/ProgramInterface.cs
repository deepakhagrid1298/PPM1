using System;
using System.Collections.Generic;
using System.Text;

namespace PPM1.Model.PPM1Interface
{
	public interface ProgramInterface<T>
	{
		ActionResult Add(T t);
		DataResult<T> ViewListAll();
		T ViewListById(uint id);
		ActionResult Delete(uint id);
	}
}
