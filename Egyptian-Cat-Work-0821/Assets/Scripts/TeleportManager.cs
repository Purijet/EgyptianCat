using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TeleportManager : MonoBehaviour
{
    // 1. �R�A�������O�Ҧ�����@�θ�ơG�I�f��������A�X�ϥ��R�A
    // 2. �R�A�b���J�����ᤣ�|��_�w�]��
    // 3. �R�A��줣�|��ܦb�ݩʭ��O

    /// <summary>
    /// �Ҧ��Ǫ��ƶq
    /// </summary>
    public static int countAllEnemy;

    // Unity Botton ���s�ƥ�ۦ�w�q�覡
    // 1. �ޥ� UnityEngine.Events API
    // 2. �w�q UnityEvent ���
    // 3. �ݭn����ƥ󪺦a��� Invoke() �I�s
    [Header("�L���ƥ�")]
    public UnityEvent onPass;

    private Animator ani;

    private void Start()
    {
        ani = GetComponent<Animator>();
        countAllEnemy = GameObject.FindGameObjectsWithTag("�Ǫ�").Length;
    }

    // Ĳ�o�ƥ�GTrigger
    // 1. ��ӸI�����󳣭n�� Collider
    // 2. �åB�䤤�@�ӭn�� Rigidbody
    // 3. ��Ө䤤�@�Ӧ��Ŀ� is Trigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "�D����" /**&& countAllEnemy == 0**/)
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
