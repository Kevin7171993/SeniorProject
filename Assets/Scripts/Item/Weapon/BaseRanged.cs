using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum RangedAnchorPoints //
{
    weaponbase, //0
    Barrel, //1
    Receiver, //2
    Magazine, //3
    StatusMod, //4
    Addon, //5
    count //6
}
public class BaseRanged : Weapon
{
    [SerializeField]
    private GenericBullet[] bulletpool; //bullet pool

    public List<GameObject> mWeaponComponents;
    public List<GameObject> mAnchorPoints;
    public bool infiniteAmmo;
    public bool crafting;

    [SerializeField]
    private List<Color> mainCols, altCols;
    public Color mainCol, altCol;
    public int clip, maxAmmo, altclip, altmaxAmmo;
    public float damage, altdamage, firerateRPM, altfirerateRPM, range, speed;
    public bool gravity, altgravity;
    public AmmoType ammoType, altAmmoType;

    [SerializeField]
    private GameObject bulletTemplate;
    [SerializeField]
    private int nextBullet = 0;
    [SerializeField]
    private float cooldown, altcooldown, firerate, altfirerate;
    private bool statsCalculated = false;
    private bool altfire = false;
    [SerializeField]
    private Transform firePoint;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        //Build ranged weapon base item data
        itemType = ItemType.Weapon;
        weaponType = WeaponType.Ranged;
        itemObject = gameObject;

        //Mark attached components as attached so player cannot pick up the components seperately
        if(mWeaponComponents.Count == 0) { return; }
        for (int i = 1; i < mWeaponComponents.Count; i++) //Don't scan base weapon by skipping i to 1
        {
            mWeaponComponents[i].GetComponent<CompRanged>().attached = true;
        }
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        InitializeAnchorPoints();
        if(mEquipped)
        {
            mVisible = true;
            SetComponentsPosition();
            if(!statsCalculated)
            {
                CalculateStats();
                bulletpool = new GenericBullet[300];

                for (int i = 0; i < bulletpool.Length; i++)
                {
                    bulletpool[i] = Instantiate(bulletTemplate).GetComponent<GenericBullet>();
                }
                statsCalculated = true;
            }
        }
        //Visible?
        if(!mVisible && mWeaponComponents.Count >= 2 && !crafting)
        {
            mMeshR.enabled = false;
            for (int i = 1; i < mWeaponComponents.Count; ++i)
            {
                if(mWeaponComponents[i] == null) { continue; }
                if (mWeaponComponents[i].GetComponent<CompRanged>() != null)
                {
                    mWeaponComponents[i].GetComponent<CompRanged>().SetVisible(false);
                }
            }
        }
        else
        {
            if (mWeaponComponents.Count >= 2)
            {
                mMeshR.enabled = true;
                for (int i = 1; i < mWeaponComponents.Count; ++i)
                {
                    if(mWeaponComponents[i] == null) { continue; }
                    if (mWeaponComponents[i].GetComponent<CompRanged>() != null)
                    {
                        mWeaponComponents[i].GetComponent<CompRanged>().SetVisible(true);
                    }
                }
            }
        }
    }
    public void FixedUpdate()
    {
        if (crafting)
        {
            SetComponentsPosition();
        }
        //Resolve fire rate cooldown
        if (cooldown < firerate)
            cooldown += Time.deltaTime;
        if (altcooldown < altfirerate)
            altcooldown += Time.deltaTime;

        //Listen For Player Input
        if (Input.GetMouseButton(0) && mEquipped) //left click is held
        {
            Attack();
        }
        if (Input.GetMouseButton(1) && mEquipped && mWeaponComponents[(int)RangedComponentType.Addon] != null) //Right click is held
        {
            AltAttack();
        }
        if (nextBullet >= 300)
        {
            nextBullet = 0;
        }
    }
    public override void Attack()
    {
        if(cooldown >= firerate && !stopFire)
        {
            SetStats(bulletpool[nextBullet]); //Set the stats of the bullet to prepare for firing
            //Set bullet spawn location
            bulletpool[nextBullet].transform.position = firePoint.position;
            Camera cam = Camera.main;
            Ray ray = new Ray(cam.transform.position, cam.transform.forward);
            RaycastHit hit;
            Vector3 dest;
            if(Physics.Raycast(ray, out hit, range))
            {
                dest = hit.point;
            }
            else
            {
                dest = cam.transform.forward * range;
            }
            bulletpool[nextBullet].transform.LookAt(dest);
            bulletpool[nextBullet].origin = firePoint.position;
            bulletpool[nextBullet].mActive = true; //Activate the bullet
            ++nextBullet;
            cooldown -= firerate;
        }
    }

    public override void AltAttack()
    {
        if(!altfire) { return; }
        if(cooldown >= altfirerate)
        {
            SetStats(bulletpool[nextBullet], true);
            //do switch case on addon component to determine what kind of alt attack this weapon has

            ++nextBullet;
            cooldown -= firerate;
        }
    }

    public void InitializeAnchorPoints()
    {
        if (!mInitPos && owner != null)
        {
            mAnchorPoints.Add(owner.GetComponent<Inventory>().RangedAnchorPoint);
            mWeaponComponents.Add(transform.parent.gameObject);
            mInitPos = true;
        }
    }
    public void SetComponentsPosition()
    {
        if (mWeaponComponents[0] != null)
        {
            if (mWeaponComponents[0].transform.parent == null)
            {
                mWeaponComponents[0].transform.position = owner.GetComponent<Inventory>().RangedAnchorPoint.transform.position;
                mWeaponComponents[0].transform.rotation = owner.GetComponent<Inventory>().RangedAnchorPoint.transform.rotation;
                mWeaponComponents[0].transform.parent = owner.GetComponent<Inventory>().RangedAnchorPoint.transform;
            }
        }
        for (int i = 1; i < mAnchorPoints.Count; ++i)
        {
            if(mWeaponComponents[i] == null) { continue; }
            if (mWeaponComponents[i].transform.parent == null)
            {
                mWeaponComponents[i].transform.position = mAnchorPoints[i].transform.position;
                mWeaponComponents[i].transform.rotation = mAnchorPoints[i].transform.rotation;
                mWeaponComponents[i].transform.parent = transform;
            }
            if(i == (int)RangedComponentType.Barrel)
            {
                firePoint = mWeaponComponents[i].GetComponent<CompRanged>().fireAnchor;
            }
        }
    }

    public void SetVisible(bool visible)
    {
        for (int i = 1; i < mWeaponComponents.Count; ++i)
        {
            if (mWeaponComponents[i] == null) { continue; }
            if (mWeaponComponents[i].GetComponent<CompRanged>() != null)
            {
                mWeaponComponents[i].GetComponent<CompRanged>().SetVisible(visible);
            }
        }
    }

    public void CalculateStats()
    {
        FlushStats();
        for (int i = 1; i < mWeaponComponents.Count; ++i)
        {
            if(mWeaponComponents[i] == null) { continue; }
            AddStats(mWeaponComponents[i].GetComponent<CompRanged>(), i);
        }
        firerate = (1.0f / (firerateRPM / 60.0f));
        altfirerate = (1.0f / (firerateRPM / 60.0f));
    }
    public void AddStats(CompRanged component, int debugindex = -1)
    {
        if(component == null)
        {
            Debug.Log("Component[" + debugindex + "] is null");
            return;
        }
        if (component.mainCol != Color.clear) mainCols.Add(component.mainCol);
        if (component.altCol != Color.clear) altCols.Add(component.altCol);
        if(mainCols.Count >= 1)
        {
            for (int i = 0; i < mainCols.Count; ++i)
            {
                mainCol += mainCols[i] / mainCols.Count;
            }
        }
        if (altCols.Count >= 1)
        {
            for (int i = 0; i < mainCols.Count; ++i)
            {
                mainCol += mainCols[i] / mainCols.Count;
            }
        }

        clip += component.clip;
        maxAmmo += component.maxAmmo;
        altclip += component.altclip;
        altmaxAmmo += component.altmaxAmmo;
        damage += component.damage;
        altdamage += component.altdamage;
        firerateRPM += component.firerateRPM;
        range += component.range;
        speed += component.speed;
        if (component.gravity) gravity = true;
        if (component.altgravity) altgravity = true;
        if (component.ammoType != AmmoType.none) ammoType = component.ammoType;
        if (component.altAmmoType != AmmoType.none) altAmmoType = component.altAmmoType;
    }
    public void FlushStats()
    {
        mainCols.Clear();
        altCols.Clear();
        mainCol = Color.clear;
        altCol = Color.clear;

        clip = 0;
        maxAmmo = 0;
        altclip = 0;
        altmaxAmmo = 0;
        damage = 0.0f;
        altdamage = 0.0f;
        firerateRPM = 0.0f;
        range = 0.0f;
        speed = 0.0f;

        gravity = false;
        altgravity = false;

        firerate = 0;
        altfirerate = 0;
        altAmmoType = AmmoType.none;
    }

    void SetStats(GenericBullet bullet, bool alt = false)
    {
        bullet.mainCol = mainCol;
        bullet.altCol = altCol;
        if (!alt)
        {
            bullet.damage = damage;
            bullet.gravity = gravity;
            bullet.ammoType = ammoType;
        }
        else
        {
            bullet.damage = altdamage;
            bullet.gravity = gravity;
            bullet.ammoType = altAmmoType;
        }
        bullet.range = range;
        bullet.speed = speed;
        bullet.owners.Add(gameObject);
        bullet.owners.Add(owner);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Camera cam = Camera.main;
        if(cam == null) { return; }
        Ray ray = new Ray(cam.transform.position + (cam.transform.forward * 10.0f), cam.transform.forward);
        RaycastHit hit;
        Physics.Raycast(ray, out hit, range);

        Gizmos.DrawLine(cam.transform.position, hit.point);
    }
}
