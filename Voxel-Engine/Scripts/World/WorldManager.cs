 //System
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using VoxelEngine.Scripts.Graphics;
using VoxelEngine.Scripts.Player;
using VoxelEngine.Scripts.World;


//Open TK
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;


namespace VoxelEngine.Scripts.World
{
	internal class WorldManager
	{
		public ShaderProgram program;

		public List<Chunk> chunks = new List<Chunk>();
		public int renderDistance;

		public WorldManager(int renderDistance)
		{
			this.renderDistance = renderDistance;

			//sets the shader program
			program = new ShaderProgram("Default.vert", "Default.frag");
		}

		public void GenerateWorld()
		{
			Vector3 cameraPos = Camera._position;

            for (int i = 0; i < renderDistance; i++)
            {
				AddChunk(new Vector3(new Vector3(cameraPos.X + i, 0, cameraPos.Y + i)));
            }
        }

		public void AddChunk(Vector3 position)
		{
			//makes chunk
			Chunk newChunk = new Chunk(position);
			newChunk.SetHeightMap(GenerateHeightMap(newChunk.chunkPosition));

			//adds the chunk to the list 
			chunks.Add(newChunk);
		}

		//generates heightmap
		public float[,] GenerateHeightMap(Vector3 chunkPosition)
		{
			//sets heightmap size of a chunk
			float[,] heightMap = new float[32, 32];

			//random seed
			Random rnd = new Random();
			int seedRand = rnd.Next(-100000000, 100000000);

			//generates the heightmap
			SimplexNoise.Noise.Seed = seedRand;

			for (int x = 0; x < 16; x++)
			{
				for (int z = 0; z < 16; z++)
				{
					heightMap[x, z] = SimplexNoise.Noise.CalcPixel2D(x, z, 0.01f);
				}
			}
			return heightMap;
		}
		public void RenderWorld()
		{

			//renders the chunks
			for (int i = 0; i < chunks.Count; i++)
			{
				chunks[i].RenderChunk(program);
			}
		}

		public void DeleteWorld()
		{
			//deletes the chunks
			for (int i = 0; i < chunks.Count; i++)
			{
				chunks[i].DeleteChunk();
			}
		}
	}
}

