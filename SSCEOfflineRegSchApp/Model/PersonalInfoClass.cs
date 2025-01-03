

using SSCEOfflineRegSchApp.Tools;
using System.Windows.Media.Imaging;

namespace SSCEOfflineRegSchApp.Model
{
    public class SerialNoModel
    {
        public string SerialNo { get; set; }
        public string SerialNoTemp { get; set; }
    }
    public class CandidateSaveModel
    {
       public PersonalInfoSaveModel personalInfo { get; set; }
       public SubjectSaveModel subjects { get; set; }
    }

  
    public class CandidateViewModel
    {
        public string serialNumber { get; set; }
        public string surName { get; set; }
        public string firstName { get; set; }
        public string middleName { get; set; }
        public string Sex { get; set; }
        public long dateOfBirth { get; set; }
        public string disabled { get; set; }
        public string stateOfOriginCode { get; set; }
        public string stateOfOriginName { get; set; }
        public string lgaCode { get; set; }
        public string lgaName { get; set; }
        public byte[] bPassport { get; set; }
        public string Passport { get; set; }
        public bool isComplete { get; set; }
        public string Gender
        {
            get
            {
                return Sex.Substring(0, 1);
            }
        }
        public string d_of_b
        {
            get
            {
                return Tools.DateTimeToLong.ConvertToDateTime(dateOfBirth).ToShortDateString();
            }
        }
        public string Name
        {
          get{
                return string.Format("{0}, {1} {2}",surName.Trim(), firstName.Trim(), middleName.Trim());
            }
        }


        public string Subj1 { get; set; }
        public string Subj1_CA1 { get; set; }
        public string Subj1_CA2 { get; set; }
        public string Sub1
        { get{
                return string.Format("{0} [{1}][{2}]", Subj1, Subj1_CA1, Subj1_CA2);
            }
        }

        public string Subj2 { get; set; }
        public string Subj2_CA1 { get; set; }
        public string Subj2_CA2 { get; set; }
        public string Sub2
        {
            get
            {
                return string.Format("{0} [{1}][{2}]", Subj2, Subj2_CA1, Subj2_CA2);
            }
        }


        public string Subj3 { get; set; }
        public string Subj3_CA1 { get; set; }
        public string Subj3_CA2 { get; set; }
        public string Sub3
        {
            get
            {
                return string.Format("{0} [{1}][{2}]", Subj3, Subj3_CA1, Subj3_CA2);
            }
        }


        public string Subj4 { get; set; }
        public string Subj4_CA1 { get; set; }
        public string Subj4_CA2 { get; set; }
        public string Sub4
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Subj4))
                    return string.Empty;
                else
                    return string.Format("{0} [{1}][{2}]", Subj4, Subj4_CA1, Subj4_CA2);
            }
        }


        public string Subj5 { get; set; }
        public string Subj5_CA1 { get; set; }
        public string Subj5_CA2 { get; set; }
        public string Sub5
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Subj5))
                    return string.Empty;
                else
                    return string.Format("{0} [{1}][{2}]", Subj5, Subj5_CA1, Subj5_CA2);
            }
        }



        public string Subj6 { get; set; }
        public string Subj6_CA1 { get; set; }
        public string Subj6_CA2 { get; set; }
        public string Sub6
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Subj6))
                    return string.Empty;
                else
                    return string.Format("{0} [{1}][{2}]", Subj6, Subj6_CA1, Subj6_CA2);
            }
        }



        public string Subj7 { get; set; }
        public string Subj7_CA1 { get; set; }
        public string Subj7_CA2 { get; set; }
        public string Sub7
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Subj7))
                    return string.Empty;
                else
                    return string.Format("{0} [{1}][{2}]", Subj7, Subj7_CA1, Subj7_CA2);
            }
        }



        public string Subj8 { get; set; }
        public string Subj8_CA1 { get; set; }
        public string Subj8_CA2 { get; set; }
        public string Sub8
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Subj8))
                    return string.Empty;
                else
                    return string.Format("{0} [{1}][{2}]", Subj8, Subj8_CA1, Subj8_CA2);
            }
        }

        public string Subj9 { get; set; }
        public string Subj9_CA1 { get; set; }
        public string Subj9_CA2 { get; set; }
        public string Sub9
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Subj9))
                    return string.Empty;
                else
                    return string.Format("{0} [{1}][{2}]", Subj9, Subj9_CA1, Subj9_CA2);
            }
        }

        public int numberOfSubjects { get; set; }
        public string BiometricsToolTip { get; set; }
        public string BiometricsImageSource { get; set; }
        public BitmapImage Pict
        {
            get
            {
                return (bPassport != null) ? SafeGuiWpf.SetImage(bPassport) : null;
            }
        }
        public string listViewItemToolTip { get; set; }
        public string UploadImageSource { get; set; }
        public string UploadImageToolTip { get; set; }
        public bool hasBiometrics { get; set; }
        public int status { get; set; }
        public bool IsSelected { get; set; }
    }
    

    public class PersonalInfoSaveModel
    {
        public string SchoolNo { get; set; }
        public string serialNumber { get; set; }
        public string surName { get; set; }
        public string firstName { get; set; }
        public string middleName { get; set; }
        public string Sex { get; set; }
        public long dateOfBirth { get; set; }
        public string disabled { get; set; }
        public string stateOfOriginCode { get; set; }
        public string stateOfOriginName { get; set; }
        public string lgaCode { get; set; }
        public string lgaName { get; set; }
        public byte[] bPassport { get; set; }
        public string Passport { get; set; }
        public bool isComplete { get; set; }
        public int status { get; set; }
    }

   
}
