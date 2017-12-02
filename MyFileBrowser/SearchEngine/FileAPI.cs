using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using System.IO;

namespace MyFileBrowser
{
    class FileAPI
    {

        public static string GetFileSize(FileInfo file)
        {
            string result = null;

            if (file.Exists == true)
            {
                long filesize = file.Length;
                if (filesize >= 1024 * 1024 * 1024)
                {
                    result = string.Format("{0:########0.00} GB", (double)filesize / (1024 * 1024 * 1024));
                }
                else if (filesize >= 1024 * 1024)
                {
                    result = string.Format("{0:####0.00} MB", (double)filesize / (1024 * 1024));
                }
                else if (filesize >= 1024)
                {
                    result = string.Format("{0:####0.00} KB", (double)filesize / 1024);
                }
                else
                {
                    result = string.Format("{0} Bytes", filesize);
                }
                return result;
            }
            return "文件不存在！";


        }



        [DllImport("Shell32.dll")]
        static extern int SHGetFileInfo(string pszPath, uint dwFileAttributes, ref   SHFILEINFO psfi, uint cbFileInfo, uint uFlags);
        struct SHFILEINFO
        {
            public IntPtr hIcon;
            public int iIcon;
            public uint dwAttributes;
            public char szDisplayName;
            public char szTypeName;
        }
        /// <summary>
        /// 从文件扩展名得到文件关联图标
        /// </summary>
        /// <param name="fileName">文件名或文件扩展名</param>
        /// <param name="smallIcon">是否是获取小图标，否则是大图标</param>
        /// <returns>图标</returns>
        static public Icon GetFileIcon(string fileName, bool smallIcon)
        {
            SHFILEINFO fi = new SHFILEINFO();
            Icon ic = null;
            //SHGFI_ICON + SHGFI_USEFILEATTRIBUTES + SmallIcon   
            int iTotal = (int)SHGetFileInfo(fileName, 100, ref fi, 0, (uint)(smallIcon ? 273 : 272));
            if (iTotal > 0)
            {
                ic = Icon.FromHandle(fi.hIcon);
            }
            return ic;
        }
    }
}
