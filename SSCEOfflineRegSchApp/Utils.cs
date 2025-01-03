using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SSCEOfflineRegSchApp
{
    public static class Utils
    {
        public static int QualityToPercent(byte value)
        {
            return (2 * value * 100 + 255) / (2 * 255);
        }

        public static byte QualityFromPercent(int value)
        {
            return (byte)((2 * value * 255 + 100) / (2 * 100));
        }

        public static string MatchingThresholdToString(int value)
        {
            double p = -value / 12.0;
            return string.Format(string.Format("{{0:P{0}}}", Math.Max(0, (int)Math.Ceiling(-p) - 2)), Math.Pow(10, p));
        }

        public static int MatchingThresholdFromString(string value)
        {
            double p = Math.Log10(Math.Max(double.Epsilon, Math.Min(1,
                double.Parse(value.Replace(CultureInfo.CurrentCulture.NumberFormat.PercentSymbol, "")) / 100)));
            return Math.Max(0, (int)Math.Round(-12 * p));
        }

        public static int MaximalRotationToDegrees(byte value)
        {
            return (2 * value * 360 + 256) / (2 * 256);
        }

        public static byte MaximalRotationFromDegrees(int value)
        {
            return (byte)((2 * value * 256 + 360) / (2 * 360));
        }

        public static string GetUserLocalDataDir(string productName)
        {
            string localDataDir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            localDataDir = Path.Combine(localDataDir, "Neurotechnology");
            if (!Directory.Exists(localDataDir))
            {
                Directory.CreateDirectory(localDataDir);
            }
            localDataDir = Path.Combine(localDataDir, productName);
            if (!Directory.Exists(localDataDir))
            {
                Directory.CreateDirectory(localDataDir);
            }

            return localDataDir;
        }

        public static void ShowException(Exception ex)
        {
            while ((ex is AggregateException) && (ex.InnerException != null))
                ex = ex.InnerException;

            MessageBox.Show(ex.ToString(), null, MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
