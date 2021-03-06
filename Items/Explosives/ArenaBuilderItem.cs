using Terraria.ModLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.Graphics.Shaders;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.Localization;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Microsoft.Xna.Framework.Input;
using Terraria.UI;
using static Terraria.ModLoader.ModContent;

namespace ExtraExplosives.Items.Explosives
{
    public class ArenaBuilderItem : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Arena Builder");
            Tooltip.SetDefault("Explodes into existents a 240 X 120 arena equiped with only the finest of platforms and lights.");
        }

        public override void SetDefaults()
        {

            item.damage = 0;     //The damage stat for the Weapon.                
            item.width = 20;    //sprite width
            item.height = 20;   //sprite height
            item.maxStack = 1;   //This defines the items max stack
            item.consumable = true;  //Tells the game that this should be used up once fired
            item.useStyle = 1;   //The way your item will be used, 1 is the regular sword swing for example
            item.rare = 10;     //The color the title of your item when hovering over it ingame
            item.UseSound = SoundID.Item1; //The sound played when using this item
            item.useAnimation = 20;  //How long the item is used for.
            //item.useTime = 20;     //How fast the item is used.
            item.value = Item.buyPrice(5, 0, 0, 50);   //How much the item is worth, in copper coins, when you sell it to a merchant. It costs 1/5th of this to buy it back from them. An easy way to remember the value is platinum, gold, silver, copper or PPGGSSCC (so this item price is 3 silver)
            item.noUseGraphic = true;
            item.noMelee = true;      //Setting to True allows the weapon sprite to stop doing damage, so only the projectile does the damge
            item.shoot = mod.ProjectileType("ArenaBuilderProjectile"); //This defines what type of projectile this item will shoot
            item.shootSpeed = 5f; //This defines the projectile speed when shot
            //item.createTile = mod.TileType("ExplosiveTile");

        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<GiganticExplosiveItem>(), 1);
            recipe.AddIngredient(mod.ItemType("TheLevelerItem"), 1);
            recipe.AddIngredient(mod.ItemType("HouseBombItem"), 1);
            recipe.AddIngredient(mod.ItemType("DeliquidifierItem"), 1);
            recipe.AddIngredient(mod.ItemType("TorchBombItem"), 1);
            recipe.AddIngredient(ItemID.Wood, 100);
            recipe.AddIngredient(ItemID.CrystalBlock, 100);
            recipe.anyWood = true;
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();

        }
    }

}