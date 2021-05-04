using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CA3_S00209029
{
    public class CarObject
    {

        public int ID { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Size { get; set; }

        public CarObject() { }

        public override string ToString()
        {
            return String.Format("{0} - {1}", Make, Model);
        }
    }
}
