using ExitGames.Client.Photon;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.VFX;
using Photon.Pun;

public class Weapon : MonoBehaviourPun
{

    public enum WeaponType { Melee, Range };
    public WeaponType type { get; private set; }
    public int weapon_damage = 20; //무기별 공격력
    public int result_damage; //최종 데미지
    public BoxCollider meleeArea;   //무기의 공격 판정 범위
    private HashSet<GameObject> hitEnemies = new HashSet<GameObject>();
    public bool isHeavyAttack = false;

    public Player player;
    public PlayerStatus status;
    public CameraShake cameraShaking;
    public AttackController attackController;
    WeaponSoundEffect soundEffect;

    [SerializeField] GameObject effectPrefab;//공격 이펙트 프리팹
    [SerializeField] GameObject shieldEffectPrefab;//방어 이펙트 프리팹
    [SerializeField] GameObject strongEffectPrefab;//필살기 이펙트 프리팹
    [SerializeField] GameObject hitEffectPrefab; //타격시 이펙트 프리팹
    [SerializeField] GameObject hitEffectPrefab2;//타격시 이펙트 프리팹 2(임시)
    GameObject nEffectPrefab;
    public Transform HandEffect;       //자식 오브젝트의 핸드 이펙트 프리팹 할당해야 함
    public bool isShield=false;



    //패링
    public bool parryingAttack;

    public GameObject parryingParticle;     //패링 파티클 프리팹
    float parryingCooldown = 3.0f;          //패링 쿨타임
    
    bool canPrrying = true;
    public Vector3 parryingPos;

    private void Awake()
    {

        player = GetComponentInParent<Player>();
        status = GetComponentInParent<PlayerStatus>();
        attackController = GetComponentInParent<AttackController>();
        soundEffect = GetComponentInChildren<WeaponSoundEffect>();
        cameraShaking = Camera.main.GetComponent<CameraShake>();
        if(HandEffect!=null)    HandEffect.gameObject.SetActive(false);

    }

    private void Update()
    {
        
    }
    public void Use(int key)
    {
        if (type == WeaponType.Melee)
        {
            StopCoroutine(Weapon_Activation());
            hitEnemies.Clear();                         //HashSet 초기화, 공격이 새롭게 시작될 때 마다 초기화.
            result_damage = status.basicStats.atk + weapon_damage + key;
            StartCoroutine(Weapon_Activation());
            if (!isHeavyAttack) soundEffect.PlayWeaponSound("Swing");
            else soundEffect.PlayWeaponSound("HeavySwing");
        }

        if (type == WeaponType.Range)
        {
            StopCoroutine(Weapon_Activation());
            hitEnemies.Clear();                         //HashSet 초기화, 공격이 새롭게 시작될 때 마다 초기화.
            StartCoroutine(Weapon_Activation());
            
        }
    }
    public void AttackOut()
    {
        meleeArea.enabled = false; 
    }


    IEnumerator Weapon_Activation()
    {
        meleeArea.enabled = true; 
        yield return null;
    }


    public void EffectInstance(bool reverse)                
    {
        
        if (effectPrefab == null)
        {
            return;
        }

        if (reverse)
        {
            GameObject effectInstance = Instantiate(effectPrefab, transform.position, transform.rotation);
            PrefabCreator info = effectInstance.AddComponent<PrefabCreator>();//프리팹 생성되면 생성한 오브젝트(플레이어 캐릭터)의 transform 받아오기
            info.isStrong = attackController.stepupBuffer;                                 //어쌔신 전용. 어쌔신 강화 중 인지 체크
            info.attackNum = CurAttackKey();
            info.result_damage= result_damage;
            info.weapon = this;
            Destroy(effectInstance, 1.0f);
        }
        if (!reverse)
            {
            Quaternion reverseRoation = Quaternion.Euler(0, 0, 180);
            GameObject effectInstance = Instantiate(effectPrefab, transform.position, transform.rotation* reverseRoation);
            PrefabCreator info = effectInstance.AddComponent<PrefabCreator>();//프리팹 생성되면 생성한 오브젝트(플레이어 캐릭터)의 transform 받아오기
            info.isStrong = attackController.stepupBuffer;                                //어쌔신 전용. 어쌔신 강화 중 인지 체크
            info.attackNum = CurAttackKey();
            info.result_damage = result_damage;
            info.weapon = this;
            Destroy(effectInstance, 1.0f);
        }
    }

    public void StrongEffectInstance()
    {
        GameObject effectInstance = Instantiate(strongEffectPrefab, (transform.position), transform.rotation);
        Destroy(effectInstance, 3.0f);
    }

    public void ShieldEffectInstance()
    {
        if (shieldEffectPrefab == null)
        {
            return;                                   
        }

        if (!isShield)
        {
            Quaternion reverseRoation = Quaternion.Euler(0, 1, 0);
            nEffectPrefab = Instantiate(shieldEffectPrefab, transform.position + new Vector3(0.0f,0.0f,-1.0f),transform.rotation* reverseRoation);
            PrefabCreator info = nEffectPrefab.AddComponent<PrefabCreator>();//프리팹 생성되면 생성한 오브젝트(플레이어 캐릭터)의 transform 받아옴
            info.creatorParentTransform = player.transform;                  //쉴드 플레이어 기준으로 회전
            isShield = true;
        }           
    }
    public void ShieldEffectOut()
    {
        if(nEffectPrefab != null)
        {
            Destroy(nEffectPrefab);
            nEffectPrefab = null;
            isShield = false;
        }
    }

    //공격 적용 트리거
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "EnemyWeapon" && parryingAttack)
        {
            parryingPos = other.ClosestPointOnBounds(transform.position);
            attackController.Parrying();
            StartCoroutine(Parrying());
        }

        if (other.tag == "MonsterEnemy" || other.tag == "Player" || other.tag == "Enemy")
        {
            GameObject enemy = other.gameObject;

            if (other.tag == "MonsterEnemy")
            {
                Monster enemyDamage = enemy.GetComponentInParent<Monster>();
                if (!hitEnemies.Contains(enemy)) // 이미 공격한 적이 아니라면
                {
                    hitEnemies.Add(enemy); // 이 적을 공격한 적 목록에 추가 //enemyDamage.curHP -= damage;//++ 여기에 enemy에게 데미지 적용하는 라인 추가 //if (hitEnemies.Contains(enemy))    {Debug.Log("추가됨");  }
                    enemyDamage.TakeDamage((result_damage));
                    GameObject hiteffectInstance = Instantiate(hitEffectPrefab, other.ClosestPointOnBounds(transform.position), Quaternion.identity);
                    GameObject hiteffectInstance2 = Instantiate(hitEffectPrefab2, other.ClosestPointOnBounds(transform.position), Quaternion.identity);
                    soundEffect.PlayWeaponSound("Monster");
                    Destroy(hiteffectInstance, 0.5f);
                    Destroy(hiteffectInstance2, 0.5f);
                    if (isHeavyAttack) //강공격일 경우 피격 반응 애니메이션 처리
                    {
                        enemyDamage.HitResponse();
                    }

                    if (parryingAttack)
                    {
                        enemyDamage.Parried();
                    }
                }
            }
            else if (other.tag == "Player"&& !(other.gameObject.GetComponent<PhotonView>().IsMine))
            {
                if (enemy == player.gameObject) { return; }

                CombatStatusManager enemyDamage = enemy.GetComponent<CombatStatusManager>();
                if (!hitEnemies.Contains(enemy)) // 이미 공격한 적이 아니라면
                {
                    hitEnemies.Add(enemy); // 이 적을 공격한 적 목록에 추가 //enemyDamage.curHP -= damage;//++ 여기에 enemy에게 데미지 적용하는 라인 추가 //if (hitEnemies.Contains(enemy))    {Debug.Log("추가됨");  }
                    enemyDamage.TakeDamage((result_damage));
                    GameObject hiteffectInstance = Instantiate(hitEffectPrefab, other.ClosestPointOnBounds(transform.position), Quaternion.identity);
                    soundEffect.PlayWeaponSound("Monster");
                    Destroy(hiteffectInstance, 0.5f);

                    if (isHeavyAttack) //여기서 HeavyAttack false로 하면, 이후 피격 대상들이 적용 안됨
                    {
                        enemyDamage.HitResponse();
                    }
                }
            }

            else if (other.tag == "Enemy")
            {
                
                Hit_Test_v2 enemyDamage = enemy.GetComponent<Hit_Test_v2>();
                if (!hitEnemies.Contains(enemy)) // 이미 공격한 적이 아니라면
                {
                    hitEnemies.Add(enemy); // 이 적을 공격한 적 목록에 추가 //enemyDamage.curHP -= damage;//++ 여기에 enemy에게 데미지 적용하는 라인 추가 //if (hitEnemies.Contains(enemy))    {Debug.Log("추가됨");  }
                    enemyDamage.TakeDamage((result_damage));
                    GameObject hiteffectInstance = Instantiate(hitEffectPrefab, other.ClosestPointOnBounds(transform.position), Quaternion.identity);
                    soundEffect.PlayWeaponSound("Monster");
                    Destroy(hiteffectInstance, 0.5f);

                    if(isHeavyAttack) { Debug.Log("강공격 들어감"); }
                }
            }

            else return;
        }

        else return;
    }

    IEnumerator Parrying()
    {
        yield return new WaitForSeconds(0.3f);              //휘두루는 모션이 조금은 나올 수 있도록 딜레이
        GameObject effectInastantiate = Instantiate(parryingParticle,parryingPos, Quaternion.identity);
        Destroy(effectInastantiate, 1.0f);                  //1초 이상 x
    }


    private int CurAttackKey()
    {
        Animator anim = player.anim;

        if (anim != null && anim.GetCurrentAnimatorStateInfo(0).IsName("Combi1")) {return  1; }
        else if (anim != null && anim.GetCurrentAnimatorStateInfo(0).IsName("Combi2")) { return  2; }
        else if (anim != null && anim.GetCurrentAnimatorStateInfo(0).IsName("Combi3")) { return  3; }
        else if (anim != null && anim.GetCurrentAnimatorStateInfo(0).IsName("SingleRightAttack")) { return 4; }
        else if (anim != null && anim.GetCurrentAnimatorStateInfo(0).IsName("JumpAttack")) { return 5; }
        else { return 0; }
    }


    public HashSet<GameObject> GethitEnemeies()
    {
        return hitEnemies;
    }

    public void AddToHitEnemeies(GameObject enemy)
    {
        hitEnemies.Add(enemy);
    }

}
