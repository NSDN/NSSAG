using System;

using dotNSGDX;
using dotNSGDX.Utility;

namespace NSSAG.Entity
{
    public class Bullet : dotNSGDX.Entity.Bullet
    {
        public Player Player { get; set; }

        public Bullet(Vec2 pos, Vec2 aim, Texture tex) : base(pos, aim, tex)
        {
            scale = 0.25F;
        }

        public override Result OnUpdate(int t)
        {
            const float border = 32.0F;
            if (pos.X > (DxManager.DevW + border) || pos.X < -border) return Result.END;
            if (pos.Y > (DxManager.DevH + border) || pos.Y < -border) return Result.END;

            if (Player != null)
                if (Player.DoJudge(new Vec2(pos)))
                    return Result.END;

            return base.OnUpdate(t);
        }
    }
}
