using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoLExePatcher
{
    public class Version
    {
        public string version { get; set; }
        public string link { get; set; }
        public bool newest { get; set; }
    }

    public class RootObject
    {
        public List<Version> versions { get; set; }
    }
}
