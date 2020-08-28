using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SpriteOrder))]
public class SpriteOrderEditor : Editor
{
    private SpriteOrder spriteOrder;
    
    public void Awake()
    {
        spriteOrder = (SpriteOrder)target;

        spriteOrder.Renderer = spriteOrder.GetComponent<SpriteRenderer>();
        spriteOrder.Pivot = spriteOrder.GetComponent<Pivot>();
    }

    protected virtual void OnSceneGUI()
    {
        UpdateOrder();
    }

    public void UpdateOrder()
    {
        spriteOrder.UpdateOrder();
    }
}