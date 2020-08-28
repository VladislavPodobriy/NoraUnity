using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Pivot))]
class PivotEditor : Editor
{
    private Pivot pivot;
   
    protected virtual void Awake()
    {
        pivot = (Pivot)target;
        pivot.GlobalPivot = pivot.transform.position - pivot.LocalPivot;
    }

    protected virtual void OnSceneGUI()
    {
        if (pivot == null)
            return;

        Handles.color = Color.red;

        EditorGUI.BeginChangeCheck();

        Vector3 pos = Handles.FreeMoveHandle(
            pivot.GlobalPivot,
            Quaternion.identity, 
            .1f, 
            new Vector3(.05f, .05f, .05f), 
            Handles.CircleHandleCap
        );

        if (EditorGUI.EndChangeCheck())
        {            
            pivot.LocalPivot = pivot.transform.position - pos;
            pivot.GlobalPivot = pos;
        }
    } 
}