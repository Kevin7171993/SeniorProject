using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvTopScript : MonoBehaviour
{
    private GameObject mObj;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.childCount > 0)
        {
            mObj = transform.GetChild(0).gameObject;
            mObj.layer = 2;
        }
    }
}
