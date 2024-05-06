using UnityEngine;
using UnityEngine.UI; 

public class RandomText : MonoBehaviour
{
    public Text textComponent; 
    public string[] randomTexts = { "텍스트1", "텍스트2", "텍스트3" }; 

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
