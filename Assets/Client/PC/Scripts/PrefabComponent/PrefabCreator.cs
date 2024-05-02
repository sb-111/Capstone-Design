using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabCreator : MonoBehaviour
{
    // Start is called before the first frame update

    public Transform creatorParentTransform;                    //프리팹 생성한 오브젝트의 Transform
    public PrefabTransformUpdate update;                        //프리팹에 있는 PrefabTransformUpdate 컴포넌트(비활성화 상태)

    private void Awake()
    {
        update= GetComponent<PrefabTransformUpdate>();
        update.enabled = true;
    }
}
