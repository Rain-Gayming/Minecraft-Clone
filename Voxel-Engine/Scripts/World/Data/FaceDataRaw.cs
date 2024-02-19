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

namespace MinecraftClone.Scripts.World.Data
{
    public struct FaceDataRaw
    {
        //the vertex data needed for a cube
        public static readonly Dictionary<Faces, List<Vector3>> rawCubeVertexData = new Dictionary<Faces, List<Vector3>>
        {
            {
                Faces.front, new List<Vector3>()
                {
                    new Vector3(-0.5f, 0.5f, 0.5f), //topleft vert
					new Vector3(0.5f, 0.5f, 0.5f), //topright vert
					new Vector3(0.5f, -0.5f, 0.5f), //bottomright vert
					new Vector3(-0.5f, -0.5f, 0.5f), //bottomleft vert
				}
            },
            {
                Faces.back, new List<Vector3>()
                {
                    new Vector3(0.5f, 0.5f, -0.5f), //topleft vert
					new Vector3(-0.5f, 0.5f, -0.5f), //topright vert
					new Vector3(-0.5f, -0.5f, -0.5f), //bottomright vert
					new Vector3(0.5f, -0.5f, -0.5f), //bottomleft vert
				}
            },
            {
                Faces.left, new List<Vector3>()
                {
                    new Vector3(-0.5f, 0.5f, -0.5f), //topleft vert
					new Vector3(-0.5f, 0.5f, 0.5f), //topright vert
					new Vector3(-0.5f, -0.5f, 0.5f), //bottomright vert
					new Vector3(-0.5f, -0.5f, -0.5f), //bottomleft vert
				}
            },
            {
                Faces.right, new List<Vector3>()
                {
                    new Vector3(0.5f, 0.5f, 0.5f), //topleft vert
					new Vector3(0.5f, 0.5f, -0.5f), //topright vert
					new Vector3(0.5f, -0.5f, -0.5f), //bottomright vert
					new Vector3(0.5f, -0.5f, 0.5f), //bottomleft vert
				}
            },
            {
                Faces.top, new List<Vector3>()
                {
                    new Vector3(-0.5f, 0.5f, -0.5f), //topleft vert
					new Vector3(0.5f, 0.5f, -0.5f), //topright vert
					new Vector3(0.5f, 0.5f, 0.5f), //bottomright vert
					new Vector3(-0.5f, 0.5f, 0.5f), //bottomleft vert
				}
            },
            {
                Faces.bottom, new List<Vector3>()
                {
                    new Vector3(-0.5f, -0.5f, 0.5f), //topleft vert
					new Vector3(0.5f, -0.5f, 0.5f), //topright vert
					new Vector3(0.5f, -0.5f, -0.5f), //bottomright vert
					new Vector3(-0.5f, -0.5f, -0.5f), //bottomleft vert
				}
            },
        };

		//the vertex data needed for a stair
		public static readonly Dictionary<Faces, List<Vector3>> rawStairVertexData = new Dictionary<Faces, List<Vector3>>
		{
			{
				Faces.front, new List<Vector3>()
				{
					new Vector3(-0.5f, 0.5f, 0.5f), //topleft vert
					new Vector3(0.5f, 0.5f, 0.5f), //topright vert
					new Vector3(0.5f, -0.5f, 0.5f), //bottomright vert
					new Vector3(-0.5f, -0.5f, 0.5f), //bottomleft vert
				}
			},
			{
				Faces.back, new List<Vector3>()
				{
					new Vector3(0.5f, 0.5f, -0.5f), //topleft vert
					new Vector3(-0.5f, 0.5f, -0.5f), //topright vert
					new Vector3(-0.5f, -0.5f, -0.5f), //bottomright vert
					new Vector3(0.5f, -0.5f, -0.5f), //bottomleft vert
				}
			},
			{
				Faces.left, new List<Vector3>()
				{
					new Vector3(0f, 0f, -0.5f), //topleft vert
					new Vector3(-0.5f, 0.5f, 0.5f), //topright vert
					new Vector3(-0.5f, -0.5f, 0.5f), //bottomright vert
					new Vector3(-0.5f, -0.5f, -0.5f), //bottomleft vert
				}
			},
			{
				Faces.right, new List<Vector3>()
				{
					new Vector3(0f, 0f, 0f), //topleft vert
					new Vector3(0.5f, 0.5f, -0.5f), //topright vert
					new Vector3(0.5f, -0.5f, -0.5f), //bottomright vert
					new Vector3(0.5f, -0.5f, 0.5f), //bottomleft vert
				}
			},
			{
				Faces.top, new List<Vector3>()
				{
					new Vector3(-0.5f, 0.5f, 0f), //topleft vert
					new Vector3(0.5f, 0.5f, 0f), //topright vert
					new Vector3(0.5f, 0.5f, 0.5f), //bottomright vert
					new Vector3(-0.5f, 0.5f, 0.5f), //bottomleft vert
				}
			},
			{
				Faces.bottom, new List<Vector3>()
				{
					new Vector3(-0.5f, -0.5f, 0.5f), //topleft vert
					new Vector3(0.5f, -0.5f, 0.5f), //topright vert
					new Vector3(0.5f, -0.5f, -0.5f), //bottomright vert
					new Vector3(-0.5f, -0.5f, -0.5f), //bottomleft vert
				}
			},
		};
	}
}
