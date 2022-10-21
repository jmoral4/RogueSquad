using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpriteConfig
{
    class SpriteFile
    {
        public string FileFullPath { get; set; }
        public string FileName { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }
       
        public int FrameRate { get; set; }
    }
}
