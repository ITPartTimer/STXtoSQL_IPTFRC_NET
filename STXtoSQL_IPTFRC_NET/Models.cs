using System;
using System.Collections.Generic;
using System.Text;


namespace STXtoSQL.Models
{
    public class IPTFRC
    {
        
        public int job_no { get; set; }
        public int ln_no { get; set; }
        public int ctl_no { get; set; }
        public string tag { get; set; }
        public int nbr_stp { get; set; }
        public int arb_pos { get; set; }
        public string asgn_bal { get; set; }
        public string slit_typ { get; set; }        
    }
}
