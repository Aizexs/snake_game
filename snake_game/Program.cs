using System.Diagnostics;
using static System.Console;
namespace snake_game
{
    class Program
    {
        private const int MapWidth = 120;

        private const int MapHeight = 30;

        private const ConsoleColor BorderColor = ConsoleColor.Gray;

        private const ConsoleColor HeadColor = ConsoleColor.DarkRed;

        private const ConsoleColor BodyColor = ConsoleColor.Yellow;

        private const ConsoleColor FoodColor = ConsoleColor.Green;

        private const int FrameMS = 200; 

        private static readonly Random random = new Random();


        static void Main()
        {
            SetWindowSize(MapWidth, MapHeight);

            SetBufferSize(MapWidth, MapHeight);

            CursorVisible = false;

            while (true)
            {
                StartGame();

                Thread.Sleep(1000);

                ReadKey();
            }                                   
        }

        static void StartGame()
        {

            Clear();

            DrawBorder();

            var snake = new Snake(60, 15, HeadColor, BodyColor);

            Pixel food = GenFood(snake);

            food.Draw();

            Direction currentMovement = Direction.Right;

            int score = 0;

            int lagMS = 0;

            Stopwatch sw = new Stopwatch();

            while (true)
            {
                sw.Restart();

                Direction oldMovement = currentMovement;

                while (sw.ElapsedMilliseconds <= FrameMS - lagMS)
                {
                    if (currentMovement == oldMovement)
                    {
                        currentMovement = ReadMovement(currentMovement);
                    }


                }

                sw.Restart();

                if (snake.Head.X == food.X && snake.Head.Y == food.Y)
                {
                    snake.Move(currentMovement, true);

                    food = GenFood(snake);

                    food.Draw();

                    score++;

                    Task.Run(() => Beep(1500, 300));
                }

                else snake.Move(currentMovement);



                if (snake.Head.X == MapWidth - 1
                    || snake.Head.X == 0
                    || snake.Head.Y == MapHeight - 1
                    || snake.Head.Y == 0
                    || snake.Body.Any(b => b.X == snake.Head.X && b.Y == snake.Head.Y)) break;

                lagMS = (int)sw.ElapsedMilliseconds;
            }

            snake.Clear();
            food.Clear();
            WriteLine($"GAME OVER \t result: {score}");

            Task.Run(() => Beep(300, 1500));
        }

        static Direction ReadMovement(Direction currentDirection)
        {
            if (!KeyAvailable) return currentDirection;

            ConsoleKey key = ReadKey(true).Key;

            currentDirection = key switch
            {
                ConsoleKey.UpArrow when currentDirection != Direction.Down => Direction.Up,
                ConsoleKey.DownArrow when currentDirection != Direction.Up => Direction.Down,
                ConsoleKey.RightArrow when currentDirection != Direction.Left => Direction.Right,
                ConsoleKey.LeftArrow when currentDirection != Direction.Right => Direction.Left,
                _ => currentDirection
            };

            return currentDirection;
        }

        static Pixel GenFood(Snake snake)
        {
            Pixel food;

            do
            {
                food = new Pixel(random.Next(1, MapWidth - 2), random.Next(1, MapHeight - 2), FoodColor);
            } while (snake.Head.X == food.X && snake.Head.Y == food.Y || snake.Body.Any(b => b.X == food.X && b.Y == food.Y));

            return food;
        }

        static void DrawBorder()
        {
            for (int i = 0; i < MapWidth; i++)
            {
                new Pixel(i, 0, BorderColor).Draw();
                new Pixel(i, MapHeight - 1, BorderColor).Draw();
            }

            for (int i = 0; i < MapHeight; i++)
            {
                new Pixel(0, i, BorderColor).Draw();
                new Pixel(MapWidth - 1, i, BorderColor).Draw();
            }
        }
    }
}
