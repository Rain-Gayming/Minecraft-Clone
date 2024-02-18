using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftClone.Scripts.World.Data
{
    public struct Biome
    {
        public string biomeName;

        public BlockType topBlock;
        public BlockType lowerBlock;

        public Climate climate;
    }
}
