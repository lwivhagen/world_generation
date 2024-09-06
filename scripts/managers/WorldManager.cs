using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            Viewer viewer = new Viewer();
            viewer.Name = "Main Viewer";
            viewer.Texture = GD.Load<Texture2D>("res://assets/white_16x16.png");
            viewer.Modulate = new Color(1, 0, 0);
            GetViewport().GetCamera2D().AddChild(viewer);
        }

        public override void _Process(double delta)
        {
            base._Process(delta);
            // Chunk[] chunksToRender = GetChunksToRender();
        }

        // private Chunk [] GetChunksToRender(Vector2I coord){
        //    Chunk[] chunksToRender = new Chunk[1 + worldSettings];

        // }
    }
}
