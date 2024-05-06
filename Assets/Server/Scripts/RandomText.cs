using UnityEngine;
using UnityEngine.UI; 

public class RandomText : MonoBehaviour
{
    public Text textComponent; 
    public string[] randomTexts = { "�ؽ�Ʈ1", "�ؽ�Ʈ2", "�ؽ�Ʈ3" }; 

    void Start()
    {
        ChangeText();
    }

    void ChangeText()
    {
        int index = Random.Range(0, randomTexts.Length);
        textComponent.text = randomTexts[index];
    }
}
