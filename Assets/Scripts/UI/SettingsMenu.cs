using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenu : GenericUI
{
    private Color panelColor;
    private MasterUI master;
    public bool active = false;
    private bool init = false;
    [SerializeField]
    private Slider_Horizontal sRed, sGreen, sBlue, sAlpha;
    private Vector3 origin;
    // Start is called before the first frame update
    public override void Start()
    {
        master = transform.parent.GetComponent<MasterUI>();
        origin = GetComponent<RectTransform>().position;
        CloseMenu();
    }

    // Update is called once per frame
    public override void Update()
    {
        if(active)
        {
            panelColor = new Color(sRed.currentValue, sGreen.currentValue, sBlue.currentValue, sAlpha.currentValue);
            master.UI_Color = panelColor;
            master.UI_UpdatePanelColor();
        }
        if(!init)
        {
            panelColor = new Color(sRed.currentValue, sGreen.currentValue, sBlue.currentValue, sAlpha.currentValue);
            master.UI_Color = panelColor;
            master.UI_UpdatePanelColor();
            init = true;
        }
    }

    public Color GetColor()
    {
        panelColor = new Color(sRed.currentValue, sGreen.currentValue, sBlue.currentValue, sAlpha.currentValue);
        return panelColor;
    }
    public override void OpenMenu()
    {
        active = true;
        GetComponent<RectTransform>().position = origin;
    }
    public override void CloseMenu()
    {
        active = false;
        GetComponent<RectTransform>().position = hideUI;
    }
}
