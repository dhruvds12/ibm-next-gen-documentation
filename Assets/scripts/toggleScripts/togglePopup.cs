using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class togglePopup : MonoBehaviour
{
    public GameObject marsinfo;
    public GameObject earthinfo;
    public Toggle interactionToggle;

    public List<string> marsInfoText = new List<string>(); // used to store the text for the info box
    public List<string> earthInfoText = new List<string>(); // used to store the text for the info box

    public TMP_Text infoBox;
    public Canvas canvas;

    private bool isDetailedInteractionEnabled;
    private Dictionary<string, GameObject> activePopups = new Dictionary<string, GameObject>();

    private int marsInfoPointer = -1;
    private int earthInfoPointer = -1;

    // Start is called before the first frame update
    void Start()
    {
        canvas.enabled = false;
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
                    activePopups.Remove("earth2");
                }
            }
        }
    }

    void HandleClick(Vector3 hitPoint, string tag, GameObject infoPrefab)
    {
        if (isDetailedInteractionEnabled)
        {
            canvas.enabled = true;
            if (tag == "mars")
            {
                marsInfoPointer = 0;
                displayInfo(tag, marsInfoPointer);
            }
            else if (tag == "earth2")
            {
                earthInfoPointer = 0;
                displayInfo(tag, earthInfoPointer);
            }

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
        if (!isDetailedInteractionEnabled)
        {
            canvas.enabled = false;
        }
    }

    void displayInfo(string tag, int pointer)
    {
        if (tag == "mars")
        {
            if (pointer >= 0 && pointer < marsInfoText.Count)
            {
                infoBox.text = marsInfoText[pointer];
            }
        }
        else if (tag == "earth2")
        {
            if (pointer >= 0 && pointer < earthInfoText.Count)
            {
                infoBox.text = earthInfoText[pointer];
            }
        }
    }

    public void nextInfo()
    {
        if (isDetailedInteractionEnabled)
        {
            if (marsInfoPointer >= 0)
            {
                marsInfoPointer++;
                if (marsInfoPointer >= marsInfoText.Count) marsInfoPointer--;
                displayInfo("mars", marsInfoPointer);
            }
            if (earthInfoPointer >= 0)
            {
                earthInfoPointer++;
                if (earthInfoPointer >= earthInfoText.Count) earthInfoPointer--;
                displayInfo("earth2", earthInfoPointer);
            }
        }
    }
}
