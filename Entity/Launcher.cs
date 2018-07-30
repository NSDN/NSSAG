using System;

using dotNSGDX;
using dotNSGDX.Utility;

namespace NSSAG.Entity
{
    public class ArrowLauncher : dotNSGDX.Entity.Exectuor
    {
        private Texture texture;

        private int t;
        private Vec2 center;

        private Player thePlayer;

        public ArrowLauncher(int t, Player player, Texture tex)
        {
            this.texture = tex;
            this.t = t;
            this.center = (Vec2)(DxManager.DevW, DxManager.DevH) * 0.5;
            thePlayer = player;
        }

        public override Result OnUpdate(int t)
        {
            if (t == this.t)
            {
                Bullet[] bullets = new Bullet[64];
                for (int i = 0; i < bullets.Length; i++)
                {
                    double angle = (double)i / bullets.Length * Math.PI * 2;
                    angle += (Math.PI * 2 * Math.Abs(Math.Sin(Vec2.D2R(t / 256.0))));
                    angle %= (Math.PI * 2);
                    Vec2 pos = !((Vec2)(1, 0) ^ angle) * 8;
                    bullets[i] = new Bullet(center + pos, center, texture);
                    Vec2 vel = (Vec2)(1, 0) ^ angle;
                    Vec2 acc = (vel ^ Vec2.D2R(90)) * 0.05;
                    bullets[i].Vel(vel).Acc(acc);

                    angle = ColorMod(t, Vec2.R2D(angle));
                    angle %= 360.0F;
                    Utility.Color3 color = Utility.Hsv2RGB((float)angle, 1.0F, 1.0F);
                    bullets[i].Color((color.r, color.g, color.b));

                    bullets[i].Player = thePlayer;

                    NSSAGCore.PoolCluster.Add(bullets[i]);
                }
                return Result.END;
            }
            return Result.DONE;
        }

        public double ColorMod(int t, double angle)
        {
            if ((t % 1280) < 640) angle = t;
            else angle += (t % 360);
            return angle;
        }
    }

    public class ArrowShow : dotNSGDX.Entity.Exectuor
    {
        private Texture texture;

        public ArrowShow(Texture tex)
        {
            texture = tex;
        }

        public override Result OnUpdate(int t)
        {
            if ((t % 320) == 0)
            {
                for (int i = 0; i < 64; i++)
                    NSSAGCore.PoolCluster.Add(new ArrowLauncher(i * 5 + t, null, texture));
            }
            return Result.DONE;
        }
    }
}
