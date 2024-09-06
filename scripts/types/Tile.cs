using System;
using Godot;

namespace world_generation.scripts.types
{
    public class Tile
    {
        //X, Y
        public Vector2I coordinate;
        public byte id = 0;

        public Tile(Vector2I position, byte id = 0)
        {
            this.coordinate = position;
            this.id = id;
        }

        private void GenerateTile()
        {
            Console.WriteLine("Generating Tile");
        }

        public virtual void Tick()
        {
            Console.WriteLine("Tile Tick");
        }
    }
}
