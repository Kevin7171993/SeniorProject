using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum WeaponType
{
    Melee,
    Ranged
}
public class Weapon : Item
{
    protected WeaponType weaponType;
    [SerializeField]
    public bool mEquipped = false;
    public bool mVisible = true;
    protected bool mInitPos = false;
    public bool stopFire = false;
    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    public virtual void Init()
    {
        
    }

    public virtual void Attack()
    {

    }

    public virtual void AltAttack()
    {

    }

    protected override void OnTriggerEnter(Collider other)
    {
        //Unique Weapon Code below
        if (!mEquipped)
        {
            base.OnTriggerEnter(other);
            mVisible = false;
        }
    }
}
