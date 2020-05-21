using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Media;

namespace Virus
{
    class ImageData
    {
        public String FilePath { get; }
        public List<double> XCord { get; set; }
        public List<double> YCord { get; set; }
        public List<Color> ColorList { get; }
        public int CurrentIndex { get; set; }
        public int TotalLabels { get; }
        public double Scale { get; set; }
        public ImageData(String filePath)
        {
            FilePath = filePath;
            XCord = new List<double>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            YCord = new List<double>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            ColorList = new List<Color>()
            {
                Colors.OrangeRed,
                Colors.Green,
                Colors.Aqua,
                Colors.Purple,
                Colors.Blue,
                Colors.HotPink,
                Colors.GreenYellow,
                Colors.Black,
                Colors.GreenYellow,
                Colors.HotPink,  
                Colors.Blue,    
                Colors.Purple,     
                Colors.Aqua,
                Colors.Green,
                Colors.OrangeRed,
                
            };
            CurrentIndex = 0;
            TotalLabels = 15;
            Scale = 1;
        }
        

    }
    
}
