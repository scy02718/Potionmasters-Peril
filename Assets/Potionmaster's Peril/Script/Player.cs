using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// C# 에선 MonoBehaviour 라는 클래스를 항상 상속받는다 -> 게임 로직 구성에 필요한 것들을 가진 클래스
public class Player : MonoBehaviour
{
    public Vector2 inputVec; 
    // 플레이어의 입력값을 받아올 변수. Vector2 타입 -> x,y 값을 가지는 변수. Public 은 이제 다른 스크립트 (클래스)에서 사용 가는하다는 뜻
    public float speed;
    // 플레이어의 이동속도를 담당하는 변수
    public Scanner scanner;
    // 플레이어의 스캐너를 담당하는 변수
    SpriteRenderer spriteRenderer;
    // 플레이어의 스프라이트를 담당하는 변수
    Animator animator;
    // 플레이어의 애니메이션을 담당하는 변수
    

    // RigidBody2D 컴포넌트를 여기로 받아와서 사용해야한다.
    public Rigidbody2D rigid; // 플레이어의 물리적인 움직임을 담당하는 컴포넌트

    void Awake()
    {
        // GetComponent<T>() : T 타입의 컴포넌트를 오브젝트에서 받아온다.
        scanner = GetComponent<Scanner>(); // scanner 변수에 Scanner 컴포넌트를 받아온다. 커스텀 클래스도 GetComponent 로 받을 수 있다.
        rigid = GetComponent<Rigidbody2D>(); // rigid 변수에 RigidBody2D 컴포넌트를 받아온다.
        spriteRenderer = GetComponent<SpriteRenderer>(); // spriteRenderer 변수에 SpriteRenderer 컴포넌트를 받아온다.
        animator = GetComponent<Animator>(); // animator 변수에 Animator 컴포넌트를 받아온다.
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // FixedUpdate : 물리적인 움직임을 담당하는 함수. 매 프레임마다 호출되는 Update 와는 다르게 일정한 시간마다 호출된다.
    // 쟘깐 뭐임? 코파일럿 얘 내가 뭘 하려는지 다 아는거 같이 미리미리 다 해주는데???? 뭐고 이새끼
    void FixedUpdate()
    {
        Vector2 nextVec = inputVec * speed * Time.fixedDeltaTime;
        // FixedDeltaTime 은 매 프레임마다 걸리는 시간을 의미한다. (1초에 60프레임이라면, 1프레임마다 걸리는 시간은 1/60초)
        // 오히려 코드에 이해 안돼는거 있을때 주석 달고 좀 기다리면 얘가 알아서 설명해줘서 개꿀임

        rigid.MovePosition(rigid.position + nextVec); 
        // rigid 의 위치를 inputVec 만큼 이동시킨다
        // 아니... 진짜 진도 나가기도 전에 다 나가네 아니 이 문장도 자동완성을 해주네 미친ㅋㅋㅋㅋ
    }

    // OnMove : 플레이어의 입력값을 받아오는 함수 (InputSystem 에서 자동으로 호출해준다)
    void OnMove(InputValue value)
    {
        inputVec = value.Get<Vector2>(); // inputVec 에 플레이어의 입력값을 Vector2 자료형으로 받아온다.
    }

    void LateUpdate()
    {
        animator.SetFloat("Speed", inputVec.magnitude); // animator 의 Speed 라는 파라미터에 inputVec 의 크기를 넣어준다

        if (inputVec.x != 0){
            spriteRenderer.flipX = inputVec.x < 0; // inputVec.x 가 0보다 작으면 flipX 를 true 로 바꾼다.
        }
    }
}
