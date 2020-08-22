using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum RangedAnchorPoints //
{
    weaponbase, //0
    Barrel, //1
    Receiver, //2
    Magazine, //3
    StatusMod, //4
    Addon, //5
    count //6
}
public class BaseRanged : Weapon
{
    public List<GameObject> mWeaponComponents;
    public List<GameObject> mAnchorPoints;
    public bool infiniteAmmo;

    [SerializeField]
    private List<Color> mainCols, altCols;
    [SerializeField]
    private Color mainCol, altCol;
    [SerializeField]
    private int clip, maxAmmo, altclip, altmaxAmmo;
    [SerializeField]
    private float damage, altdamage, firerateRPM, range;
    [SerializeField]
    private bool gravity, altgravity;
    [SerializeField]
    public AmmoType ammoType, altAmmoType;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        //Build ranged weapon base item data
        itemType = ItemType.Weapon;
        weaponType = WeaponType.Ranged;
        itemObject = gameObject;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        InitializeAnchorPoints();
        if(mEquipped)
        {
            mVisible = true;
            SetComponentsPosition();
        }

        //Visible?
        if(!mVisible && mWeaponComponents.Count >= 2)
        {
            mMeshR.enabled = false;
            for (int i = 1; i < mWeaponComponents.Count; ++i)
            {
                if (mWeaponComponents[i].GetComponent<CompRanged>() != null)
                {
                    mWeaponComponents[i].GetComponent<CompRanged>().SetVisible(false);
                }
            }
        }
        else
        {
            if (mWeaponComponents.Count >= 2)
            {
                mMeshR.enabled = true;
                for (int i = 1; i < mWeaponComponents.Count; ++i)
                {
                    if (mWeaponComponents[i].GetComponent<CompRanged>() != null)
                    {
                        mWeaponComponents[i].GetComponent<CompRanged>().SetVisible(true);
                    }
                }
            }
        }
    }

    public void InitializeAnchorPoints()
    {
        if (!mInitPos && owner != null)
        {
            mAnchorPoints.Add(owner.GetComponent<Inventory>().RangedAnchorPoint);
            mWeaponComponents.Add(transform.parent.gameObject);
            mInitPos = true;
        }
    }
    public void SetComponentsPosition()
    {
        mWeaponComponents[1].transform.position = owner.GetComponent<Inventory>().RangedAnchorPoint.transform.position;
        mWeaponComponents[1].transform.rotation = owner.GetComponent<Inventory>().RangedAnchorPoint.transform.rotation;
        for (int i = 1; i < mAnchorPoints.Count; ++i)
        {
            mWeaponComponents[i].transform.position = mAnchorPoints[i].transform.position;
            mWeaponComponents[i].transform.rotation = mAnchorPoints[i].transform.rotation;
        }
    }

    public void CaculateStats()
    {
        FlushStats();
        for (int i = 1; i < mWeaponComponents.Count; ++i)
        {
            AddStats(mWeaponComponents[i].GetComponent<CompRanged>(), i);
        }
    }

    public void AddStats(CompRanged component, int debugindex = -1)
    {
        if(component == null)
        {
            Debug.Log("Component[" + debugindex + "] is null");
            return;
        }
        if (component.mainCol != Color.clear) mainCols.Add(component.mainCol);
        if (component.altCol != Color.clear) altCols.Add(component.altCol);
        if(mainCols.Count >= 1)
        {
            for (int i = 0; i < mainCols.Count; ++i)
            {
                mainCol += mainCols[i] / mainCols.Count;
            }
        }
        if (altCols.Count >= 1)
        {
            for (int i = 0; i < mainCols.Count; ++i)
            {
                mainCol += mainCols[i] / mainCols.Count;
            }
        }

        clip += component.clip;
        maxAmmo += component.maxAmmo;
        altclip += component.altclip;
        altmaxAmmo += component.altmaxAmmo;
        damage += component.damage;
        altdamage += component.damage;
        firerateRPM += component.firerateRPM;
        range += range;
        if (component.gravity) gravity = true;
        if (component.altgravity) altgravity = true;
        if (component.ammoType != AmmoType.none) ammoType = component.ammoType;
        if (component.altAmmoType != AmmoType.none) altAmmoType = component.altAmmoType;
    }
    public void FlushStats()
    {
        mainCols.Clear();
        altCols.Clear();
        mainCol = Color.clear;
        altCol = Color.clear;

        clip = 0;
        maxAmmo = 0;
        altclip = 0;
        altmaxAmmo = 0;
        damage = 0.0f;
        altdamage = 0.0f;
        firerateRPM = 0.0f;
        range = 0.0f;

        gravity = false;
        altgravity = false;
    }
}
