using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BrashMonkeySpriter.Spriter
{
    public class CharacterMap
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public MapInstruction[] Maps { get; set; }
    }


    public struct MapInstruction
    {
        public int Folder { get; set; }
        public int File { get; set; }

        public int TargetFolder { get; set; }
        public int TargetFile { get; set; }

    }



}
