using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallPlatform : MonoBehaviour
{
    [Header("¼Y¸¨³t«×")]
    public float speedG = 0.1f;

    private Rigidbody2D rig;

    private void Start()
    {
        rig = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            rig.bodyType = RigidbodyType2D.Dynamic;
            rig.constraints = RigidbodyConstraints2D.FreezeRotation;
            rig.gravityScale = speedG;
        }
    }
}
