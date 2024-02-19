//System
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoxelEngine.Scripts.Player;


//Open TK
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace VoxelEngine.Scripts.World.Data
{
    //the UVs and Vertices of each face
    public struct FaceData
    {
        public List<Vector3> vertices;
        public List<Vector2> uv;
    }
}
