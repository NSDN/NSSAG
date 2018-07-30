using System;

using DxLib;
using dotNSGDX;
using dotNSGDX.Utility;

namespace NSSAG.Entity
{
    public class Player : dotNSGDX.Entity.Player
    {
        public Player(Texture tex) : base(tex)
        {
        
        }

        public override Result OnUpdate(int t)
        {
            Vec2 vel = (0, 0);
            if (DX.CheckHitKey(DX.KEY_INPUT_UP) > 0) vel += (-5, 0);
            else if (DX.CheckHitKey(DX.KEY_INPUT_DOWN) > 0) vel += (5, 0);
            if (DX.CheckHitKey(DX.KEY_INPUT_LEFT) > 0) vel += (-5, 0);
            else if (DX.CheckHitKey(DX.KEY_INPUT_RIGHT) > 0) vel += (5, 0);
            Move(vel);
            return base.OnUpdate(t);
        }

        public bool DoJudge(Vec2 vec)
        {
            const float len = 4.0F; vec -= pos;
            if (Math.Abs(vec.X) < len && Math.Abs(vec.Y) < len)
                if (~vec < len) return true;
            return false;
        }
    }
}
