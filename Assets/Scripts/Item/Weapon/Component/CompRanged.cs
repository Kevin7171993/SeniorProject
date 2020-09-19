using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RangedComponentType
{
    none, //0
    Barrel, //1
    Receiver, //2
    Magazine, //3
    StatusMod, //4
    Addon, //5
    count //6
}
public enum AmmoType
{
    none,
    basic,
    grenade,
    count
}
public class CompRanged : MonoBehaviour
{
    public Color mainCol, altCol;
    public int clip, maxAmmo, altclip, altmaxAmmo;
    public float damage, altdamage, firerateRPM, range, speed;
    public bool gravity, altgravity;
    public RangedComponentType compType;
    public AmmoType ammoType, altAmmoType;
    public List<MeshRenderer> mMeshR;
    public Transform fireAnchor;
    public bool attached = false;
    [SerializeField]
    private Transform anchor;
    // Start is called before the first frame update
    public virtual void Start()
    {
    }

    // Update is called once per frame
    public virtual void Update()
    {
    }

    public void SetVisible(bool visible)
    {
        for (int i = 0; i < mMeshR.Count; ++i)
        {
            mMeshR[i].enabled = visible;
        }
    }
}
