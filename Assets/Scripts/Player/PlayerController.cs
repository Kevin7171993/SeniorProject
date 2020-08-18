using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Cinemachine.CinemachineFreeLook mPlayerCam;
    public Rigidbody mRigidbody;
    public float mMaxSpeed, mDecceleration, mAcceleration;
    
    private Vector3 camForward, camRight, moveBuffer;
    public Vector3 mVelocity;
    public float mSpeed;
    public float debugVelocity;
    private bool moving;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        moving = false;
        moveBuffer = Vector3.zero;
        camForward = mPlayerCam.gameObject.transform.position - transform.position;
        camForward.y = 0.0f;
        camRight = Vector3.Cross(camForward.normalized, transform.up);

        if(Input.GetKey(KeyCode.W))
        {
            moveBuffer += -camForward.normalized;
            moving = true;
        }
        if (Input.GetKey(KeyCode.S))
        {
            moveBuffer += camForward.normalized;
            moving = true;
        }
        if (Input.GetKey(KeyCode.A))
        {
            moveBuffer += -camRight;
            moving = true;
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveBuffer += camRight;
            moving = true;
        }
        if(moveBuffer.magnitude > 0.0f)
        {
            moving = true;
            if (mSpeed < mMaxSpeed)
            {
                mSpeed += mAcceleration * Time.deltaTime;
            }
        }
        else
        {
            moving = false;
            if(mSpeed > 0.0f)
            {
                mSpeed -= mDecceleration;
            }
            else
            {
                mSpeed = 0.0f;
            }
        }
        mVelocity = moveBuffer.normalized * mMaxSpeed * Time.deltaTime;
        mRigidbody.MovePosition(transform.position + mVelocity);
    }
}
