using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSCEOfflineRegSchApp.Model
{
    public class SubjectSaveModel
    {
        public string SerialNumber { get; set; }
        public string Subj1 { get; set; }
        public string Subj2 { get; set; }
        public string Subj3 { get; set; }
        public string Subj4 { get; set; }
        public string Subj5 { get; set; }
        public string Subj6 { get; set; }
        public string Subj7 { get; set; }
        public string Subj8 { get; set; }
        public string Subj9 { get; set; }

        public string Code1 { get; set; }
        public string Code2 { get; set; }
        public string Code3 { get; set; }
        public string Code4 { get; set; }
        public string Code5 { get; set; }
        public string Code6 { get; set; }
        public string Code7 { get; set; }
        public string Code8 { get; set; }
        public string Code9 { get; set; }


        public string Subj1_CA1 { get; set; }
        public string Subj2_CA1 { get; set; }
        public string Subj3_CA1 { get; set; }
        public string Subj4_CA1 { get; set; }
        public string Subj5_CA1 { get; set; }
        public string Subj6_CA1 { get; set; }
        public string Subj7_CA1 { get; set; }
        public string Subj8_CA1 { get; set; }
        public string Subj9_CA1 { get; set; }


        public string Subj1_CA2 { get; set; }
        public string Subj2_CA2 { get; set; }
        public string Subj3_CA2 { get; set; }
        public string Subj4_CA2 { get; set; }
        public string Subj5_CA2 { get; set; }
        public string Subj6_CA2 { get; set; }
        public string Subj7_CA2 { get; set; }
        public string Subj8_CA2 { get; set; }
        public string Subj9_CA2 { get; set; }
        public int numberOfSubjects { get; set; }
    }


    public class SubjectModel
    {
        public string code { get; set; }
        public string Subject { get; set; }
        public string CA1 { get; set; }
        public string CA2 { get; set; }
       
    }

    public class SubjectRefModel
    {
        public string code { get; set; }
        public string subject { get; set; }
        public string descript { get; set; }
    }

}
