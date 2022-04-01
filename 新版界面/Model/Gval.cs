using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RootNS.Model
{
    public static class Gval
    {
        public static Book CurrentBook { get; set; } = new Book("测试书籍");
        public static Material Material { get; set; } = new Material();
    }
}
