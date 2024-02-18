//System
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    internal class Block
	{
		public Vector3 position;

		public BlockType type;

		private Dictionary<Faces, FaceData> faces;

		//gets the position of the faces UVs
		public Dictionary<Faces, List<Vector2>> GetUVsFromCoordinates(Dictionary<Faces, Vector2> coords)
		{
			Dictionary<Faces, List<Vector2>> faceData = new Dictionary<Faces, List<Vector2>>();

			foreach (var faceCoord in coords)
			{
				faceData[faceCoord.Key] = new List<Vector2>()
				{
					new Vector2((faceCoord.Value.X + 1)/ 16, (faceCoord.Value.Y + 1) / 16),
					new Vector2(faceCoord.Value.X / 16, (faceCoord.Value.Y + 1) / 16),
					new Vector2(faceCoord.Value.X / 16, faceCoord.Value.Y / 16),
					new Vector2((faceCoord.Value.X + 1) / 16, faceCoord.Value.Y / 16),
				};
			}

			return faceData;
		}

		//dictionary of the faces on the cube
		public Dictionary<Faces, List<Vector2>> blockUV = new Dictionary<Faces, List<Vector2>>()
		{
			{Faces.top, new List<Vector2>() },
			{Faces.bottom, new List<Vector2>() },
			{Faces.front, new List<Vector2>() },
			{Faces.back, new List<Vector2>() },
			{Faces.left, new List<Vector2>() },
			{Faces.right, new List<Vector2>() },
		};

		public Block(Vector3 position, BlockType blockType = BlockType.air)
		{
			this.position = position;
			this.type = blockType;

			//gets the UV coords for each face
			if(blockType != BlockType.air)
			{
				blockUV = GetUVsFromCoordinates(TextureData.blockTypeUVCoord[blockType]);
			}

			//sets textures per face
			faces = new Dictionary<Faces, FaceData>
			{
				{
					Faces.top, new FaceData
					{
						vertices = AddTransformedVertices(FaceDataRaw.rawVertexData[Faces.top]),
						uv = blockUV[Faces.top]
					}
				},
				{
					Faces.bottom, new FaceData
					{
						vertices = AddTransformedVertices(FaceDataRaw.rawVertexData[Faces.bottom]),
						uv = blockUV[Faces.bottom]
					}
				},
				{
					Faces.front, new FaceData
					{
						vertices = AddTransformedVertices(FaceDataRaw.rawVertexData[Faces.front]),
						uv = blockUV[Faces.front]
					}
				},
				{
					Faces.back, new FaceData
					{
						vertices = AddTransformedVertices(FaceDataRaw.rawVertexData[Faces.back]),
						uv = blockUV[Faces.back]
					}
				},
				{
					Faces.left, new FaceData
					{
						vertices = AddTransformedVertices(FaceDataRaw.rawVertexData[Faces.left]),
						uv = blockUV[Faces.left]
					}
				},
				{
					Faces.right, new FaceData
					{
						vertices = AddTransformedVertices(FaceDataRaw.rawVertexData[Faces.right]),
						uv = blockUV[Faces.right]
					}
				}

			};
		}

		//adds blcok vertices
		public List<Vector3> AddTransformedVertices(List<Vector3> vetices)
		{
			List<Vector3> transformedVertices = new List<Vector3>();

			foreach (var vert in vetices) 
			{
				transformedVertices.Add(vert + position);
			}

			return transformedVertices;
		}

		//gets a face
		public FaceData GetFace(Faces face)
		{
			return faces[face];
		}
	}
}
