using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum RangedAnchorPoints //
{
    Barrel, //1
    Receiver, //2
    Magazine, //3
    Silencer, //4
    count //5
}
public class BaseRanged : Weapon
{
    public List<CompRanged> mWeaponComponents;
    public List<Vector3> mAnchorPoints;
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
    void Update()
    {
        
    }

    public void InitializeAnchorPoints()
    {

    }
    public void SetComponentsPosition()
    {
        
    }
}
