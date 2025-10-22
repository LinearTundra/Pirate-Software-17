using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
    
    [SerializeField]
    private PlayerObject[] Characters;


    [SerializeField, Range(0,30), Tooltip("How fast the character reaches top speed")]
    private float moveForce;

    [SerializeField, Range(0,10), Tooltip("Force applied to the character to initiate initialiseJump")]
    private float jumpForce;

    [SerializeField, Range(0,10), Tooltip("How fast character stops moving")]
    private float stopForce;

    [SerializeField, Range(3,15)]
    private float maxHorizontalSpeed;

    [SerializeField, Range(3,15)]
    private float maxVerticalSpeed;

    [SerializeField, Range(1,5)]
    private float fallGravityMultiplier;


    [SerializeField]
    private InputActionAsset inputActions;
    private InputAction moveInput;
    private InputAction jumpInput;
    private InputAction attackInput;
    private InputAction sprintInput;

    
    // 
    // action gives normalised vector value which can be 
    // (1,0), (0,1), (-1,0), (0,-1), (0.71,0.71), (-0.71,0.71), (0.71,-0.71), (-0.71,-0.71)
    // we only want the x part to determine what direction to go
    // so multiplying it with any number between 1.5 and 1.9 will give -- 1 < value < 2
    // coonverting said value into int will give the direction -1, 0 or 1
    // 
    // Mathf.Abs() does not return 0 but 1 when 0 is passed

    private int moveDir => (int)(moveInput.ReadValue<Vector2>().x * 1.9);

    public static Action<float, int> Move;
    public static Action<float> Stop;
    public static Action<float> Jump;
    public static Action Attack;
    public static Action<bool> Sprint;


    public static Collider2D player1;
    public static Collider2D player2;
    // private static PlayerController instance;

    
    private void OnEnable() {
        inputActions.FindActionMap("Player").Enable();
    }

    private void OnDisable() {
        inputActions.FindActionMap("Player").Disable();
    }

    private void Awake() {
        // if (instance == null) {
        //     instance = this;
        //     DontDestroyOnLoad(instance);
        // }
        // else if (instance != this) Destroy(gameObject);

        moveInput = inputActions.FindAction("Move");
        jumpInput = inputActions.FindAction("Jump");
        attackInput = inputActions.FindAction("Attack");
        sprintInput = inputActions.FindAction("Sprint");
    }

    private void Start() {
        assignValues();
        
        player1 = Characters[0].GetComponent<Collider2D>();
        player2 = Characters[1].GetComponent<Collider2D>();
    }

    private void Update() {
        bool isAttacking = attackInput.WasPressedThisFrame();

        // Jump
        if (jumpInput.WasPressedThisFrame()) Jump?.Invoke(jumpForce);

        // Attack
        if (isAttacking) Attack?.Invoke();
    }

    private void FixedUpdate() {
        bool isSprinting = sprintInput.IsPressed();

        // Movement
        float appliedMoveForce = isSprinting ? moveForce * 1.5f : moveForce;
        if (moveDir == 0) Stop?.Invoke(stopForce);
        else Move?.Invoke(appliedMoveForce, moveDir);
        
        // Sprinting
        Sprint?.Invoke(isSprinting);
    }

    private void assignValues() {
        foreach (PlayerObject character in Characters) {
            character.fallGravityMultiplier = fallGravityMultiplier;
            character.maxHorizontalSpeed = maxHorizontalSpeed;
            character.maxVerticalSpeed = maxVerticalSpeed;
        }
    }

}
