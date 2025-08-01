using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class CameraControll : MonoBehaviour
{
    [Header("鏡像設定")]
    [SerializeField] public Transform target;
    [SerializeField] private float smoothSpeed = 0.03f;
    [SerializeField] private Vector3 offset = new Vector3(0, 0, -10);
    
    [Header("震動設定")]
    [SerializeField] private float shakeDuration = 0.5f; // 震動持續時間
    [SerializeField] private float shakeAmount = 0.7f; // 震動效果的最大偏移量
    [SerializeField] private float decreaseFactor = 1.0f; // 震動效果隨時間减弱的因子
    
    private Vector3 originalPos;
    private TreeMan_Controller targetController;
    private float currentShakeDuration = 0f;
    
    void Start()
    {
        originalPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(target != null && targetController == null)
        {
            targetController = target.GetComponent<TreeMan_Controller>();
        }
        
        if (target != null)
        {
            // 正常跟隨目標
            Vector3 desiredPosition = target.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            
            // 如果正在震動，應用震動效果
            if (currentShakeDuration > 0)
            {
                smoothedPosition += UnityEngine.Random.insideUnitSphere * shakeAmount;
                currentShakeDuration -= Time.deltaTime * decreaseFactor;
            }
            
            transform.position = smoothedPosition;
        }
    }
    
    /// <summary>
    /// 觸發相機震動效果
    /// </summary>
    /// <param name="duration">震動持續時間，如果為null則使用默認值</param>
    /// <param name="amount">震動強度，如果為null則使用默認值</param>
    //打擊時相機特效
    public void TriggerShake(float? duration = null, float? amount = null)
    {
        originalPos = transform.position;
        currentShakeDuration = duration ?? shakeDuration;
        
        // 如果提供了自定義震動強度，則使用它
        if (amount.HasValue)
        {
            shakeAmount = amount.Value;
        }
    }
    public void SetTarget(Transform target) { 
        this.target = target;
    }
}
