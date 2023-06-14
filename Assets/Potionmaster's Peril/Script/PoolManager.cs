using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    // .. 프리펩들을 보관할 변수
    public GameObject[] prefabs;
    // .. 풀 담당을 하는 리스트들
    public List<GameObject>[] pools;
    // .. 프리펩과 리스트는 1대1로 매칭되어야 한다.

    void Awake()
    {
        pools = new List<GameObject>[prefabs.Length];
        // .. 프리펩의 개수만큼 리스트를 만든다.
        for (int i = 0; i < pools.Length; i++)
        {
            pools[i] = new List<GameObject>();
            // .. 리스트를 초기화한다.
        }
    }

    public GameObject GetFromPool(int index)
    {
        GameObject select = null;
        // .. 반환할 오브젝트를 담을 변수

        // .. 선택한 풀의 놀고 (비활성화 된) 있는 게임오브젝트 접근
        foreach (GameObject obj in pools[index])
        {
            // .. 만약 발견 하면 select 변수에 할당
            if (!obj.activeSelf)
            {
                select = obj;
                select.SetActive(true);
                break;
            }
        }

        // .. 못 찾으면 ? 지금 모든 오브젝트들이 다 바쁘다는 뜻
        // .. 새롭게 생성하고 select 변수에 할당
        if (!select)
        {
            select = Instantiate(prefabs[index], transform);
            // Transform 은 부모를 의미한다. 깔끔하게 만드는 방법이다. 그게 아니라면 옆의 화면에 쭈루루루룩 오브젝트가 뜬다.
            pools[index].Add(select);
        }

        return select;
    }
}
