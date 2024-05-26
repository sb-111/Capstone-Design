using System.Collections;
using UnityEngine;
using Photon.Pun;

public class DeadState : MonoBehaviour, IMonsterState
{
    private Monster monster;
    public DeadState(Monster monster)
    {
        this.monster = monster;
    }
    // 1. 들어올 때 한번 실행
    public void EnterState()
    {
        Debug.Log("DeadState 진입");
        // Dead 애니메이션 실행
        monster.Anim.SetTrigger("doDie");
    }
    // 2. Dead 애니메이션 완전히 실행 후 몬스터 삭제 및 스크립트 정지
    public void ExecuteState()
    {
            if(monster.Anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f)
            {
                PhotonNetwork.Destroy(monster.gameObject);
                monster.enabled = false; 
            }
    }

    // 실행 X
    public void ExitState()
    {

    }
}
