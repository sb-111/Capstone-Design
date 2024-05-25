using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterEndSpawn : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject mon;
    public GameObject boss;
    int max;
    int num=0;
    int time;
    void OnEnable()
    {
        
        
    }

    public void GetStarted(int x,int y)
    {
        max = x;
        time = y;
        StartCoroutine("SpawnMon");
    }
    IEnumerator SpawnMon()
    {
        while (max>=num)
          {

           // Instantiate(mon, transform.position + new Vector3(2, 0, 4), transform.rotation);
            Instantiate(mon, transform.position+new Vector3(2,0,0), transform.rotation);
            Instantiate(boss, transform.position + new Vector3(0, 0, 4), transform.rotation);
            Instantiate(mon, transform.position + new Vector3(-2, 0, 0), transform.rotation);
            //Instantiate(mon, transform.position + new Vector3(-2, 0, -4), transform.rotation);
            yield return new WaitForSeconds(time);
            num++;
         }
    }
    // Update is called once per frame
    void Update()
    {
       
    }
}
