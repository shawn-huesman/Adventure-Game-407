﻿namespace Adventure_Game_407
{
    // HealthPotion class
    public class HealthPotion : Item
    {
        private int RestoreAmount { get; set; }

        //Default constructor for health potion
        public HealthPotion()
        {
            // Randomly create restore amount from 5 to 50
            var rollForRestoreAmount = StaticRandom.Instance.Next(46);
            RestoreAmount = rollForRestoreAmount + 5;
            Name = "Health Potions (" + RestoreAmount + " HP)";
        }
        
        public override void Use()
        {
            Owner.RestoreHealth(RestoreAmount);
            RemoveItemFromInventory();
        }
    }
}