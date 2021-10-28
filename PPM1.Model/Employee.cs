using System;
using System.Collections.Generic;
using System.Text;

namespace PPM1.Model
{
    public class Employee
    {
        public uint Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmployeeName { get; set; }
        public DateTime DOB { get; set; }
        public ulong Contact { get; set; }
        public string RoleName { get; set; }
    }
}
