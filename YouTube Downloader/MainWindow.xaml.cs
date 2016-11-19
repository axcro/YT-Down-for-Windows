using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Text.RegularExpressions;
using VideoLibrary;

namespace YouTube_Downloader
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<string> links = new List<string>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Download_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.DefaultExt = ".txt";
            dlg.Filter = "Text Files (*.txt)|*.txt";
            dlg.CheckFileExists = true;
            dlg.ShowDialog();
            string filename = dlg.FileName;
            links = ParseLinks(filename);

            FolderBrowserDialog odlg = new FolderBrowserDialog();
            odlg.ShowDialog();
            string foldername;
            foldername = odlg.SelectedPath;

            string logPath = foldername + "\\log.txt";
            StreamWriter outputFile = new StreamWriter(logPath);
            outputFile.WriteLine("Begin Log:\n");

            int numb = 1, numbDone=0;

            foreach (string link in links)
            {
                var youTube = YouTube.Default; // starting point for YouTube actions
                numb++;
                try
                {
                    var video = youTube.GetVideo(link); // gets a Video object with info about the video
                    string videoName = video.FullName;
                    videoName = videoName.Replace(" - YouTube", "");

                    if (File.Exists(foldername + "\\" + video.FullName)) ;
                    //System.Windows.Forms.MessageBox.Show(foldername + "\\" + video.FullName + " Exists already, skipping");
                    else
                    {
                        File.WriteAllBytes(foldername + "\\" + videoName, video.GetBytes());
                        //System.Windows.Forms.MessageBox.Show(foldername + "\\" + video.FullName + " being created!");
                    }
                    numbDone++;
                }
                catch
                {
                    outputFile.WriteLine("Link #" + numb.ToString() + " Failed\n");
                    outputFile.WriteLine(link);
                    outputFile.WriteLine("\n\n");
                }
            }
            System.Windows.Forms.MessageBox.Show("Completed!");
            outputFile.Close();
        }

        public static List<string> ParseLinks(string fileName)
        {
            string line;
            List<string> linkList = new List<string>();
            using (StreamReader file = new StreamReader(fileName))
            {
                while ((line = file.ReadLine()) != null)
                {
                    linkList.Add(line);
                }
                file.Close();
                return (linkList);
            }
        }


    }
}
