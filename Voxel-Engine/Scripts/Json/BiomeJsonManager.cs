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

//Json
using Newtonsoft.Json;
using VoxelEngine.Scripts.World.Data;
using System.Reflection;

namespace VoxelEngine.Scripts.Json
{
	internal class BiomeJsonManager
	{
		public void ReadBiomeJson(string path)
		{
			string readPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Jsons\" + path + ".json");
			Biome biome = JsonConvert.DeserializeObject<Biome>(readPath);
			Console.Write(biome.biomeName);
		}
	}
}
