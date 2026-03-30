using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab_3
{
    public interface IVisitor
    {
        void Visit(Airplane airplane);
        void Visit(Bicycle bicycle);
        void Visit(Motorcycle motorcycle);  
        void Visit(Car car);
        void Visit(Liner liner);
        void Visit(Boat boat);
    }
}
