using UnityEngine;
using UnityEngine.UI;

public class InteractionManager : MonoBehaviour
{
    public Toggle interactionToggle;
    public static bool isDetailedInteractionEnabled;

    void Start()
    {
        if (interactionToggle != null)
        {
            interactionToggle.onValueChanged.AddListener(delegate { ToggleValueChanged(interactionToggle); });
        }
    }

    void ToggleValueChanged(Toggle change)
    {
        isDetailedInteractionEnabled = change.isOn;
    }
}
