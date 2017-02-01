using System.Collections.Generic;
using Assets.Main;
using Assets.Scripts.Misc.Handlers;

namespace Assets.Scripts.Misc.CustomFunctions
{
    public static class ListExtensions
    {
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = GameHandler.Game.Random.Seed.Next(0, n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}