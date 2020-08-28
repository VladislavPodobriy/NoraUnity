using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelEditorController))]
public class LevelEditor : Editor
{
    private LevelEditorController _controller;

    private bool _mouseDown;

    private bool _grid = true;
    private bool _snap = true;

    private bool _selection = false;
    private Vector2 _startPoint;
    private Vector2 _endPoint;
    private bool a = true;
    public LevelEditor()
    {
        
    }

    void OnSceneGUI()
    {
        _controller  = target as LevelEditorController;

        if (_controller?.CurrentPrefab == null || _controller.Prefabs.Count < 1)
            return;

        if (_controller.CurrentPrefab.Type != _controller.PrefabType)
        {
            if (_controller.Mode == Mode.Add)
                _controller.Destroy();

            _controller.Set(_controller.PrefabType);
        }

        
        MoveSelection();

        DrawSelection();
        DrawHandles();

        ListenEvents();
    }

    private void MoveSelection()
    {
        if (_controller.SelectedObjects.Count < 1)
            return;

        var pivot = _controller.SelectedObjects[_controller.SelectedObjects.Count - 1];
        if (pivot.position != _controller.Selection.position)
        {
            var diff = _controller.Selection.position - pivot.position;
            foreach (var selectedObject in _controller.SelectedObjects)
                selectedObject.position += diff;
            _controller.Selection.position -= diff;
        }

        if (_controller.Mode != Mode.Select)
        {
            var position = GetMousePosition();
            Vector2 pos;
            if (_grid)
            {
                if (_snap)
                    pos = new Vector2(Mathf.Round(position.x), Mathf.Round(position.y));
                else
                    pos = new Vector2(Mathf.Round(position.x * 20) / 20, Mathf.Round(position.y * 20) / 20);
            }
            else
                pos = new Vector2(position.x, position.y);
          
            _controller.Selection.position = pos;
        }
    }

    private void DrawHandles()
    {
        var objects = FindObjectsOfType<LevelObject>();
        foreach (var levelObject in objects)
        {
            if (_controller.SelectionContains(levelObject.transform) && _controller.Mode == Mode.Add)
                continue;

            if (Handles.Button(levelObject.transform.position, Quaternion.identity, 0.1f, 0.1f,
                Handles.RectangleHandleCap))
            {
                if (_controller.SelectionContains(levelObject.transform))
                {
                    _controller.RemoveFromSelection(levelObject.transform);
                    return;
                }

                if (_controller.Mode != Mode.Select)
                {
                    _controller.ClearSelection();
                    _controller.Mode = Mode.Edit;
                }

                _controller.AddToSelection(levelObject.transform);
            }
        }
    }

    private void DrawSelection()
    {
        if (_selection)
            Handles.DrawSolidRectangleWithOutline(new Rect(_startPoint, GetMousePosition() - _startPoint), new Color(0, 0.5f, 0.8f, 0.1f), new Color(0, 0.5f, 0.8f, 0.3f));

        Handles.DrawLine(_controller.Selection.position - new Vector3(0f, 0.5f), _controller.Selection.position + new Vector3(0f, 0.5f));
        Handles.DrawLine(_controller.Selection.position - new Vector3(0.5f, 0f), _controller.Selection.position + new Vector3(0.5f, 0f));
    }

    private void ListenEvents()
    {
        Event e = Event.current;
        int controlId = GUIUtility.GetControlID(FocusType.Passive);
        switch (e.GetTypeForControl(controlId))
        {
            case EventType.MouseDown:
                GUIUtility.hotControl = controlId;
                if (e.button == 0)
                    if (_controller.Mode == Mode.Select)
                        StartSelection();
                e.Use();
                break;
            case EventType.MouseUp:
                GUIUtility.hotControl = 0;
                if (e.button == 0) 
                    if (_controller.Mode == Mode.Select)
                        EndSelection();
                    else
                        Submit();
                if (e.button == 1)
                    Cancel();
                e.Use();
                break;
            case EventType.MouseDrag:
                GUIUtility.hotControl = controlId;
                e.Use();
                break;
            case EventType.KeyDown:
                if (e.keyCode == KeyCode.LeftAlt)
                    ToggleGrid(false);
                if (e.keyCode == KeyCode.LeftControl)
                    ToggleSnap(false);
                if (e.keyCode == KeyCode.A)
                    ToggleSelectMode(true);
                e.Use();
                break;
            case EventType.KeyUp:
                if (e.keyCode == KeyCode.LeftAlt)
                    ToggleGrid(true);
                if (e.keyCode == KeyCode.LeftControl)
                    ToggleSnap(true);
                if (e.keyCode == KeyCode.Minus)
                    Scale(-1);
                if (e.keyCode == KeyCode.Plus)
                    Scale(1);
                if (e.keyCode == KeyCode.LeftArrow)
                    Rotate(+1);
                if (e.keyCode == KeyCode.RightArrow)
                    Rotate(-1);
                if (e.keyCode == KeyCode.A)
                    ToggleSelectMode(false);
                if (e.keyCode == KeyCode.Delete)
                    _controller.Destroy();
                e.Use();
                break;
        }
    }

    private void ToggleGrid(bool value)
    {
        _grid = value;
    }

    private void ToggleSnap(bool value)
    {
        _snap = value;
    }

    private void StartSelection()
    {
        
        _selection = true;
        _startPoint = GetMousePosition();
    }

    private void EndSelection()
    {
        _selection = false;
        _endPoint = GetMousePosition();

        var min = new Vector2(Mathf.Min(_startPoint.x, _endPoint.x), Mathf.Min(_startPoint.y, _endPoint.y));
        var max = new Vector2(Mathf.Max(_startPoint.x, _endPoint.x), Mathf.Max(_startPoint.y, _endPoint.y));

        var objects = FindObjectsOfType<LevelObject>();
        foreach (var obj in objects) {
            bool inBounds = !(obj.transform.position.x < min.x || 
                              obj.transform.position.x > max.x ||
                              obj.transform.position.y < min.y ||
                              obj.transform.position.y > max.y);

            if (inBounds)
                _controller.AddToSelection(obj.transform);
        }

        if (_controller.SelectedObjects.Count > 0 && _controller.Mode != Mode.Edit)
            _controller.Mode = Mode.Edit;
    }

    private void CancelSelection()
    {
        _selection = false;
    }

    private void Submit()
    {
        _controller.ClearSelection();

        if (_controller.Mode == Mode.Add)
        {
            var instance = Instantiate(_controller.CurrentPrefab.GameObject, _controller.Selection.position, Quaternion.identity);
            instance.transform.parent = _controller.World;
            _controller.AddToSelection(instance.transform);
        }

        _controller.Mode = Mode.Add;
    }

    private void Cancel()
    {
        if (_controller.Mode == Mode.Add)
            _controller.Destroy();
        else
            _controller.ClearSelection();
        
        _controller.Mode = Mode.Add;
    }

    private void Rotate(int dir)
    {
        var angle = _controller.Selection.eulerAngles.z;
        
        if (dir < 0)
            _controller.Selection.eulerAngles = new Vector3(0,0, angle - 15);
        else
            _controller.Selection.eulerAngles = new Vector3(0, 0, angle + 15);
    }

    private void Scale(int dir)
    {
        var scale = _controller.Selection.localScale;

        if (dir < 0)
            _controller.Selection.localScale = new Vector3(scale.x - 0.05f, scale.y - 0.05f);
        else
            _controller.Selection.localScale = new Vector3(scale.x + 0.05f, scale.y + 0.05f);
    }

    private void ToggleSelectMode(bool value)
    {
        if (value)
            _controller.Mode = Mode.Select;
        else
        {
            _controller.Mode = Mode.Edit;
            CancelSelection();
        }
    }

    private Vector2 GetMousePosition()
    {
        return HandleUtility.GUIPointToWorldRay(Event.current.mousePosition).origin;
    }
}
