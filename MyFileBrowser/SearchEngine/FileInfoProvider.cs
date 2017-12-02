using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using System.IO;

namespace MyFileBrowser
{
    internal abstract class FileInfoProvider
    {
       internal abstract string FileContent(FileInfo strFileName);

    }
}
