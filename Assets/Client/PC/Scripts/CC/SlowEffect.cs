using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowEffect : StatusEffect
{
    private float speedRate = 0.5f;
    private float originSpeed;

    public override void OnStart()
    {
        base.OnStart();
        // 슬로우 이펙트 부여

        // 대상의 이동관련 컴포넌트를 가져와 저장

        // originSpeed 저장

        // 대상의 이동속도 *= speedRate


    }
    public override void OnExit()
    {
        base.OnExit();
        // 슬로우 이펙트 삭제

        // 대상의 이동속도 = originSpeed
    }
}
