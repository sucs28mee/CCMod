using CCMod.Common;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using CCMod.Utils;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CCMod.Content.Items.Weapons.Magic
{
    public class HexedSkyBladesProjectile : ModProjectile
    {
        public override string Texture => base.Texture.Replace("Projectile", string.Empty);

        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 1;

            Projectile.aiStyle = -1;

            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = -1;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;

            Projectile.netImportant = true;

            Projectile.timeLeft = 500;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 999;
        }

        bool onSpawnStuff = true;
        public override void AI()
        {
            if (onSpawnStuff)
            {
                onSpawnStuff = false;

                Projectile.rotation = Projectile.velocity.ToRotation();
                Projectile.hostile = true;
                Projectile.netUpdate = true;

                CCModUtils.NewDustCircular(Projectile.Center, 10, DustID.SilverFlame, 16, minMaxSpeedFromCenter: (6, 6), dustAction: d => d.noGravity = true);
            }

            Projectile.velocity *= 0.87f;

            float lSQ = Projectile.velocity.LengthSquared();
            if (lSQ < 500f)
            {
                if ((Projectile.alpha += 4) >= 255)
                    Projectile.Kill();
            }
            
            if (Projectile.timeLeft % 3 == 0)
            {
                Dust.NewDustDirect(Projectile.Center + Projectile.rotation.ToRotationVector2() * 32 * Main.rand.NextFloatDirection(), 0, 0, DustID.SilverFlame).noGravity = true;
            }
        }

        public override bool? CanHitNPC(NPC target)
        {
            return target.friendly ? false : null;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Projectile.velocity *= 0.3f;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Vector2 rotVector = Projectile.rotation.ToRotationVector2();
            float x = 0;
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center - rotVector * 32, Projectile.Center + rotVector * 32, 10, ref x);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.EasyDraw(lightColor, rotation: Projectile.rotation + MathHelper.PiOver4);
            return false;
        }
    }
}
