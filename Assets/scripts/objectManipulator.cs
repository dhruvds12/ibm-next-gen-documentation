using UnityEngine;

public class ObjectManipulator : MonoBehaviour
{
    [HideInInspector] public ImageTrackerWithObjectManipulation imageTracker;
    [HideInInspector] public string imageName;

    private Vector2 initialTouchPosition;
    private float initialTouchDistance;
    private float currentTouchDistance;
    private Vector3 initialScale;
    private float initialRotationAngleY;
    private float currentRotationAngleY;

    void Update()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                initialTouchPosition = touch.position;
                initialRotationAngleY = GetAngle(Vector2.zero, touch.position);
                imageTracker.SetManipulating(imageName, true); // Start manipulation
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                currentRotationAngleY = GetAngle(Vector2.zero, touch.position);
                HandleRotate();
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                imageTracker.SetManipulating(imageName, false); // End manipulation
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
                imageTracker.SetManipulating(imageName, true); // Start manipulation
            }
            else if (touch1.phase == TouchPhase.Moved || touch2.phase == TouchPhase.Moved)
            {
                currentTouchDistance = Vector2.Distance(touch1.position, touch2.position);
                HandleScale();
            }
            else if (touch1.phase == TouchPhase.Ended || touch2.phase == TouchPhase.Canceled)
            {
                imageTracker.SetManipulating(imageName, false); // End manipulation
            }
        }
    }

    void HandleScale()
    {
        float scaleFactor = currentTouchDistance / initialTouchDistance;
        transform.localScale = initialScale * scaleFactor;
        imageTracker.SetRotation(imageName, transform.rotation); // Update rotation in tracker
    }

    void HandleRotate()
    {
        float angleDeltaY = currentRotationAngleY - initialRotationAngleY;
        transform.Rotate(Vector3.up, angleDeltaY); // Rotate around the y-axis
        initialRotationAngleY = currentRotationAngleY;
        imageTracker.SetRotation(imageName, transform.rotation); // Update rotation in tracker
    }

    float GetAngle(Vector2 pos1, Vector2 pos2)
    {
        Vector2 direction = pos2 - pos1;
        return Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    }
}
