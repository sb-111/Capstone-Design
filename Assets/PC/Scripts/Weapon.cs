using ExitGames.Client.Photon;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    
    public enum WeaponType {Melee,Range};
    public WeaponType type;
    public int damage; //atk
    public float rate;   //공격속도
    public BoxCollider meleeArea;   //공격범위
    public TrailRenderer trailEffect; //공격이펙트


    //private HashSet<GameObject> alreadyHitObjects;

    private HashSet<GameObject> hitEnemies = new HashSet<GameObject>();

    //패링
    public GameObject parryingParticle;     //패링 파티클 프리팹
    bool canPrrying = true;                 
    float parryingCooldown = 3.0f;          //패링 쿨타임
    ParryingEffect parryingEffect = null;

    private void Awake()
    {
        parryingEffect = GetComponent<ParryingEffect>();
        //alreadyHitObjects=new HashSet<GameObject>();
    }
    public void Use(float attackEndTime)
    {
        if(type == WeaponType.Melee)
        {
            StopCoroutine(Swing(attackEndTime));
            hitEnemies.Clear();                         //HashSet 초기화, 공격이 새롭게 시작될 때 마다 초기화.
            Debug.Log("HashSet 클리어");
            StartCoroutine(Swing(attackEndTime));
            
        }
    }

    IEnumerator Swing(float attackEndTime)
    {
        meleeArea.enabled = true;
        trailEffect.enabled = true;
        //Debug.Log("Swinging"+ System.DateTime.Now.ToString("HH:mm:ss.fff"));
        yield return new WaitForSeconds(attackEndTime);          //받아온 종료 시간으로 >> 직접 애니메이션 보고 설정해야 함 ㅠ
        meleeArea.enabled = false;
        yield return new WaitForSeconds(0.2f);
        trailEffect.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "ParryingBox"&&canPrrying)
        {
            StartCoroutine(Parrying());
        }

        if (other.tag == "Enemy")
        {
            GameObject enemy = other.gameObject;
            Enemy enemyDamage = enemy.GetComponent<Enemy>();
            if (!hitEnemies.Contains(enemy)) // 이미 공격한 적이 아니라면
            {
                hitEnemies.Add(enemy); // 이 적을 공격한 적 목록에 추가 //enemyDamage.curHP -= damage;//++ 여기에 enemy에게 데미지 적용하는 라인 추가 //if (hitEnemies.Contains(enemy))    {Debug.Log("추가됨");  }
                enemyDamage.OnDamage(damage, transform.position);
            }
        }
    }

    IEnumerator Parrying()
    {
        canPrrying = false;

        yield return new WaitForSeconds(0.3f);              //휘두루는 모션이 조금은 나올 수 있도록 딜레이
        Player player = GetComponentInParent<Player>();
        player.Parrying();
        if (parryingEffect != null)
        {
            StartCoroutine(parryingEffect.ShakeCamera());
        }

        GameObject effectInastantiate = Instantiate(parryingParticle,transform.position, Quaternion.identity);
        Destroy(effectInastantiate, 1.0f);                  //1초 이상 x

        yield return new WaitForSeconds(parryingCooldown);
        canPrrying=true;
    }

}
