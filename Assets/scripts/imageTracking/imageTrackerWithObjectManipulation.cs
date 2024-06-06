using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ImageTrackerWithObjectManipulation : MonoBehaviour
{
    private ARTrackedImageManager trackedImages;
    public GameObject[] arPrefabs;

    private Dictionary<string, GameObject> arObjects = new Dictionary<string, GameObject>();
    private List<GameObject> arObjectList = new List<GameObject>();

    void Awake()
    {
        trackedImages = GetComponent<ARTrackedImageManager>();
    }

    void OnEnable()
    {
        trackedImages.trackedImagesChanged += OnTrackedImagesChanged;
    }

    void OnDisable()
    {
        trackedImages.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    // Event Handler
    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        // Create object based on image tracked
        foreach (var trackedImage in eventArgs.added)
        {
            foreach (var arPrefab in arPrefabs)
            {
                if (trackedImage.referenceImage.name == arPrefab.name)
                {
                    var newPrefab = Instantiate(arPrefab, trackedImage.transform.position, trackedImage.transform.rotation);
                    newPrefab.AddComponent<ObjectManipulator>(); // Add ObjectManipulator script
                    arObjects[trackedImage.referenceImage.name] = newPrefab;
                    arObjectList.Add(newPrefab);
                }
            }
        }

        // Update tracking position
        foreach (var trackedImage in eventArgs.updated)
        {
            if (arObjects.ContainsKey(trackedImage.referenceImage.name))
            {
                var arObject = arObjects[trackedImage.referenceImage.name];
                arObject.transform.position = trackedImage.transform.position;
                arObject.transform.rotation = trackedImage.transform.rotation;
                arObject.SetActive(trackedImage.trackingState == TrackingState.Tracking);
            }
        }

        // Handle removed images
        foreach (var trackedImage in eventArgs.removed)
        {
            if (arObjects.ContainsKey(trackedImage.referenceImage.name))
            {
                var arObject = arObjects[trackedImage.referenceImage.name];
                arObject.SetActive(false);
                arObjectList.Remove(arObject);
                arObjects.Remove(trackedImage.referenceImage.name);
            }
        }
    }
}
