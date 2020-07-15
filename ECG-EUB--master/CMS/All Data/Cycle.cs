using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.All_Data
{
    class Cycle
    {
        public int CycleId { get; set; }

        public int PDataId { get; set; }
        public PDataProcessing PDataValue { get; set; }

        public int QDataId { get; set; }
        public QDataProcessing QDataValue { get; set; }
    }
}
