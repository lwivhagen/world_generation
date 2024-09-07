using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using world_generation.scripts.other;
using world_generation.scripts.types;

namespace world_generation.scripts.managers
{
    public static class WorldGenerator
    {
        public static Chunk[] GenerateWorld(WorldSettings ws)
        {
            Chunk[] chunkArray = new Chunk[ws.worldHeight * ws.worldWidth];
            for (int Y = 0; Y < ws.worldHeight; Y++)
            {
                for (int X = 0; X < ws.worldWidth; X++)
                {
                    chunkArray[Y * ws.worldWidth + X] = GenerateChunk(new Vector2I(X, Y));
                }
            }
            return chunkArray;
        }

        public static Chunk GenerateChunk(Vector2I coordinate)
        {
            // Console.WriteLine("Generating Chunk");
            Chunk chunk = new Chunk(coordinate);
            WorldManager wm = WorldManager.Instance;
            WorldSettings ws = wm.worldSettings;

            Tile[][] tiles = new Tile[ws.chunkSize][];
            for (int y = 0; y < ws.chunkSize; y++)
            {
                tiles[y] = new Tile[ws.chunkSize];
                for (int x = 0; x < ws.chunkSize; x++)
                {
                    tiles[y][x] = new Tile(new Vector2I(x, y), (byte)wm.random.Next(0, 5));
                }
            }
            chunk.tiles = tiles;
            // GD.Print("Chunk: " + chunk.tiles[0].Length);
            return chunk;
        }
    }
}
