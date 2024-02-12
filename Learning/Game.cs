//System
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Open TK
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

//STB
using StbImageSharp;


namespace MinecraftClone
{
	internal class Game : GameWindow
	{
		float[] vertices =
		{
			-0.5f, 0.5f, 0f, // top left vertex - 0
            0.5f, 0.5f, 0f, // top right vertex - 1
            0.5f, -0.5f, 0f, // bottom right - 2
            -0.5f, -0.5f, 0f // bottom left - 3
        };

		float[] texCoords =
		{
			0f, 1f,
			1, 1f,
			1f, 0f,
			0f, 0f, 
		};

		uint[] indicies =
		{
			//top triangle
			0, 1, 2,
			//bottom triangle
			2, 3, 0
		};

		//render pipeline variables
		int vao;
		int shaderProgram;
		int vbo;
		int textureVBO;
		int ebo;
		int textureID;

		//constants
		int screenWidth;
		int screenHeight;

		public Game(int width, int height) : base(GameWindowSettings.Default, NativeWindowSettings.Default)
		{
			this.screenHeight = height;
			this.screenWidth = width;

			//center the window
			CenterWindow(new Vector2i(width, height));
		}

		protected override void OnResize(ResizeEventArgs e)
		{
			base.OnResize(e);

			GL.Viewport(0, 0, e.Width, e.Height);

			this.screenWidth = e.Width;
			this.screenHeight = e.Height;
		}

		protected override void OnLoad()
		{
			base.OnLoad();

			#region Vertex
			vao = GL.GenVertexArray();

			vbo = GL.GenBuffer();
			//binds VBO
			GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
			GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

			//bind VAO
			GL.BindVertexArray(vao);
			//sets vao slot 0
			GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
			//enables vao slot 0
			GL.EnableVertexArrayAttrib(vao, 0);

			textureVBO = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ArrayBuffer, textureVBO);
			GL.BufferData(BufferTarget.ArrayBuffer, texCoords.Length * sizeof(float), texCoords, BufferUsageHint.StaticDraw);
			//sets texture vao slot 0
			GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, 0);
			//enables texture vao slot 0
			GL.EnableVertexArrayAttrib(vao, 1);

			//unbinds VBO
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
			GL.BindVertexArray(0);

			//creates a gen buffer
			ebo = GL.GenBuffer();
			//binds gen buffer to ebo
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
			//sets ebo's data
			GL.BufferData(BufferTarget.ElementArrayBuffer, indicies.Length * sizeof(uint), indicies, BufferUsageHint.StaticDraw);
			//unbinds ebo
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
			#endregion

			#region Shaders
			//creates the shader program
			shaderProgram = GL.CreateProgram();

			//loads the vertex shader
			int vertexShader = GL.CreateShader(ShaderType.VertexShader);
			GL.ShaderSource(vertexShader, LoadShaderSource("Default.vert"));
			GL.CompileShader(vertexShader);
			

			//loads gragment shader
			int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
			GL.ShaderSource(fragmentShader, LoadShaderSource("Default.frag"));
			GL.CompileShader(fragmentShader);

			//attaches the shaders
			GL.AttachShader(shaderProgram, vertexShader);
			GL.AttachShader(shaderProgram, fragmentShader);

			//links the shaders to the program
			GL.LinkProgram(shaderProgram);

			//deletes the shaders
			GL.DeleteShader(vertexShader);
			GL.DeleteShader(fragmentShader);
			#endregion

			#region Textures

			textureID = GL.GenTexture();

			//activate texture
			GL.ActiveTexture(TextureUnit.Texture0);
			GL.BindTexture(TextureTarget.Texture2D, textureID);

			//texture parameters
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

			//load image
			StbImage.stbi_set_flip_vertically_on_load(1);
			ImageResult dirtTexture = ImageResult.FromStream(File.OpenRead("../../../Textures/thing.PNG"), ColorComponents.RedGreenBlueAlpha);

			GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, dirtTexture.Width, 
					dirtTexture.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, dirtTexture.Data);

			//unbind texture
			GL.BindTexture(TextureTarget.Texture2D, 0);

			#endregion
		}

		protected override void OnUnload()
		{
			base.OnUnload();

			//unloads memory
			GL.DeleteVertexArray(vao);
			GL.DeleteVertexArray(vbo);
			GL.DeleteVertexArray(ebo);
			GL.DeleteTexture(textureID);
			GL.DeleteProgram(shaderProgram);
		}

		protected override void OnRenderFrame(FrameEventArgs args)
		{
			//sets background colour
			GL.ClearColor(0.6f, 0.3f, 1f, 1f);
			GL.Clear(ClearBufferMask.ColorBufferBit);

			//draws triangle
			GL.UseProgram(shaderProgram);
			GL.BindTexture(TextureTarget.Texture2D, textureID);
			GL.BindVertexArray(vao);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
			GL.DrawElements(PrimitiveType.Triangles, indicies.Length, DrawElementsType.UnsignedInt, 0);
			//GL.DrawArrays(PrimitiveType.Triangles, 0, 3); // draw the triangle | args = Primitive type, first vertex, last vertex

			Context.SwapBuffers();

			base.OnRenderFrame(args);
		}

		protected override void OnUpdateFrame(FrameEventArgs args)
		{
			base.OnUpdateFrame(args);
		}

		public static string LoadShaderSource(string filePath)
		{
			string shaderSource = "";

			try
			{
				using (StreamReader reader = new StreamReader("../../../Shaders/" + filePath))
				{
					shaderSource = reader.ReadToEnd();
				}
			}
			catch(Exception e)
			{
				Console.WriteLine("Failed to load shader source file: " + e.Message);
			}

			return shaderSource;
		}
	}
}
