using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ARInteractionManager : MonoBehaviour
{
    [System.Serializable]
    public struct InfoData
    {
        public string tag;
        public GameObject infoPrefab;
        public List<string> infoTexts;
    }

    public List<InfoData> infoDataList;
    public Toggle interactionToggle;
    public TMP_Text headerText;
    public TMP_Text infoBox;
    public Canvas canvas;
    public Button previousButton; // Reference to the Previous Button
    public Button nextButton; // Reference to the Next Button

    private bool isDetailedInteractionEnabled;
    private Dictionary<string, GameObject> activePopups = new Dictionary<string, GameObject>();
    private Dictionary<string, List<string>> infoTextsDict = new Dictionary<string, List<string>>();
    private Dictionary<string, int> infoPointers = new Dictionary<string, int>();

    // Start is called before the first frame update
    void Start()
    {
        canvas.enabled = false;
        if (interactionToggle != null)
        {
            interactionToggle.onValueChanged.AddListener(delegate { ToggleValueChanged(interactionToggle); });
        }
        isDetailedInteractionEnabled = interactionToggle.isOn;

        foreach (var infoData in infoDataList)
        {
            infoTextsDict[infoData.tag] = infoData.infoTexts;
            infoPointers[infoData.tag] = -1;
        }

        previousButton.gameObject.SetActive(false); // Hide previous button initially
        nextButton.gameObject.SetActive(false); // Hide next button initially

        previousButton.onClick.AddListener(PreviousInfo);
        nextButton.onClick.AddListener(NextInfo);
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
                HandleClick(hit.point, hit.transform.tag, hit.transform.gameObject);
            }
        }
    }

    void HandleClick(Vector3 hitPoint, string tag, GameObject hitObject)
    {
        if (infoTextsDict.ContainsKey(tag))
        {
            if (isDetailedInteractionEnabled)
            {
                canvas.enabled = true;
                infoPointers[tag] = 0;
                DisplayInfo(tag, infoPointers[tag]);
                UpdateButtonStates(tag);
            }
            else
            {
                if (activePopups.ContainsKey(tag) && activePopups[tag] != null)
                {
                    return; // Popup already exists, do nothing
                }

                Vector3 pos = hitPoint;
                pos.z += 0.1f;
                pos.y += 0.1f;
                GameObject popup = Instantiate(GetPrefab(tag), pos, transform.rotation);
                activePopups[tag] = popup;
            }
        }
        else if (tag.EndsWith("info"))
        {
            Destroy(hitObject);
            activePopups.Remove(tag.Replace("info", ""));
        }
    }

    GameObject GetPrefab(string tag)
    {
        foreach (var infoData in infoDataList)
        {
            if (infoData.tag == tag)
            {
                return infoData.infoPrefab;
            }
        }
        return null;
    }

    void ToggleValueChanged(Toggle change)
    {
        isDetailedInteractionEnabled = change.isOn;
        if (!isDetailedInteractionEnabled)
        {
            canvas.enabled = false;
        }
    }

    void DisplayInfo(string tag, int pointer)
    {
        if (infoTextsDict.ContainsKey(tag) && pointer >= 0 && pointer < infoTextsDict[tag].Count)
        {
            headerText.text = tag.ToUpper();
            infoBox.text = infoTextsDict[tag][pointer];
        }
    }

    public void NextInfo()
    {
        if (isDetailedInteractionEnabled)
        {
            foreach (var tag in infoPointers.Keys)
            {
                if (infoPointers[tag] >= 0)
                {
                    infoPointers[tag]++;
                    if (infoPointers[tag] >= infoTextsDict[tag].Count) infoPointers[tag]--;
                    DisplayInfo(tag, infoPointers[tag]);
                    UpdateButtonStates(tag);
                }
            }
        }
    }

    public void PreviousInfo()
    {
        if (isDetailedInteractionEnabled)
        {
            foreach (var tag in infoPointers.Keys)
            {
                if (infoPointers[tag] > 0)
                {
                    infoPointers[tag]--;
                    DisplayInfo(tag, infoPointers[tag]);
                    UpdateButtonStates(tag);
                }
            }
        }
    }

    void UpdateButtonStates(string tag)
    {
        previousButton.gameObject.SetActive(infoPointers[tag] > 0);
        nextButton.gameObject.SetActive(infoPointers[tag] < infoTextsDict[tag].Count - 1);
    }
}
