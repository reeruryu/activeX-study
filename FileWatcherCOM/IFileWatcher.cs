using System;
using System.Runtime.InteropServices;

namespace FileWatcherCOM
{
    [ComVisible(true)]
    [Guid("525061A5-5C56-42A2-9B60-6A877A86247A")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    internal interface IFileWatcher
    {
        string Path { get; set; }

        string Hi();

    }
}