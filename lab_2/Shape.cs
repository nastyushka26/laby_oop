global using System.Drawing;
global using System;
global using System.Collections.Generic;
global using System.Linq;
global using System.Security.Cryptography.X509Certificates;
global using System.Text;
global using System.Threading.Tasks;

namespace laba1oop
{
    // base abstract class for inheritance and overriding
    internal abstract class Shape
    {
        // main fields - top-left coordinates
        protected int CoordX;
        protected int CoordY;

        // base class constructor
        public Shape(int x1, int y1)
        {
            CoordX = x1;
            CoordY = y1;
        }
    }
}
