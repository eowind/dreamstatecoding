using System;
using techdump.opengl.Components;

namespace techdump.opengl
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            new MainWindow().Run(60);
        }
    }
}
