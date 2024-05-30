using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 해당 컴포넌트는 상태이상 컴포넌트
 * 미리 오브젝트에 할당해두는 것이아닌 동적으로 할당할 목적으로 설계
 */
public abstract class StatusEffect : MonoBehaviour
{

    public float Duration
    {
        get { return duration; }
        set { duration = value; }
    }

    protected float duration; // 지속 시간
    private float currentTime = 0f; // 현재 시간

    // 처음 상태이상 진입할 때 호출
    public virtual void OnStart()
    {

    }
    // 상태이상 지속 시간 동안 호출
    public virtual void OnUpdate()
    {

    }
    // 상태이상 지속 시간 종료 뒤 호출
    public virtual void OnExit()
    {

    }
    // 상태 이상 발현 시 호출되는 함수
    private void Update()
    {
        currentTime += Time.deltaTime;

        if(currentTime < duration) // 지속시간 내
        {
            OnUpdate();
        }
        else // 지속시간 후
        {
            OnExit();
            Destroy(this); // StatusEffect 컴포넌트 삭제
        }
    }
}
