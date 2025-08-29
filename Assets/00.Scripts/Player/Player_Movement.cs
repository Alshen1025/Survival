using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Player_Movement : MonoBehaviour
{
    public static Player_Movement instance = null;

    //movementSetting
    public float moveSpeed = 5.0f;
    public float gravity = -9.81f;

    //Rotation
    public LayerMask groundLayer;
    public float rotationSpeed = 10.0f;


    [SerializeField] private GameObject[] Equipments;


    private CharacterController controller;
    private Animator animator;
    private Player_FindObject FindObject;

    void Awake()
    {
        if (instance == null) instance = this;
    }

    public void AnimationChange(string temp)
    {
        animator.SetTrigger(temp);
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        FindObject = GetComponent<Player_FindObject>();

        Delegate_Handler.OnInteraction += () =>
        {
            animator.SetBool("Interaction", true);
            animator.SetFloat("Speed", 0.0f);

        };
        Delegate_Handler.OutInteraction += () => animator.SetBool("Interaction", false);
    }

    // Update is called once per frame
    void Update()
    {
        if (FindObject.OnInteraction)
        {
            if (Input.anyKeyDown && !Input.GetKeyDown(KeyCode.F))
            {
                Delegate_Handler.OnEndInteraction();
            }
            return;
        }
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

    public void ChangeEquipment(Object_Type type, bool active)
    {
        Equipments[(int)type].SetActive(active);
    }

    public void DeactiveEquipment()
    {
        for(int i = 0; i< Equipments.Length; i++)
        {
            Equipments[i].SetActive(false);
        }
    }

}
