//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace inlineStock_rs.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class tb_log_reason
    {
        public int ID { get; set; }
        public string ITEM_NUMBER { get; set; }
        public string MATCODE { get; set; }
        public string BIZ { get; set; }
        public string PROCESS { get; set; }
        public string COST_CENTER { get; set; }
        public string REASON { get; set; }
        public Nullable<System.DateTime> CREATE_DATE { get; set; }
        public string CREATE_BY { get; set; }
    }
}