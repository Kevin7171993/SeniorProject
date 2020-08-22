using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericBullet : MonoBehaviour
{
    public Color mainCol, altCol;
    public float damage, firerateRPM, range, speed;
    public bool gravity;
    public AmmoType ammoType;
    public MeshRenderer mMeshR;
    public Rigidbody mRb;
    public List<GameObject> owners;
    public bool mActive = false;
    public Vector3 origin;
    // Start is called before the first frame update
    public virtual void Start()
    {
        mMeshR = GetComponent<MeshRenderer>();
        mRb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    public virtual void Update()
    {
        mMeshR.enabled = mActive;
        if (!mActive) { return; }
        transform.position += (transform.forward * speed) * Time.deltaTime;
        if(Vector3.Distance(origin, transform.position) >= range)
        {
            mActive = false;
        }
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        if(!mActive) { return; }
        bool hitowner = false;
        for (int i = 0; i < owners.Count; ++i)
        {
            if(owners[i] == other.gameObject)
            {
                hitowner = true;
            }
        }
        if(!hitowner)
        {
            CharacterStats hit = other.gameObject.GetComponent<CharacterStats>();
            if(hit != null)
            {
                hit.health -= damage;
            }
            mActive = false;
        }
    }
}
