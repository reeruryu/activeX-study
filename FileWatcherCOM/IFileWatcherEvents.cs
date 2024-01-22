using System;
using System.Runtime.InteropServices;

namespace FileWatcherCOM
{
    [ComVisible(true)]
    [Guid("5C061998-1C44-4DC0-9763-7D1A0496DF38")]
    [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    internal interface IFileWatcherEvents
    {
        [DispId(1)]
        void FileCreated(string filePath);
    }
}
