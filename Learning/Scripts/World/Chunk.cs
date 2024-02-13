//System
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

		public Chunk(Vector3 position)
		{
			this.position = position;

			chunkVerts = new List<Vector3>();
			chunkUVs = new List<Vector2>();
			chunkIndices = new List<uint>();


			GenBlocks();
			BuildChunk();
		}

		//generates data
		public void GenChunk()
		{

		}

		//generates correct block faces
		public void GenBlocks()
		{
            for (int i = 0; i < 3; i++)
            {
				Block block = new Block(new Vector3(i, 0, 0));

				var frontFaceData = block.GetFace(Faces.front);
				chunkVerts.AddRange(frontFaceData.vertices);
				chunkUVs.AddRange(frontFaceData.uv);

				var leftFaceData = block.GetFace(Faces.left);
				chunkVerts.AddRange(leftFaceData.vertices);
				chunkUVs.AddRange(leftFaceData.uv);

				var rightFaceData = block.GetFace(Faces.right);
				chunkVerts.AddRange(rightFaceData.vertices);
				chunkUVs.AddRange(rightFaceData.uv);

				var backFaceData = block.GetFace(Faces.back);
				chunkVerts.AddRange(backFaceData.vertices);
				chunkUVs.AddRange(backFaceData.uv);

				var topFaceData = block.GetFace(Faces.top);
				chunkVerts.AddRange(topFaceData.vertices);
				chunkUVs.AddRange(topFaceData.uv);

				var bottomFaceData = block.GetFace(Faces.bottom);
				chunkVerts.AddRange(bottomFaceData.vertices);
				chunkUVs.AddRange(bottomFaceData.uv);


				AddFaceIndices(6);
            }
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
