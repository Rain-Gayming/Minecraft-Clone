
namespace MinecraftClone
{
	class Program
	{
		static void Main(string[] args)
		{
			using (Game game = new Game(500, 500))
			{
				//runs the game
				game.Run();
			}
		}
	}
}