using System;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;

/*
 Cool ass stuff people could implement:
 > jumping
 > attacking
 > randomly moving monsters
 > smarter moving monsters
*/

public class Screen {
    private GameObject[,] grid;
    public int NumRows {
        get;
        private set;
    }
    public int NumCols {
        get;
        private set;
    }

    public Screen(int numRows, int numCols){
        NumRows = numRows;
        NumCols = numCols;
        this.grid = new GameObject[NumRows, NumCols];
    }


    public GameObject this[int row, int col]
    {
        get { 
            UseRowAndCol(row, col);
            return grid[row, col];
        }
        set {
            UseRowAndCol(row, col);
            grid[row, col] = value;
        }
    }

    protected Boolean CheckRow(int row){
        return row >= 0 && row < NumRows;
    }

    protected Boolean CheckCol(int col){
        return col >= 0 && col < NumCols;
    }

    internal Boolean CheckRowAndCol(int row, int col){
        // TODO: Check for obstacles
        return CheckRow(row) && CheckCol(col);
    }

    protected void UseRowAndCol(int row, int col){
        if (!CheckRow(row)){
            throw new ArgumentOutOfRangeException("row", $"{row} is out of range");
        }
        if (!CheckCol(col)){
            throw new ArgumentOutOfRangeException("col", $"{col} is out of range");
        }
    }

    public override String ToString() {
        // create walls if needed
        StringBuilder result = new StringBuilder();
        result.Append("+");
        result.Append(String.Concat(Enumerable.Repeat("-", NumCols)));
        result.Append("+\n");
        for (int r=0; r < NumRows; r++){
            result.Append('|');
            for (int c=0; c < NumCols; c++){
                GameObject gameObject = this[r, c];
                if (gameObject == null){
                    result.Append(' ');
                } else {
                    result.Append(gameObject.ToToken());
                }
            }
            //Console.WriteLine($"newline for {r}");
            result.Append("|\n");
        }
        result.Append('+');
        result.Append(String.Concat(Enumerable.Repeat("-", NumRows)));
        result.Append('+');
        return result.ToString();
    }
}

public abstract class GameObject {
    
    public int Row {
        get;
        protected set;
    }
    public int Col {
        get;
        protected set;
    }

    public Screen Screen {
        get;
        protected set;
    }

    public GameObject(int row, int col, Screen screen){
        Row = row;
        Col = col;
        Screen = screen;
        Screen[row, col] = this;
    }

    public override String ToString() {
        return this.ToToken();
    }
    
    public Boolean IsInBounds(int row, int col) {
        // delegate to the grid, which knows about number of rows and number of cols
        return Screen.CheckRowAndCol(row, col);
    }

    public Boolean IsOtherObject(int row, int col){
        return Screen[row, col] != null;
    }
    public string Move(int deltaRow, int deltaCol) {
        // TODO: only moveable things can move
        int newRow = deltaRow + Row;
        int newCol = deltaCol + Col;
        if (!IsInBounds(newRow, newCol)) {
            return "";
            //throw new ArgumentOutOfRangeException("row,col",
            //    $"new location at row ${newRow}, col ${newCol} is out of bounds");
        }
        if (IsOtherObject(newRow, newCol)){
            GameObject other = Screen[newRow, newCol];
            // TODO: How to handle other objects?
            // walls just stop you
            // objects can be picked up
            // people can be interactd with
            // also, when you move, some things may also move
            // maybe i,j,k,l can attack in different directions?
            // can have a "shout" command, so some objects require shouting
            return "TODO: Handle interaction";
        }
        int originalRow = Row;
        int originalCol = Col;
        // now change the location of the object, if the move was legal
        Row = newRow;
        Col = newCol;
        Screen[originalRow, originalCol] = null;
        Screen[Row, Col] = this;
        return "";
    }

    public abstract string ToToken();

    // TODO: Handle how player and other objects interact
    // FIXME: add an Update() method to Screen to move the other things
    //public abstract string Interact(GameObject other);
}

class Player : GameObject {

    public Player(int row, int col, Screen screen, string name) : base(row, col, screen) {
        Name = name;
    }
    public string Name {
        get;
        protected set;
    }

    public override String ToToken() {
        // return Name[0].ToString();
        return "@";
    }

    // public override string Interact(GameObject other) {
    //     throw new Exception("PLAYER SHOULD NOT BE THE CALLER OF THE METHOD");
    // }
}

class Wall : GameObject {
    public Wall(int row, int col, Screen screen) : base(row, col, screen) {}

    public override String ToToken() {
        return "=";
    }
}

class Treasure : GameObject {
    
    public Treasure(int row, int col, Screen screen) : base(row, col, screen) {}

    public override string ToToken() {
        return "T";
    }
}

public class Game {
    private static Boolean Eq(char c1, char c2){
        return c1.ToString().Equals(c2.ToString(), StringComparison.OrdinalIgnoreCase);
    }

    private static string Menu() {
        return "WASD to move\nEnter command: ";
    }

    private static void WriteLine(string text, ConsoleColor color) {
        Console.ForegroundColor = color;
        Console.WriteLine(text);
        Console.ResetColor();
    }

    private static void PrintScreen(Screen screen, string message) {
        Console.Clear();
        Console.WriteLine(screen);
        Console.WriteLine($"\n{message}");
        Console.WriteLine($"\n{Menu()}");
    }

    public static void Main(string[] args){
        //Console.BackgroundColor = ConsoleColor.Blue;
        Console.ForegroundColor = ConsoleColor.Green;

        Screen screen = new Screen(10, 10);
        // add a couple of walls
        for (int i=0; i < 3; i++){
            new Wall(1, 2 + i, screen);
        }
        // add a player
        Player player = new Player(0, 0, screen, "Zelda");
        // add a treasure
        Treasure treasure = new Treasure(6, 2, screen);
        
        // initially print the game board
        PrintScreen(screen, "Welcome!");

        while (true) {
            char input = Console.ReadKey(true).KeyChar;

            String message = $"{(int)input}";

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
            } else if (Eq(input, 'i')) {
                // TODO: handle inventory
                message = "You have nothing";
            } else if (Eq(input, 'o')) {
                // attack up
            } else if (Eq(input, 'l')) {
                // attack down
            } else if (Eq(input, 'k')) {
                // attack left
            } else if (Eq(input, ';')) {
                // attack right
            } else {
                message = $"Unknown command: {input}";
            }
            PrintScreen(screen, message);
        }
    }
}