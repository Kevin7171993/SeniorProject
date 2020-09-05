using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CharacterController controller;
    public Cinemachine.CinemachineFreeLook mCinemachine;
    public Rigidbody rb;
    public Transform cam;
    public Animator anim;

    public float mSpeed;
    public float FallMoveSpeed = 2.0f; //How much the player can move the character when falling
    public float turnSmoothTime = 0.0f;
    float turnSmoothVelocity;
    public bool moving;

    private float camSpeedX, camSpeedY;
    [SerializeField]
    float vertical, horizontal;
    // Start is called before the first frame update
    void Start()
    {
        camSpeedX = mCinemachine.m_XAxis.m_MaxSpeed;
        camSpeedY = mCinemachine.m_YAxis.m_MaxSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        anim.SetInteger("Forward", (int)vertical);
        anim.SetInteger("Right", (int)horizontal);

        //Character Movement Direction
        Vector3 direction = new Vector3(horizontal, 0.0f, vertical).normalized;

        //Character Look Direction
        if(vertical < 0.0f)
        {
            vertical = 1.0f;
        }
        Vector3 lookdir = new Vector3(0.0f, 0.0f, vertical).normalized;
        float lookAngle = Mathf.Atan2(lookdir.x, lookdir.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, lookAngle, ref turnSmoothVelocity, turnSmoothTime);
        transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);

        moving = false;
        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            if (controller.isGrounded)
            {
                controller.Move(moveDir.normalized * mSpeed * Time.deltaTime);
            }
            else
            {
                controller.Move(moveDir.normalized * (mSpeed / FallMoveSpeed) * Time.deltaTime);
            }

            moving = true;
        }

        //Let unity apply gravity
        controller.SimpleMove(Vector3.zero);

        //UI
        UIState();
    }

    public void UIState()
    {
        if(GetComponent<Inventory>().InvGuiActive)
        {
            mCinemachine.m_XAxis.m_MaxSpeed = 0.0f;
            mCinemachine.m_YAxis.m_MaxSpeed = 0.0f;
        }
        else
        {
            mCinemachine.m_XAxis.m_MaxSpeed = camSpeedX;
            mCinemachine.m_YAxis.m_MaxSpeed = camSpeedY;
        }
    }
}
