using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Cams
{
    MainCam,
    CraftCam
}
public class CameraManager : MonoBehaviour
{

    public List<Camera> cams;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < cams.Count; i++)
        {
            if (i == 0) { cams[i].enabled = true; }
            else { cams[i].enabled = false; }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SwitchCamera(Cams camera)
    {
        int x;
        x = (int)camera;
        cams[x].enabled = true;
        for (int i = 0; i < cams.Count; i++)
        {
            if(i != x)
            {
                cams[i].enabled = false;
            }
        }
    }
    public static void ActivateCamera(Cams camera)
    {
        CameraManager mCamManager = GameObject.Find("CameraManager").GetComponent<CameraManager>();
        int x;
        x = (int)camera;
        mCamManager.cams[x].enabled = true;
        for (int i = 0; i < mCamManager.cams.Count; i++)
        {
            if (i != x)
            {
                mCamManager.cams[i].enabled = false;
            }
        }
    }

}
