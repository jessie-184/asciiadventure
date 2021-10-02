using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;

/*
 Cool ass stuff people could implement:
 > jumping
 > attacking
 > randomly moving monsters
 > smarter moving monsters
*/
namespace asciiadventure
{
    public class Game
    {
        List<Mob> mobs = new List<Mob>();
        private Random random = new Random();
        private static Boolean Eq(char c1, char c2)
        {
            return c1.ToString().Equals(c2.ToString(), StringComparison.OrdinalIgnoreCase);
        }

        private static string Menu()
        {
            return "WASD to move\nIJKL to attack/interact\nTFGH to jump\nEnter command: ";
        }

        private static void PrintScreen(Screen screen, string message, string menu)
        {
            Console.Clear();
            Console.WriteLine(screen);
            Console.WriteLine($"\n{message}");
            Console.WriteLine($"\n{menu}");
        }
        public void Run()
        {
            Console.ForegroundColor = ConsoleColor.Green;

            Screen screen = new Screen(10, 10);
            // add a couple of walls
            for (int i = 0; i < 3; i++)
            {
                new Wall(1, 2 + i, screen);
            }
            for (int i = 1; i < 4; i += 2)
            {
                new Wall(3 + i, 4, screen);
            }
            List<MovingWall> movingWalls = new List<MovingWall>();
            for (int i = 0; i < 4; i += 2)
            {
                movingWalls.Add(new MovingWall(3 + i, 4, screen));
            }

            // add a player
            Player player = new Player(0, 0, screen, "Zelda");

            // add a treasure
            Treasure treasure = new Treasure(6, 2, screen);

            // add some health
            List<Health> healths = new List<Health>();
            healths.Add(new Health(7, 5, screen));
            healths.Add(new Health(2, 9, screen));

            // add some mobs

            mobs.Add(new Mob(9, 9, screen));
            mobs.Add(new Mob(9, 1, screen));

            // initially print the game board
            PrintScreen(screen, "Welcome!", Menu());

            Boolean gameOver = false;

            while (!gameOver)
            {
                char input = Console.ReadKey(true).KeyChar;

                String message = "";

                if (Eq(input, 'q'))
                {
                    break;
                }
                else if (Eq(input, 'w'))
                {
                    player.Move(-1, 0);
                }
                else if (Eq(input, 's'))
                {
                    player.Move(1, 0);
                }
                else if (Eq(input, 'a'))
                {
                    player.Move(0, -1);
                }
                else if (Eq(input, 'd'))
                {
                    player.Move(0, 1);
                }
                else if (Eq(input, 't'))
                {
                    message += player.Jump(-2, 0) + "\n";
                }
                else if (Eq(input, 'g'))
                {
                    message += player.Jump(2, 0) + "\n";
                }
                else if (Eq(input, 'f'))
                {
                    message += player.Jump(0, -2) + "\n";
                }
                else if (Eq(input, 'h'))
                {
                    message += player.Jump(0, 2) + "\n";
                }
                else if (Eq(input, 'i'))
                {
                    message += player.Action(-1, 0) + "\n";
                }
                else if (Eq(input, 'k'))
                {
                    message += player.Action(1, 0) + "\n";
                }
                else if (Eq(input, 'j'))
                {
                    message += player.Action(0, -1) + "\n";
                }
                else if (Eq(input, 'l'))
                {
                    message += player.Action(0, 1) + "\n";
                }
                else if (Eq(input, 'v'))
                {
                    // TODO: handle inventory
                    message = "You have nothing\n";
                }
                else
                {
                    message = $"Unknown command: {input}";
                }
                // Move the moving walls
                foreach (MovingWall mw in movingWalls)
                {
                    mw.VerticalMove();
                }
                // OK, now move the mobs
                foreach (Mob mob in mobs)
                {
                    if (!mob.IsAlive)
                    {
                        continue;
                    }
                    // TODO: Make mobs smarter, so they jump on the player, if it's possible to do so
                    List<Tuple<int, int>> moves = screen.GetLegalMoves(mob.Row, mob.Col);
                    if (moves.Count == 0)
                    {
                        continue;
                    }
                    // mobs move randomly
                    var (deltaRow, deltaCol) = moves[random.Next(moves.Count)];

                    if (screen[mob.Row + deltaRow, mob.Col + deltaCol] is Player)
                    {
                        Player p = (Player)screen[mob.Row + deltaRow, mob.Col + deltaCol];
                        // check the health of player
                        // if the health is more than 1 the player can still be alive
                        // if the health is only  the mob can kill the player
                        if (p.Health > 1)
                        {
                            p.Health -= 1;
                            message += $"A mob is trying to attack you! Your health now is {p.Health}\n";
                        }
                        else
                        {
                            // the mob got the player!
                            mob.Token = "*";
                            message += "A MOB GOT YOU! GAME OVER\n";
                            gameOver = true;
                        }
                    }
                    mob.Move(deltaRow, deltaCol);
                }

                PrintScreen(screen, message, Menu());
            }
        }

        public static void Main(string[] args)
        {
            Game game = new Game();
            game.Run();
        }
    }
}