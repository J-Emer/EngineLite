using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineLite.Managers
{
    public class Resolution
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public bool IsFullScreen { get; set; }

        public Resolution(int width, int height, bool isfullscreen)
        {
            Width = width;
            Height = height;
            IsFullScreen = isfullscreen;
        }

        public override string ToString()
        {
            return $"Width: {Width} | Height: {Height} | IsFullScreen: {IsFullScreen}";
        }
    }
}
