//System
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MinecraftClone.Scripts.Player;

//Open TK
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace MinecraftClone.Scripts.World
{
	internal class Block
	{
		public Vector3 position;

		public BlockType type;

		private Dictionary<Faces, FaceData> faces;

		public List<Vector2> dirtUV = new List<Vector2>
		{
			new Vector2(0f, 1f),
			new Vector2(1f, 1f),
			new Vector2(1f, 0f),
			new Vector2(0f, 0f),
		};

		public Block(Vector3 position, BlockType blockType = BlockType.air)
		{
			this.position = position;
			this.type = blockType;

			faces = new Dictionary<Faces, FaceData>
			{
				{
					Faces.front, new FaceData
					{
						vertices = AddTransformedVertices(FaceDataRaw.rawVertexData[Faces.front]),
						uv = dirtUV
					}
				},
				{
					Faces.back, new FaceData
					{
						vertices = AddTransformedVertices(FaceDataRaw.rawVertexData[Faces.back]),
						uv = dirtUV
					}
				},
				{
					Faces.left, new FaceData
					{
						vertices = AddTransformedVertices(FaceDataRaw.rawVertexData[Faces.left]),
						uv = dirtUV
					}
				},
				{
					Faces.right, new FaceData
					{
						vertices = AddTransformedVertices(FaceDataRaw.rawVertexData[Faces.right]),
						uv = dirtUV
					}
				},
				{
					Faces.top, new FaceData
					{
						vertices = AddTransformedVertices(FaceDataRaw.rawVertexData[Faces.top]),
						uv = dirtUV
					}
				},
				{
					Faces.bottom, new FaceData
					{
						vertices = AddTransformedVertices(FaceDataRaw.rawVertexData[Faces.bottom]),
						uv = dirtUV
					}
				},

			};
		}

		public List<Vector3> AddTransformedVertices(List<Vector3> vetices)
		{
			List<Vector3> transformedVertices = new List<Vector3>();

			foreach (var vert in vetices) 
			{
				transformedVertices.Add(vert + position);
			}

			return transformedVertices;
		}

		public FaceData GetFace(Faces face)
		{
			return faces[face];
		}
	}
}
