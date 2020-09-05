using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TypedSlot : InventorySlot
{
    public ItemType slotType;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }
    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    public override bool AddItem(Item item, bool overwriteslot = false, bool NullItemIfEmpty = false)
    {
        if (item != null)
        {
            if (!overwriteslot && mItem != null) //don't overwrite slot
            {
                return false;
            }
            if (item.itemType != slotType) //Make sure it's a weapon
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
        if (other == null) { Debug.Log("[TypedSlot] Attempted to swap slot with a different item type."); return; }
        if (other.GetComponent<WeaponSlot>() == null && other.GetComponent<TypedSlot>() == null)
        {
            if (other.GetItem() == null)
            {
                base.SwapSlot(other);
                return;
            }
            else
            {
                if(other.GetItem().itemType != slotType)
                {
                    return;
                }
            }
        } //If moving to generic slot, allow it
        if (other.GetItem() != null)
        {
            if (other.GetItem().itemType != slotType)
            {
                Debug.Log("[TypedSlot] Attempted to swap slot with a non weapon item.");
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
}
