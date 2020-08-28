using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Pivot))]
public class SpriteOrder : MonoBehaviour
{
    public SpriteRenderer Renderer;
    public Pivot Pivot;

    [SerializeField] private bool _update;

    public void Awake()
    {
        Renderer = GetComponent<SpriteRenderer>();
        Pivot = GetComponent<Pivot>();
    }

    public void Start()
    {
        UpdateOrder();
    }

    public void Update()
    {
        if (_update)
            UpdateOrder();
    }

    public void UpdateOrder()
    {
        var order = (int)(Pivot.GlobalPivot.y * 10);
        Renderer.sortingOrder = -order;
    }
}
