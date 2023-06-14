using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    // Relational Database 로 구현가능해 보임.
    public int id;
    public int prefabId;
    public float damage;
    public int count; // 근접에서는 공의 갯수, 원거리면 관통력을 의미한다.
    public float speed;

    float timer;
    Player player;

    void Awake()
    {
        player = GetComponentInParent<Player>();
        // .. 부모를 찾아서 할당한다.
    }

    // Start는 Awake와 비슷한데, Awake는 게임이 시작되자마자 실행되는 반면, Start는 게임이 시작되고 나서 Update가 실행되기 전에 실행된다.
    void Start(){
        Init();
    }

    public void LevelUp(float damage, int count)
    {
        this.damage = damage;
        this.count += count;

        if (id == 0)
        {
            Batch();
        }
    }

    public void Init()
    {
        // .. 무기의 정보를 무기의 아이디에 따라 초기화한다.
        switch (id)
        {
            case 0:
                speed = 150; // 시계방향으로 돌 수 있도록 음수로 설정
                Batch();
                break;
            case 1:
                speed = 0.5f; // 0.5초에 한 번씩 발사. Speed 가 의미하는게 근접이랑 원거리랑 다름.
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // .. 무기의 정보를 무기의 아이디에 따라 초기화한다.
        switch (id)
        {
            case 0:
                transform.Rotate(Vector3.back * speed * Time.deltaTime);
                // 프레임에 영향받지 않기 위해서 deltaTime 을 곱해준다. 이걸 안하면 프레임이 다르면 속도도 달라짐.
                break;
            case 1:
                timer += Time.deltaTime;
                if (timer > speed)
                {
                    timer = 0;
                    Fire();
                }
                break;
            default:
                break;
        }

        // .. Test Code..
        if (Input.GetButtonDown("Jump")){
            LevelUp(10,1);
        }
    }

    // 실제 Batch 가 아니라 "배치" 한다는 함수ㅋㅋㅋ
    void Batch()
    {
        // Count 만큼 for 문을 돌면서 Poolmanager 에 있는 프리팹을 가지고 온다.
        for (int i=0;i<count;i++){
            Transform bullet;
            // 이 게임매니저 스태틱 변수는, 이 실제 게임 자체의 오브젝트를 나타낸다.
            // 총알 을 만들면서 그거의 "트랜스폼" 을 가지고 온다. 트랜스폼을 가지고 오는 이유는, 그 부모 오브젝트에 접근해야하기 때문.

            if (i < transform.childCount)
            {
                bullet = transform.GetChild(i);
                // 만약에 이미 배치된 총알이 있다면, 그 총알을 재활용한다. 이걸 안 하면 이미 n 개의 총알을 가지고 있어도 n + 1 개의 총알을 새로 배치한다.
            }
            else
            {
                bullet = GameManager.instance.pool.GetFromPool(prefabId).transform;
                // 추가로 배치될 총알을 이제 풀링해서 가지고 온다.
                bullet.parent = transform;
                // 각 총알의 부모를 그에 해당하는 무기 타입으로 설정한다.
            }

            bullet.localPosition = Vector3.zero;
            // 로컬 포지션을 0으로 설정한다. 이걸 안하면, 배치할 때마다 위치가 달라진다. (플레이어의 위치로 기준을 시키는 것.)
            bullet.localRotation = Quaternion.identity;
            // 회전도 0으로 설정한다.

            Vector3 rotVec = Vector3.forward * (360f / count) * i;
            bullet.Rotate(rotVec);
            // 360 도에 꽉 맞춰서 배치하게 한다.

            bullet.Translate(bullet.up * 1.5f, Space.World);

            bullet.GetComponent<Bullet>().Init(damage,-1, Vector3.zero);
            // 근접무기이기 때문에 관통력을 무한으로 놓는다. 특수 -1로 설정 -> Infinity Penetration
        }
    }

    void Fire()
    {
        if (!player.scanner.nearestTarget)
            return;
        
        Vector3 targetDirection = player.scanner.nearestTarget.position - transform.position;
        // 제일 가까운 적까지의 방향을 구한다.
        targetDirection = targetDirection.normalized;
        // 크기를 1로 만든다. (방향만 구하고 싶기 때문에)

        Transform bullet = GameManager.instance.pool.GetFromPool(prefabId).transform;
        // 근접무기와 동일함.

        bullet.position = transform.position;
        // 현재 플레이어의 위치에서 발사함.

        bullet.rotation = Quaternion.FromToRotation(Vector3.up, targetDirection);
        // 목표 방향으로 돌려주는 함수.
        // 축 : Vector3.up (z축으로 돌리기 위해서)
        // 목표 방향 : targetDirection

        bullet.GetComponent<Bullet>().Init(damage, count, targetDirection);
    }
}
