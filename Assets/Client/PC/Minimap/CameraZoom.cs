using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CameraZoom : MonoBehaviour
{
    [SerializeField]
    private Camera minimapCamera;
    [SerializeField]
    private float zoomMin = 1;
    [SerializeField]
    private float zoomMax = 20;
    [SerializeField]
    private float zoomlevel = 1;
    [SerializeField]
    private Text mapName;

    private void Awake()
    {
        mapName.text = SceneManager.GetActiveScene().name;       
    }
    // Start is called before the first frame update
    public void ZoomIn()
    {
        minimapCamera.orthographicSize = Mathf.Max(minimapCamera.orthographicSize-zoomlevel, zoomMin);
    }

    public void ZoomOut()
    {
        minimapCamera.orthographicSize = Mathf.Min(minimapCamera.orthographicSize + zoomlevel, zoomMax);
    }
}
