using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class previewTest : MonoBehaviour
{
    private Renderer previewRenderer;
    private Material originalMaterial;
    private Material collisionMaterial;


    // Start is called before the first frame update
    void Start()
    {
        previewRenderer = GetComponentInChildren<Renderer>();
        // 미리보기의 기본 머티리얼을 저장
        originalMaterial = previewRenderer.material;
      
      
    }

    private void Update()
    {
        // 바닥의 위치를 확인하기 위한 레이캐스트
        RaycastHit hit;
        if (Physics.Raycast(transform.position+Vector3.up*3f, Vector3.down, out hit))
        {
            // 바닥과 충돌한 경우
            Vector3 floorPosition = hit.point;
            Debug.Log(hit.point);
            // 자식 객체의 위치를 바닥의 위치로 이동시킴

            BoxCollider boxCollider = GetComponent<BoxCollider>();

            // 박스 콜라이더가 존재하는지 확인
            if (boxCollider == null)
            {
                Debug.LogError("BoxCollider not found on specified GameObject.");
            }
            // 건물을 바닥에 배치하기 위해 건물의 높이 절반을 더함
            float height = boxCollider.size.y;
            Vector3 offset = new Vector3(0f, height / 2, 0f);
            //transform.position = transform.position+Vector3.up*floorPosition.y+offset;
            Debug.Log(transform.position);
        }
        
    }
   

    // Update is called once per frame
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("충돌");
        // 충돌한 오브젝트가 벽인 경우에만 처리
        if (!collision.gameObject.CompareTag("floor"))
        {
            // previewPrefab의 머티리얼을 가져옴
            collisionMaterial = originalMaterial;

            // 새로운 색상을 만들고 투명도 설정
            Color color = collisionMaterial.color;
            color.a = 0.5f;

            // 변경된 색상을 머티리얼에 적용
            collisionMaterial.color = color;
            // 색상을 변경하여 충돌 여부를 표시
            previewRenderer.material = collisionMaterial;
        }
    }

    // 충돌이 종료될 때 호출됨
    void OnCollisionExit(Collision collision)
    {
        // 기존 색상으로 되돌림
        previewRenderer.material = originalMaterial;
    }
}
