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
        private WorldManager worldManager;
        private WorldSettings worldSettings
        {
            get { return worldManager.worldSettings; }
        }

        [Export]
        private Vector2I coordinate;

        [Export]
        private Vector2I chunkCoordinate;
        private Vector2I lastChunkCoordinate;

        private Queue<Vector2I> chunksToRender = new Queue<Vector2I>();

        public override void _Ready()
        {
            if (worldManager == null)
            {
                worldManager = WorldManager.Instance;
            }

            // Called every time the node is added to the scene.
            // Initialization here
        }

        public override void _Process(double delta)
        {
            CalculatePositions();

            RenderChunksInView();
        }

        private void CalculatePositions()
        {
            Vector2 offset = new Vector2(
                worldSettings.worldWidth * worldSettings.chunkSize * worldSettings.tileSize / 2,
                worldSettings.worldHeight * worldSettings.chunkSize * worldSettings.tileSize / 2
            );
            //bitshift instead?
            coordinate = new Vector2I(
                (int)(((GlobalPosition.X + offset.X) / worldSettings.tileSize)),
                (int)(((-GlobalPosition.Y + offset.Y) / worldSettings.tileSize))
            );
            chunkCoordinate = new Vector2I(
                (int)(coordinate.X / worldSettings.chunkSize),
                (int)(coordinate.Y / worldSettings.chunkSize)
            );
        }

        private void QueueChunks() { }

        private void RenderChunksInView()
        {
            if (lastChunkCoordinate == chunkCoordinate)
            {
                return;
            }

            WorldRenderer.Instance.ModulateChunk(lastChunkCoordinate, new Color(1, 1, 1));
            WorldRenderer.Instance.ModulateChunk(chunkCoordinate, new Color(1, 0, 0));
            lastChunkCoordinate = chunkCoordinate;
        }
    }
}
