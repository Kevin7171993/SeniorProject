using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class InventorySlot : MonoBehaviour
{
    public GameObject mIconObject;
    [SerializeField]
    private GameObject mTopTransform;
    private GameObject player;
    protected Image mIcon;
    private GameObject mDragRef;
    [SerializeField]
    protected Inventory mInventory;
    [SerializeField]
    public int index = -1;
    [SerializeField]
    protected Item mItem;
    private Vector3 rPos;
    [SerializeField]
    protected bool iconFollow = false;
    private RectTransform mTrans;
    // Start is called before the first frame update
    virtual public void Start()
    {
        mIcon = mIconObject.GetComponent<Image>();
        mTrans = mIconObject.GetComponent<RectTransform>();
        mIcon.sprite = null;
        rPos = mTrans.position;
        mTopTransform = GameObject.Find("InventoryTop");
        if (mIcon == null)
        {
            Debug.Log("[InventorySlot] mIcon is null");
        }
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    virtual public void Update()
    {
        if (!mInventory.InvGuiActive) { return; }
        Dragging(ref iconFollow);
        if(mIcon.sprite == null)
        {
            //mIcon.color = Color.clear;
            mIcon.enabled = false;
        }
        else if(mInventory.InvGuiActive)
        {
            mIcon.enabled = true;
            //mIcon.color = Color.white;
        }
        if(!iconFollow)
        {
            mIconObject.GetComponent<RectTransform>().localPosition = Vector3.zero;
        }
    }
    public Image GetIcon()
    {
        return mIcon;
    }
    public void SetInventory(Inventory _inv)
    {
        mInventory = _inv;
    }

    public virtual bool AddItem(Item item, bool overwriteslot = false, bool NullItemIfEmpty = false)
    {
        if (item != null)
        {
            if (!overwriteslot && mItem != null) //don't overwrite slot
            {
                return false;
            }
            mItem = item;
            mIcon.sprite = mItem.itemIcon;
            return true;
        }
        if(NullItemIfEmpty)
        {
            mItem = null;
            mIcon.sprite = null;
            return true;
        }
        return false;
    }

    public virtual void SwapSlot(InventorySlot other)
    {
        if (other == null) { Debug.Log("[InventorySlot] Attempted to swap slot with a null slot."); return; }
        Debug.Log("[InventorySlot] Swapping Item");
        Item temp1, temp2;
        temp1 = mItem;
        if (other.mItem != null) //Make sure the other slot have an item to swap
        {
            temp2 = other.mItem;
            mItem = temp2;
            mIcon.sprite = temp2.itemIcon;
        }
        else //If the other slot does not have an item, null this slot and skip to moving this slot's item to the other slot
        {
            mItem = null;
            mIcon.sprite = null;
            mInventory.ReturnFreeSlot(index);
            mInventory.RemoveFreeSlot(other.index);
        }
        other.mItem = temp1;
        other.mIcon.sprite = temp1.itemIcon;
    }

    public void DropItem(float dropItemDistance = 0.75f)
    {
        mItem = null;
        mIcon.sprite = null;
        mItem.mMeshR.enabled = true;
        mItem.transform.position = player.transform.position + player.transform.forward * dropItemDistance;
    }

    public Item GetItem()
    {
        return mItem;
    }
    public void NullItem()
    {
        DeleteItem();
    }
    public void DeleteItem()
    {
        mItem = null;
        mIcon.sprite = null;
    }

    public void SetIndex(int _i)
    {
        index = _i;
    }
    public void ResetPos()
    {
        mTrans.position = rPos;
    }
    public virtual void OnClick()
    {
        //Left Click
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

        //Right Click
        if(Input.GetMouseButtonDown(1))
        {
            if (mInventory.cursorRef2 == null && !iconFollow && Input.GetMouseButtonDown(1))
            {
                CraftMenu c_menu = GameObject.FindGameObjectWithTag("CraftMenu").GetComponent<CraftMenu>();
                if (mItem.itemType == ItemType.Weapon && c_menu.isActive == false)
                {
                    Debug.Log("[InventorySlot] Weapon Right Click Detected, Opening Craft Menu");
                    c_menu.OpenMenu(mItem.GetComponent<BaseRanged>());
                }
                else if (mItem.itemType == ItemType.Weapon && c_menu.isActive == true)
                {
                    Debug.Log("[InventorySlot] Weapon Right Click Detected, Closing Craft Menu");
                    c_menu.CloseMenu();
                }
            }
        }
    }
    private void Dragging(ref bool iconFollow) //helper function to detect if item is being dragged
    {
        if (!mInventory.InvGuiActive) { return; }
        if (iconFollow && Input.GetMouseButton(0)) //is dragging
        {
            mTrans.position = Input.mousePosition;
            mIconObject.transform.SetParent(mTopTransform.transform);
        }
        if(Input.GetMouseButtonUp(0)) //button released from dragging
        {
            mIconObject.transform.SetParent(this.transform);
            ResetPos();
        }
        if (iconFollow && Input.GetMouseButtonUp(0))
        {
            iconFollow = false;
            mTrans.position = rPos;
            if(mInventory.cursorRef2 == null)
            {
                mInventory.cursorRef1 = null;
            }
            else //Do the swap
            {
                if(mInventory.cursorRef2.GetInstanceID() == mInventory.mWeaponSlot.GetInstanceID()) //Weapon slot swap is special
                {
                    mInventory.cursorRef2.SwapSlot(this);
                    return;
                }
                if(mInventory.cursorRef2.GetComponent<TypedSlot>() != null) //TypedSlot is also special
                {
                    mInventory.cursorRef2.SwapSlot(this);
                    return;
                }
                SwapSlot(mInventory.cursorRef2);
            }
        }
    }
    public void OnMouseOver()
    {
        if (!mInventory.InvGuiActive) { return; }
        if (!Input.GetMouseButton(0))
        {
            mInventory.cursorRef1 = this;
        }
        else if(Input.GetMouseButton(0) && mInventory.cursorRef1 != null)
        {
            mInventory.cursorRef2 = this;
        }
        if((mInventory.cursorRef1 == this && mInventory.cursorRef2 == null && !iconFollow) || mInventory.cursorRef2 == this)
        {
            Color _col = new Color(1, 165.0f/255.0f, 0);
            mIcon.color = _col;
        }
        else
        {
            mIcon.color = Color.white;
        }
    }

    public void OnMouseExit()
    {
        if (!mInventory.InvGuiActive) { return; }
        if (!Input.GetMouseButton(0))
        {
            mInventory.cursorRef1 = null;
            mInventory.cursorRef2 = null;
        }
        else
        {
            mInventory.cursorRef2 = null;
        }
        mIcon.color = Color.white;
    }
}
