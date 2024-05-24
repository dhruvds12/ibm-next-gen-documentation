using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class togglePopup : MonoBehaviour
{
    public GameObject marsinfo;
    public GameObject earthinfo;
    public Toggle interactionToggle;

    private bool isDetailedInteractionEnabled;
    private Dictionary<string, GameObject> activePopups = new Dictionary<string, GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        if (interactionToggle != null)
        {
            interactionToggle.onValueChanged.AddListener(delegate { ToggleValueChanged(interactionToggle); });
        }
        isDetailedInteractionEnabled = interactionToggle.isOn;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Mouse Clicked");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log(hit.transform.tag);
                if (hit.transform.tag == "mars")
                {
                    HandleClick(hit.point, "mars", marsinfo);
                }

                if (hit.transform.tag == "earth2")
                {
                    HandleClick(hit.point, "earth2", earthinfo);
                }

                if (hit.transform.tag == "marsInfo")
                {
                    Destroy(hit.transform.gameObject);
                    activePopups.Remove("mars");
                }

                if (hit.transform.tag == "earthInfo")
                {
                    Destroy(hit.transform.gameObject);
                    activePopups.Remove("mars");
                }
            }
        }
    }

    void HandleClick(Vector3 hitPoint, string tag, GameObject infoPrefab)
    {
        if (isDetailedInteractionEnabled)
        {
            // Placeholder for future detailed card implementation
            return;
        }

        if (activePopups.ContainsKey(tag) && activePopups[tag] != null)
        {
            return; // Popup already exists, do nothing
        }

        Vector3 pos = hitPoint;
        pos.z += 0.1f;
        pos.y += 0.25f;
        GameObject popup = Instantiate(infoPrefab, pos, transform.rotation);
        activePopups[tag] = popup;
    }

    void ToggleValueChanged(Toggle change)
    {
        isDetailedInteractionEnabled = change.isOn;
    }
}
