﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment
{
    class DDJBFlight : Flight
    {
        public DDJBFlight() { }
        public DDJBFlight(string f, string o, string d, DateTime e, string s) : base(f, o, d, e, s) { }
        public override double CalculateFees()
        {
            return
        }
        public override string ToString()
        {
            return base.ToString();
        }
    }
}
