using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ImageTrackerWithObjectManipulation : MonoBehaviour
{
    private ARTrackedImageManager trackedImages;
    public GameObject[] arPrefabs;

    private Dictionary<string, GameObject> arObjects = new Dictionary<string, GameObject>();
    private Dictionary<string, Quaternion> objectRotations = new Dictionary<string, Quaternion>();
    private List<GameObject> arObjectList = new List<GameObject>();
    private Dictionary<string, bool> isManipulating = new Dictionary<string, bool>();

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
                    var manipulator = newPrefab.AddComponent<ObjectManipulator>();
                    manipulator.imageTracker = this; // Pass reference to ImageTracker
                    manipulator.imageName = trackedImage.referenceImage.name;
                    arObjects[trackedImage.referenceImage.name] = newPrefab;
                    arObjectList.Add(newPrefab);
                    isManipulating[trackedImage.referenceImage.name] = false; // Initialize manipulation state
                    objectRotations[trackedImage.referenceImage.name] = newPrefab.transform.rotation; // Initialize rotation state
                }
            }
        }

        // Update tracking position
        foreach (var trackedImage in eventArgs.updated)
        {
            if (arObjects.ContainsKey(trackedImage.referenceImage.name) && !isManipulating[trackedImage.referenceImage.name])
            {
                var arObject = arObjects[trackedImage.referenceImage.name];
                arObject.transform.position = trackedImage.transform.position;
                arObject.transform.rotation = objectRotations[trackedImage.referenceImage.name];
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
                isManipulating.Remove(trackedImage.referenceImage.name); // Remove manipulation state
                objectRotations.Remove(trackedImage.referenceImage.name); // Remove rotation state
            }
        }
    }

    // Methods to set manipulation state
    public void SetManipulating(string imageName, bool manipulating)
    {
        if (isManipulating.ContainsKey(imageName))
        {
            isManipulating[imageName] = manipulating;
        }
    }

    // Methods to set rotation
    public void SetRotation(string imageName, Quaternion rotation)
    {
        if (objectRotations.ContainsKey(imageName))
        {
            objectRotations[imageName] = rotation;
        }
    }

    // Method to get rotation
    public Quaternion GetRotation(string imageName)
    {
        if (objectRotations.ContainsKey(imageName))
        {
            return objectRotations[imageName];
        }
        return Quaternion.identity;
    }
}
