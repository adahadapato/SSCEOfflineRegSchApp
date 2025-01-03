

namespace SSCEOfflineRegSchApp.Model
{
    public class TemplatesModel
    {
        public string ser_no { get; set; }
        public byte[] finger { get; set; }
        public byte[] template { get; set; }
        public int quality { get; set; }
    }

    public class BiometricsSaveModel
    {
        public string serialNumber { get; set; }


        public byte[] leftThumbImage { get; set; }
        public byte[] leftThumbTemplate { get; set; }
        public int leftThumbQuality { get; set; }

        public byte[] leftIndexImage { get; set; }
        public byte[] leftIndexTemplate { get; set; }
        public int leftIndexQuality { get; set; }

        public byte[] leftMiddleImage { get; set; }
        public byte[] leftMiddleTemplate { get; set; }
        public int leftMiddleQuality { get; set; }

        public byte[] leftRingImage { get; set; }
        public byte[] leftRingTemplate { get; set; }
        public int leftRingQuality { get; set; }

        public byte[] leftPinkyImage { get; set; }
        public byte[] leftPinkyTemplate { get; set; }
        public int leftPinkyQuality { get; set; }


        public byte[] rightThumbImage { get; set; }
        public byte[] rightThumbTemplate { get; set; }
        public int rightThumbQuality { get; set; }

        public byte[] rightIndexImage { get; set; }
        public byte[] rightIndexTemplate { get; set; }
        public int rightIndexQuality { get; set; }

        public byte[] rightMiddleImage { get; set; }
        public byte[] rightMiddleTemplate { get; set; }
        public int rightMiddleQuality { get; set; }

        public byte[] rightRingImage { get; set; }
        public byte[] rightRingTemplate { get; set; }
        public int rightRingQuality { get; set; }

        public byte[] rightPinkyImage { get; set; }
        public byte[] rightPinkyTemplate { get; set; }
        public int rightPinkyQuality { get; set; }

        public bool hasBiometrics { get; set; }
    }
}
