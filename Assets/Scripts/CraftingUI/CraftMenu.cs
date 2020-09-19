using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftMenu : GenericUI
{
    public bool isActive = false;
    [SerializeField]
    private BaseRanged mTargetWeapon;
    private BaseRanged mPrevWeapon;
    [SerializeField]
    private List<TypedSlot> mComponentsSlots;
    private RectTransform mTransform;
    private Vector3 origin;
    //private Vector3 hideUI = new Vector3(9999, 9999, 9999);
    [SerializeField]
    private Inventory mInventory;
    // Start is called before the first frame update
    public override void Start()
    {
        mTransform = GetComponent<RectTransform>();
        origin = mTransform.position;
        CloseMenu();
    }

    // Update is called once per frame
    public override void Update()
    {
        if(!mInventory.InvGuiActive && isActive)
        {
            CloseMenu();
        }
    }
    public void OpenMenu(BaseRanged _weapon)
    {
        Flush();
        ParseWeapon(_weapon);
        isActive = true;
        mTransform.position = origin;
    }
    public override void CloseMenu()
    {
        Apply();
        if (mPrevWeapon != null)
        {
            mPrevWeapon.crafting = false;
        }
        mTransform.position = hideUI;
    }
    public void Flush()
    {
        FlushComponents();
        FlushWeapon();
    }
    public void FlushComponents()
    {
        for (int i = 0; i < mComponentsSlots.Count; i++)
        {
            mComponentsSlots[i].NullItem();
        }
    }
    public void FlushWeapon()
    {
        mPrevWeapon = mTargetWeapon;
        mTargetWeapon = null;
    }

    public void ParseWeapon(BaseRanged _weapon)
    {
        mTargetWeapon = _weapon;
        for (int i = 1; i < _weapon.mWeaponComponents.Count; ++i)
        {
            if(i-1 >= mComponentsSlots.Count) { continue; }
            if(_weapon.mWeaponComponents[i] == null)
            {
                mComponentsSlots[i - 1].NullItem();
                continue;
            }
            mComponentsSlots[i - 1].AddItem(_weapon.mWeaponComponents[i].GetComponent<Item>(), true, true);
            mComponentsSlots[i - 1].mIconObject.GetComponent<RectTransform>().localPosition = Vector3.zero;
        }
    }
    public void Apply() //Apply components and stats to weapon based on the menu
    {
        if (mTargetWeapon == null) { return; }
        //Apply modification
        for (int i = 0; i < mComponentsSlots.Count; i++)
        {
            if(mComponentsSlots[i].GetItem() == null)
            {
                mTargetWeapon.mWeaponComponents[i + 1] = null;
                continue;
            }
            mTargetWeapon.mWeaponComponents[i + 1] = mComponentsSlots[i].GetItem().gameObject;
        }
        mTargetWeapon.CalculateStats();
        //Flush the Crafting Menu
        Flush();
        isActive = false;
    }

    public void Refresh()
    {
        Apply();
        ParseWeapon(mPrevWeapon);
    }
}
