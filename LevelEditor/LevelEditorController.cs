using System;
using System.Collections.Generic;
using System.Linq;
using Nora;
using UnityEngine;

public class LevelEditorController : Singleton<LevelEditorController>
{
    public Mode Mode = Mode.Add;

    public List<Prefab> Prefabs = new List<Prefab>();

    [HideInInspector]
    public Prefab CurrentPrefab;

    public PrefabType PrefabType;

    [SerializeField] public Transform World;
    [SerializeField] public Transform Selection;
    [SerializeField] public List<Transform> SelectedObjects;

    public GameObject Set(PrefabType type)
    {
        CurrentPrefab = Prefabs.FirstOrDefault(x => x.Type == type);
        return CurrentPrefab?.GameObject;
    }

    public void Destroy()
    {
        while (SelectedObjects.Count > 0)
        {
            var obj = SelectedObjects[0];
            RemoveFromSelection(obj);
            DestroyImmediate(obj.gameObject);
        }
    }

    public void AddToSelection(Transform child)
    {
        if (Selection == null)
            return;

        child.parent = Selection;

        if (!SelectionContains(child))
            SelectedObjects.Add(child);
    }

    public void RemoveFromSelection(Transform child)
    {
        child.parent = World;

        if (SelectionContains(child))
            SelectedObjects.Remove(child);
    }

    public void ClearSelection()
    {
        while (SelectedObjects.Count > 0)
            RemoveFromSelection(SelectedObjects[0]);
    }

    public bool SelectionContains(Transform child)
    {
        return SelectedObjects.Contains(child);
    }
}

public enum PrefabType
{
    Wall,
    Star,
    Key,
    Pipe,
    Cut,
    Box,
    Enter,
    Exit,
    Spike,
    Wall2,
}

public enum Mode
{
    Add,
    Edit,
    Select
}

[Serializable]
public class Prefab
{
    public PrefabType Type;
    public GameObject GameObject;

}
