﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sj.Mg.CliLib.Model
{
    public class Rpt
    {
        public Rpt()
        {
            permissions = new List<resource>();
            active = true;
        }
        public bool active { get; set; }
        public double exp { get; set; }
        public double iat { get; set; }
        public List<resource> permissions { get; set; }
    }
}
