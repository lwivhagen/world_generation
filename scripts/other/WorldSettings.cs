using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace world_generation.scripts.other
{
    public class WorldSettings
    {
        public int seed = 0;

        //tileSize is the size of each tile in pixels
        public int tileSize = 16;

        //chunkSize is the size of each chunk in tiles
        public int chunkSize = 16;
        public int regionSize = 32;

        //worldSize is the size of the world in chunks (for now)
        public int worldWidth = 4;
        public int worldHeight = 3;

        //Amount of chunks to generate in each direction from the player
        public int generateDistance = 1;
    }
}
