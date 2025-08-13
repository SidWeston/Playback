using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class UpdateText : MonoBehaviour
{
    public TextMeshProUGUI text;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateTextFromFloat(float newText)
    {
        text.text = (Mathf.Floor(newText * 10) / 10).ToString();
    }
}