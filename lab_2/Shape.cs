global using System.Drawing;
global using System;
global using System.Collections.Generic;
global using System.Linq;
global using System.Security.Cryptography.X509Certificates;
global using System.Text;
global using System.Threading.Tasks;

namespace laba1oop
{
    internal class Shape
    {
        protected int CoordX;
        protected int CoordY;
        // конструктор базового класса
        public Shape(int x1, int y1)
        {
            CoordX = x1;
            CoordY = y1;
        }
        public virtual void Draw(Graphics g, Pen pen, SolidBrush brush) { }
    }
}
