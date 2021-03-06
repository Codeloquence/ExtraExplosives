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
using ExtraExplosives.NPCs;
using ExtraExplosives.Projectiles;
using ExtraExplosives.Items;
using ExtraExplosives;
using ExtraExplosives.Items.Pets;

namespace ExtraExplosives.NPCs
{
    [AutoloadHead]
    public class CaptainExplosive : ModNPC
    {
        Mod CalamityMod = ModLoader.GetMod("CalamityMod");
        Mod ThoriumMod = ModLoader.GetMod("ThoriumMod");

        public static bool CaptianIsDed = true;
        internal static bool CanBreakWalls;
        internal static bool CanBreakTiles;

        public override bool Autoload(ref string name)
        {
            name = "CaptainExplosive";
            return mod.Properties.Autoload;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Captain Explosive");
            Main.npcFrameCount[npc.type] = 25; //npc defines how many frames the npc sprite sheet has
            NPCID.Sets.ExtraFramesCount[npc.type] = 9;
            NPCID.Sets.AttackFrameCount[npc.type] = 4;
            NPCID.Sets.DangerDetectRange[npc.type] = 1000; //150 //npc defines the npc danger detect range
            NPCID.Sets.AttackType[npc.type] = 0; //npc is the attack type,  0 (throwing), 1 (shooting), or 2 (magic). 3 (melee)
            NPCID.Sets.AttackTime[npc.type] = 50; //npc defines the npc attack speed
            NPCID.Sets.AttackAverageChance[npc.type] = 5;//npc defines the npc atack chance
            NPCID.Sets.HatOffsetY[npc.type] = 4; //npc defines the party hat position

        }

        public override void SetDefaults()
        {
            //npc.name = "Custom Town NPC";   //the name displayed when hovering over the npc ingame.
            npc.townNPC = true; //npc defines if the npc is a town Npc or not
            npc.friendly = true;  //npc defines if the npc can hur you or not()
            npc.width = 40; //the npc sprite width
            npc.height = 56;  //the npc sprite height
            npc.aiStyle = 7; //npc is the npc ai style, 7 is Pasive Ai
            npc.defense = 25;  //the npc defense
            npc.lifeMax = 400;// the npc life
            npc.HitSound = SoundID.NPCHit1;  //the npc sound when is hit
            npc.DeathSound = SoundID.NPCDeath1;  //the npc sound when he dies
            npc.knockBackResist = 0.5f;  //the npc knockback resistance
            animationType = NPCID.Guide;  //npc copy the guide animation
        }

        //public override bool CanTownNPCSpawn(int numTownNPCs, int money) //Whether or not the conditions have been met for npc town NPC to be able to move into town.
        //{
        //    if (NPC.downedBoss1)  //so after the EoC is killed
        //    {
        //        return true;
        //    }
        //    return false;
        //}

        public override bool CheckActive()
        {
            Vector2 position = npc.Center;
            CaptianIsDed = false;
            Dust dust;

            if (Main.rand.NextFloat() < 0.131579f && npc.direction == -1)
            {
                Vector2 position1 = new Vector2(position.X + 3, position.Y - 20);
                // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
                dust = Terraria.Dust.NewDustPerfect(position1, 114, new Vector2(1f, -1f), 0, new Color(255, 125, 0), 0.7894737f);
                dust.noGravity = true;
                dust.noLight = false;
                //dust.fadeIn = 1.657895f;
            }
            else if (Main.rand.NextFloat() < 0.131579f && npc.direction == 1)
            {
                Vector2 position1 = new Vector2(position.X - 3, position.Y - 20);
                // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
                dust = Terraria.Dust.NewDustPerfect(position1, 114, new Vector2(-1f, -1f), 0, new Color(255, 125, 0), 0.7894737f);
                dust.noGravity = true;
                dust.noLight = false;
                //dust.fadeIn = 1.657895f;
            }
            return base.CheckActive();
        }

        public override string TownNPCName()     //Allows you to give npc town NPC any name when it spawns
        {
            switch (WorldGen.genRand.Next(1))
            {
                case 0:
                    return "???";
                default:
                    return "Unknown";
            }

        }

        public override bool CheckDead()
        {

            CaptianIsDed = true;

            Vector2 position = npc.Center;

            Main.PlaySound(SoundID.Item14, (int)position.X, (int)position.Y);
            int radius = 20;     //this is the explosion radius, the highter is the value the bigger is the explosion

            //damage part of the bomb
            ExplosionDamageProjectile.DamageRadius = (float)(radius * 1.5f);
            Projectile.NewProjectile(position.X, position.Y, 0, 0, mod.ProjectileType("ExplosionDamageProjectile"), 1000, 40, Main.myPlayer, 0.0f, 0);

            if (CanBreakTiles == true)
            {
                for (int x = -radius; x <= radius; x++)
                {
                    for (int y = -radius; y <= radius; y++)
                    {
                        int xPosition = (int)(x + position.X / 16.0f);
                        int yPosition = (int)(y + position.Y / 16.0f);

                        if (Math.Sqrt(x * x + y * y) <= radius + 0.5)   //this make so the explosion radius is a circle
                        {

                            if (Main.tile[xPosition, yPosition].type == TileID.LihzahrdBrick || Main.tile[xPosition, yPosition].type == TileID.LihzahrdAltar || Main.tile[xPosition, yPosition].type == TileID.LihzahrdFurnace || Main.tile[xPosition, yPosition].type == TileID.BlueDungeonBrick || Main.tile[xPosition, yPosition].type == TileID.GreenDungeonBrick
                                || Main.tile[xPosition, yPosition].type == TileID.PinkDungeonBrick || Main.tile[xPosition, yPosition].type == TileID.Cobalt || Main.tile[xPosition, yPosition].type == TileID.Palladium || Main.tile[xPosition, yPosition].type == TileID.Mythril || Main.tile[xPosition, yPosition].type == TileID.Orichalcum || Main.tile[xPosition, yPosition].type == TileID.Adamantite || Main.tile[xPosition, yPosition].type == TileID.Titanium ||
                                Main.tile[xPosition, yPosition].type == TileID.Chlorophyte || Main.tile[xPosition, yPosition].type == TileID.DefendersForge || Main.tile[xPosition, yPosition].type == TileID.DemonAltar)
                            {

                            }
                            else if (CalamityMod != null && (Main.tile[xPosition, yPosition].type == CalamityMod.TileType("SeaPrism") || Main.tile[xPosition, yPosition].type == CalamityMod.TileType("AerialiteOre") || Main.tile[xPosition, yPosition].type == CalamityMod.TileType("CryonicOre")
                        || Main.tile[xPosition, yPosition].type == CalamityMod.TileType("CharredOre") || Main.tile[xPosition, yPosition].type == CalamityMod.TileType("PerennialOre") || Main.tile[xPosition, yPosition].type == CalamityMod.TileType("ScoriaOre")
                        || Main.tile[xPosition, yPosition].type == CalamityMod.TileType("AstralOre") || Main.tile[xPosition, yPosition].type == CalamityMod.TileType("ExodiumOre") || Main.tile[xPosition, yPosition].type == CalamityMod.TileType("UelibloomOre")
                        || Main.tile[xPosition, yPosition].type == CalamityMod.TileType("AuricOre") || Main.tile[xPosition, yPosition].type == CalamityMod.TileType("AbyssGravel") || Main.tile[xPosition, yPosition].type == CalamityMod.TileType("Voidstone") || Main.tile[xPosition, yPosition].type == CalamityMod.TileType("PlantyMush")
                        || Main.tile[xPosition, yPosition].type == CalamityMod.TileType("Tenebris") || Main.tile[xPosition, yPosition].type == CalamityMod.TileType("ArenaBlock") || Main.tile[xPosition, yPosition].type == CalamityMod.TileType("Cinderplate") || Main.tile[xPosition, yPosition].type == CalamityMod.TileType("ExodiumClusterOre")))
                            {
                                if (Main.tile[xPosition, yPosition].type == TileID.Dirt)
                                {
                                    WorldGen.KillTile(xPosition, yPosition, false, false, false);  //this makes the explosion destroy tiles  
                                    if (CanBreakWalls) WorldGen.KillWall(xPosition, yPosition, false);
                                }
                            }
                            else if (ThoriumMod != null && (Main.tile[xPosition, yPosition].type == ThoriumMod.TileType("Aquaite") || Main.tile[xPosition, yPosition].type == ThoriumMod.TileType("LodeStone") || Main.tile[xPosition, yPosition].type == ThoriumMod.TileType("ValadiumChunk") || Main.tile[xPosition, yPosition].type == ThoriumMod.TileType("IllumiteChunk")
                                || Main.tile[xPosition, yPosition].type == ThoriumMod.TileType("PearlStone") || Main.tile[xPosition, yPosition].type == ThoriumMod.TileType("DepthChestPlatform")))
                            {
                                if (Main.tile[xPosition, yPosition].type == TileID.Dirt)
                                {
                                    WorldGen.KillTile(xPosition, yPosition, false, false, false);  //this makes the explosion destroy tiles  
                                    if (CanBreakWalls) WorldGen.KillWall(xPosition, yPosition, false);
                                }
                            }
                            else
                            {
                                WorldGen.KillTile(xPosition, yPosition, false, false, false);  //this make the explosion destroy tiles  
                                //Dust.NewDust(position, 22, 22, DustID.Smoke, 0.0f, 0.0f, 120, new Color(), 1f);  //this is the dust that will spawn after the explosion
                                if (CanBreakWalls) WorldGen.KillWall(xPosition, yPosition, false);
                            }


                            if (Main.rand.NextFloat() < ExtraExplosives.dustAmount )
                            {
                                if (Main.rand.NextFloat() < 0.2f)
                                {

                                    int Hw = 550;
                                    float scale = 10f;

                                    Dust dust;
                                    // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
                                    Vector2 vev = new Vector2(position.X - (Hw / 2), position.Y - (Hw / 2));
                                    dust = Main.dust[Terraria.Dust.NewDust(vev, Hw, Hw, 6, 0f, 0.5263162f, 0, new Color(255, 0, 0), scale)];
                                    dust.noGravity = true;
                                    dust.fadeIn = 2.486842f;


                                    // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
                                    dust = Main.dust[Terraria.Dust.NewDust(vev, Hw, Hw, 203, 0f, 0f, 0, new Color(255, 255, 255), scale)];
                                    dust.noGravity = true;
                                    dust.noLight = true;


                                    // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
                                    dust = Main.dust[Terraria.Dust.NewDust(vev, Hw, Hw, 31, 0f, 0f, 0, new Color(255, 255, 255), scale)];
                                    dust.noGravity = true;
                                    dust.noLight = true;
                                }
                            }
                        }
                    }
                }
            }

           
            return base.CheckDead();
        }

        public override bool CheckConditions(int left, int right, int top, int bottom)    //Allows you to define special conditions required for npc town NPC's house
        {
            return true;  //so when a house is available the npc will  spawn
        }

        public override void SetChatButtons(ref string button, ref string button2)  //Allows you to set the text for the buttons that appear on npc town NPC's chat window.
        {
            button = "Buy Explosioves";   //npc defines the buy button name
            button2 = "Combine";
        }
        public override void OnChatButtonClicked(bool firstButton, ref bool openShop) //Allows you to make something happen whenever a button is clicked on npc town NPC's chat window. The firstButton parameter tells whether the first button or second button (button and button2 from SetChatButtons) was clicked. Set the shop parameter to true to open npc NPC's shop.
        {

            if (firstButton)
            {
                openShop = true;   //so when you click on buy button opens the shop
            }
            else
            {
                Main.playerInventory = true;
                Main.npcChatText = "";

                GetInstance<ExtraExplosives>().ExtraExplosivesUserInterface.SetState(new UI.ExtraExplosivesUI());
            }
        }

        public override void SetupShop(Chest shop, ref int nextSlot)       //Allows you to add items to npc town NPC's shop. Add an item by setting the defaults of shop.item[nextSlot] then incrementing nextSlot.
        {

            if (NPC.downedMechBoss1)
            {
                shop.item[nextSlot].SetDefaults(mod.ItemType("ArenaBuilderItem"));
                nextSlot++;
            }

            if (NPC.downedPirates)
            {
                shop.item[nextSlot].SetDefaults(mod.ItemType("CritterBombItem"));
                nextSlot++;
            }

            if (NPC.downedClown)
            {
                shop.item[nextSlot].SetDefaults(mod.ItemType("DaBombItem"));
                nextSlot++;
            }

            if (NPC.downedBoss3)
            {
                shop.item[nextSlot].SetDefaults(mod.ItemType("HeavyBombItem"));
                nextSlot++;
            }

            if (NPC.downedPlantBoss)
            {
                shop.item[nextSlot].SetDefaults(mod.ItemType("C4Item"));
                nextSlot++;
            }

            if (NPC.downedPirates && Main.hardMode)
            {
                shop.item[nextSlot].SetDefaults(mod.ItemType("BulletBoomItem"));
                nextSlot++;
            }

            if (Main.hardMode)   
            {
                shop.item[nextSlot].SetDefaults(ModContent.ItemType<BombBuddyItem>());
                nextSlot++;
                shop.item[nextSlot].SetDefaults(mod.ItemType("MegaExplosiveItem"));
                nextSlot++;
                shop.item[nextSlot].SetDefaults(mod.ItemType("GiganticExplosiveItem"));
                nextSlot++;
            }

            shop.item[nextSlot].SetDefaults(mod.ItemType("BasicExplosiveItem"));
            nextSlot++;
            shop.item[nextSlot].SetDefaults(mod.ItemType("SmallExplosiveItem"));
            nextSlot++;
            shop.item[nextSlot].SetDefaults(mod.ItemType("MediumExplosiveItem"));
            nextSlot++;
            shop.item[nextSlot].SetDefaults(mod.ItemType("LargeExplosiveItem"));
            nextSlot++;
            shop.item[nextSlot].SetDefaults(mod.ItemType("TorchBombItem"));
            nextSlot++;
            shop.item[nextSlot].SetDefaults(mod.ItemType("DynaglowmiteItem"));
            nextSlot++;
            shop.item[nextSlot].SetDefaults(mod.ItemType("DeliquidifierItem"));
            nextSlot++;
            shop.item[nextSlot].SetDefaults(mod.ItemType("HydromiteItem"));
            nextSlot++;
            shop.item[nextSlot].SetDefaults(mod.ItemType("LavamiteItem"));
            nextSlot++;
            shop.item[nextSlot].SetDefaults(mod.ItemType("HouseBombItem"));
            nextSlot++;
            shop.item[nextSlot].SetDefaults(mod.ItemType("BunnyiteItem"));
            nextSlot++;

        }

        public override string GetChat()       //Allows you to give npc town NPC a chat message when a player talks to it.
        {
            switch (Main.rand.Next(10))    //npc are the messages when you talk to the npc
            {
                case 0:
                    return "Psst... PSSST... Hows it going?";
                case 1:
                    return "I have bombs, TONS OF BOMBS!!!";
                case 2:
                    return "Get scammed by the tinkerer yet?";
                case 3:
                    return "My explosives are too powerful for mortal man... why I'd say its impossible for you to even consider... Oh wait... Your a sprite. Nevermind, carry on!";
                case 4:
                    return "< Not Responsible For Any Causualties That May Or May Not Result From These Devices >";
                case 5:
                    return "Like I, your friend... the one you worked so hard for... would sell you items that could hurt you. *scoffs* Nonsense!";
                case 6:
                    return "I really don't see why you think I shouldn't carry matches, I mean come on, fires a good thing right?";
                case 7:
                    return "To all the haters, I'm not bald, I just have a short fuse.";
                case 8:
                    return "Sometimes I don't feel appreciated, after everything I've destroyed for you... couldn't I at least have a bigger house?";
                case 9:
                    return "If I were you, I'd spend your money here, you got enough potions and reforges!";
                case 10:
                    return "Don't let me die, you will regret it...";
                default:
                    return "Let me tell you a secret...";

            }
        }
        public override void TownNPCAttackStrength(ref int damage, ref float knockback)//  Allows you to determine the damage and knockback of npc town NPC attack
        {
            damage = 40;  //npc damage
            knockback = 2f;   //npc knockback
        }

        public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)  //Allows you to determine the cooldown between each of npc town NPC's attack. The cooldown will be a number greater than or equal to the first parameter, and less then the sum of the two parameters.
        {
            cooldown = 5;
            randExtraCooldown = 10;
        }

        //------------------------------------npc is an example of how to make the npc use a sward attack-------------------------------
        //public override void DrawTownAttackSwing(ref Texture2D item, ref int itemSize, ref float scale, ref Vector2 offset)//Allows you to customize how npc town NPC's weapon is drawn when npc NPC is swinging it (npc NPC must have an attack type of 3). Item is the Texture2D instance of the item to be drawn (use Main.itemTexture[id of item]), itemSize is the width and height of the item's hitbox
        //{
        //    scale = 1f;
        //    item = Main.itemTexture[mod.ItemType("CustomSword")]; //npc defines the item that npc npc will use
        //    itemSize = 56;
        //}

        //public override void TownNPCAttackSwing(ref int itemWidth, ref int itemHeight) //  Allows you to determine the width and height of the item npc town NPC swings when it attacks, which controls the range of npc NPC's swung weapon.
        //{
        //    itemWidth = 56;
        //    itemHeight = 56;
        //}

        //----------------------------------npc is an example of how to make the npc use a gun and a projectile ----------------------------------

        public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
        {
            multiplier = 12f;
            randomOffset = 2f;
        }

        //public override void DrawTownAttackGun(ref float scale, ref int item, ref int closeness) //Allows you to customize how npc town NPC's weapon is drawn when npc NPC is shooting (npc NPC must have an attack type of 1). Scale is a multiplier for the item's drawing size, item is the ID of the item to be drawn, and closeness is how close the item should be drawn to the NPC.
        //{
        //    scale = .7f; //1f;
        //    item = 164; // mod.ItemType("Handgun");  
        //    closeness = 0; //20S
        //}

        public override void TownNPCAttackProj(ref int projType, ref int attackDelay)//Allows you to determine the projectile type of npc town NPC's attack, and how long it takes for the projectile to actually appear
        {
            projType = mod.ProjectileType("NPCProjectile");
            attackDelay = 1;
        }
        public override void DrawTownAttackSwing(ref Texture2D item, ref int itemSize, ref float scale, ref Vector2 offset)
        {
            scale = .7f; //1f;
            base.DrawTownAttackSwing(ref item, ref itemSize, ref scale, ref offset);
        }
    }   
}
