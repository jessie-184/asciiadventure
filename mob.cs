using System;

namespace asciiadventure {
    public class Mob : MovingGameObject {
        public Mob(int row, int col, Screen screen) : base(row, col, "#", screen)
        {
            IsAlive = true;
        }

        public Boolean IsAlive
        {
            get;
            protected internal set;
        }
    }
}