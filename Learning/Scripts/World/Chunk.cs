//System
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using MinecraftClone.Scripts.Graphics;
using MinecraftClone.Scripts.Player;

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

		const int size = 16;
		const int maxChunkHeight = 32;
		public Vector3 position;

		uint indexCount;

		VAO chunkVAO;
		VBO chunkVertexVBO;
		VBO chunkUVVBO;
		IBO chunkIBO;

		Texture texture;

		Block[,,] chunkBlocks;

		public Chunk(Vector3 position)
		{
			this.position = position;

			chunkVerts = new List<Vector3>();
			chunkUVs = new List<Vector2>();
			chunkIndices = new List<uint>();

			chunkBlocks = new Block[size, maxChunkHeight, size];
			float[,] heightMap = GenChunk();

			GenBlocks(heightMap);
			GenFaces(heightMap);
			BuildChunk();
		}

		//generates data
		public float[,] GenChunk()
		{
			float[,] heightMap = new float[size, size];

			//random seed
			Random rnd = new Random();
			int seedRand = rnd.Next(-100000, 100000);

			SimplexNoise.Noise.Seed = seedRand;

			for (int x = 0; x < size; x++)
			{
				for (int z = 0; z < size; z++)
				{
					heightMap[x, z] = SimplexNoise.Noise.CalcPixel2D(x, z, 0.01f);
				}
			}
			return heightMap;
		}

		//generates correct block faces
		public void GenBlocks(float[,] heightMap)
		{
            for (int x = 0; x < size; x++)
            {
				for (int z = 0; z < size; z++)
				{
					int columnHeight = (int)(heightMap[x, z] / 10);
					for (int y = 0; y < maxChunkHeight; y++)
					{
						if (y < columnHeight)
						{
							chunkBlocks[x, y, z] = new Block(new Vector3(x + position.X, y, z + position.Z), BlockType.stone);
						}
						else
						{
							chunkBlocks[x, y, z] = new Block(new Vector3(x + position.X, y, z + position.Z), BlockType.air);
						}
					}
				}
			}
        }

		public void GenFaces(float[,] heightMap)
		{

			for (int x = 0; x < size; x++)
			{
				for (int z = 0; z < size; z++)
				{
					int columnHeight = (int)(heightMap[x, z] / 10);
					for (int y = 0; y < columnHeight; y++)
					{
						int numFaces = 0;
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
						if (x < size - 1)
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
						if (y < columnHeight - 1)
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
						if (z < size - 1)
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
			texture = new Texture("dirtTex.png");
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
