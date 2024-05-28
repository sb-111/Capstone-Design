using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerStatus;
using Photon.Pun;

public class CombatStatusManager : MonoBehaviourPun
{
    [HideInInspector] public Player player;
    [HideInInspector] public PlayerStatus player_status;
    [HideInInspector] public Animator anim;

    public float knockbackPower = 8f; // 넉백 파워
    public float knockbackDuration = 0.2f; // 넉백 지속 시간
    PhotonView pv;


    // Start is called before the first frame update
    private void Awake()
    {
        player=GetComponent<Player>();
        anim=GetComponent<Animator>();
        player_status=GetComponent<PlayerStatus>();
        pv= GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        if (player.isJump) return;              //구르기 중이라면 무적
        Debug.Log("데미지 판정");
        int result_damage = (int)(damage * (1 - (player_status.basicStats.def / (player_status.basicStats.def + player_status.combatStats.constant_def))));//데미지 = 데미지*피해흡수율(= 방어력/방어력+방어상수)
        //player_status.basicStats.hp -= result_damage; // 캡슐화 이용한 밑줄이 더 적합
        player_status.DecreaseHP(result_damage);
        //Debug.Log(result_damage);
        if (result_damage > 0 && player.isDefense) //플레이어가 현재 방패로 막는 중이라면 디펜싱 히트로
        {
            player.DefensingHit();
        }
        //pv.RPC("RPCDamage", RpcTarget.All, damage);
        //서버 접속하지 않았을 시 아래 함수 사용
    }

    [PunRPC]
    public void RPCDamage(int damage)
    {
        if (player.isJump) return;              //구르기 중이라면 무적

        int result_damage = (int)(damage * (1 - (player_status.basicStats.def / (player_status.basicStats.def + player_status.combatStats.constant_def))));//데미지 = 데미지*피해흡수율(= 방어력/방어력+방어상수)
        //player_status.basicStats.hp -= result_damage; // 캡슐화 이용한 밑줄이 더 적합
        player_status.DecreaseHP(result_damage); 
        //Debug.Log(result_damage);
        if (result_damage > 0 && player.isDefense) //플레이어가 현재 방패로 막는 중이라면 디펜싱 히트로
        {
            player.DefensingHit();
        }
    }

    public void HitResponse(float cctime=1.0f)       //강공격에 의한 피격 반응 애니메이션 출력(cc기 시간)
    {
        anim.SetTrigger("getHit");
        player.isCC = true;
        Invoke("HitResponseEnd", cctime);
    }
    public void HitResponseEnd()
    {
        player.isCC = false;
    }

    /// <summary>
    /// 캐릭터가 넉백되는 함수
    /// </summary>
    /// <param name="direction">넉백 방향벡터</param>
    public void TakeKnockback(Vector3 direction)
    {
        // 받아온 방향벡터의 y성분이 존재하거나, 정규화가 안되어있는 경우를 가정
        if(direction.y != 0) direction.y = 0; 
        if(direction.magnitude > 1f) direction = direction.normalized; // 정규화

        StartCoroutine(OnKnockback(direction));
    }
    private IEnumerator OnKnockback(Vector3 direction)
    {
        float elapsed = 0;
        while (elapsed < knockbackDuration)
        {
            elapsed += Time.deltaTime;
            transform.Translate(direction * knockbackPower * Time.deltaTime);
            yield return null;
        }
    }
}
