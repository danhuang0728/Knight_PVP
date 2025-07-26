using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TreeMan_Controller : MonoBehaviour
{
    [Header("移動設定")]
    [SerializeField] private float moveSpeed = 5f;
    public Animator animator;
    private PlayerInput playerInput;
    private Vector2 moveInput;
    private Rigidbody2D rb;
    private bool ismove = false;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {
        ismove = moveInput != Vector2.zero;
        if (ismove) { 
            animator.SetBool("ismove", true);
        }
        else{
            animator.SetBool("ismove", false);
        }
        // 獲取輸入
        moveInput = playerInput.actions["move"].ReadValue<Vector2>();
    }
    
    void FixedUpdate()
    {
        // 移動角色
        Move();
    }
    
    private void Move()
    {
        // 使用Rigidbody2D移動角色
        Vector2 movement = moveInput * moveSpeed;
        rb.velocity = movement;
    }
}
