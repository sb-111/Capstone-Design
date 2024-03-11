using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParryingEffect : MonoBehaviour
{
    private float shakeTime = 1.3f;
    private Transform cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public IEnumerator ShakeCamera()
    {
        Vector3 originPoint = cam.localPosition;
        float elapsedTime = 0.0f;

        while (elapsedTime<shakeTime)
        {
            Vector3 randomPoint = originPoint + Random.insideUnitSphere * 10.0f;//Èçµå´ÂÁ¤µµ
            cam.localPosition = Vector3.Lerp(cam.localPosition, randomPoint, Time.deltaTime * 1.0f);

            yield return null;

            elapsedTime += Time.deltaTime;
        }
        cam.localPosition = originPoint; 
    }
}
