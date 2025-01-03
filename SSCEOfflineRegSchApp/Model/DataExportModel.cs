using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSCEOfflineRegSchApp.Model
{
    public class DataExportModel
    {
        public string ExamType { get; set; }
        public string ExamYear { get; set; }
        public string CentreNo { get; set; }
        public string CentreName { get; set; }
        //public string Operatorid { get; set; }
        public List<Registration> registration { get; set; }
        public List<Biometrics> biometrics { get; set; }
    }

    public class Registration
    { 
        public string ser_no { get; set; }
        public string disabled { get; set; }
        public string surname { get; set; }
        public string firstname { get; set; }
        public string middlename { get; set; }
        public string s_of_o { get; set; }
        public int lgaid { get; set; }
        public string d_of_b { get; set; }
        public string sex { get; set; }
        public string passport { get; set; }

        public string subj1 { get; set; }
        public string subj2 { get; set; }
        public string subj3 { get; set; }
        public string subj4 { get; set; }
        public string subj5 { get; set; }
        public string subj6 { get; set; }
        public string subj7 { get; set; }
        public string subj8 { get; set; }
        public string subj9 { get; set; }

        public string subj1_CA1 { get; set; }
        public string subj2_CA1 { get; set; }
        public string subj3_CA1 { get; set; }
        public string subj4_CA1 { get; set; }
        public string subj5_CA1 { get; set; }
        public string subj6_CA1 { get; set; }
        public string subj7_CA1 { get; set; }
        public string subj8_CA1 { get; set; }
        public string subj9_CA1 { get; set; }

        public string subj1_CA2 { get; set; }
        public string subj2_CA2 { get; set; }
        public string subj3_CA2 { get; set; }
        public string subj4_CA2 { get; set; }
        public string subj5_CA2 { get; set; }
        public string subj6_CA2 { get; set; }
        public string subj7_CA2 { get; set; }
        public string subj8_CA2 { get; set; }
        public string subj9_CA2 { get; set; }
    }

    public class Biometrics
    {
        public string ser_no { get; set; }

        public string leftThumbImage { get; set; }
        public string leftThumbTemplate { get; set; }
        public int leftThumbQuality { get; set; }

        public string leftIndexImage { get; set; }
        public string leftIndexTemplate { get; set; }
        public int leftIndexQuality { get; set; }

        public string leftMiddleImage { get; set; }
        public string leftMiddleTemplate { get; set; }
        public int leftMiddleQuality { get; set; }

        public string leftRingImage { get; set; }
        public string leftRingTemplate { get; set; }
        public int leftRingQuality { get; set; }

        public string leftPinkyImage { get; set; }
        public string leftPinkyTemplate { get; set; }
        public int leftPinkyQuality { get; set; }


        public string rightThumbImage { get; set; }
        public string rightThumbTemplate { get; set; }
        public int rightThumbQuality { get; set; }

        public string rightIndexImage { get; set; }
        public string rightIndexTemplate { get; set; }
        public int rightIndexQuality { get; set; }

        public string rightMiddleImage { get; set; }
        public string rightMiddleTemplate { get; set; }
        public int rightMiddleQuality { get; set; }

        public string rightRingImage { get; set; }
        public string rightRingTemplate { get; set; }
        public int rightRingQuality { get; set; }

        public string rightPinkyImage { get; set; }
        public string rightPinkyTemplate { get; set; }
        public int rightPinkyQuality { get; set; }
    }
}
