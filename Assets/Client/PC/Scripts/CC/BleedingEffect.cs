using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BleedingEffect : StatusEffect
{
    private float damagePerSecond = 5f; // 초당 데미지
    private float damageInterval = 1f; // 데미지 간격(1초)
    private float lastDamageTime = 0f; // 마지막 데미지 준 시간


    public override void OnStart()
    {
        base.OnStart();
        lastDamageTime = Time.time; // 현재 시간 기록 후 시작
        // 출혈 이펙트 부여

        // 대상의 status 관련 컴포넌트를 가져와 변수에 저장
    }
    public override void OnUpdate()
    {
        base.OnUpdate();
        if (Time.time - lastDamageTime > damageInterval) // 현재시간 - 마지막 데미지 시간 > 데미지 간격 시간
        {
            // 출혈데미지 부여
            ApplyBlood();
            lastDamageTime = Time.time;
        }
    }
    public override void OnExit()
    {
        base.OnExit();
        // 출혈 이펙트 삭제
    }
    private void ApplyBlood()
    {
        // status 컴포넌트의 체력을 가져와서 깎기
        //gameObject.GetComponent
    }
}
