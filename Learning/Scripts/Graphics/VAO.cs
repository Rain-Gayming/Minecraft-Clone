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

namespace MinecraftClone.Scripts.Graphics
{
	internal class VAO
	{
		public int id;

		public VAO()
		{
			id = GL.GenVertexArray();
			GL.BindVertexArray(id);
		}

		public void LinkToVAO(int location, int size, VBO vbo)
		{
			Bind();
			vbo.Bind();
			GL.VertexAttribPointer(location, size, VertexAttribPointerType.Float, false, 0, 0);
			GL.EnableVertexAttribArray(location);
			Unbind();
		}
		public void Bind()
		{
			GL.BindVertexArray(id);
		}
		public void Unbind()
		{
			GL.BindVertexArray(0);
		}
		public void Delete()
		{
			GL.DeleteVertexArray(id);
		}
	}
}
