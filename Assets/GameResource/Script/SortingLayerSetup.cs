using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortingLayerSetup : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // 根據 Y 座標動態調整 sortingOrder
        spriteRenderer.sortingOrder = Mathf.RoundToInt(-transform.position.y * 10);
    }
}
