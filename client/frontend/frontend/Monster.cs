using System;

namespace frontend
{
    public class Monster
    {
        public int posX { get; private set; }
        public int posY { get; private set; }
        public int Health { get; set; } = 70;
        private Random random = new();

        public Monster(int mapSize, int playerX, int playerY)
        {
            do
            {
                posX = random.Next(0, mapSize);
                posY = random.Next(0, mapSize);
            }
            while (posX == playerX && posY == playerY); 
        }

        public int Attack()
        {
            return random.Next(12, 17);
        }
    }
}
