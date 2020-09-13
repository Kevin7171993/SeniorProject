using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Settings_button : MonoBehaviour
{
    public Color col, hoverCol;
    public bool isOver;
    public SettingsMenu mSettingsMenu;
    public Image mIcon;
    // Start is called before the first frame update
    void Start()
    {
        mIcon = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isOver)
        {
            if (Input.GetMouseButtonDown(0) && !mSettingsMenu.active)
            {
                mSettingsMenu.OpenMenu();
            }
            else if (Input.GetMouseButtonDown(0) && mSettingsMenu.active)
            {
                mSettingsMenu.CloseMenu();
            }
            mIcon.color = hoverCol;
        }
        else
        {
            mIcon.color = col;
        }
    }


    public void OnMouseOver()
    {
        isOver = true;
    }

    public void OnMouseExit()
    {
        isOver = false;
    }

    public void OnMouseClick()
    {
        if (Input.GetMouseButtonDown(0) && !mSettingsMenu.active)
        {
            mSettingsMenu.OpenMenu();
        }
        else if (Input.GetMouseButtonDown(0) && mSettingsMenu.active)
        {
            mSettingsMenu.CloseMenu();
        }
    }
}
