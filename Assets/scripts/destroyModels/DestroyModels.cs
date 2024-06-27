/*using UnityEngine;

public class ClearModelsHandler : MonoBehaviour
{
    public ImageTrackerWithObjectManipulation imageTracker; // Reference to your ImageTracker script

    public void ClearAllModels()
    {
        if (imageTracker != null)
        {
            imageTracker.ClearAllModels();
        }
        else
        {
            Debug.LogError("ImageTracker reference is not assigned in the ClearModelsHandler script.");
        }
    }
}
*/


using UnityEngine;
using UnityEngine.SceneManagement;

public class DestroyModels : MonoBehaviour
{
    public void DestroyAllModels()
    {
        ARSessionManager arSessionManager = FindObjectOfType<ARSessionManager>();
        arSessionManager.StopARSession();
        arSessionManager.RestartARSession();
    }


}