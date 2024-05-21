using UnityEngine;
using UnityEngine.SceneManagement;

public class SwipeDetector : MonoBehaviour
{
    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;
    private float minSwipeDistance;

    void Start()
    {
        // Set minSwipeDistance as 10% of the screen height
        minSwipeDistance = Screen.height * 0.1f;
    }

    // This method will be called from the WebView JavaScript
    public void HandleJSMessage(string message)
    {
        string[] parts = message.Split(':');
        if (parts.Length != 2) return;

        string eventType = parts[0];
        string[] coordinates = parts[1].Split(',');

        if (coordinates.Length != 2) return;

        float x = float.Parse(coordinates[0]);
        float y = float.Parse(coordinates[1]);
        Vector2 touchPosition = new Vector2(x, y);

        if (eventType == "TouchStart")
        {
            startTouchPosition = touchPosition;
        }
        else if (eventType == "TouchEnd")
        {
            endTouchPosition = touchPosition;
            Vector2 swipeDirection = endTouchPosition - startTouchPosition;

            if (swipeDirection.magnitude >= minSwipeDistance)
            {
                if (Mathf.Abs(swipeDirection.x) > Mathf.Abs(swipeDirection.y))
                {
                    // Handle horizontal swipes
                    if (swipeDirection.x < 0)
                    {
                        OnSwipeLeft();
                    }
                    else
                    {
                        OnSwipeRight();
                    }
                }
                else
                {
                    // Handle vertical swipes
                    if (swipeDirection.y < 0)
                    {
                        OnSwipeDown();
                    }
                    else
                    {
                        OnSwipeUp();
                    }
                }
            }
        }
    }

    void OnSwipeLeft()
    {
        // Handle left swipe if needed
    }

    void OnSwipeRight()
    {
        // Handle right swipe if needed
        SceneManager.LoadScene(0); // Assumes AR scene is at index 0
    }

    void OnSwipeUp()
    {
        // Handle up swipe if needed
    }

    void OnSwipeDown()
    {
        // Handle down swipe to go back to the AR scene
    }
}
