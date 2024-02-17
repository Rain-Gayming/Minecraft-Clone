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


namespace MinecraftClone.Scripts
{
    internal class Game : GameWindow
    {
        Chunk chunk;
        public List<Chunk> chunks;
        ShaderProgram program;

        //tranformation variables
        float yRot = 0f;

        //camera
        public Camera camera;

        //width and height of screen
        int width, height;

        //game variables
        public bool isPaused;
        public int renderDistance = 16;

        public World.WorldManager wm;

        //constructor that sets the width, height, and calls the base constructor (GameWindow's Constructor) with default args
        public Game(int width, int height) : base(GameWindowSettings.Default, NativeWindowSettings.Default)
        {
            this.width = width;
            this.height = height;

            //center window
            CenterWindow(new Vector2i(width, height));
        }
        //called whenever window is resized
        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, e.Width, e.Height);
            width = e.Width;
            height = e.Height;
        }

        //called once when game is started
        protected override void OnLoad()
        {
            base.OnLoad();

            //chunk = new Chunk(Vector3.Zero);

            chunks = new List<Chunk>();

            //moved to WorldManager.cs
            
            /*Vector3 startPos = Vector3.Zero;
			startPos.X -= renderDistance * 8;
			startPos.Z -= renderDistance * 8;

            for (int x = 0; x < renderDistance; x++)
            {
                Vector3 vPos = new Vector3(startPos.X, startPos.Y, startPos.Z);

                vPos.X = startPos.X + (16 * x);
                chunks.Add(new Chunk(vPos));
                for (int y = 0; y < renderDistance; y++)
                {
                    Vector3 cPos = new Vector3(startPos.X, startPos.Y, startPos.Z);
                    cPos.X = vPos.X;
                    cPos.Z = startPos.X + (y * 16);
                    chunks.Add(new Chunk(cPos));
                }
            }*/	

            program = new ShaderProgram("Default.vert", "Default.frag");

			Render();

            wm = new WorldManager(renderDistance);
            wm.GenerateWorld();

            camera = new Camera(width, height, new Vector3(0, 25, 0));
            CursorState = CursorState.Grabbed;
        }

        public void Render()
        {
            program = new ShaderProgram("Default.vert", "Default.frag");
            
			GL.Enable(EnableCap.DepthTest);
            GL.FrontFace(FrontFaceDirection.Cw);
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);
        }

        //called once when game is closed
        protected override void OnUnload()
        {
            base.OnUnload();

            for (int i = 0; i < chunks.Count; i++)
            {
                chunks[i].Delete();
            }

            //chunk.Delete();
            program.Delete();
        }
        //called every frame. All rendering happens here
        protected override void OnRenderFrame(FrameEventArgs args)
        {
            //Set the color to fill the screen with
            GL.ClearColor(0.3f, 0.3f, 1f, 1f);
            //Fill the screen with the color
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);


            //transformation marices
            Matrix4 model = Matrix4.Identity;
            Matrix4 view = camera.GetViewMatrix();
            Matrix4 projection = camera.GetProjectionMatrix();

            //sets the variables in the shaders
            int modelLocation = GL.GetUniformLocation(program.ID, "model");
            int viewLocation = GL.GetUniformLocation(program.ID, "view");
            int projectionLocation = GL.GetUniformLocation(program.ID, "projection");

            GL.UniformMatrix4(modelLocation, true, ref model);
            GL.UniformMatrix4(viewLocation, true, ref view);
            GL.UniformMatrix4(projectionLocation, true, ref projection);

            for (int i = 0; i < chunks.Count; i++)
            {
                chunks[i].Render(program);
            }

            //chunk.Render(program);

            //GL.DrawElements(PrimitiveType.Triangles, indices.Count, DrawElementsType.UnsignedInt, 0);

            //swap the buffers
            Context.SwapBuffers();

            base.OnRenderFrame(args);
        }
        //called every frame. All updating happens here
        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            MouseState mouse = MouseState;
            KeyboardState input = KeyboardState;

            base.OnUpdateFrame(args);
            camera.Update(input, mouse, args);
            InputController(input, mouse, args);
        }


        public void InputController(KeyboardState input, MouseState mouse, FrameEventArgs e)
		{
			if (input.IsKeyDown(Keys.Escape))
			{
				TogglePause();
			}
			if (input.IsKeyDown(Keys.R))
			{
                chunk = new Chunk(Vector3.Zero);
			}
		}

        public void TogglePause()
        {
            isPaused = !isPaused;

            if (isPaused)
            {
                CursorState = CursorState.Normal;
            }
            else
            {
                CursorState = CursorState.Grabbed;
            }
        }

        //Function to load a text file and return its contents as a string
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
            catch (Exception e)
            {
                Console.WriteLine("Failed to load shader source file: " + e.Message);
            }

            return shaderSource;
        }
    }
}
