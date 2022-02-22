using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace inlineStock_rs.Models
{
    public class Model_Search
    {
        public string date_start { get; set; }
        public string date_end { get; set; }
        public string purpose_select { get; set; }
        public string type { get; set; }
        public string process { get; set; }
        public string emp_id { get; set; }
        public string password_axis { get; set; }

    }

  
}