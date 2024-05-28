using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class touchTest : MonoBehaviour
{
    public GameObject marsinfo;
    public GameObject earthinfo;

    //public TMP_Text infoBox;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Mouse Clicked");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit))
            {
                Debug.Log(hit.transform.tag);
                if(hit.transform.tag == "mars")
                {
                    Vector3 pos = hit.point;
                    pos.z += 0.25f;
                    pos.y += 0.25f;
                    Instantiate(marsinfo, pos, transform.rotation);
                }

                if (hit.transform.tag == "earth2")
                {
                    Vector3 pos = hit.point;
                    pos.z += 0.25f;
                    pos.y += 0.25f;
                    Instantiate(earthinfo, pos, transform.rotation);
                }

                if (hit.transform.tag == "marsInfo")
                {
                    Destroy(hit.transform.gameObject);
                }

                if (hit.transform.tag == "earthInfo")
                {
                    Destroy(hit.transform.gameObject);
                }
            }
        }
        
    }
}
