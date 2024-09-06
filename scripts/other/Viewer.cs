using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using world_generation.scripts.managers;

namespace world_generation.scripts.other
{
    public partial class Viewer : Sprite2D
    {
        [Export]
        private Vector2I coordinate;
        private WorldManager worldManager;
        private WorldSettings worldSettings
        {
            get { return worldManager.worldSettings; }
        }

        public override void _Ready()
        {
            if (worldManager == null)
            {
                worldManager = WorldManager.Instance;
            }
            RenderChunksInView();
            // Called every time the node is added to the scene.
            // Initialization here
        }

        public override void _Process(double delta)
        {
            float chunkSizeInUnits = worldSettings.chunkSize * worldSettings.tileSize;
            coordinate = new Vector2I(
                (int)(GlobalPosition.X % chunkSizeInUnits),
                (int)(GlobalPosition.Y % chunkSizeInUnits)
            );
        }

        private void RenderChunksInView() { }
    }
}
