using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    // 몬스터의 이동속도
    public Rigidbody2D target;
    // 몬스터가 추적할 대상
    public RuntimeAnimatorController[] animators;
    // 몬스터의 애니메이션을 담당할 애니메이터 컨트롤러들 -> 여러종류의 몬스터들을 동시에 관리할 수 있다.
    public float hp;
    // 몬스터의 현재체력
    public float maxHp;
    // 몬스터의 최대체력

    bool isLive; // 나중에 바꾸기
    // 몬스터의 생존여부

    Animator animator;
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;

    void Awake()
    {
        animator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        if (isLive)
        {
            Vector2 dirVec = target.position - rigid.position;
            // 몬스터의 다음 위치를 목표물의 위치에서 현재 위치를 뺀 값으로 설정한다.
            rigid.MovePosition(rigid.position + dirVec.normalized * speed * Time.fixedDeltaTime);
            // 몬스터의 위치를 몬스터의 현재 위치에서 몬스터의 다음 위치로 이동시킨다.
            // Time.fixedDeltaTime 은 프레임의 영향을 받지 않도록 하는것이다.

            rigid.velocity = Vector2.zero;
        }
    }

    void LateUpdate()
    {
        spriteRenderer.flipX = target.position.x < rigid.position.x;
    }

    void OnEnable()
    {
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();
        // 유니티 이벤트. 생성될 때마다 플레이어를 찾아 target 으로 설정한다.
        isLive = true;
        hp = maxHp;
    }

    public void Init(SpawnData data)
    {
        animator.runtimeAnimatorController = animators[data.spriteType];
        // 인풋 데이터로 받은 스프라이트 타입에 맞는 애니메이터 컨트롤러를 설정한다.
        speed = data.speed;
        maxHp = data.hp;
        hp = data.hp;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet"))
            return; // 총알이 아니면 리턴
        
        
        hp -= collision.GetComponent<Bullet>().damage;
        // 총알의 데미지만큼 체력을 깎는다.
        if (hp <= 0)
        {
            Dead();
        }
        else
        {
            // .. 아직 살아있으니 피격 액션 구현
        }
    }

    void Dead()
    {
        gameObject.SetActive(false);
    }
}


