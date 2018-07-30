using System;
using System.Windows;

using DxLib;
using static dotNSGDX.Utility.RenderUtil;

namespace NSSAG
{
    public class DxManager : IDisposable
    {
        public static DxManager Instance { get; protected set; }

        public const int DevW = 1920;
        public const int DevH = 1080;

        public bool UseDXA { get; protected set; }
        public string Key { get; protected set; }

        protected DxManager(bool useDxA, string key)
        {
            UseDXA = useDxA; Key = key;
        }

        public DxManager(bool useDxA, string key, string title, int width, int height, bool full)
        {
            DX.SetOutApplicationLogValidFlag(0);
            if (full)
            {
                DX.ChangeWindowMode(1);
                DX.SetWindowStyleMode(2);
                DX.SetWindowSize((int)SystemParameters.PrimaryScreenWidth, (int)SystemParameters.PrimaryScreenHeight);
            }
            else
            {
                DX.ChangeWindowMode(1);
                DX.SetWindowStyleMode(5);
                DX.SetWindowSize(width, height);
            }
            
            DX.SetGraphMode(DevW, DevH, 32);
            DX.SetWindowText(title);
            if (DX.ChangeFont("Consolas") == -1)
                DX.ChangeFont("Terminal");
            DX.SetAlwaysRunFlag(1);

            DX.SetUseDXArchiveFlag(UseDXA ? 1 : 0);
            if (UseDXA)
            {
                DX.SetDXArchiveExtension(Key);
                DX.SetDXArchiveKeyString(Key);
            }

            if (DX.DxLib_IsInit() == 0)
            {
                if (DX.DxLib_Init() == -1)
                    Instance = null;
                DX.SetDrawScreen(DX.DX_SCREEN_BACK);
                DX.SetUse3DFlag(0);
            }

            Instance = new DxManager(useDxA, key);
        }

        public int ProcessInfo()
        {
            return DX.ProcessMessage();
        }

        public void Dispose()
        {
            if (DX.DxLib_IsInit() == 1)
                DX.DxLib_End();
        }
    }

    public class Texture : IDrawable, IDisposable
    {
        public int Handle { get; protected set; }
        public int Width { get; protected set; }
        public int Height { get; protected set; }

        public Texture(string path)
        {
            if (DxManager.Instance != null)
            {
                Handle = DX.LoadGraph(path);
                DX.GetGraphSize(Handle, out int w, out int h);
                Width = w; Height = h;
            }
        }

        public void Dispose()
        {
            DX.DeleteGraph(Handle);
            Handle = -1; Width = 0; Height = 0;
        }

        public object GetRes()
        {
            return Handle;
        }
    }

    public class Renderer : IRenderer
    {
        public void Begin()
        {
            DX.ClsDrawScreen();
        }

        public void Draw(IDrawable drawable, float x, float y, float rotate, float scale, Color4 color)
        {
            if (drawable is Texture)
            {
                DX.SetDrawBlendMode(DX.DX_BLENDMODE_ALPHA, (int)(color.a * 255));
                DX.SetDrawBright((int)(color.r * 255), (int)(color.g * 255), (int)(color.b * 255));
                DX.DrawRotaGraphFastF(x, y, scale, (float)(rotate + Math.PI), (int)drawable.GetRes(), 1);
                //DX.DrawBoxAA(x - 2, y - 2, x + 2, y + 2, 0xFFFFFF, 1);
                DX.SetDrawBright(255, 255, 255);
                DX.SetDrawBlendMode(DX.DX_BLENDMODE_ALPHA, 255);
            }
        }

        public void DrawStr(float x, float y, float scale, Color4 color, string str)
        {
            DX.SetFontSize((int)scale);
            DX.SetDrawBlendMode(DX.DX_BLENDMODE_ALPHA, (int)(color.a * 255));
            DX.SetDrawBright(255, 255, 255);
            DX.DrawStringF(x, y, str, color.ToRGB());
            DX.SetDrawBright(255, 255, 255);
            DX.SetDrawBlendMode(DX.DX_BLENDMODE_ALPHA, 255);
        }

        public void End()
        {
            DX.ScreenFlip();
        }
    }
}
