using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
///using System.Windows.Data;
///using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
///using System.Windows.Navigation;
using System.Windows.Shapes;
using WPFFolderBrowser;
using System.IO;
///using System.Diagnostics;

namespace Virus
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private List<ImageData> imageList = new List<ImageData>();
        private int CurrentImageIndex=0;
        private int PointDiameter=10;
        private string folder = "";
        private void FolderPickerButton_Click(object sender, RoutedEventArgs e)
        {
            folder = GetFolder();
            if (folder!="")
            {
                CreateImageData(folder);
                if (imageList.Count!=0)
                {
                    ShowOnCanvas();
                    StartLabeling();
                }
                
            }
            
        }

        private void StartLabeling()
        {
            MainCanvas.MouseLeftButtonDown += MainCanvas_MouseLeftButtonDown;
        }

        private void MainCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var point = e.GetPosition(MainCanvas);
            LabelAtPoint(point);
        }

        private void LabelAtPoint(Point point)
        {
            ImageData currentImage = imageList[CurrentImageIndex];
            int index = currentImage.CurrentIndex;
            double scale = currentImage.Scale;
            if (index < currentImage.TotalLabels)
            {
                Color color = currentImage.ColorList[index];
                currentImage.XCord[index] = point.X*scale;
                currentImage.YCord[index] = point.Y*scale;
                currentImage.CurrentIndex += 1;
                Ellipse ellipse = new Ellipse();
                ellipse.Height = PointDiameter;
                ellipse.Width = PointDiameter;
                ellipse.Fill = new SolidColorBrush(color);
                Canvas.SetLeft(ellipse, point.X - (ellipse.Width / 2));
                Canvas.SetTop(ellipse, point.Y - (ellipse.Height / 2));
                MainCanvas.Children.Add(ellipse);
            }
            else
            {
                NextImage();
                
            }

        }

        private void ShowOnCanvas()
        {
            MainCanvas.Children.Clear();
            string filepath = imageList[CurrentImageIndex].FilePath;
            BitmapImage bitmapImage = new BitmapImage(new Uri(filepath));
            MainCanvas.Width = bitmapImage.PixelWidth*600/ bitmapImage.PixelHeight;
            imageList[CurrentImageIndex].Scale = bitmapImage.PixelHeight / 600.0;
            MainCanvas.Background = new ImageBrush(bitmapImage); 
        }

        private String GetFolder()
        {
            String folderpath="";
            WPFFolderBrowserDialog folderBrowser = new WPFFolderBrowserDialog("Choose the Folder");
            folderBrowser.InitialDirectory = "C:\\";
            if (folderBrowser.ShowDialog() == true)
            {
                folderpath = folderBrowser.FileName;
            }
           
            return folderpath;
        }
        private void CreateImageData(String folder)
        {
            DirectoryInfo directory = new DirectoryInfo(folder);
            FileInfo[] fileinfo = directory.GetFiles("*.jpg");
            foreach (FileInfo file in fileinfo)
            {
                ImageData imageData = new ImageData(file.ToString());
                imageList.Add(imageData);
            }
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            NextImage();
        }

        private void PrevButton_Click(object sender, RoutedEventArgs e)
        {
            PrevImage();
        }
        private void NextImage()
        {
            if (CurrentImageIndex < imageList.Count - 1)
            {
                imageList[CurrentImageIndex].CurrentIndex = 0;
                CurrentImageIndex += 1;
                ShowOnCanvas();
            }
        }
        private void PrevImage()
        {
            if (CurrentImageIndex > 0)
            {
                imageList[CurrentImageIndex].CurrentIndex = 0;
                CurrentImageIndex -= 1;
                ShowOnCanvas();
            }
        }

        private void SaveToFile()
        {
            ///SaveButton.IsEnabled = false;
            if (imageList.Count!=0)
            {
                string currentTime = DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss");
                for (int i = 0; i <= CurrentImageIndex; i++)
                {
                    var c = imageList[i];
                    var xCordList = c.XCord;
                    var yCordList = c.YCord;
                    var fname = c.FilePath.Split("\\").Last();
                    ///Debug.WriteLine(fname.Last());
                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.Append(fname);
                    ///stringBuilder.Append(",X").Append(i);
                    foreach (double coord in xCordList)
                    {
                        stringBuilder.Append(",").Append(coord);
                    }
                    ////stringBuilder.Append(",Y").Append(i);
                    foreach (double coord in yCordList)
                    {
                        stringBuilder.Append(",").Append(coord);
                    }
                    String s = stringBuilder.ToString();
                    using (var writer = new StreamWriter(folder + "\\labels_" + 
                        CurrentImageIndex.ToString() + "_" +currentTime+ ".csv", true))
                    {
                        writer.WriteLineAsync(s);

                    }

                }
            }
           
            
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            SaveToFile();
            SaveButton_Cooldown();
        }

        private async void SaveButton_Cooldown()
        {
            SaveButton.IsEnabled = false;
            await Task.Delay(2000);
            SaveButton.IsEnabled = true;
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            if (imageList.Count!=0)
            {
                imageList[CurrentImageIndex].CurrentIndex = 0;
                ShowOnCanvas();
            }
        }
    }
}
