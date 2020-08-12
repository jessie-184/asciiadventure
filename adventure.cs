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
namespace asciiadventure {
    public class Driver {
        public static void Main(string[] args){
            Game game = new Game();
            game.Run();
        }
    }

    public class Game {
        private static readonly Boolean DEBUG = true;
        private static Boolean Eq(char c1, char c2){
            return c1.ToString().Equals(c2.ToString(), StringComparison.OrdinalIgnoreCase);
        }

        private static string Menu() {
            return "WASD to move\nEnter command: ";
        }

        private static void Debug(string message){
            if (DEBUG) {
                Console.WriteLine(message);
            }
        }

        private static void PrintScreen(Screen screen, string message, string menu) {
            Console.Clear();
            Console.WriteLine(screen);
            Console.WriteLine($"\n{message}");
            Console.WriteLine($"\n{menu}");
        }

        public void Initialize() {

        }

        public void Run() {
            Console.ForegroundColor = ConsoleColor.Green;

            Screen screen = new Screen(10, 10);
            // add a couple of walls
            for (int i=0; i < 3; i++){
                new Wall(1, 2 + i, screen);
            }
            for (int i=0; i < 4; i++){
                new Wall(3 + i, 4, screen);
            }
            
            // add a player
            Player player = new Player(0, 0, screen, "Zelda");
            
            // add a treasure
            Treasure treasure = new Treasure(6, 2, screen);
            
            // initially print the game board
            PrintScreen(screen, "Welcome!", Menu());

            while (true) {
                char input = Console.ReadKey(true).KeyChar;

                String message = "";

                if (Eq(input, 'q')) {
                    break;
                } else if (Eq(input, 'w')) {
                    player.Move(-1, 0);
                } else if (Eq(input, 's')) {
                    player.Move(1, 0);
                } else if (Eq(input, 'a')) {
                    player.Move(0, -1);
                } else if (Eq(input, 'd')) {
                    player.Move(0, 1);
                } else if (Eq(input, 'v')) {
                    // TODO: handle inventory
                    message = "You have nothing";
                } else if (Eq(input, 'i')) {
                    Debug("action up");
                } else if (Eq(input, 'k')) {
                    Debug("action down");
                } else if (Eq(input, 'j')) {
                    Debug("action left");
                } else if (Eq(input, 'l')) {
                    Debug("action right");
                } else {
                    message = $"Unknown command: {input}";
                }
                PrintScreen(screen, message, Menu());
            }
        }
    }
}