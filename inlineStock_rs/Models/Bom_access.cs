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
    
    public partial class Bom_access
    {
        public int B_id { get; set; }
        public string Biz { get; set; }
        public string Process { get; set; }
        public string Material_Code { get; set; }
        public string Material_Name { get; set; }
        public string Material_Group { get; set; }
        public string Material_Category { get; set; }
        public string Item_Name { get; set; }
        public string Category { get; set; }
        public Nullable<int> Cost_Center { get; set; }
        public Nullable<double> Usage_perIC { get; set; }
    }
}
