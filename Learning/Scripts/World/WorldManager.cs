﻿//System
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
			program = new ShaderProgram("Default.vert", "Default.frag");
		}

		public void GenerateWorld()
		{
			//generates heightmap
			float[,] heightMap = GenerateHeightMap();

			//sets the position for the chunks to be centered
			Vector3 startPos = Vector3.Zero;
			startPos.X -= renderDistance * 8;
			startPos.Z -= renderDistance * 8;

			for (int x = 0; x < renderDistance; x++)
			{
				//x chunk position
				Vector3 vPos = new Vector3(startPos.X, startPos.Y, startPos.Z);
				vPos.X = startPos.X + (16 * x);
				
				//adds the chunk to the list 
				chunks.Add(new Chunk(vPos, heightMap));

				for (int y = 0; y < renderDistance; y++)
				{
					//y chunk position acording to x chunk
					Vector3 cPos = new Vector3(startPos.X, startPos.Y, startPos.Z);
					cPos.X = vPos.X;
					cPos.Z = startPos.X + (y * 16);

					//adds the chunk to the list 
					chunks.Add(new Chunk(cPos, heightMap));
				}
			}

			RenderWorld();
		}

		//generates heightmap
		public float[,] GenerateHeightMap()
		{
			float[,] heightMap = new float[256, 256];

			//random seed
			Random rnd = new Random();
			int seedRand = rnd.Next(-100000, 100000);

			SimplexNoise.Noise.Seed = seedRand;

			for (int x = 0; x < 256; x++)
			{
				for (int z = 0; z < 256; z++)
				{
					heightMap[x, z] = SimplexNoise.Noise.CalcPixel2D(x, z, 0.01f);
				}
			}
			return heightMap;
		}
		public void RenderWorld()
		{
			for (int i = 0; i < chunks.Count; i++)
			{
				chunks[i].Render(program);
			}
		}

		public void Unload()
		{
			for (int i = 0; i < chunks.Count; i++)
			{
				chunks[i].Delete();
			}
		}
	}
}
