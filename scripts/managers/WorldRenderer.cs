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
        private List<Node2D> chunkNodePool = new List<Node2D>();

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
            if (!chunkNodes.ContainsKey(coord))
            {
                GD.PrintErr("UnRender failed, Chunk not found");
                return;
            }
            RecycleChunkNode(coord, chunkNodes[coord]);
        }

        private Node2D AquireChunkNode(Chunk chunk)
        {
            if (chunkNodes.ContainsKey(chunk.coordinate))
            {
                return chunkNodes[chunk.coordinate];
            }
            else if (chunkNodePool.Count > 0)
            {
                return ReviveChunkNode(chunk);
            }
            else
            {
                chunkNodes.Add(chunk.coordinate, CreateChunkNode(chunk));
                return chunkNodes[chunk.coordinate];
            }
        }

        private Node2D ReviveChunkNode(Chunk chunk)
        {
            Node2D chunkNode = chunkNodePool[0];
            chunkNodePool.RemoveAt(0);
            chunkNode.Visible = true;
            chunkNode.Name = "chunk " + chunk.coordinate.X + ", " + chunk.coordinate.Y;
            chunkNode.Position = ChunkCoordinateToGlobalPosition(chunk.coordinate);
            chunkNodes.Add(chunk.coordinate, chunkNode);
            return chunkNode;
        }

        private void RecycleChunkNode(Vector2I coord, Node2D chunkNode)
        {
            chunkNode.Visible = false;
            chunkNodes.Remove(coord);
            chunkNodePool.Add(chunkNode);
        }

        private Node2D CreateChunkNode(Chunk chunk)
        {
            WorldManager wm = WorldManager.Instance;
            WorldSettings ws = wm.worldSettings;

            //Godot has position in middle of sprite, so we need to offset it
            Vector2 anchorOffset = new Vector2(ws.tileSize / 2, -ws.tileSize / 2);
            Vector2 centerOffset = new Vector2(
                -ws.worldWidth * ws.chunkSize * ws.tileSize / 2,
                ws.worldHeight * ws.chunkSize * ws.tileSize / 2
            );
            Vector2 position = new Vector2(
                chunk.coordinate.X * ws.chunkSize * ws.tileSize,
                -chunk.coordinate.Y * ws.chunkSize * ws.tileSize
            );
            Node2D chunkNode = new Node2D();
            chunkNode.Name = "chunk " + chunk.coordinate.X + ", " + chunk.coordinate.Y;
            chunkNode.Position = position + anchorOffset + centerOffset;

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
                        -tile.coordinate.Y % ws.chunkSize * ws.tileSize
                    );

                    chunkNode.AddChild(tileNode);
                }
            }
            return chunkNode;
        }

        private Vector2 ChunkCoordinateToGlobalPosition(Vector2I coord)
        {
            WorldManager wm = WorldManager.Instance;
            WorldSettings ws = wm.worldSettings;
            Vector2 anchorOffset = new Vector2(ws.tileSize / 2, -ws.tileSize / 2);
            Vector2 centerOffset = new Vector2(
                -ws.worldWidth * ws.chunkSize * ws.tileSize / 2,
                ws.worldHeight * ws.chunkSize * ws.tileSize / 2
            );
            Vector2 position = new Vector2(
                coord.X * ws.chunkSize * ws.tileSize,
                -coord.Y * ws.chunkSize * ws.tileSize
            );
            return position + anchorOffset + centerOffset;
        }

        public void ModulateChunk(Vector2I coord, Color color)
        {
            if (chunkNodes.ContainsKey(coord))
            {
                chunkNodes[coord].Modulate = color;
            }
        }

        public void RenderChunk(Chunk chunk)
        {
            Node2D chunkNode = AquireChunkNode(chunk);

            //Update textures
            for (int y = 0; y < chunk.tiles.Length; y++)
            {
                for (int x = 0; x < chunk.tiles[y].Length; x++)
                {
                    Sprite2D tileNode = chunkNode.GetChild<Sprite2D>(y * chunk.tiles.Length + x);

                    tileNode.Texture = textures[chunk.tiles[y][x].id];
                    if (chunk.tiles[y][x].id == 0)
                    {
                        tileNode.Modulate = new Color(0, 0, 0);
                    }
                    else
                    {
                        tileNode.Modulate = new Color(1, 1, 1);
                    }
                }
            }
        }
    }
}
