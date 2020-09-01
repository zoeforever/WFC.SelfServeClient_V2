using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WFC.SelfServeClient.Models
{
    [PropertyChanged.ImplementPropertyChanged]
    public class Visitor
    {
        public string First { get; set; }
        public string Second { get; set; }
    }
}
