using ExitGames.Client.Photon;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.VFX;

public class Weapon : MonoBehaviour
{

    public enum WeaponType { Melee, Range };
    public WeaponType type;
    public int weapon_damage; //무기별 공격력
    public float weapon_rate; // 무기별 공격 속도
    public BoxCollider meleeArea;   //무기의 공격 판정 범위
    //public TrailRenderer trailEffect; //공격시 생성 이펙트
    private HashSet<GameObject> hitEnemies = new HashSet<GameObject>();

    public Player player;
    public PlayerStatus status;
    public CameraShake cameraShaking;
    public AttackController attackController;

    public GameObject effectPrefab;//공격 이펙트 프리팹
    public GameObject shieldEffectPrefab;//방어 이펙트 프리팹
    public GameObject strongEffectPrefab;
    GameObject nEffectPrefab;
    


    //패링
    public GameObject parryingParticle;     //패링 파티클 프리팹
    float parryingCooldown = 3.0f;          //패링 쿨타임
    bool canPrrying = true;
    public Transform parryingPos;

    private void Awake()
    {
        
        player = GetComponentInParent<Player>();
        status = GetComponentInParent<PlayerStatus>();
        attackController = GetComponentInParent<AttackController>();
        cameraShaking =Camera.main.GetComponent<CameraShake>();
        
        
    }
    public void Use()
    {
        if (type == WeaponType.Melee)
        {
            StopCoroutine(Weapon_Activation());
            hitEnemies.Clear();                         //HashSet 초기화, 공격이 새롭게 시작될 때 마다 초기화.
            Debug.Log("HashSet 클리어");
            StartCoroutine(Weapon_Activation());
        }

        if (type == WeaponType.Range)
        {
            StopCoroutine(Weapon_Activation());
            hitEnemies.Clear();                         //HashSet 초기화, 공격이 새롭게 시작될 때 마다 초기화.
            Debug.Log("HashSet 클리어");
            StartCoroutine(Weapon_Activation());
        }
    }
    public void AttackOut()
    {
        meleeArea.enabled = false;
        //trailEffect.enabled = false;
    }


    IEnumerator Weapon_Activation()
    {
        meleeArea.enabled = true;
        //trailEffect.enabled = true;
      
        yield return null;
    }


    public void EffectInstance(bool reverse)
    {
        Debug.Log(reverse);
        if (effectPrefab == null)
        {
            return;
        }
        if (reverse)
        {
            GameObject effectInstance = Instantiate(effectPrefab, transform.position, transform.rotation);
            Destroy(effectInstance, 1.0f);
        }
        if (!reverse)
            {
            Quaternion reverseRoation = Quaternion.Euler(0, 0, 180);
            GameObject effectInstance = Instantiate(effectPrefab, transform.position, transform.rotation* reverseRoation);
            Destroy(effectInstance, 1.0f);
        }
    }

    public void StrongEffectInstance()
    {
        GameObject effectInstance = Instantiate(strongEffectPrefab, transform.position, transform.rotation);
        Destroy(effectInstance, 3.0f);
    }

    public void ShieldEffectInstance()
    {
        if (!player.isDefense)
        {
            Quaternion reverseRoation = Quaternion.Euler(0, 1, 0);
            nEffectPrefab = Instantiate(shieldEffectPrefab, transform.position + new Vector3(0.0f,0.0f,-1.0f),transform.rotation* reverseRoation); 
            player.isDefense = true;
        }           
    }
    public void ShieldEffectOut()
    {
        if(nEffectPrefab != null)
        {

            Destroy(nEffectPrefab);
            nEffectPrefab = null;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        Debug.LogError("확인");
        if (other.collider.CompareTag("Build"))
        {
            GameObject build = other.gameObject;
            Barricade buildDamage = build.GetComponent<Barricade>();
            if (!hitEnemies.Contains(build)) // 이미 공격한 적이 아니라면
            {
                hitEnemies.Add(build); // 이 적을 공격한 적 목록에 추가 //enemyDamage.curHP -= damage;//++ 여기에 enemy에게 데미지 적용하는 라인 추가 //if (hitEnemies.Contains(enemy))    {Debug.Log("추가됨");  }
                buildDamage.TakeDamage((status.basicStats.atk + weapon_damage), transform.position);
                cameraShaking.Shaking();
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "ParryingBox" && canPrrying)
        {
            attackController.Parrying();
            StartCoroutine(Parrying());
        }

        if (other.tag == "MonsterEnemy"|| other.tag == "Enemy")
        {
            GameObject enemy = other.gameObject;
            Monster enemyDamage = enemy.GetComponent<Monster>();
            if (!hitEnemies.Contains(enemy)) // 이미 공격한 적이 아니라면
            {
                hitEnemies.Add(enemy); // 이 적을 공격한 적 목록에 추가 //enemyDamage.curHP -= damage;//++ 여기에 enemy에게 데미지 적용하는 라인 추가 //if (hitEnemies.Contains(enemy))    {Debug.Log("추가됨");  }
                enemyDamage.TakeDamage((status.basicStats.atk + weapon_damage), transform.position);
                cameraShaking.Shaking();
            }
        }
    }

    IEnumerator Parrying()
    {
        canPrrying = false;
        yield return new WaitForSeconds(0.2f);              //휘두루는 모션이 조금은 나올 수 있도록 딜레이
        //attackController.Parrying();                      //반응이 늦어서 뺌
        cameraShaking.Shaking();

        GameObject effectInastantiate = Instantiate(parryingParticle,parryingPos.position, Quaternion.identity);
        Destroy(effectInastantiate, 1.0f);                  //1초 이상 x
        yield return new WaitForSeconds(parryingCooldown);
        canPrrying = true;

    }

}
