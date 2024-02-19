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

		//world info
		public int waterLevel = 3;

		//chunk info
		public Biome chunkBiome;
		const int maxChunkSize = 32;
		const int maxChunkHeight = 32;
		public Vector3 chunkPosition;

		uint indexCount;

		//chunk rendering
		VAO chunkVAO;
		VBO chunkVertexVBO;
		VBO chunkUVVBO;
		IBO chunkIBO;
		Texture texture;

		//chunk blocks n stuff
		Block[,,] chunkBlocks;
		float[,] heightMap;

		public Chunk(Vector3 position )
		{
			this.chunkPosition = position;

			//creates the verts, uvs and		
			chunkVerts = new List<Vector3>();
			chunkUVs = new List<Vector2>();
			chunkIndices = new List<uint>();

			//creates a list of blocks based on the max sizes
			chunkBlocks = new Block[maxChunkSize, maxChunkHeight, maxChunkSize];

			//list of test biomes
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

			//generates a random biome for testing
			Random rnd = new Random();
			int seedRand = rnd.Next(0, biomes.Count);
			chunkBiome = biomes[seedRand];
		}

		public void SetHeightMap(float[,] map)
		{
			heightMap = map;

			//creates the chunk
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
					//height of each y row of a chunk
					int columnHeight = (int)(heightMap[blockX, blockZ] / 20);
					for (int blockY = 0; blockY < maxChunkHeight; blockY++)
					{
						//sets default block type
						BlockType type = BlockType.air;
						BlockStyle style = BlockStyle.block;
						
						//sets the top layer of a chunk
						if (blockY == columnHeight - 1)
						{
							type = chunkBiome.topBlock;
						}

						//sets below the top of a chunk
						if (blockY < columnHeight - 1)
						{
							type = chunkBiome.lowerBlock;
						}

						//sets the stone layer
						if (blockY <= columnHeight - 5)
						{
							type = BlockType.stone;
						}
						//sets the water layer
						if (blockY <= columnHeight - waterLevel)
						{
							//type = BlockType.water;
						}

						//adds the blocks
						chunkBlocks[blockX, blockY, blockZ] = new Block(new Vector3(blockX + chunkPosition.X, blockY, blockZ + chunkPosition.Z), type, style);						
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
						{
							//left faces
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
			//adds faces
			var faceData = block.GetFace(face);
			chunkVerts.AddRange(faceData.vertices);
			chunkUVs.AddRange(faceData.uv);
		}

		public void AddFaceIndices(int amount)
		{
            for (int i = 0; i < amount; i++)
			{
				//indices based on the face
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
		public void RenderChunk(ShaderProgram program)
		{
			program.Bind();
			chunkVAO.Bind();
			chunkIBO.Bind();
			texture.Bind();
			GL.DrawElements(PrimitiveType.Triangles, chunkIndices.Count, DrawElementsType.UnsignedInt, 0);
		}

		public void DeleteChunk()
		{
			chunkVAO.Delete();
			chunkVertexVBO.Delete();
			chunkUVVBO.Delete();
			texture.Delete();
			chunkIBO.Delete();
		}
	}
}
