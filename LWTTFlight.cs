﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment
{
    class LWTTFlight : Flight
    {
        public double RequestFee { get; set; }
        public LWTTFlight() { }
        public LWTTFlight(double fee, string f, string o, string d, DateTime e, string s) : base(f, o, d, e, s)
        {
            RequestFee = fee;
        }
        public override double CalculateFees()
        {
            return 500 /* base.CalculateFees() + RequestFee*/;
        }
        public override string ToString()
        {
            return base.ToString();
        }
    }
}
