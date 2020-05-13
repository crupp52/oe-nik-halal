using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halal.Problems.WorkAssignment
{
    public class Person
    {
        public float Salary { get; set; }
        public float Quality { get; set; }
        public float SharingValue { get; set; }
        public float Fitness { get; set; }

        public int N { get; set; }
        public List<Person> S { get; set; }
        public int Rank { get; set; }
        public float Distance { get; set; }
    }
}
