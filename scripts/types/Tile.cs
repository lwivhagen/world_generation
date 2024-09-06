using System;
using Godot;

namespace world_generation.scripts.types
{
    public class Tile
    {
        public Vector2I position;

        public Tile(Vector2I position)
        {
            this.position = position;
        }

        public virtual void Tick()
        {
            Console.WriteLine("Tile Tick");
        }
    }
}
