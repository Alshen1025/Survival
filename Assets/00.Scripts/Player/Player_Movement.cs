using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CharacterController))]
public class Player_Movement : Character
{
    public static Player_Movement instance = null;

    //movementSetting
    public float moveSpeed = 5.0f;
    public float gravity = -9.81f;

    //Rotation
    public LayerMask groundLayer;
    public float rotationSpeed = 10.0f;



    private CharacterController controller;
    private Player_FindObject FindObject;

    void Awake()
    {
        if (instance == null) instance = this;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Start()
    {
        base.Start();
        controller = GetComponent<CharacterController>();
        FindObject = GetComponent<Player_FindObject>();

        Delegate_Handler.OnInteraction += ReturnCharacterMove;
        Delegate_Handler.OutInteraction += () => animator.SetBool("Interaction", false);
    }

    public void ReturnCharacterMove()
    {
        animator.SetBool("Interaction", true);
        animator.SetFloat("Speed", 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (FindObject.OnInteraction)
        {
            //!EventSystem.current.IsPointerOverGameObject(0) -> UI클릭시 인터렉션이 종료되지 않음
            if (Input.anyKeyDown && !Input.GetKeyDown(KeyCode.F) && !Canvas_Handler.IsPointerOverUIObject())
            {
                Delegate_Handler.OnEndInteraction();
            }  
            return;
        }
        if (Canvas_Handler.Uis.Count > 0) return;
        Move();
        RotateTowardsMouse();
    }

    void Move()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

       Vector3 cameraForward = Camera.main.transform.forward;
       Vector3 cameraRight = Camera.main.transform.right;

        cameraForward.y = 0.0f;
        cameraRight.y = 0.0f;

        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 moveDirection = cameraRight * horizontal + cameraForward * vertical;

        controller.Move(moveDirection * moveSpeed * Time.deltaTime);

        float currentSpeed = moveDirection.magnitude * moveSpeed;
        animator.SetFloat("Speed", currentSpeed);
    }

    void RotateTowardsMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer))
        {
            Vector3 targetPosition = hit.point;
            Vector3 direction = (targetPosition - transform.position).normalized;
            direction.y = 0.0f;

            if(direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
    }

    

}
