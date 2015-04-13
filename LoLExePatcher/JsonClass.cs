using System.Collections.Generic;

namespace LoLExePatcher
{
    public class Version
    {
        public string version { get; set; }
        public string link { get; set; }
        public bool newest { get; set; }
        public bool @switch { get; set; }
    }

    public class RootObject
    {
        public List<Version> versions { get; set; }
    }
}
