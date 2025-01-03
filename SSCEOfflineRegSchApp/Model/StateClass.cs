using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSCEOfflineRegSchApp.Model
{
    public class StateSaveModel
    {
        public string stateCode { get; set; }
        public string stateName { get; set; }
    }

    public class LGASaveModel
    {
        public string lgaCode { get; set; }
        public string stateCode { get; set; }
        public string LGAName { get; set; }
    }
}
