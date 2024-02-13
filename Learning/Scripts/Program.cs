namespace MinecraftClone.Scripts
{
    class Program
    {
        static void Main(string[] args)
        {
            using (Game game = new Game(1280, 720))
            {
                //runs the game
                game.Run();
            }
        }
    }
}