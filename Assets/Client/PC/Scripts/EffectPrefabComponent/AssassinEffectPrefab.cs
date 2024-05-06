using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssassinEffectPrefab : MonoBehaviour
{

    bool isStrong;
    PrefabCreator creator;
    int attackNum;
    // Start is called before the first frame update
    void Start()
    {
        creator = GetComponent<PrefabCreator>();
        isStrong = creator.isStrong;                        //활성화 타이밍 안맞으면 실패할 수도 있음
        attackNum = creator.attackNum;
        InitialTransform();

        Transform flameSlashSecond = transform.Find("flame slash-second");      //켜졌다 꺼졌다하는 오브젝트 스스로 수행할 수도 있을듯?
        if (flameSlashSecond != null)
        {
            Transform flameSlashFirst = transform.Find("flame slash-white");
            flameSlashFirst.gameObject.SetActive(!isStrong);
            flameSlashSecond.gameObject.SetActive(isStrong);
        }
        else return;
    }

    void InitialTransform()
    {
        switch (attackNum)
        {
            case 1:
                //transform.rotation=Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
                transform.localScale = new Vector3(2.2f, 2.2f, 2.2f);

                break;
            case 2:
                //transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + 160.0f, transform.rotation.eulerAngles.z);
                transform.localScale = new Vector3(2.2f, 2.2f, 2.2f);
                break;
            case 3:
                //transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + 160.0f, transform.rotation.eulerAngles.z);
                transform.localScale = new Vector3(2.2f, 2.2f, 2.2f);
                break;
            case 4:
                //transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + 40.0f, transform.rotation.eulerAngles.z);
                transform.localScale = new Vector3(2.2f, 2.2f, 2.2f);
                break;
            case 5:
                //transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + 160.0f, transform.rotation.eulerAngles.z);
                break;
        }
    }
}
