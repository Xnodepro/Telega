using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telega.Types
{
    class TempGood
    {
        private string name = "";
        private string _telegram = "";
        private int dateItems = 0;

        public string Name { get => name; set => name = value; }
        public string telegram { get => _telegram; set => _telegram = value; }
        public int DateItems { get => dateItems; set => dateItems = value; }
    }
}
