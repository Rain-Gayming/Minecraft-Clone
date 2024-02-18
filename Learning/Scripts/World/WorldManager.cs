//System
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using MinecraftClone.Scripts.Graphics;
using MinecraftClone.Scripts.Player;
using MinecraftClone.Scripts.World;


//Open TK
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;


namespace MinecraftClone.Scripts.World
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
			//generates heightmap

			//sets the position for the chunks to be centered
			Vector3 startPos = Vector3.Zero;
			startPos.X -= renderDistance * 8;
			startPos.Z -= renderDistance * 8;

			for (int x = 0; x < renderDistance; x++)
			{
				//purely to set the x position
				Vector3 vPos = new Vector3(startPos.X, startPos.Y, startPos.Z);
				vPos.X = startPos.X + (16 * x);
				
				for (int y = 0; y < renderDistance; y++)
				{
					float[,] heightMapY = GenerateHeightMap();
					//y chunk position acording to x chunk
					Vector3 cPos = new Vector3(startPos.X, startPos.Y, startPos.Z);
					cPos.X = vPos.X;
					cPos.Z = startPos.X + (y * 16);

					//adds the chunk to the list 
					chunks.Add(new Chunk(cPos, heightMapY));
				}
			}

			RenderWorld();
		}

		//generates heightmap
		public float[,] GenerateHeightMap()
		{
			//sets heightmap size of a chunk
			float[,] heightMap = new float[16, 16];

			//random seed
			Random rnd = new Random();
			int seedRand = rnd.Next(-100000, 100000);

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

