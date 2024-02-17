//System
using System;
using System.Collections.Generic;
using System.Linq;
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

//STB
using StbImageSharp;

namespace MinecraftClone.Scripts.World
{
	internal class TextureData
	{
		public static readonly Dictionary<BlockType, Dictionary<Faces, List<Vector2>>> blockTypeUVs = new Dictionary<BlockType, Dictionary<Faces, List<Vector2>>>()
		{
			{BlockType.dirt, new Dictionary<Faces, List<Vector2>>()
			{
				{Faces.front, new List<Vector2>()
				{
					new Vector2(2/16f, 15/16f),
					new Vector2(3/16f, 15/161f),
					new Vector2(3/16f, 1f),
					new Vector2(2/16f, 1f),
				}
				},
				{Faces.back, new List<Vector2>()
				{
					new Vector2(2/16f, 15/16f),
					new Vector2(3/16f, 15/161f),
					new Vector2(3/16f, 1f),
					new Vector2(2/16f, 1f),
				}
				},
				{Faces.top, new List<Vector2>()
				{
					new Vector2(2/16f, 15/16f),
					new Vector2(3/16f, 15/161f),
					new Vector2(3/16f, 1f),
					new Vector2(2/16f, 1f),
				}
				},
				{Faces.bottom, new List<Vector2>()
				{
					new Vector2(2/16f, 15/16f),
					new Vector2(3/16f, 15/161f),
					new Vector2(3/16f, 1f),
					new Vector2(2/16f, 1f),
				}
				},
				{Faces.left, new List<Vector2>()
				{
					new Vector2(2/16f, 15/16f),
					new Vector2(3/16f, 15/161f),
					new Vector2(3/16f, 1f),
					new Vector2(2/16f, 1f),
				}
				},
				{Faces.right, new List<Vector2>()
				{
					new Vector2(2/16f, 15/16f),
					new Vector2(3/16f, 15/161f),
					new Vector2(3/16f, 1f),
					new Vector2(2/16f, 1f),
				}
				},
			}
			}
		};
	}
}
