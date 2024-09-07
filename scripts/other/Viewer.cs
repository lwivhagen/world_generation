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

        private List<Vector2I> chunksInView = new List<Vector2I>();
        private List<Vector2I> chunksToRender = new List<Vector2I>();
        private List<Vector2I> chunksToUnRender = new List<Vector2I>();

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
            if (lastChunkCoordinate != chunkCoordinate)
            {
                QueueChunks();
                RenderQueue();
            }
            lastChunkCoordinate = chunkCoordinate;
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

        private void QueueChunks()
        {
            List<Vector2I> inNewView = new List<Vector2I>();
            for (int x = -worldSettings.renderDistance.X; x <= worldSettings.renderDistance.X; x++)
            {
                for (
                    int y = -worldSettings.renderDistance.Y;
                    y <= worldSettings.renderDistance.Y;
                    y++
                )
                {
                    Vector2I chunkCoord = new Vector2I(
                        chunkCoordinate.X + x,
                        chunkCoordinate.Y + y
                    );
                    inNewView.Add(chunkCoord);
                }
            }
            List<Vector2I> inViewLastFrame = new List<Vector2I>(chunksInView);
            chunksToRender = inNewView.Except(inViewLastFrame).ToList();
            chunksToUnRender = inViewLastFrame.Except(inNewView).ToList();
            chunksInView = inNewView;
        }

        private void RenderQueue()
        {
            foreach (Vector2I chunkCoord in chunksToRender)
            {
                WorldRenderer.Instance.RenderChunk(worldManager.chunks[chunkCoord]);
                WorldRenderer.Instance.ModulateChunk(chunkCoord, new Color(1, 0, 0));
            }
            foreach (Vector2I chunkCoord in chunksToUnRender)
            {
                WorldRenderer.Instance.ModulateChunk(chunkCoord, new Color(1, 1, 1));
            }

            // WorldRenderer.Instance.ModulateChunk(chunkCoordinate, new Color(0.5f, 1, 0.5f));
            // if (
            //     lastChunkCoordinate != chunkCoordinate
            //     && !chunksToRender.Contains(lastChunkCoordinate)
            // )
            // {
            //     WorldRenderer.Instance.ModulateChunk(lastChunkCoordinate, new Color(1, 1, 1));
            // }
        }
    }
}
