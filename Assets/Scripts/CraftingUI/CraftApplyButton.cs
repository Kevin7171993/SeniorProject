using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CraftApplyButton : MonoBehaviour
{
    [SerializeField]
    CraftMenu mMenu;
    private Color mColOff;
    [SerializeField]
    private Color mColOn;
    // Start is called before the first frame update
    void Start()
    {
        mColOff = GetComponent<Text>().color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnClick()
    {
        if(Input.GetMouseButtonDown(0))
        {
            CameraManager.ActivateCamera(Cams.MainCam);
            mMenu.CloseMenu();
        }
    }

    public void OnButtonEnter()
    {
        GetComponent<Text>().color = mColOn;
    }

    public void OnButtonExit()
    {
        GetComponent<Text>().color = mColOff;
    }
}
