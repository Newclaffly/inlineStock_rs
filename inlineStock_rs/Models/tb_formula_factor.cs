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
    
    public partial class tb_formula_factor
    {
        public int ID { get; set; }
        public string MATCODE { get; set; }
        public string ITEM_NUMBER { get; set; }
        public string FLAG_MAT { get; set; }
        public string FACTOR_USAGE { get; set; }
        public string FLAG_USAGE { get; set; }
        public Nullable<double> FACTOR { get; set; }
        public string UNIT { get; set; }
        public string FLAG_CAL { get; set; }
        public Nullable<System.DateTime> UPDATE_DATE { get; set; }
        public string UPDATE_BY { get; set; }
    }
}
