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
        private static FastNoiseLite noise = new FastNoiseLite();

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

        public static Chunk GenerateChunk(Vector2I chunkCoordinate)
        {
            noise.SetNoiseType(FastNoiseLite.NoiseTypeEnum.Simplex);
            noise.SetSeed(WorldManager.Instance.worldSettings.seed);
            noise.SetFrequency(0.01f);

            // Console.WriteLine("Generating Chunk");
            Chunk chunk = new Chunk(chunkCoordinate);
            WorldManager wm = WorldManager.Instance;
            WorldSettings ws = wm.worldSettings;

            int worldYCenterCoord = ws.worldHeight * ws.chunkSize / 2;

            Tile[][] tiles = new Tile[ws.chunkSize][];
            for (int y = 0; y < ws.chunkSize; y++)
            {
                // GD.Print("WorldYCenterCoord: " + worldYCenterCoord);
                tiles[y] = new Tile[ws.chunkSize];
                for (int x = 0; x < ws.chunkSize; x++)
                {
                    Vector2I globalCoord = new Vector2I(
                        chunkCoordinate.X * ws.chunkSize + x,
                        chunkCoordinate.Y * ws.chunkSize + y
                    );
                    int peak =
                        worldYCenterCoord
                        + (int)(ws.mountainHeight * Math.Abs(noise.GetNoise1D(globalCoord.X)));

                    if (globalCoord.Y <= peak)
                    {
                        tiles[y][x] = new Tile(new Vector2I(x, y), 0);
                    }
                    else
                    {
                        GD.Print("Y: " + y + " more than " + peak);
                        tiles[y][x] = new Tile(new Vector2I(x, y), 1);
                    }
                    // if (noise.GetNoise1D(chunkCoordinate.X * ws.chunkSize + x) > 0f)
                    // {
                    //     tiles[y][x] = new Tile(new Vector2I(x, y), 5);
                    // }
                    // else
                    // {
                    //     tiles[y][x] = new Tile(new Vector2I(x, y), 0);
                    // }
                    // tiles[y][x] = new Tile(new Vector2I(x, y), (byte)wm.random.Next(0, 5));
                }
            }
            chunk.tiles = tiles;
            // GD.Print("Chunk: " + chunk.tiles[0].Length);
            return chunk;
        }
    }
}
