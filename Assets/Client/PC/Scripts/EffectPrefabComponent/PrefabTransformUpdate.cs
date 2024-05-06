using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabTransformUpdate : MonoBehaviour
{
    PrefabCreator creator;               //프리팹 생성 시 추가된 컴포넌트
    
    void Start()
    {
        creator=GetComponent<PrefabCreator>();
    }

    
    void Update()
    {
        //프리팹 생성한 오브젝트의 rotation과 프리팹 rotation 동기화
        transform.rotation=creator.creatorParentTransform.transform.rotation;
    }
}
