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
    private Collider mCollider;
    [SerializeField]
    private Light mLight;
    [SerializeField]
    private Material mMat;
    // Start is called before the first frame update
    public virtual void Start()
    {
        mMeshR = GetComponent<MeshRenderer>();
        mRb = GetComponent<Rigidbody>();
        mCollider = GetComponent<Collider>();
    }

    // Update is called once per frame
    public virtual void Update()
    {
        mMeshR.enabled = mActive;
        mCollider.enabled = mActive;
        mLight.enabled = mActive;
        if (!mActive) { mLight.enabled = false; return; }
        transform.position += (transform.forward * speed) * Time.deltaTime;
        mLight.color = mainCol;
        mMat.color = mainCol;
        mMat.SetColor("_EmissionColor", mainCol);
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
            if(other.gameObject.GetComponent<GenericBullet>() != null)
            {
                hitowner = false;
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
