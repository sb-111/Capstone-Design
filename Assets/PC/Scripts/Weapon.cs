using System.Collections;
using System.Collections.Generic;
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
    public void Use()
    {
        if(type == WeaponType.Melee)
        {
            StopCoroutine("Swing");
            StartCoroutine("Swing");
            
        }
    }

    IEnumerator Swing()
    {
        yield return new WaitForSeconds(0.1f);
        meleeArea.enabled = true;
        trailEffect.enabled = true;

        yield return new WaitForSeconds(0.3f);          //다른 방법 필요해 보임
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
