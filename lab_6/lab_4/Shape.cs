global using System.Drawing;
global using System;
global using System.Collections.Generic;
global using System.Linq;
global using System.Security.Cryptography.X509Certificates;
global using System.Text;
global using System.Threading.Tasks;

namespace laba2oop
{
    // base abstract class for inheritance and overriding
    public abstract class Shape
    {
        // main fields - top-left coordinates
        public int CoordX;
        public int CoordY;

        // base class constructor
        public Shape(int x1, int y1)
        {
            CoordX = x1;
            CoordY = y1;
        }
        public abstract void Accept(IShapeVisitor visitor);
    }
}
