using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VFXUpdatePos : MonoBehaviour
{
    // Start is called before the first frame update
    public VisualEffect visualEffect;
    public Transform target;
    
    public Vector3 updatePos;
    public Vector3 originPos;
    private void Start()
    {
        visualEffect = GetComponent<VisualEffect>();
        originPos = target.position;
    }
    // Update is called once per frame
    void Update()
    {
        updatePos = target.position - originPos;
        visualEffect.SetVector3("updatePos", updatePos);
    }
}
