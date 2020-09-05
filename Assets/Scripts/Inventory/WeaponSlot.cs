using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class WeaponSlot : InventorySlot
{

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        if (!mInventory.InvGuiActive) { return; }
        base.Update();
        if(mItem != null)
        {
            mItem.GetComponent<Weapon>().mEquipped = true;
        }
    }
    public override bool AddItem(Item item, bool overwriteslot = false, bool NullItemIfEmpty = false)
    {
        if (item != null)
        {
            if (!overwriteslot && mItem != null) //don't overwrite slot
            {
                return false;
            }
            if(item.GetComponent<Weapon>() == null) //Make sure it's a weapon
            {
                return false;
            }
            mItem = item;
            mIcon.sprite = mItem.itemIcon;
            return true;
        }
        if (NullItemIfEmpty)
        {
            mItem = null;
            mIcon.sprite = null;
            return true;
        }
        return false;
    }
    public override void SwapSlot(InventorySlot other)
    {
        if (other == null) { Debug.Log("[InventorySlot] Attempted to swap slot with a null slot."); return; }
        if (other.GetItem() != null)
        {
            if (other.GetItem().GetComponent<Weapon>() == null)
            {
                Debug.Log("[WeaponSlot] Attempted to swap slot with a non weapon item.");
                return;
            }
        }
        
        Item temp1, temp2;
        temp1 = mItem;
        if (other.GetItem() != null) //Make sure the other slot have an item to swap
        {
            temp2 = other.GetItem();
            AddItem(temp2, true);
            other.NullItem();
            Image _icon = other.GetIcon();
            _icon.sprite = null;
            mInventory.ReturnFreeSlot(other.index);
        }
        else //If the other slot does not have an item, null this slot and skip to moving this slot's item to the other slot
        {
            mItem = null;
            mIcon.sprite = null;
            mInventory.mEquippedWeapon.mEquipped = false;
            mInventory.mEquippedWeapon.mVisible = false;
            mInventory.mEquippedWeapon = null;
        }
        other.AddItem(temp1, true);
    }

    public override void OnClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!mInventory.InvGuiActive) { return; }
            if (iconFollow == false && mItem != null) //If the item is not already being dragged and an item exists
            {
                iconFollow = true;
                mInventory.cursorRef1 = this;
                mIcon.color = Color.white;

            }
        }
    }
}
