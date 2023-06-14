using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    public float scanRange;
    public LayerMask targetLayer;
    public RaycastHit2D[] targets;
    public Transform nearestTarget; // 제일 가까운 몬스터. 나중에 무기에 따라 바꿀수도?

    void FixedUpdate()
    {
        targets = Physics2D.CircleCastAll(transform.position, scanRange, Vector2.zero, 0, targetLayer);
        // .. 내 위치에서 scanRange 만큼의 반지름을 가진 원을 그려서 그 안에 있는 모든 오브젝트를 찾는다.
        // 1. 캐스팅 시작위치 -> 본인의 위치
        // 2. 원의 반지름 -> scanRange -> 사거리 (나중에 바꾸기 가능)
        // 3. 캐스팅 방향 -> 원형으로 다 하기 때문에 방향이 없음. Vector2.zero
        // 4. 캐스팅 길이 -> 원형으로 다 하기 때문에 길이가 없음. 0
        // 5. 대상 레이어 -> 레이어마스크를 이용해서 대상을 설정한다.

        nearestTarget = GetNearest();
    }

    // .. 가장 가까운 타겟을 찾는 함수
    Transform GetNearest()
    {
        Transform result = null;
        float diff = 100;
        // .. 가장 가까운 타겟과의 거리를 담을 변수

        foreach (RaycastHit2D target in targets)
        {
            float distance = Vector3.Distance(transform.position, target.transform.position);
            // .. 내 위치와 타겟의 위치를 빼서 거리를 구한다.

            if (distance < diff)
            {
                diff = distance;
                result = target.transform;
                // Transform 이 정확히 뭐지? -> 게임오브젝트의 위치, 회전, 크기 등을 담고 있는 컴포넌트
            }
        }

        return result;
    }
}
