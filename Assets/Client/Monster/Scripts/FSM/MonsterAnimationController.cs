using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAnimationController : MonoBehaviour
{
    private Animator anim;
    [SerializeField] private AnimationClip[] attackClips;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        foreach (var clip in anim.runtimeAnimatorController.animationClips)
        {
            // walk 애니메이션에 대해서만 핸들러가 작동하는 듯
            AnimationEvent endEvent = new AnimationEvent();
            endEvent.time = clip.length;
            endEvent.functionName = "AnimationEndHandler";
            endEvent.stringParameter = clip.name;

            clip.AddEvent(endEvent);
            Debug.Log($"event의 time:{endEvent.time}, event의 pram:{endEvent.stringParameter}");
        }
    }
    public void AnimationEndHandler(string name)
    {
        Debug.Log($"이벤트 출력 확인 - 현재 클립: {name}");
    }
    public void testHandler(string name)
    {

        // 인스펙터 상에서 1 가까이에 두면 호출 안되고, 0.8 이하로 두면 호출됨.. 이유를 찾아야 할 듯
        Debug.Log("공격 테스트");
    } 
}
