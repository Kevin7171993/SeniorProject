using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Weapon,
    Barrel,
    Receiver,
    Magazine,
    StatusMod,
    Addon,
    Generic
}

public class Item : MonoBehaviour
{
    public string itemName;
    public int ID;
    public ItemType itemType;
    public GameObject itemObject;
    public Sprite itemIcon;

    public bool mEnabled = true;
    public MeshRenderer mMeshR;

    protected GameObject owner;
    //Getters

    public virtual void Start()
    {
        mMeshR = GetComponent<MeshRenderer>();
    }
    public virtual void Update()
    {

    }

    public GameObject GetOwner() => owner;
    public string GetItemName() => itemName;
    public int GetItemID() => ID;
    public ItemType GetItemType() => itemType;

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (mEnabled)
        {
            Inventory inv;
            if (other.GetComponent<Inventory>() != null)
            {
                inv = other.GetComponent<Inventory>();
                inv.AddItem(inv.GetSmallestFreeSlot(), this);
                mEnabled = false;
                mMeshR.enabled = false;
                owner = other.gameObject;
            }
        }
    }
}
