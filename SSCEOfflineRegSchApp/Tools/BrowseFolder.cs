using System;
using System.Runtime.InteropServices;


namespace SSCEOfflineRegSchApp.Tools
{
    public enum ShowCommands : int
    {
        SW_HIDE = 0,
        SW_SHOWNORMAL = 1,
        SW_NORMAL = 1,
        SW_SHOWMINIMIZED = 2,
        SW_SHOWMAXIMIZED = 3,
        SW_MAXIMIZE = 3,
        SW_SHOWNOACTIVATE = 4,
        SW_SHOW = 5,
        SW_MINIMIZE = 6,
        SW_SHOWMINNOACTIVE = 7,
        SW_SHOWNA = 8,
        SW_RESTORE = 9,
        SW_SHOWDEFAULT = 10,
        SW_FORCEMINIMIZE = 11,
        SW_MAX = 11
    }

    public class BrowseFolder
    {
        [DllImport("shell32.dll")]
       public static extern IntPtr ShellExecute(
       IntPtr hwnd,
       string lpOperation,
       string lpFile,
       string lpParameters,
       string lpDirectory,
       ShowCommands nShowCmd);



        [DllImport("shell32.dll", SetLastError = true)]
        public static extern int SHOpenFolderAndSelectItems(
            IntPtr pidlFolder, 
            uint cidl, 
            [In, MarshalAs(UnmanagedType.LPArray)] IntPtr[] apidl, 
            uint dwFlags);

        [DllImport("shell32.dll", SetLastError = true)]
        public static extern void SHParseDisplayName(
            [MarshalAs(UnmanagedType.LPWStr)] string name, 
            IntPtr bindingContext, 
            [Out] out IntPtr pidl, 
            uint sfgaoIn, 
            [Out] out uint psfgaoOut);

        public static void OpenFolderAndSelectItem(string folderPath, string file)
        {
            IntPtr nativeFolder;
            uint psfgaoOut;
            SHParseDisplayName(folderPath, IntPtr.Zero, out nativeFolder, 0, out psfgaoOut);

            if (nativeFolder == IntPtr.Zero)
            {
                // Log error, can't find folder
                return;
            }

            IntPtr nativeFile;
            SHParseDisplayName(System.IO.Path.Combine(folderPath, file), IntPtr.Zero, out nativeFile, 0, out psfgaoOut);

            IntPtr[] fileArray;
            if (nativeFile == IntPtr.Zero)
            {
                // Open the folder without the file selected if we can't find the file
                fileArray = new IntPtr[0];
            }
            else
            {
                fileArray = new IntPtr[] { nativeFile };
            }

            SHOpenFolderAndSelectItems(nativeFolder, (uint)fileArray.Length, fileArray, 0);

            Marshal.FreeCoTaskMem(nativeFolder);
            if (nativeFile != IntPtr.Zero)
            {
                Marshal.FreeCoTaskMem(nativeFile);
            }
        }
    }
}
