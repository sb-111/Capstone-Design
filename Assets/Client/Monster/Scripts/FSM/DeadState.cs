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
        PlaySound();
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
    private void PlaySound()
    {
        // 임시 오브젝트 
        GameObject tempObj = new GameObject("deadAudio");
        tempObj.transform.position = monster.transform.position;

        // 임시 오브젝트에 AudioSource 추가
        AudioSource audioSource = tempObj.AddComponent<AudioSource>();
        audioSource.clip = monster.MSound.GetClip(0);
        audioSource.Play();

        // 임시 오브젝트 파괴 예약
        Destroy(tempObj, audioSource.clip.length);
    }
    
}
