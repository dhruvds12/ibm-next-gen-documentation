using UnityEngine;
using UnityEngine.SceneManagement;

public class SwipeDetectorAR : MonoBehaviour
{
    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;
    private float minSwipeDistance;

    void Start()
    {
        // Set minSwipeDistance as 10% of the screen height
        minSwipeDistance = Screen.height * 0.1f;
    }

    void Update()
    {
        DetectSwipe();
    }

    void DetectSwipe()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                startTouchPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                endTouchPosition = touch.position;
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
    }

    void OnSwipeLeft()
    {
        // Handle left swipe if needed
    }

    void OnSwipeRight()
    {
        // Handle right swipe if needed
    }

    void OnSwipeUp()
    {
        // Handle up swipe if needed
        SceneManager.LoadScene(1); // Assumes AR scene is at index 0
    }

    void OnSwipeDown()
    {
        // Handle down swipe to go back to the AR scene
    }
}
