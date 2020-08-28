using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Nora.Misc
{
    public static class CollectionExtensions
    {
        public static T GetRandom<T>(this IEnumerable<T> collection)
        {
            var array = collection.ToArray();
            var index = Random.Range(0, array.Length);
            return array[index];
        }

        public static T GetRandomWithChance<T>(this Dictionary<T, int> chances)
        {
            var chancesSum = chances.Values.ToList().Sum();
            float randomChance = Random.Range(0, chancesSum);

            float totalChance = 0;
            foreach (KeyValuePair<T, int> pair in chances)
            {
                totalChance += pair.Value;
                if (totalChance > randomChance)
                    return pair.Key;
            }

            return default;
        }
    }
}
