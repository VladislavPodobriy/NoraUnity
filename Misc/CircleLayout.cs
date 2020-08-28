/*using UnityEngine;
using Pixelplacement;
using Pixelplacement.TweenSystem;
public class CircleLayout : MonoBehaviour
{
    [SerializeField] private float angularStep;
    [SerializeField] private float spacing;

    [SerializeField] private float sensetivity;
    [SerializeField] private bool inertia;
    [SerializeField] private float deceleration;

    private float R;
    private Vector3 pivot;

    private Vector2 startTouchPosition;
    private float touchTimer;
    private TweenBase inertiaTween;

    private float maxAngle;

    void Start()
    {
        if (transform.childCount == 0)
            return;

        var calculatedAngularStep = 360f / transform.childCount;

        if (calculatedAngularStep < angularStep)
            angularStep = calculatedAngularStep;

        R = (spacing / Mathf.Sin(Mathf.Deg2Rad * (angularStep / 2))) / 2;

        transform.localPosition -= new Vector3(0, R, 0);

        ResetLayout();
    }

    void ResetLayout()
    {
        var angle = 0f;
        foreach (Transform child in transform)
        {
            child.localPosition = new Vector3(R * Mathf.Sin(Mathf.Deg2Rad * angle), R * Mathf.Cos(Mathf.Deg2Rad * angle));
            child.eulerAngles = new Vector3(0, 0, -angle);

            maxAngle = angle;
            angle += angularStep;
        }
    }

    private void Update()
    {
        foreach(var touch in Input.touches)
        {
            if (touch.fingerId != 0)
                return;

            if (touch.phase == TouchPhase.Began)
                OnTouchBegan(touch);
            else if (touch.phase == TouchPhase.Moved)
                OnTouchMoved(touch);
            else if (touch.phase == TouchPhase.Ended)
                OnTouchEnded(touch);

            touchTimer += Time.deltaTime;
        }
    }

    private void OnTouchBegan(Touch touch)
    {
        startTouchPosition = GetTouchPosition(touch);
        touchTimer = 0;

        inertiaTween?.Stop();
    }

    private void OnTouchMoved(Touch touch)
    {
        transform.Rotate(new Vector3(0, 0, -touch.deltaPosition.x * Time.deltaTime * sensetivity));

        if (transform.eulerAngles.z > maxAngle)
        {
            if (touch.deltaPosition.x < 0)
                transform.eulerAngles = new Vector3(0, 0, maxAngle);
            else
                transform.eulerAngles = new Vector3(0, 0, 0);
        }
    }

    private void OnTouchEnded(Touch touch)
    {
        Vector2 touchPosition = GetTouchPosition(touch);

        var touchPositionDelta = startTouchPosition - touchPosition;
        var force = touchPositionDelta.x / touchTimer / 100;

        if (transform.eulerAngles.z > maxAngle)
        {
            if (force < 0)
                transform.eulerAngles = new Vector3(0, 0, 0);
            else
                transform.eulerAngles = new Vector3(0, 0, maxAngle);

            return;
        }

        if (Mathf.Abs(force) > 0.1f && inertia)
            StartIntertia(force);
    }

    private void StartIntertia(float force)
    {
        inertiaTween = Tween.Value(force, 0,
            (float value) => {
                transform.Rotate((new Vector3(0, 0, value)));

                if (transform.eulerAngles.z > maxAngle)
                {
                    if (force < 0)
                        transform.eulerAngles = new Vector3(0, 0, 0);
                    else
                        transform.eulerAngles = new Vector3(0, 0, maxAngle);

                    inertiaTween.Stop();
                }
            },
            deceleration, 0
        );
    }

    private Vector3 GetTouchPosition(Touch touch)
    {
        return Camera.main.ScreenToWorldPoint(touch.position);
    }
}*/
