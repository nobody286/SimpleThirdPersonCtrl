using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerCharater : MonoBehaviour
{
    private PlayerMove playerMove;

    [SerializeField]
    private Photograher photograher;

    [SerializeField]
    private Transform followTarget;

    [SerializeField]
    private float movementThreshold = 0.01f;

    [SerializeField]
    private float rotationSpeed = 720f; // 角速度：度/秒

    private Animator animator;
    private const string IsWalkingParam = "isWalking";
    private bool currentWalkingState;

    // Start is called before the first frame update
    void Awake()
    {
        playerMove = GetComponent<PlayerMove>();

        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogWarning($"{nameof(PlayerCharater)}: Animator not found on the same GameObject.");
        }

        photograher.InitCamera(followTarget);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMovementInput();
    }
    private void UpdateMovementInput()
    {
        Quaternion rot = Quaternion.Euler(0f, photograher.Yaw, 0f);

        Vector3 input = rot * Vector3.forward * Input.GetAxis("Vertical") +
            rot * Vector3.right * Input.GetAxis("Horizontal");

        playerMove.SetMovementInput(input);

        // 动画切换：根据 PlayerMove.CurrentInput 的大小判断是否在移动
        if (animator != null)
        {
            bool isWalking = playerMove.CurrentInput.sqrMagnitude > (movementThreshold * movementThreshold);
            if (isWalking != currentWalkingState)
            {
                currentWalkingState = isWalking;
                animator.SetBool(IsWalkingParam, isWalking);
            }
        }

        // 朝前进方向转向（只围绕 Y 轴）
        Vector3 moveDir = playerMove.CurrentInput;
        moveDir.y = 0f; // 只在 XZ 平面考虑方向
        if (moveDir.sqrMagnitude > (movementThreshold * movementThreshold))
        {
            Quaternion targetRot = Quaternion.LookRotation(moveDir.normalized, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
        }
    }
}