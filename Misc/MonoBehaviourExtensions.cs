using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class MonoBehaviourExtensions 
{
    public static List<ExtendedMonoBehaviour> FindByTag(this MonoBehaviour ctx, string tag)
    {
        var objects = Object.FindObjectsOfType<ExtendedMonoBehaviour>();
        return objects.Where(obj => obj.Tags.Contains(tag)).ToList();
    }
}
