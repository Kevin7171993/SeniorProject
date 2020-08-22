using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [Header("Inventory")]
    [SerializeField]
    private List<Item> mInventory;
    public Weapon mEquippedWeapon;
    public int mInvSize;
    public GameObject RangedAnchorPoint;
    public GameObject MeleeAnchorPoint;
    private List<int> mFreeslots = new List<int>();
    private GameObject owner;
    private Item empty = new Item();
    // Start is called before the first frame update
    void Start()
    {
        owner = gameObject;
        Flush();
    }

    // Update is called once per frame
    void Update()
    {
        if(mEquippedWeapon != null)
        {
            mEquippedWeapon.mEquipped = true;
            mEquippedWeapon.mMeshR.enabled = true;
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            DebugEquipWeapon();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            DebugPrintItemName();
        }
    }

    public void Flush()
    {
        mInventory.Clear();
        mFreeslots.Clear();
        mInventory.Capacity = mInvSize;
        for (int i = 0; i < mInvSize; i++)
        {
            mFreeslots.Add(i);
            mInventory.Add(empty);
        }
    }

    public int GetSmallestFreeSlot()
    {
        if(mFreeslots.Count <= 0)
        {
            Debug.Log("[Inventory] No Free Slot");
            return -1;
        }
        int index = -1;
        int rIndex = -1;
        for (int i = 0; i < mFreeslots.Count; ++i)
        {
            if(mFreeslots[i] != -1)
            {
                if (index == -1)
                {
                    index = mFreeslots[i];
                    rIndex = i;
                }
                else
                {
                    if(index > mFreeslots[i])
                    {
                        index = mFreeslots[i];
                        rIndex = i;
                    }
                }
            }
        }
        mFreeslots[rIndex] = -1;
        return index;
    }
    public void ReturnFreeSlot(int slot)
    {
        for (int i = 0; i < mFreeslots.Count; ++i)
        {
            if (mFreeslots[i] == -1)
            {
                mFreeslots[i] = slot;
                return;
            }
        }
    }

    public void AddItem(int _freeslot, Item _item)
    {
        mInventory[_freeslot] = _item;
    }

    public void AddItemToSmallestSlot(Item _item)
    {
        AddItem(GetSmallestFreeSlot(), _item);
    }

    public void DropItem(int _slot)
    {
        //Drop Item Code here
    }

    public void DeleteItem(int _slot)
    {
        Destroy(mInventory[_slot].gameObject);
        mInventory[_slot] = empty;
        ReturnFreeSlot(_slot);
    }

    public GameObject GetOwner()
    {
        return gameObject;
    }
//#if DEBUG
    public void DebugEquipWeapon()
    {
        for (int i = 0; i < mInventory.Count; i++)
        {
            if(mInventory[i].itemType == ItemType.Weapon)
            {
                mEquippedWeapon = mInventory[i].itemObject.GetComponent<Weapon>();
                mEquippedWeapon.Init(); //Check if it's a ranged weapon or a melee weapon, then behave accordingly
                mInventory[i] = empty;
                ReturnFreeSlot(i);
                return;
            }
        }
    }

    public void DebugPrintItemName()
    {
        for (int i = 0; i < mInventory.Count; i++)
        {
            Debug.Log(mInventory[i].itemName);
        }
    }

}
