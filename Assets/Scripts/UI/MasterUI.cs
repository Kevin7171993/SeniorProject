using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MasterUI : MonoBehaviour
{
    public List<GameObject> UIs;
    [SerializeField]
    private List<Image> PanelArts;
    public Color UI_Color;
    private Vector3 hideUI = new Vector3(9999, 9999, 9999);
//    private bool init = false;
    // Start is called before the first frame update
    void Start()
    {
        //Register All panels into PanelArts
        for (int i = 0; i < UIs.Count; i++)
        {
            for(int j = 0; j < UIs[i].transform.childCount; ++j)
            {
                var obj = UIs[i].transform.GetChild(j).gameObject;
                if (obj.name == "PanelArt" && obj.GetComponent<Image>() != null)
                {
                    PanelArts.Add(obj.GetComponent<Image>());
                    break;
                }
            }
        }
        UI_UpdatePanelColor();
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if(!init)
        {
            UI_Color = GameObject.Find("SettingsUI").GetComponent<SettingsMenu>().GetColor();
            UI_UpdatePanelColor();
            init = true;
        }
        */
    }

    public void UI_UpdatePanelColor()
    {
        for (int i = 0; i < PanelArts.Count; ++i)
        {
            PanelArts[i].color = UI_Color;
        }
    }
}


public class GenericUI : MonoBehaviour
{
    [SerializeField]
    public Vector3 hideUI = new Vector3(-9999, -9999, -9999);
    public virtual void Start()
    {

    }
    public virtual void Update()
    {

    }
    public virtual void OpenMenu()
    {

    }

    public virtual void CloseMenu()
    {

    }
}