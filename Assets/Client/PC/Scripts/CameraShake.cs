using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public Transform mainCamera;
    public bool shakeRotate = false;

    public Vector3 originPos;
    public Quaternion originRot;

    private float defaultZoomInTime = 0.2f; // 줌 인에 걸리는 시간
    private float defaultZoomOutTime = 0.2f; // 줌 아웃에 걸리는 시간
    private float defaultZoomWaitTime = 0.0f; // 줌을 유지하는 시간
    private float defaultZoomAmount = 30f; // 줌 인/아웃의 정도 (FOV 변경량)

    bool isZoom;
    bool isShake;
    private int defence=0;
    public void SetPlayer(GameObject clone)
    {
        
        mainCamera = clone.GetComponent<Transform>();
        originPos = mainCamera.localPosition;
        originRot = mainCamera.localRotation;
    }
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = this.transform;
        originPos = mainCamera.localPosition;
        originRot = mainCamera.localRotation;

    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.mode == 1&& defence ==0)
        {
            Shaking(4f, 5f);
            defence = 1;
        }
    }
    public void Shaking(float shakeTime = 0.1f, float amount = 5.0f)
    {
        if (!isShake)
        {
            StartCoroutine(Shake(shakeTime,amount));
        }
    }

    public void Zoom(float zoomInTime = 0.2f, float zoomOutTime = 0.2f, float zoomWaitTime = 0.0f, float zoomAmount = 30.0f)
    {
        if (!isZoom)
        {
            StartCoroutine(ZoomCamera(zoomInTime, zoomOutTime, zoomWaitTime, zoomAmount));
        }
    }

    public IEnumerator Shake(float shakeTime, float amount) //피격, 충돌 등 다양하게 사용
    {
        isShake = true;
        Vector3 originPoint = mainCamera.localPosition;
        float elapsedTime = 0.0f;

        while (elapsedTime < shakeTime)
        {
            Vector3 randomPoint = originPoint + Random.insideUnitSphere * amount;//흔드는정도
            mainCamera.localPosition = Vector3.Lerp(mainCamera.localPosition, randomPoint, Time.deltaTime * 1.0f);

            yield return null;

            elapsedTime += Time.deltaTime;
        }
        mainCamera.localPosition = originPoint;
        isShake = false;
    }


    //1. 줌인 걸리는 시간, 2. 줌 아웃 걸리는 시간, 3. 줌이 유지되는 시간  4. 줌이되는 정도 FOV의 변경량
    public IEnumerator ZoomCamera(float zoomInTime, float zoomOutTime, float zoomWaitTime, float zoomAmount)    //큰 공격 시 일시적인 줌인 효과
    {
        isZoom = true;
        float originalFoV = mainCamera.GetComponent<Camera>().fieldOfView;
        float zoomedFoV = originalFoV - zoomAmount; // 줌 인 시 FOV 감소
        float elapsedTime = 0.0f;

        // 줌 인
        while (elapsedTime < zoomInTime)
        {
            mainCamera.GetComponent<Camera>().fieldOfView = Mathf.Lerp(originalFoV, zoomedFoV, elapsedTime / zoomInTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(zoomWaitTime);

        elapsedTime = 0.0f; // 시간 리셋

        // 줌 아웃
        while (elapsedTime < zoomOutTime)
        {
            mainCamera.GetComponent<Camera>().fieldOfView = Mathf.Lerp(zoomedFoV, originalFoV, elapsedTime / zoomOutTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        isZoom = false;
    }

}
