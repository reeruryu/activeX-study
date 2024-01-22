using System;
using System.IO;
using System.Runtime.InteropServices;

namespace FileWatcherCOM
{
    
    [ComVisible(true)]
    [Guid("3966CDF2-E064-46FE-8549-A91091087991")]
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ComSourceInterfaces(typeof(IFileWatcherEvents))]
    [ProgId("FileWatcherCOM.FileWatcher")]
    public class FileWatcher : IFileWatcher, IDisposable
    {

        private FileSystemWatcher fileSystemWatcher;

        public delegate void FileCreateHandler(string filePath);
        public event FileCreateHandler FileCreated;

        public string Path { get; set; }

        public FileWatcher()
        {
            InitializeFileSystemWatcher();
        }

        private void InitializeFileSystemWatcher()
        {
            fileSystemWatcher = new FileSystemWatcher();
            fileSystemWatcher.Path = "C:\\fileTest";
            fileSystemWatcher.Filter = "*.bmp";
            fileSystemWatcher.Created += FileSystemWatcher_Created;

            fileSystemWatcher.EnableRaisingEvents = true;
        }

        public string Hi()
        {
            return "hi";
        }

        private void FileSystemWatcher_Created(object sender, FileSystemEventArgs e)
        {
            FileCreated?.Invoke(e.FullPath);
        }

        public void Dispose()
        {
            if (fileSystemWatcher != null)
            {
                fileSystemWatcher.Created -= FileSystemWatcher_Created;
                fileSystemWatcher.Dispose();
            }
        }

    }
}
