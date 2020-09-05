using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyStats stats;
    private MeshRenderer meshR;
    private Collider mCollider;
    public bool isActive = true;
    // Start is called before the first frame update
    void Start()
    {
        meshR = GetComponent<MeshRenderer>();
        mCollider = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (stats.health <= 0.0f)
        {
            Die();
        }

        meshR.enabled = isActive;
        mCollider.enabled = isActive;
    }

    public void Die()
    {
        if (isActive)
        {
            isActive = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {

    }
}
