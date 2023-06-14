using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint;
    // Transform 은 자식 오브젝트를 의미한다.
    public SpawnData[] spawnData;
    // SpawnData 클래스의 배열. 각 인덱스는 레벨을 나타낸다. 즉 각 레벨에서의 몬스터의 스펙.

    int level;
    float timer;

    void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();
        // GetComponentsInChildren<Transform>() 은 자식 오브젝트들을 가져온다.
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        level = Mathf.FloorToInt(GameManager.instance.gameTime / 10f); // 10초마다 난이도가 올라감

        if (timer > spawnData[level].spawnTime) // 이 레벨에서 사용돼는 몬스터 스폰시간 적용
        {
            timer = 0;
            Spawn();
        }
    }

    void Spawn()
    {
        GameObject enemy = GameManager.instance.pool.GetFromPool(0);  // 어차피 이제 SpawnData 클래스로 몬스터를 컨트롤하니까 0번째 인덱스만 가져오면 된다.
        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position; // 스폰포인트중 랜덤하게 가져온다. 1 에서 시작하는거는 Transform 의 0번째는 자기 자신이기 때문이다.
        // 즉 Spawner 라는 클래스가 들어있는것이다.
        enemy.GetComponent<Enemy>().Init(spawnData[level]);
        // Enemy 클래스의 Init 함수를 호출한다. 즉 Spawndata 의 정보를 받아 현재 몬스터의 정보를 초기화한다.
    }
}

[System.Serializable] // 인스펙터 창에 보이게 해준다.
public class SpawnData // 몬스터의 정보를 담는 클래스
{
    public float spawnTime;
    public int spriteType;
    public int hp;
    public float speed;
}