using System;

using dotNSGDX;
using dotNSGDX.Utility;
using static dotNSGDX.Utility.RenderUtil;

namespace NSSAG
{
    public class NSSAGCore : NSGDX
    {
        private static NSSAGCore _core;
        public static NSSAGCore Instance
        {
            get
            {
                if (_core == null)
                    _core = new NSSAGCore();
                return _core;
            }
        }
        public static ObjectPoolCluster PoolCluster
        {
            get => Instance.poolCluster;
        }

        public bool IsExit { get; protected set; }

        public class InfoShow : IObject
        {
            protected ObjectPoolCluster cluster;

            public InfoShow(ObjectPoolCluster cluster) => this.cluster = cluster;

            public Result OnUpdate(int t)
            {
                return Result.DONE;
            }

            public Result OnRender(IRenderer renderer)
            {
                string info = cluster.ToString();
                renderer.DrawStr(16, 16, 16, (1, 1, 1), info);
                info = "frame: " + cluster.TickTime.ToString();
                renderer.DrawStr(16, DxManager.DevH - 32, 16, (1, 1, 1), info);
                return Result.DONE;
            }
        }

        private Texture arrow;

        public NSSAGCore() : base(new Renderer(), 1024)
        {
            IsExit = false;
        }

        public override void Setup()
        {
            base.Setup();

            arrow = new Texture("Assets/arrow.png");

            poolCluster.Add(new InfoShow(poolCluster));
            poolCluster.Add(new Entity.ArrowShow(arrow));
        }

        public override void Loop()
        {
            base.Loop();
        }
    }
}
