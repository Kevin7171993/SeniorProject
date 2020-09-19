using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemComponent : Item
{
    public List<MeshRenderer> mMeshRs;
    public override void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if(transform.GetChild(i) == null)
            {
                return;
            }
            mMeshRs.Add(transform.GetChild(i).GetComponent<MeshRenderer>());
        }
    }
    protected override void OnTriggerEnter(Collider other)
    {
        if (GetComponent<CompRanged>() == null) //Not a Component
        {
            Debug.Log("[ItemComponent] Error: Object is not a component, picking it up as normal item");
            modifiedTriggerEnter(other);
            return;
        }
        if (!GetComponent<CompRanged>().attached)
        {
            modifiedTriggerEnter(other);
        }
    }

    private void modifiedTriggerEnter(Collider other)
    {
        if (mEnabled)
        {
            Inventory inv;
            if (other.GetComponent<Inventory>() != null)
            {
                inv = other.GetComponent<Inventory>();
                inv.AddItem(inv.GetSmallestFreeSlot(), this);
                mEnabled = false;
                for (int i = 0; i < mMeshRs.Count; i++)
                {
                    if (mMeshRs.Count == 0) { break; }
                    mMeshRs[i].enabled = false;
                }
                owner = other.gameObject;
            }
        }
    }
}
