using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BuildTest : MonoBehaviourPun
{

    public GameObject buildingPrefab; // 구조물 요소 프리팹
    public float maxHeight = 1.5f; // 구조물을 설치할 수 있는 최대 높이
    public float raycastDistance = 1f; // 레이를 쏠 거리
    public float distance = 1f; // 구조물 설치 위치
    private bool isBuilding = false;
    public GameObject previewPrefab;
    public bool isPreviewCol =false;
    private GameObject preview;
    public GameObject[] buildArray = new GameObject[4];
    public GameObject[] previewArray = new GameObject[4];

    public float rotationSpeed = 50f;
    private float height = 0;

    void Start()
    {
        //Instantiate(buildingPrefab, new Vector3(1, 1, 1), Quaternion.identity);
        buildingPrefab = buildArray[0];
        previewPrefab = previewArray[0];
    }
    
    void Update()
    {
     
        if (Input.GetKeyDown(KeyCode.R))
        {
            //r을 누르면 건축 모드로 변경이 목표.
            
           
            if (!isBuilding)
            {
                PlacePreview();
            }
            else
            {
          
                DestroyPreview();
            }
            isBuilding = !isBuilding;
            Debug.Log("모드 전환");
        }

        if (isBuilding) 
        {
            
            if (Input.GetKeyDown(KeyCode.T))
            {
                Build();
                
                Debug.Log("설치");
            }
            if (Input.GetKey(KeyCode.Q))
            {
                RotateObject(-1); // 시계 방향으로 회전
            }

            // "E" 키를 누르고 있으면 자식 객체를 반시계 방향으로 지속적으로 회전시킴
            if (Input.GetKey(KeyCode.E))
            {
                RotateObject(1); // 반시계 방향으로 회전
            }
            switch (Input.inputString)
            {
                case "1":
                    ChangeBuild(0);
                    break;
                case "2":
                    ChangeBuild(1);
                    break;
                case "3":
                    ChangeBuild(2);
                    break;
                case "4":
                    ChangeBuild(3);
                    break;
            }
            MovePreview();
            //생성이 빨라서 튕기는 경우 존재...
        }

    }

    void ChangeBuild(int x)
    {
        buildingPrefab= buildArray[x];
        previewPrefab= previewArray[x];
        DestroyPreview();
        PlacePreview();
    }


    void RotateObject(int direction)
    {

        float rotationAmount = rotationSpeed * direction * Time.deltaTime;


        preview.transform.Rotate(Vector3.up, rotationAmount);
    }
    void RotateReset()
    {
     
    }

    void Build()
    {
        DestroyPreview();
        //PhotonNetwork.Instantiate(buildingPrefab.name, preview.transform.position, preview.transform.rotation);
      Instantiate(buildingPrefab, preview.transform.position, preview.transform.rotation);

        PlacePreview();

    }


    void DestroyPreview()
    {
        if (preview != null)
        {
            Destroy(preview);
        }
    }
    void OnDrawGizmos()
    {
        Debug.DrawRay(transform.position + Vector3.forward * distance + Vector3.up * maxHeight, Vector3.down * raycastDistance, Color.red);
    }

    public void PlacePreview()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position + transform.forward * distance+Vector3.up*maxHeight, Vector3.down, out hit, maxHeight+raycastDistance))
        {
        
            Vector3 buildingPosition = hit.point;
            BoxCollider boxCollider = buildingPrefab.GetComponent<BoxCollider>();

         
            if (boxCollider == null)
            {
                Debug.LogError("BoxCollider not found on specified GameObject.");
            }
      
            height = boxCollider.size.y;
            Vector3 offset = new Vector3(0f, height/2, 0f);

            Debug.Log(height);
            Debug.Log(hit.point);
            preview =Instantiate(previewPrefab, buildingPosition,transform.rotation* Quaternion.Euler(new Vector3(0, 90, 0)));
            
           
        }
    }
    public void MovePreview()
    {
        
       
        RaycastHit hit;
        int layerMask = (-1) - (1 << LayerMask.NameToLayer("IgnoreRaycast"));

        if (Physics.Raycast(preview.transform.position + Vector3.up * 3f, Vector3.down, out hit, layerMask))
        {
            // 바닥과 충돌한 경우
            Vector3 floorPosition = hit.point;
           
       
            // 자식 객체의 위치를 바닥의 위치로 이동시킴
           
            //transform.position = transform.position+Vector3.up*floorPosition.y+offset;
            preview.transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z)+ transform.forward * distance;
            preview.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal)* transform.rotation * Quaternion.Euler(new Vector3(0, 90, 0));

            Debug.Log(transform.rotation);
            Debug.Log(hit.normal);

        }
     

    }

}
