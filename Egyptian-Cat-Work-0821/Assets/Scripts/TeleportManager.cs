using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TeleportManager : MonoBehaviour
{
    // 1. 靜態為此類別所有物件共用資料：富庶物件較不適合使用靜態
    // 2. 靜態在載入場景後不會恢復預設值
    // 3. 靜態欄位不會顯示在屬性面板

    /// <summary>
    /// 所有怪物數量
    /// </summary>
    public static int countAllEnemy;

    // Unity Botton 按鈕事件自行定義方式
    // 1. 引用 UnityEngine.Events API
    // 2. 定義 UnityEvent 欄位
    // 3. 需要執行事件的地方用 Invoke() 呼叫
    [Header("過關事件")]
    public UnityEvent onPass;

    private Animator ani;

    private void Start()
    {
        ani = GetComponent<Animator>();
        countAllEnemy = GameObject.FindGameObjectsWithTag("怪物").Length;
    }

    // 觸發事件：Trigger
    // 1. 兩個碰撞物件都要有 Collider
    // 2. 並且其中一個要有 Rigidbody
    // 3. 兩個其中一個有勾選 is Trigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "主角貓" /**&& countAllEnemy == 0**/)
        {
            ani.enabled = true;
            StartCoroutine(Coroutine());
        }
    }

    IEnumerator Coroutine()
    {
        yield return new WaitForSeconds(8);
        onPass.Invoke();
    }
}
