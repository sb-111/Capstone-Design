using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullTest : MonoBehaviour
{
    // Start is called before the first frame update
    int phase = 0;
    bool isGame = true;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        Screen.SetResolution(1920, 1080, true);
        StartCoroutine("Defense");
    }
    IEnumerator Defense()
    {
        while (true)
        {
            Debug.Log(phase + "코루틴" + isGame);
            if (phase == 2)
            {
                Debug.Log(phase + "끝");
                yield break;
            }
            else if (!isGame)
            {
                Debug.Log(phase + "라");

                yield return new WaitForSeconds(5);

                phase++;
            }
            else if (isGame)
            {
                Debug.Log(phase + "휴식");
                yield return new WaitForSeconds(2);
            }
            isGame = !isGame;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            Cursor.lockState = CursorLockMode.None;
            Debug.Log("눌림");
        }
        else if (Input.GetKeyUp(KeyCode.LeftAlt))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Debug.Log("안 눌림");
        }

    }
}
