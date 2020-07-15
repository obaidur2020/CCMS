using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.All_Data
{
    class Average
    {
        public int DataAverage(List<int> data)
        {
            int total = 0;
            if (data.Count()!=0)
            {
                foreach (var item in data)
                {
                    total = total + item;
                }
                int average = total / data.Count();
                return average;
            }
            else
            {
                return 0; 
            }
            
        }
        public double DataAverageDouble(List<double> data)
        {
            double total = 0.0;
            if (data.Count() != 0)
            {
                foreach (var item in data)
                {
                    total = total + item;
                }
                double average = total / data.Count();
                return average;
            }
            else
            {
                return 0;
            }

        }
    }
}
