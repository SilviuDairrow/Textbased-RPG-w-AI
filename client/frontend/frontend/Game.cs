using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace frontend
{
    public class Game
    {
        private int mapSize; 
        private Player player;
        public event Action<string> MinimapUpdated;
        public List<Monster> Monsters { get; private set; }


        public Game(int mapSize, Player player)
        {
            this.mapSize = mapSize;
            this.player = player;
            Monsters = new List<Monster>();


            int numberOfMonsters = 5; 
            for (int i = 0; i < numberOfMonsters; i++)
            {
                Monsters.Add(new Monster(mapSize, player.posX, player.posY));
            }

            OnPlayerMove();
        }

        
        public string GenerateMinimapContent()
        {
            StringBuilder minimapContent = new StringBuilder();

            for (int row = 0; row < mapSize; row++)
            {
                for (int col = 0; col < mapSize; col++)
                {
                    minimapContent.Append(GetMinimapCharacter(row, col));
                }
                minimapContent.AppendLine();
            }

            return minimapContent.ToString();
        }

        public char DetectTerrain(int targetX, int targetY)
        {
            if (targetX < 0 || targetX >= mapSize || targetY < 0 || targetY >= mapSize)
            {
                return 'O'; // O = out of bounds
            }
            return GetMinimapCharacter(targetY, targetX);
        }

        private char GetMinimapCharacter(int row, int col)
        {
            if (row == player.posY && col == player.posX)
            {
                return 'P'; 
            }
            foreach (var monster in Monsters)
            {
                if(monster.posX == row && monster.posY == col) 
                    return 'G';
            }
            if (row % 2 == 0 && col % 2 == 0)
            {
                return 'M'; // M = mountain
            }
            else if (row % 3 == 0)
            {
                return 'W'; // W = water
            }
            else
            {
                return '.'; // . = empty space
            }

        }

        public void OnPlayerMove()
        {
            MinimapUpdated?.Invoke(GenerateMinimapContent());
        }

        public Monster CheckForMonster(int x, int y)
        {
            foreach (var monster in Monsters)
            {
                if (monster.posX == x && monster.posY == y)
                {
                    return monster;
                }
            }
            return null;
        }
    }
}
