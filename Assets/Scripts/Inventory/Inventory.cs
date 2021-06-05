using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [Header("Inventory")]
    [SerializeField]
    private List<InventorySlot> mInventory;
    public WeaponSlot mWeaponSlot;
    public Weapon mEquippedWeapon;
    public int mInvSize;
    public GameObject RangedAnchorPoint;
    public GameObject MeleeAnchorPoint;
    [SerializeField]
    private List<int> mFreeslots = new List<int>();
    private GameObject owner;
    [SerializeField]
    private Vector3 mousePos;
    public Item empty = new Item();
    public InventorySlot cursorRef1, cursorRef2;
    public bool InvGuiActive = true;
    private bool init = false;
    private Vector3 hideUI = new Vector3(9999, 9999, 9999);
    private Vector3 originPos;
    private RectTransform mTransform;
    // Start is called before the first frame update
    void Start()
    {
        owner = gameObject;
        mWeaponSlot = GameObject.FindGameObjectWithTag("WeaponSlot").GetComponent<WeaponSlot>();
        mWeaponSlot.SetInventory(this);
        Flush();
        mTransform = GameObject.FindGameObjectWithTag("InventoryPanel").GetComponent<RectTransform>();
        originPos = mTransform.position;
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = Input.mousePosition;
        if (mWeaponSlot.GetItem() != null)
        {
            mEquippedWeapon = mWeaponSlot.GetItem().GetComponent<Weapon>();
        }
        if(mEquippedWeapon != null)
        {
            mEquippedWeapon.mEquipped = true;
            mEquippedWeapon.mMeshR.enabled = true;
            mEquippedWeapon.stopFire = InvGuiActive; //If InvGui is active, prevent weapon from firing
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            DebugEquipWeapon();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            DebugPrintItemName();
        }
        if(Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventoryGUI();
        }
    }

    public void Flush() //To do
    {
        GameObject[] _invslots = GameObject.FindGameObjectsWithTag("InventorySlot");
        if(!init)
        {
            foreach (GameObject slot in _invslots)
            {
                slot.GetComponent<InventorySlot>().Start();
            }
            init = true;
        }
        mInventory.Clear();
        mFreeslots.Clear();
        for (int i = 0; i < _invslots.Length; i++)
        {
            mFreeslots.Add(i);
            mInventory.Add(_invslots[i].GetComponent<InventorySlot>());
            mInventory[i].SetInventory(this);
            mInventory[i].DeleteItem();
            mInventory[i].SetIndex(i);
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
    public void RemoveFreeSlot(int slot)
    {
        for (int i = 0; i < mFreeslots.Count; ++i)
        {
            if(mFreeslots[i] == slot)
            {
                mFreeslots[i] = -1;
            }
        }
    }
    public void AddItem(int _freeslot, Item _item, bool overwrite = false)
    {
        mInventory[_freeslot].AddItem(_item, overwrite);
    }

    public void AddItemToSmallestSlot(Item _item)
    {
        AddItem(GetSmallestFreeSlot(), _item);
    }

    public void DropItem(int _slot)
    {
        //Drop Item Code here
        mInventory[_slot].DropItem();
    }

    public void DeleteItem(int _slot)
    {
        Destroy(mInventory[_slot].GetItem().gameObject);
        mInventory[_slot].DeleteItem();
        ReturnFreeSlot(_slot);
    }

    public GameObject GetOwner()
    {
        return gameObject;
    }

    public void ToggleInventoryGUI()
    {
        if(InvGuiActive == false)
        {
            InvGuiActive = true;
            //Turn on Gui
            mTransform.position = originPos;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            InvGuiActive = false;
            //Turn off Gui
            mTransform.position = hideUI;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public bool isWeapon(Item _item)
    {
        if(_item.gameObject.GetComponent<Weapon>() != null)
        {
            return true;
        }
        return false;
    }
//#if DEBUG
    public void DebugEquipWeapon()
    {
        for (int i = 0; i < mInventory.Count; i++)
        {
            if(mInventory[i].GetItem().itemType == ItemType.Weapon)
            {
                //mEquippedWeapon = mInventory[i].mItem.itemObject.GetComponent<Weapon>();
                mWeaponSlot.AddItem(mInventory[i].GetItem());
                //mEquippedWeapon.Init(); //Check if it's a ranged weapon or a melee weapon, then behave accordingly
                mInventory[i].DeleteItem();
                ReturnFreeSlot(i);
                return;
            }
        }
    }

    public void DebugPrintItemName()
    {
        for (int i = 0; i < mInventory.Count; i++)
        {
            Debug.Log(mInventory[i].GetItem().itemName);
        }
    }

}
