using System;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.IO;
using System.Collections.Concurrent;
using System.Threading;

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

        /*스레드 간 데이터를 안전하게 공유하기 위해서 BlockingCollection 사용
        BlockingCollection은 컬렉션을 한번 더 래핑하는 것, 기본 컬렉션은 ConcurrentQueue
        따라서 사실 아래 코드에서 ConcurrentQueue를 생략할 수 있음
        // private BlockingCollection<string> processingQueue = new BlockingCollection<string>();*/
        private BlockingCollection<string> processingQueue = new BlockingCollection<string>(new ConcurrentQueue<string>());
        private Thread processingThread;

        public delegate void FileCreateHandler(string filePath);
        public event FileCreateHandler FileCreated;

        public string Path { get; set; }

        public FileWatcher()
        {
            InitializeFileSystemWatcher();
            processingThread = new Thread(ProcessFilesInBackground)
            {
                IsBackground = true
            };
            processingThread.Start();
        }

        private void InitializeFileSystemWatcher()
        {
            fileSystemWatcher = new FileSystemWatcher();
            fileSystemWatcher.Path = "C:\\fileTest";
            fileSystemWatcher.Filter = "*.bmp";
            fileSystemWatcher.Created += FileSystemWatcher_Created;

            fileSystemWatcher.EnableRaisingEvents = true;
        }

        public async Task<string> CopyFile(string destPath)
        {
            string originPath = @"C:\fileTest2";
            string randomFileName = System.IO.Path.GetRandomFileName();
            string defaultSavePath = System.IO.Path.Combine(originPath, $"{randomFileName}.bmp");

            try
            {
                if (File.Exists(destPath))
                {
                    await Task.Run(() => File.Copy(defaultSavePath, destPath, true));
                    return defaultSavePath;
                }
                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }

        }

        private void FileSystemWatcher_Created(object sender, FileSystemEventArgs e)
        {
            processingQueue.Add(e.FullPath);
        }

        private void ProcessFilesInBackground()
        {
            foreach (string filePath in processingQueue.GetConsumingEnumerable())  // 대기하다 큐에 들어오면 알아서 아래 코드 실행
            {
                FileCreated?.Invoke(filePath);
            }
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
