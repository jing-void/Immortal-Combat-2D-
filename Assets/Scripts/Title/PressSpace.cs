using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class PressSpace : MonoBehaviour
{
    Text text;
    float time;
    bool isClickSpace;
    private void Start()
    {
        text = GetComponent<Text>();
    }
    void Update()
    {
        text.color = GetAlphaColor(text.color);

        if (Input.GetKeyDown(KeyCode.Space) && isClickSpace == false)
        {
            isClickSpace = true;
        }
         if (isClickSpace == true)
        {
            time = Time.deltaTime;

            if (time < 7.0f)
            {
                SceneManager.LoadScene("WizardScene");
            }
        }
    }

    Color GetAlphaColor(Color color)
    {
        time += Time.deltaTime * 5.0f;
        color.a = Mathf.Sin(time);

        return color;
    }
}
