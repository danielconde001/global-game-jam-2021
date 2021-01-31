using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonMovement : MonoBehaviour
{
    [SerializeField] private PlayerShoot playerShoot;
    [SerializeField] private float _Speed = 12f;
    [SerializeField] private GameObject _GroundChecker;
    [SerializeField] private LayerMask _GroundMask;
    [SerializeField] private float _GroundDistance;
    [SerializeField] private float _GravityScale = 1;
    [SerializeField] private bool canJump = true;
    [SerializeField] private float _JumpHeight;
    
    private CharacterController _CharacterController;
    
    private float _Gravity = -9.81f; 
    private float originalSpeed;
    private float aimSpeed;
    private Vector3 _CurrentVelocity;

    void Awake()
    {
        _CharacterController = GetComponent<CharacterController>();
    }

    private void Start()
    {
        originalSpeed = _Speed;
        aimSpeed = originalSpeed - playerShoot.AimMoveSpeedDecrease;
    }

    void Update()
    {
        bool isGrounded = Physics.CheckSphere(_GroundChecker.transform.position, _GroundDistance, _GroundMask);

        if (isGrounded && _CurrentVelocity.y < 0f)
        {
            _CurrentVelocity.y = -.2f;
        }

        float xMove = Input.GetAxis("Horizontal");
        float yMove = Input.GetAxis("Vertical");

        Vector3 move = transform.right * xMove + transform.forward * yMove;
        _Speed = Mathf.Lerp(originalSpeed, aimSpeed, playerShoot.AimBlend);
        _CharacterController.Move(Vector3.ClampMagnitude(move, 1) * _Speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded && canJump)
        {
            _CurrentVelocity.y = Mathf.Sqrt(_JumpHeight * -2 * (_Gravity * _GravityScale));
        }

        _CurrentVelocity.y += (_Gravity * _GravityScale) * Time.deltaTime;
        _CharacterController.Move(_CurrentVelocity * Time.deltaTime);
    }
}
