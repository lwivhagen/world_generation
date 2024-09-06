using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;

namespace world_generation.scripts.types
{
    public class Chunk
    {
        public Vector2I coordinate;
        public Tile[][] tiles;

        public Chunk(Vector2I coordinate, Tile[][] tiles = null)
        {
            this.coordinate = coordinate;
            this.tiles = tiles;
        }
    }
}
