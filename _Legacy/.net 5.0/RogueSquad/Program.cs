using System;
using RogueSquadLib;

namespace RogueSquad
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new Engine())
                game.Run();
        }
    }
}
