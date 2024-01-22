using FileWatcherCOM;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace ActiveXTestForm
{
    public partial class Form1 : Form
    {

        private const string DefaultSavePath = @"C:\fileTest";
        private FileWatcher fileWatcher;


        public Form1()
        {
            InitializeComponent();

            fileWatcher = new FileWatcher();
            fileWatcher.FileCreated += FileWatcher_FileCreated;
        }

        private void FileWatcher_FileCreated(string path)
        {
            fileWatcher.Path = path;

            // Handle the event
            MessageBox.Show(path);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            // 파일 다이얼로그의 속성 설정
            openFileDialog.Title = "파일 선택";
            openFileDialog.Filter = "모든 파일 (*.*)|*.*";
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // 선택한 파일의 경로
                string selectedFilePath = openFileDialog.FileName;

                // 저장할 파일의 기본 경로
                string randomFileName = Path.GetRandomFileName();
                string defaultSavePath = Path.Combine(DefaultSavePath, $"{randomFileName}.bmp");

                try
                {
                    // 파일 복사
                    File.Copy(selectedFilePath, defaultSavePath, true);
                    // 여기에 필요한 작업을 추가할 수 있습니다.
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"파일 저장 중 오류가 발생했습니다: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                //MessageBox.Show("파일이 성공적으로 저장되었습니다.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Dispose the COM object when the form is closing
            if (fileWatcher != null)
            {
                fileWatcher.Dispose();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                pictureBox1.Image.Dispose();
                pictureBox1.Image = null;
            }

            // Set the path to textBox1
            string path = fileWatcher.Path;
            if (path == null)
            {
                MessageBox.Show("no detection");
                return;
            }

            // Set the path to textBox1
            textBox1.Text = path;

            // Load the image into pictureBox1
            try
            {
                pictureBox1.Image = Image.FromFile(path);
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading image: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
