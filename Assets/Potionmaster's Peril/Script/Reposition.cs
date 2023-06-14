using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reposition : MonoBehaviour
{
    Collider2D col;

    void Awake(){
        col = GetComponent<Collider2D>();
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Area"))
            return;
        // other 오브젝트의 태그가 Area 가 아니라면 함수를 종료한다.

        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 myPos = transform.position;
        float diffX = Mathf.Abs(playerPos.x - myPos.x);
        float diffY = Mathf.Abs(playerPos.y - myPos.y);

        Vector3 playerDir = GameManager.instance.player.inputVec;
        // 플레이어의 이동방향을 받아온다.

        float dirX = playerDir.x < 0 ? -1 : 1;
        float dirY = playerDir.y < 0 ? -1 : 1;

        switch (transform.tag){
            case "Ground":
                if (diffX > diffY){
                    transform.Translate(Vector3.right * dirX * 40);
                }
                else if (diffX < diffY) {
                    transform.Translate(Vector3.up * dirY * 40);
                }
                break;
            case "Enemy":
            // 적이 너무 멀어졌을땐 반대쪽에서 나타나게 해서 메모리를 아낀다.
                if (col.enabled){ // 적이 살아있을때만
                    transform.Translate(playerDir * 20 + new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f), 0f));
                    // 플레이어의 이동방향으로 20만큼 이동시킨고, 랜덤한 노이즈를 적용시킨다.
                }
                break;
        }
    }
}
