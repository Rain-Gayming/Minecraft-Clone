using OpenTK.Windowing.Common;

namespace MinecraftClone.Scripts
{
    class Program
    {
        static void Main(string[] args)
        {
            using (Game game = new Game(1280, 720, "minecraft 2"))
            {
                double maxFps = 60;
                //runs the game
                //game.VSync = VSyncMode.On;
                game.Run();
            }
        }
    }
}