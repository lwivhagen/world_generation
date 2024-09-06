using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using world_generation.scripts.other;
using world_generation.scripts.types;

namespace world_generation.scripts.managers
{
    public partial class WorldManager : Node2D
    {
        private static WorldManager instance;

        #region Singleton


        public static WorldManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new WorldManager();
                }
                return instance;
            }
        }

        private void SingletonCheck()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                GD.PrintErr("WorldManager instance already exists!");
                QueueFree();
            }
        }
        #endregion
        public WorldSettings worldSettings { get; private set; }
        public Random random { get; private set; }

        private Dictionary<Vector2I, Chunk> chunks = new Dictionary<Vector2I, Chunk>();

        public override void _Ready()
        {
            SingletonCheck();
            worldSettings = new WorldSettings();
            random = new Random(worldSettings.seed);
            // Chunk[] test = WorldGenerator.GenerateWorld(worldSettings);
            // foreach (Chunk chunk in test)
            // {
            //     GD.Print("Chunk: " + chunk.coordinate);
            // }

            chunks = WorldGenerator
                .GenerateWorld(worldSettings)
                .ToDictionary(c => c.coordinate, c => c);
            GD.Print("chunks: " + chunks.Count);
            WorldRenderer wr = WorldRenderer.Instance;
            foreach (Chunk chunk in chunks.Values)
            {
                wr.RenderChunk(chunk);
            }
        }
    }
}
