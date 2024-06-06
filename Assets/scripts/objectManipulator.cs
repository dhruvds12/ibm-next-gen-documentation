using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ObjectManipulator : MonoBehaviour
{
    private ARRaycastManager arRaycastManager;
    private Vector2 initialTouchPosition;
    private Vector2 currentTouchPosition;
    private float initialTouchDistance;
    private float currentTouchDistance;
    private Vector3 initialScale;

    void Start()
    {
        arRaycastManager = FindObjectOfType<ARRaycastManager>();
    }

    void Update()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                initialTouchPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                currentTouchPosition = touch.position;
                HandleMove();
            }
        }
        else if (Input.touchCount == 2)
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            if (touch1.phase == TouchPhase.Began || touch2.phase == TouchPhase.Began)
            {
                initialTouchDistance = Vector2.Distance(touch1.position, touch2.position);
                initialScale = transform.localScale;
            }
            else if (touch1.phase == TouchPhase.Moved || touch2.phase == TouchPhase.Moved)
            {
                currentTouchDistance = Vector2.Distance(touch1.position, touch2.position);
                HandleScale();
            }
        }
    }

    void HandleMove()
    {
        Vector2 delta = currentTouchPosition - initialTouchPosition;
        Vector3 translation = new Vector3(delta.x, delta.y, 0) * Time.deltaTime;
        transform.Translate(translation);
        initialTouchPosition = currentTouchPosition;
    }

    void HandleScale()
    {
        float scaleFactor = currentTouchDistance / initialTouchDistance;
        transform.localScale = initialScale * scaleFactor;
    }
}
