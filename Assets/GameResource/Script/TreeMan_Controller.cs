using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using Unity.VisualScripting;

public class TreeMan_Controller : NetworkBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Animator animator;
    //基本數值
    [Networked]public float HP { get; set; }
    public float speed = 7f;
    public float attackRange;
    public GameObject ClientCamera;
    [Networked]public NetworkBool Ismove { get; set; } //host端判斷用
    [Networked]public NetworkBool Isflip { get; set; } //host端判斷用
    public float dashCooldown = 3f;
    private bool dashing = false;
    private bool dashReady = true;
    [Networked]public NetworkBool Skill1Ready { get; set; }
    private float dashCooldownTimer;
    private float Skill1CooldownTimer;
    private Vector2 targetPosition;
    private Vector2 moveInput;
    private Rigidbody2D rig2d;
    public override void FixedUpdateNetwork()
    {
        if (!Object.HasStateAuthority)
        { return; }

        if (GetInput(out NetWorkInputData data))
        {
            if(!dashing)
            {
                moveInput = data.moveInput_NetworkData;
                transform.Translate(moveInput * speed * Runner.DeltaTime);
            }

            //dash事件觸發
            if (data.playerEvent == NetWorkInputData.NetworkEvents.Dash)
            {
                if (!dashing && dashReady) {
                    Debug.Log(data.playerEvent);
                    Dash();
                }
            }
            if (data.playerEvent == NetWorkInputData.NetworkEvents.Skill1)
            {
                if (Skill1Ready) { 
                    Debug.Log(data.playerEvent);
                    Attack_Skill1();
                }
            }
        }
        HostBoolSet();
        if(dashing)
        {
            rig2d.transform.position = Vector2.Lerp(rig2d.position, targetPosition, Runner.DeltaTime * 50f);
            //目標點在玩家附近0.5f reset dash狀態
            if(Vector2.Distance(targetPosition, rig2d.transform.position) < 0.5f)
            {
                dashing = false;
            }
        }
        //衝刺冷卻計時器
        if (!dashReady)
        {
            dashCooldownTimer += Runner.DeltaTime;
            if (dashCooldownTimer >= dashCooldown)
            {
                dashReady = true;
                dashCooldownTimer = 0f;
            }
        }
        //技能1冷卻計時器
        Skill1CooldownTimer += Runner.DeltaTime;
        if (Skill1CooldownTimer >= dashCooldown)
        {
            Skill1Ready = true;
            Skill1CooldownTimer = 0f;
        }
    }
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rig2d = GetComponent<Rigidbody2D>();
        if(Object.HasInputAuthority)
        {
            if (Object.HasStateAuthority) {
                Camera.main.GetComponent<CameraControll>().SetTarget(transform);
            }
            else { 
                GameObject camera  = Instantiate(ClientCamera);
                camera.GetComponent<CameraControll>().SetTarget(transform);
                Camera.main.enabled = false;
            }
        }
    }
    private void Update()
    {
        // 本地玩家才顯示 UI 或啟動攝影機等
        if (Object != null && Object.HasInputAuthority)
        {
            // 例如：啟動跟隨攝影機
        }
        AniController();
        
    }
    private void AniController() { 
        //判斷左右動畫
        spriteRenderer.flipX = Isflip;

        
        //切換走路動畫
        animator.SetBool("ismove", Ismove);
    }
    void HostBoolSet() { 
        //判斷左右動畫
        if(moveInput.x < 0)
        {
            Isflip = false;
        }
        if(moveInput.x > 0)
        {
            Isflip = true;
        }
        //切換走路狀態
        if(moveInput.x != 0 || moveInput.y != 0)
        {
            Ismove = true;
        }
        else
        {
            Ismove = false;
        }
    }
    void Dash() {
        dashReady = false;
        targetPosition = (Vector2)transform.position + (moveInput.normalized * 5f);
        dashing = true;
    }
    void Attack_Skill1() { 
        //取得範圍內的玩家
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, attackRange);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                TreeMan_Controller player = collider.GetComponent<TreeMan_Controller>();
                //排除自己不造成傷害
                if (player != null && player != this)
                {
                    player.TakeDamage(10f);
                }
            }
        }
    }

    ///<summary>
    ///扣血方法
    ///</summary>
    public void TakeDamage(float damage) {
        if (HP > 0) { 
            HP -= damage;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
