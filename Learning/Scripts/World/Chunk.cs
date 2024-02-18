//System
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using MinecraftClone.Scripts.Graphics;
using MinecraftClone.Scripts.Player;
using MinecraftClone.Scripts.World.Data;


//Open TK
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace MinecraftClone.Scripts.World
{
    internal class Chunk
	{
		private List<Vector3> chunkVerts;
		private List<Vector2> chunkUVs;
		private List<uint> chunkIndices;

		public Biome chunkBiome;

		const int maxChunkSize = 16;
		const int maxChunkHeight = 64;
		public Vector3 chunkPosition;

		uint indexCount;

		VAO chunkVAO;
		VBO chunkVertexVBO;
		VBO chunkUVVBO;
		IBO chunkIBO;

		Texture texture;

		Block[,,] chunkBlocks;

		public Chunk(Vector3 position, float[,] heightMap)
		{
			this.chunkPosition = position;

			chunkVerts = new List<Vector3>();
			chunkUVs = new List<Vector2>();
			chunkIndices = new List<uint>();

			chunkBlocks = new Block[maxChunkSize, maxChunkHeight, maxChunkSize];

			List<Biome> biomes = new List<Biome>()
			{
				new Biome()
				{
					topBlock = BlockType.dirt,
					lowerBlock = BlockType.grass,
					climate = Climate.neutral,
				},
				new Biome()
				{
					topBlock = BlockType.grass,
					lowerBlock = BlockType.dirt,
					climate = Climate.warm,
				},
				new Biome()
				{
					topBlock = BlockType.stone,
					lowerBlock = BlockType.dirt,
					climate = Climate.cold,
				},
			};

			Random rnd = new Random();
			int seedRand = rnd.Next(0, biomes.Count);
			chunkBiome = biomes[seedRand];

			GenBlocks(heightMap);
			GenFaces(heightMap);
			BuildChunk();
		}

		//generates correct block faces
		public void GenBlocks(float[,] heightMap)
		{
            for (int blockX = 0; blockX < maxChunkSize; blockX++)
            {
				for (int blockZ = 0; blockZ < maxChunkSize; blockZ++)
				{
					int columnHeight = (int)(heightMap[blockX, blockZ] / 20);
					for (int blockY = 0; blockY < maxChunkHeight; blockY++)
					{
						BlockType type = BlockType.air;
						if (blockY < columnHeight - 1)
						{
							type = chunkBiome.lowerBlock;
						}
						if (blockY == columnHeight - 1)
						{
							type = chunkBiome.topBlock;
						}

						if (blockY <= columnHeight - 5)
						{
							type = BlockType.stone;
						}
						chunkBlocks[blockX, blockY, blockZ] = new Block(new Vector3(blockX + chunkPosition.X, blockY, blockZ + chunkPosition.Z), type);						
					}
				}
			}
        }

		public void GenFaces(float[,] heightMap)
		{

			for (int x = 0; x < maxChunkSize; x++)
			{
				for (int z = 0; z < maxChunkSize; z++)
				{
					for (int y = 0; y < maxChunkHeight; y++)
					{
						int numFaces = 0;

						if (chunkBlocks[x, y, z].type != BlockType.air)
						{//left faces
							if (x > 0)
							{
								if (chunkBlocks[x - 1, y, z].type == BlockType.air)
								{
									IntegrateFace(chunkBlocks[x, y, z], Faces.left);
									numFaces++;
								}
							}
							else
							{
								IntegrateFace(chunkBlocks[x, y, z], Faces.left);
								numFaces++;
							}

							//right faces
							if (x < maxChunkSize - 1)
							{
								if (chunkBlocks[x + 1, y, z].type == BlockType.air)
								{
									IntegrateFace(chunkBlocks[x, y, z], Faces.right);
									numFaces++;
								}
							}
							else
							{
								IntegrateFace(chunkBlocks[x, y, z], Faces.right);
								numFaces++;
							}

							//top faces
							if (y < maxChunkHeight - 1)
							{
								if (chunkBlocks[x, y + 1, z].type == BlockType.air)
								{
									IntegrateFace(chunkBlocks[x, y, z], Faces.top);
									numFaces++;
								}
							}
							else
							{
								IntegrateFace(chunkBlocks[x, y, z], Faces.top);
								numFaces++;
							}

							//bottom faces
							if (y > 0)
							{
								if (chunkBlocks[x, y - 1, z].type == BlockType.air)
								{
									IntegrateFace(chunkBlocks[x, y, z], Faces.bottom);
									numFaces++;
								}
							}
							else
							{
								IntegrateFace(chunkBlocks[x, y, z], Faces.bottom);
								numFaces++;
							}

							//front faces
							if (z < maxChunkSize - 1)
							{
								if (chunkBlocks[x, y, z + 1].type == BlockType.air)
								{
									IntegrateFace(chunkBlocks[x, y, z], Faces.front);
									numFaces++;
								}
							}
							else
							{
								IntegrateFace(chunkBlocks[x, y, z], Faces.front);
								numFaces++;
							}

							//back faces
							if (z > 0)
							{
								if (chunkBlocks[x, y, z - 1].type == BlockType.air)
								{
									IntegrateFace(chunkBlocks[x, y, z], Faces.back);
									numFaces++;
								}
							}
							else
							{
								IntegrateFace(chunkBlocks[x, y, z], Faces.back);
								numFaces++;
							}


							AddFaceIndices(numFaces);
						}						
					}
				}
			}
		}

		public void IntegrateFace(Block block, Faces face)
		{
			var faceData = block.GetFace(face);
			chunkVerts.AddRange(faceData.vertices);
			chunkUVs.AddRange(faceData.uv);
		}

		public void AddFaceIndices(int amount)
		{
            for (int i = 0; i < amount; i++)
			{
				chunkIndices.Add(0 + indexCount);
				chunkIndices.Add(1 + indexCount);
				chunkIndices.Add(2 + indexCount);
				chunkIndices.Add(2 + indexCount);
				chunkIndices.Add(3 + indexCount);
				chunkIndices.Add(0 + indexCount);

				indexCount += 4;
			}
        }

		//proccess data for rendering
		public void BuildChunk()
		{
			chunkVAO = new VAO();
			chunkVAO.Bind();

			chunkVertexVBO = new VBO(chunkVerts);
			chunkVertexVBO.Bind();
			chunkVAO.LinkToVAO(0, 3, chunkVertexVBO);

			chunkUVVBO = new VBO(chunkUVs);
			chunkUVVBO.Bind();
			chunkVAO.LinkToVAO(1, 2, chunkUVVBO);

			chunkIBO = new IBO(chunkIndices);
			texture = new Texture("atlas.png");
		}

		//renders the cunk
		public void Render(ShaderProgram program)
		{
			program.Bind();
			chunkVAO.Bind();
			chunkIBO.Bind();
			texture.Bind();
			GL.DrawElements(PrimitiveType.Triangles, chunkIndices.Count, DrawElementsType.UnsignedInt, 0);
		}

		public void Delete()
		{
			chunkVAO.Delete();
			chunkVertexVBO.Delete();
			chunkUVVBO.Delete();
			texture.Delete();
			chunkIBO.Delete();
		}
	}
}
