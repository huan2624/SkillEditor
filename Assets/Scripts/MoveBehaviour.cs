using UnityEngine;
using System.Collections;

public class MoveBehaviour : MonoBehaviour {

    public float speed = 3.0F;
    public float jumpSpeed = 8.0F;
    public float gravity = 20.0F;
    private Vector3 moveDirection = Vector3.zero;

    private BaseActor m_Actor;

    private CharacterController m_Controller;
    private CollisionFlags collisionFlags;

    // Use this for initialization
    void Start () {
        m_Actor = gameObject.GetComponent<BaseActor>();
        m_Controller = GetComponent<CharacterController>();
    }
	
	// Update is called once per frame
	void Update () {
        //if (m_Controller != null && m_Controller.enabled)
        //{
        //    float translation = Input.GetAxis("Vertical") * speed;
        //    float rotation = Input.GetAxis("Horizontal") * speed;
        //    moveDirection = new Vector3(translation, 0, rotation);
        //    //忽略微小操作
        //    if (moveDirection.magnitude < 0.1f)
        //    {
        //        moveDirection = Vector2.zero;
        //    }

        //    if (!m_Controller.isGrounded)
        //    {

        //        moveDirection.y -= gravity * Time.deltaTime;
        //    }
        //    else
        //    {

        //        Debug.Log("isGrounded");
        //    }

        //    if (IsGrounded())
        //    {
        //        Debug.Log("IsGrounded()");
        //    }

        //    Vector3 lookDir = new Vector3(translation, 0, rotation);

        //    if (lookDir != Vector3.zero)
        //    {
        //        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(lookDir), Time.deltaTime * 20);
        //    }
        //    collisionFlags = m_Controller.Move(moveDirection * Time.deltaTime);
        //}

        bool isMoving = UpdateKeyPress();

        if (!isMoving)
        {
            //鼠标左键点击  
            if (Input.GetMouseButtonDown(0))
            {
                //摄像机到点击位置的的射线  
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    //判断点击的是否地形  
                    if (!hit.collider.tag.Equals("Ground"))
                    {
                        return;
                    }
                    //点击位置坐标  
                    Vector3 point = hit.point;

                    m_Actor.MoveTo(point);
                }
            }
            else
            {
                m_Actor.StopJoystick();
            }
        }
    }

    /// <summary>
    /// 是否着地
    /// </summary>
    /// <returns></returns>
    public bool IsGrounded()
    {
        return (collisionFlags & CollisionFlags.CollidedBelow) != 0;
    }

    /* 响应按键操作 */
    bool UpdateKeyPress()
    {
        bool joystick = false;
        Vector3 direction = Vector3.zero;
        if (Input.GetKey(KeyCode.A))
            direction.x = -1;
        if (Input.GetKey(KeyCode.D))
            direction.x = 1;
        if (Input.GetKey(KeyCode.W))
            direction.z = 1;
        if (Input.GetKey(KeyCode.S))
            direction.z = -1;
        if (direction.x != 0 || direction.z != 0)
        {
            Vector3 forward = Camera.main.transform.TransformDirection(Vector3.forward);
            forward.y = 0;
            forward = forward.normalized;
            Vector3 right = new Vector3(forward.z, 0, -forward.x);
            Vector3 currentPoint = transform.position;
            direction = (direction.x * right + direction.z * forward).normalized;
            Vector3 point = transform.position + direction.normalized * 1.0f;
            
            m_Actor.MoveTo(point, true);
            joystick = true;
        }
        return joystick;
    }

    void CalcRaycast(GameObject go, Vector3 validStartPos, ref Vector3 targetPos, bool ajust)
    {
        Vector3 targetPosBackup = targetPos;
        if (ajust)
        {
            //CastOntoGround(ref validStartPos);
            //CastOntoGround(ref targetPos);
        }

        float selfR = 1;
        NavMeshHit hit;
        if (NavMesh.Raycast(
                    validStartPos, targetPos, out hit, 1))
        {
            Vector3 HitPos = hit.position;
            if (ajust && targetPosBackup.y > HitPos.y)
            {
                HitPos.y = targetPosBackup.y;
            }
            Vector3 addrc = HitPos - targetPos;
            targetPos = HitPos + addrc.normalized * selfR;
        }
    }
}
