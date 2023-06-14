using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public float gameTime;
    // 실제로 흐르는 게임시간
    public float maxGameTime = 2*10f; // 20초
    // 게임이 끝나는 시간
    public PoolManager pool;
    public Player player;
    // 플레이어 오브젝트를 받아올 변수

    // 싱글톤 패턴 : 게임 내에서 유일하게 하나만 존재하는 클래스를 만들 때 사용하는 패턴

    void Awake()
    {
        instance=this;
    }

    // Update is called once per frame
    void Update()
    {
        gameTime += Time.deltaTime;

        if (gameTime > maxGameTime)
        {
            gameTime = maxGameTime;
        }
    }
}
