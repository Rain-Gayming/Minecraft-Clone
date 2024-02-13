using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftClone.Player
{
	internal class Camera
	{
		//camera settings
		public float speed = 8f;
		public float sensitivity = 135f;
		public float fieldOfView = 45;

		//positions
		public Vector3 position;
		public Vector3 up = Vector3.UnitY;	
		public Vector3 front = -Vector3.UnitZ;
		public Vector3 right = Vector3.UnitX;

		//rotations
		public float pitch;
		public float yaw = -90f;

		bool firstMove = true;
		public Vector2 lastPos;
		
		//screen settings
		public float width;
		public float height;

		public Camera(float width, float height, Vector3 position)
		{
			this.width = width;
			this.height = height;
			this.position = position;
		}

		public void Update(KeyboardState input, MouseState mouse, FrameEventArgs e)
		{
			InputController(input, mouse, e);
		}

		public void InputController(KeyboardState input, MouseState mouse, FrameEventArgs e)
		{
			if (input.IsKeyDown(Keys.W))
			{
				position += front * speed * (float)e.Time;
			}

			if (input.IsKeyDown(Keys.A))
			{
				position -= right * speed * (float)e.Time;
			}

			if (input.IsKeyDown(Keys.S))
			{
				position -= front * speed * (float)e.Time;
			}

			if (input.IsKeyDown(Keys.D))
			{
				position += right * speed * (float)e.Time;
			}

			if (input.IsKeyDown(Keys.Space))
			{
				position.Y += speed * (float)e.Time;
			}

			if (input.IsKeyDown(Keys.LeftControl))
			{
				position.Y -= speed * (float)e.Time;
			}

			if (firstMove)
			{
				lastPos = new Vector2(mouse.X, mouse.Y);
				firstMove = false;
			}
			else
			{
				var deltaX = mouse.X - lastPos.X;
				var deltaY = mouse.Y - lastPos.Y;
				lastPos = new Vector2(mouse.X, mouse.Y);

				yaw += deltaX * sensitivity * (float)e.Time;
				pitch -= deltaY * sensitivity * (float)e.Time;
			}

			UpdateVectors();
		}

		public void UpdateVectors()
		{
			if (pitch > 80.0f)
			{
				pitch = 80f;
			}
			if (pitch < -80.0f)
			{
				pitch = -80f;
			}
			front.X = MathF.Cos(MathHelper.DegreesToRadians(pitch)) * MathF.Cos(MathHelper.DegreesToRadians(yaw));
			front.Y = MathF.Sin(MathHelper.DegreesToRadians(pitch));
			front.Z = MathF.Cos(MathHelper.DegreesToRadians(pitch)) * MathF.Sin(MathHelper.DegreesToRadians(yaw));

			front = Vector3.Normalize(front);
			right = Vector3.Normalize(Vector3.Cross(front, Vector3.UnitY));
			up = Vector3.Normalize(Vector3.Cross(right, front));
		}

		public Matrix4 GetViewMatrix()
		{
			return Matrix4.LookAt(position, position + front, up);
		}
		public Matrix4 GetProjectionMatrix()
		{
			return Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(fieldOfView), width/height, 0.01f, 100f);
		}
	}
}
