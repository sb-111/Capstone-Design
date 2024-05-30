using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGame_Panel : MonoBehaviour
{
    public RectTransform hpBar;
    public Image faceImage;
    public Image classImage;
    public Image buffImage1;
    public Image buffImage2;
    public Image buffImage3;
   

    public PlayerStatus playerstats;

    // Start is called before the first frame update
    void Start()
    {
        playerstats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatus>();
  
    }

   
    void LateUpdate()
    {
        hpBar.localScale = new Vector3((float)playerstats.basicStats.hp / playerstats.basicStats.maxhp, 1, 1);
        
    }
}
