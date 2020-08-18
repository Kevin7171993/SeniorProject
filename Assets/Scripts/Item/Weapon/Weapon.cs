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
    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        //Update shit
        Attack();
        AltAttack();
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
        base.OnTriggerEnter(other);
        //Unique Weapon Code below
    }
}
