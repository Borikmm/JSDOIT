using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace JUSTDOIT
{
    internal class Task
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Discription { get; set; }
        public string Time { get; set; }
        public bool Doing = false;

        public Canvas canvas { get; set; }

        public Polygon crest = new Polygon();

    }


    [Serializable]
    internal class Task2
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Discription { get; set; }
        public string Time { get; set; }
        public bool Doing = false;


    }

}
