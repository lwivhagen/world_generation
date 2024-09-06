using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using world_generation.scripts.other;
using world_generation.scripts.types;

namespace world_generation.scripts.managers
{
    public partial class WorldRenderer : Node2D
    {
        private static WorldRenderer instance;

        #region Singleton
        public static WorldRenderer Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new WorldRenderer();
                }
                return instance;
            }
        }
        #endregion
        private Dictionary<Vector2I, Node2D> chunkNodes = new Dictionary<Vector2I, Node2D>();

        [Export] // Assigned in inspector
        private Node2D chunkRoot = null;
        private Texture2D[] textures;

        public override void _Ready()
        {
            instance = this;
            textures = new Texture2D[256];
            for (int i = 0; i < textures.Length; i++)
            {
                textures[i] = GD.Load<Texture2D>("res://assets/white_spaced_16x16.png");
            }
        }

        public void UnRenderChunk(Vector2I coord)
        {
            if (chunkNodes.ContainsKey(coord))
            {
                chunkNodes[coord].QueueFree();
                chunkNodes.Remove(coord);
            }
        }

        private Node2D CreateChunkNode(Chunk chunk)
        {
            WorldManager wm = WorldManager.Instance;
            WorldSettings ws = wm.worldSettings;

            //Create chunk node
            Node2D chunkNode = new Node2D();
            chunkNode.Name = "chunk " + chunk.coordinate.X + ", " + chunk.coordinate.Y;
            chunkNode.Position = new Vector2(
                chunk.coordinate.X * ws.chunkSize * ws.tileSize,
                chunk.coordinate.Y * ws.chunkSize * ws.tileSize
            );

            if (chunkRoot != null)
            {
                chunkRoot.AddChild(chunkNode);
            }
            else
            {
                chunkNodes.Add(chunk.coordinate, chunkNode);
            }

            //Add tiles to chunk node
            foreach (Tile[] row in chunk.tiles)
            {
                foreach (Tile tile in row)
                {
                    Sprite2D tileNode = new Sprite2D();
                    tileNode.Name = "tile " + tile.coordinate.X + ", " + tile.coordinate.Y;
                    tileNode.Position = new Vector2(
                        tile.coordinate.X % ws.chunkSize * ws.tileSize,
                        tile.coordinate.Y % ws.chunkSize * ws.tileSize
                    );

                    chunkNode.AddChild(tileNode);
                }
            }
            return chunkNode;
        }

        public void RenderChunk(Chunk chunk)
        {
            Node2D chunkNode;
            //Add chunkNode if missing
            if (!chunkNodes.ContainsKey(chunk.coordinate))
            {
                chunkNodes.Add(chunk.coordinate, CreateChunkNode(chunk));
            }
            chunkNode = chunkNodes[chunk.coordinate];

            //Update textures
            for (int y = 0; y < chunk.tiles.Length; y++)
            {
                for (int x = 0; x < chunk.tiles[y].Length; x++)
                {
                    chunkNode.GetChild<Sprite2D>(0).Texture = textures[chunk.tiles[y][x].id];
                }
            }
        }
    }
}
