using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace frontend
{
    public class Player
    {

        public int posX { get; set; }
        public int posY { get; set; }
        public int MaxMoves { get; private set; } = 5;
        public int CntMountain { get; private set; }
        public int CntWater { get; private set; }
        public string Class { get; private set; } = "N ai setat inca clasa?";
        public int killCount { get; set; } = 0;
        public int itemLevel { get; set; } = 1;

        // Stats
        public int RemainingMoves { get; set; }
        public int Health { get; private set; } = 100;
        public int Mana { get; private set; } = 70;
        public int Att { get; private set; } = 23;
        public int spellAtt { get; private set; } = 23;


        public Player() { }

        public Player(int mapSize)
        {
            posX = mapSize / 2;
            posY = mapSize / 2;
            RemainingMoves = MaxMoves;
        }

        public void UpdateTerrainCounter(char terrainType)
        {
            if (terrainType == 'M') CntMountain++;
            if (terrainType == 'W') CntWater++;
        }

        public int Move(int deltaX, int deltaY, int mapSize, Game game)
        {
            int Attacked = 0;
            if (!CanMove()) return 0;

            int newX = Math.Max(0, Math.Min(posX + deltaX, mapSize - 1));
            int newY = Math.Max(0, Math.Min(posY + deltaY, mapSize - 1));

            Monster nearbyMonster = game.CheckForMonster(newX, newY);
            if (nearbyMonster != null)
            {
                int damage = nearbyMonster.Attack();
                TakeDamage(damage);
                Attacked = 1; 
            }

            posX = newX;
            posY = newY;
            --RemainingMoves;
            return Attacked;
        }

        public void TakeDamage(int damage)
        {
            Health = Math.Max(0, Health - damage);
            ShowDeathPopup();
        }

        public void UseMana(int amount)
        {
            Mana = Math.Max(0, Mana - amount);
        }
        public bool CanMove()
        {
            return RemainingMoves > 0;
        }

        public void SetHealth(int health)
        {
            Health = health;
        }

        public void SetMana(int mana)
        {
            Mana = mana;
        }

        public void SetClass(string clasa)
        {
            Class = clasa;
        }

        public void SetAtt(int att)
        {
            Att = att;
        }
        public void SetSpellAtt(int spellatt)
        {
            spellAtt = spellatt;
        }
        public void SetPosX(int posx)
        {
            posX = posx;
        }
        public void SetPosY(int posy)
        {
            posY = posy;
        }

        public void Sleep()
        {
            RemainingMoves = MaxMoves;
        }

        private async void ShowDeathPopup()
        {
            ContentDialog deathDialog = new ContentDialog
            {
                Title = "Game Over",
                Content = "You died! The goblins defeated you! Try a new game or load a previous save.",
                CloseButtonText = "Ok"
            };

            var result = await deathDialog.ShowAsync();
            MaxMoves = 0;
            Att = 0;
        }

        public void IncreaseKillCount()
        {
            killCount++;

            if(killCount > 2)
            {
                itemLevel = 2;
            }
        }
    }
}
