using System;

namespace asciiadventure
{
    class Player : MovingGameObject
    {
        public Player(int row, int col, Screen screen, string name) : base(row, col, "@", screen)
        {
            Name = name;
            Health = 1;
        }
        public string Name
        {
            get;
            protected set;
        }
        public int Health
        {
            get;
            protected internal set;
        }
        public override Boolean IsPassable()
        {
            return true;
        }



        public String Action(int deltaRow, int deltaCol)
        {
            int newRow = Row + deltaRow;
            int newCol = Col + deltaCol;
            if (!Screen.IsInBounds(newRow, newCol))
            {
                return "nope";
            }
            GameObject other = Screen[newRow, newCol];
            if (other == null)
            {
                return "negative";
            }
            // TODO: Interact with the object
            if (other is Treasure)
            {
                other.Delete();
                return "Yay, we got the treasure!";
            }
            if (other is Health)
            {
                other.Delete();
                Health += 1;
                return $"You got one more health. Now your health is {Health}.";
            }
            if (other is Mob)
            {
                Mob m = (Mob)other;
                m.IsAlive = false;
                other.Delete();
                return "Yay, you killed the mob!";
            }

            return "ouch";
        }

        public String Jump(int deltaRow, int deltaCol)
        {
            int newRow = Row + deltaRow;
            int newCol = Col + deltaCol;
            if (!Screen.IsInBounds(newRow, newCol))
            {
                return "nope";
            }
            GameObject other = Screen[newRow, newCol];
            // TODO: Interact with the object
            if (other is Treasure)
            {
                other.Delete();
                return "Yay, we got the treasure!";
            }
            if (other is Health)
            {
                other.Delete();
                Health += 1;
                return $"You got one more health. Now your health is {Health}.";
            }
            if (other is Mob)
            {
                Mob m = (Mob)other;
                m.IsAlive = false;
                other.Delete();
                return "Yay, you killed the mob!";
            }
            // Now just make the move
            int originalRow = Row;
            int originalCol = Col;
            // now change the location of the object, if the move was legal
            Row = newRow;
            Col = newCol;
            Screen[originalRow, originalCol] = null;
            Screen[Row, Col] = this;
            return "";
        }
    }
}
