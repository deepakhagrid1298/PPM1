using System;
using System.Collections.Generic;
using System.Text;

namespace PPM1.Model
{
    public class ActionResult
    {
        public bool IsSuccess { get; set; }
        public string Status { get; set; }

    }

    public class DataResult<T> : ActionResult
    {
        public IEnumerable<T> Results { get; set; }
    }
}
