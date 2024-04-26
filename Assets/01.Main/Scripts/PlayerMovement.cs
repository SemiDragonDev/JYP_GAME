using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("플레이어 움직임")]
    [SerializeField]
    private float moveSpeed = 2f;
    [SerializeField]
    private float sprintSpeed = 5.335f;
    [SerializeField]
    [Range(0f, 0.3f)]
    private float rotationSmoothTime = 0.12f;
    [SerializeField]
    private float speedChangeRate = 10f;

    [Space(10)]
    [SerializeField]
    private float gravity = -15f;
    [SerializeField]
    private float jumpHeight = 1.2f;
    [SerializeField]
    private float jumpTimeout = 0.5f;
    [SerializeField]
    private float fallTimeout = 0.15f;

    [Header("그라운드 체크")]
    [SerializeField]
    private Transform groundCheck;
    [SerializeField]
    private LayerMask groundMask;
    private float groundedOffset = -0.14f;
    private float groundedRadius = 0.28f;
    private bool isGrounded = true;

    [Header("Cinemachine")]
    [SerializeField]
    private GameObject cinemachineCameraTarget;
    [SerializeField]
    private float topClamp = 70f;
    [SerializeField]
    private float bottomClamp = -30f;
    private float cameraAngleOverride = 0f;
    private bool lockCameraPosition = false;

    [Header("KnockBack")]
    public Vector3 startPos;
    public Vector3 endPos;

    // cinemachine
    private float cinemachineTargetYaw;
    private float cinemachineTargetPitch;

    // player
    private float speed;
    private float animationBlend;
    private float targetRotation = 0f;
    private float rotationVelocity;
    private float verticalVelocity;
    private float terminalVelocity = 53f;
    private bool isSprint = false;

    // timeout deltatime
    private float jumpTimeoutDelta;
    private float fallTimeoutDelta;

    // animation IDs
    private int animIDSpeed;
    private int animIDGrounded;
    private int animIDJump;
    private int animIDFreeFall;
    private int animIDMotionSpeed;
    private int animIDAttack;

    private Vector3 velocity;
    private CharacterController charController;
    private Animator animator;
    private GameObject mainCamera;
    private SwitchFPandTP switchFPandTP;

    [Space(10)]
    [Header("Interactor")]
    [SerializeField]
    private Interactor interactor;
    [SerializeField]
    private TargetAttack targetAttack;

    private const float threshold = 0.01f;

    private bool hasAnimator;

    private void Awake()
    {
        if(mainCamera == null)
        {
            mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            switchFPandTP = mainCamera.GetComponent<SwitchFPandTP>();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        cinemachineTargetYaw = cinemachineCameraTarget.transform.rotation.eulerAngles.y;

        hasAnimator = TryGetComponent(out animator);
        charController = GetComponent<CharacterController>();

        AssignAnimationIDs();

        jumpTimeoutDelta = jumpTimeout;
        fallTimeoutDelta = fallTimeout;
    }

    // Update is called once per frame
    void Update()
    {
        hasAnimator = TryGetComponent(out animator);

        JumpAndGravity();
        GroundedCheck();
        Move();
        AnimAttackPlay();
    }

    private void LateUpdate()
    {
        CameraRotation();
    }

    private void AssignAnimationIDs()
    {
        animIDSpeed = Animator.StringToHash("Speed");
        animIDGrounded = Animator.StringToHash("Grounded");
        animIDJump = Animator.StringToHash("Jump");
        animIDFreeFall = Animator.StringToHash("FreeFall");
        animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
        animIDAttack = Animator.StringToHash("Attack");
    }

    private void GroundedCheck()
    {
        Vector3 spherePosition = new Vector3(groundCheck.position.x, groundCheck.position.y - groundedOffset, groundCheck.position.z);
        isGrounded = Physics.CheckSphere(spherePosition, groundedRadius, groundMask, QueryTriggerInteraction.Ignore);

        if(hasAnimator)
        {
            animator.SetBool(animIDGrounded, isGrounded);
        }
    }

    private void CameraRotation()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        if ((mouseX >= threshold || mouseY >= threshold || mouseX <= -threshold || mouseY <= -threshold) && (!lockCameraPosition))
        {
            cinemachineTargetYaw += mouseX;
            cinemachineTargetPitch -= mouseY;
        }

        cinemachineTargetYaw = ClampAngle(cinemachineTargetYaw, float.MinValue, float.MaxValue);
        cinemachineTargetPitch = ClampAngle(cinemachineTargetPitch, bottomClamp, topClamp);

        cinemachineCameraTarget.transform.rotation = Quaternion.Euler(cinemachineTargetPitch + cameraAngleOverride, cinemachineTargetYaw, 0f);
    }

    private void Move()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            isSprint = true;
        }
        else isSprint = false;

        float targetSpeed = isSprint ? sprintSpeed : moveSpeed;

        var moveInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        var dir = moveInput.normalized;

        if (moveInput == Vector3.zero) targetSpeed = 0f;

        //Debug.Log("Target Speed : " + targetSpeed);

        float currentHorizontalSpeed = new Vector3(charController.velocity.x, 0f, charController.velocity.z).magnitude;

        float speedOffset = 0.1f;

        if(currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
        {
            speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed, Time.deltaTime * speedChangeRate);

            speed = Mathf.Round(speed * 1000f) / 1000f;
        }
        else
        {
            speed = targetSpeed;
        }

        //Debug.Log("Current Speed : " + speed);

        animationBlend = Mathf.Lerp(animationBlend, targetSpeed, Time.deltaTime * speedChangeRate);
        if (animationBlend < 0.01f) animationBlend = 0f;

        if (moveInput != Vector3.zero)
        {
            // WASD 입력에 의한 캐릭터의 몸 회전 + 카메라의 좌우 입력에 대한 캐릭터의 몸 회전
            // (if문 조건으로 인해, 이동 입력값이 있을때만 카메라의 회전에 따른 몸 회전이 적용)
            // (3인칭 카메라의 경우, 가만히 서있을 때는 카메라를 좌우로 움직여도 몸이 회전하지 않음)
            // (1인칭 카메라에는 따로 RotatePlayerBody 클래스를 사용해 카메라의 움직임만으로 몸이 회전하게 함)
            // tan(x/z) 의 Radian 값을 Degree 값으로 변환 + 카메라의 y축 회전
            
            targetRotation = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg + mainCamera.transform.eulerAngles.y;
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref rotationVelocity, rotationSmoothTime);

            transform.rotation = Quaternion.Euler(0f, rotation, 0f);
        }

        Vector3 targetDirection = Quaternion.Euler(0f, targetRotation, 0f) * Vector3.forward;

        charController.Move(targetDirection.normalized * (speed * Time.deltaTime) + new Vector3(0f, verticalVelocity, 0f) * Time.deltaTime);

        if(hasAnimator)
        {
            animator.SetFloat(animIDSpeed, animationBlend);
            animator.SetFloat(animIDMotionSpeed, 1f);
        }
    }

    private void JumpAndGravity()
    {
        if (isGrounded)
        {
            fallTimeoutDelta = fallTimeout;

            if (hasAnimator)
            {
                animator.SetBool(animIDJump, false);
                animator.SetBool(animIDFreeFall, false);
            }

            if (verticalVelocity < 0f)
            {
                verticalVelocity = -2f;
            }

            if (Input.GetButton("Jump") && jumpTimeoutDelta <= 0f)
            {
                verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);

                if (hasAnimator)
                {
                    animator.SetBool(animIDJump, true);
                }
            }

            if(jumpTimeoutDelta >= 0f)
            {
                jumpTimeoutDelta -= Time.deltaTime;
            }
        }
        else
        {
            jumpTimeoutDelta = jumpTimeout;

            if(fallTimeoutDelta >= 0f)
            {
                fallTimeoutDelta -= Time.deltaTime;
            }
            else
            {
                if(hasAnimator)
                {
                    animator.SetBool(animIDFreeFall, true);
                }
            }
        }

        if (verticalVelocity < terminalVelocity)
        {
            verticalVelocity += gravity * Time.deltaTime;
        }
    }

    private void AnimAttackPlay()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            {
                animator.SetTrigger(animIDAttack);
            }
        }
    }

    private void ClickAction()
    {
        interactor.ClickInteract();
        targetAttack.ClickAttack();
    }

    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }

    private void OnDrawGizmosSelected()
    {
        Color transparentGreen = new Color(0f, 1f, 0f, 0.35f);
        Color transparentRed = new Color(1f, 0f, 0f, 0.35f);

        if(isGrounded) { Gizmos.color = transparentGreen;}
        else Gizmos.color = transparentRed;

        Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - groundedOffset, transform.position.z), groundedRadius);
    }
}
