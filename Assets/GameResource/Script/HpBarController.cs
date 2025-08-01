using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using TMPro.Examples;

public class HpBarController : NetworkBehaviour
{
    private float HP;
    private float BarValue;
    [SerializeField]private NetworkObject Player = null;
    [SerializeField]private Transform HPbar;

    // 使用 Networked 属性同步 PlayerRef
    [Networked] public PlayerRef TargetPlayerRef { get; set; }

    public override void Spawned() {
        Debug.Log("測試spawned");
    }

    public override void FixedUpdateNetwork() {
        //Debug.Log("測試FixedUpdateNetwork");
        if(Player == null){
            Player = NetworkManager.Instance.GetPlayerObject(TargetPlayerRef);
        }
        if (Player != null) { 
            HP = Player.GetComponent<TreeMan_Controller>().HP;
            BarValue = HP / 100f;
        }
        if (Player != null) { 
            FollowPlayer(Player.transform);    
            
        }
        HPbar.transform.localScale = new Vector3(BarValue, 1, 1);
    }
    // public override void Render()
    // {
    //     if (!Object.HasStateAuthority) { 
    //         // 所有客户端都会执行 Render
    //         if(Player == null){
    //             Player = NetworkManager.Instance.GetPlayerObject(TargetPlayerRef);
    //         }
    //         if (Player != null) { 
    //             HP = Player.GetComponent<TreeMan_Controller>().HP;
    //             BarValue = HP / 100f;
    //         }
    //         if (Player != null) { 
    //             FollowPlayer(Player.transform);    
                
    //         }
    //     }
    //     HPbar.transform.localScale = new Vector3(BarValue, 1, 1);
        
    // }
        void Start()
    {
    
    }
    void Update()
    {
        //數字鍵2測試
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            Debug.Log(BarValue);
            Debug.Log("血量變數:"+HP);
            Debug.Log("TargetPlayerRef:"+TargetPlayerRef);
            Debug.Log("以綁定玩家物件:"+Player);
        }

    }
    void FollowPlayer(Transform target)
    {
        transform.position = target.position + new Vector3(0,1,0);
    }
    
}
