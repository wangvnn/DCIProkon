using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProkonDCI.Domain.Data
{
    public struct Point
    {
        public int x, y;
        public Point(int inX, int inY)
        {
            x = inX;
            y = inY;
        }
    }
}
