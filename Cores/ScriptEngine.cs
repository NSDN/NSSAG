using System;
using System.IO;

using dotNSASM;

namespace NSSAG.Cores
{
    public class ScriptEngine : NSASM
    {
        public ScriptEngine() : base(128, 64, 32, null)
        {
            Util.Output = (value) => Console.Write(value);
            Util.Input = () => { return Console.ReadLine(); };
            Util.FileInput = (path) =>
            {
                StreamReader reader = new StreamReader(new FileStream(path, FileMode.Open));
                String var = reader.ReadToEnd();
                reader.Close();
                return var;
            };

            this.Execute("ld \"init.ns\"");
            this.Execute("run <main>");
        }

        protected override void LoadFuncList()
        {
            base.LoadFuncList();
        }
    }
}
