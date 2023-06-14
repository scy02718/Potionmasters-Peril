using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;
    public int per;

    Rigidbody2D rigid;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    public void Init(float damage, int per, Vector3 dir) 
    {
        this.damage = damage;
        this.per = per;

        if (per != -1)
        {
            rigid.velocity = dir * 15f; // 15f 는 투사체 속도를 구현한것. 바꿀 수 있다.
        }
    }

    // 관통력 구현 함수
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy") || per == -1)
            return;
        
        per--;
        // 관통력이 남아있으면 관통력을 1 감소시키고, 관통력이 -1 이 된다면 없앤다.
        if (per == -1)
        {
            rigid.velocity = Vector2.zero;
            gameObject.SetActive(false);
            // Destroy 를 쓰면 안된다.
        }
    }
}
