using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorMiniGameScript : MonoBehaviour
{
    public GameObject[][] colorButtons;
    public GameObject buttonsContainer;
    public string colorButtonTag = "ColorButton";

    // Start is called before the first frame update
    void Start()
    {
        GameObject[] buttonsWithTag = GameObject.FindGameObjectsWithTag(colorButtonTag);
        colorButtons = new GameObject[3][];

        for (int i = 0; i < 3; i++)
        {
            colorButtons[i] = new GameObject[3];
            for (int j = 0; j < 3; j++)
            {
                string buttonName = "Button" + i + j;

                colorButtons[i][j] = System.Array.Find(buttonsWithTag, b => b.name == buttonName);
            }
        }

        Color[] solutionColors = new Color[6];
        solutionColors[0] = Color.red;
        solutionColors[1] = Color.green;
        solutionColors[2] = Color.blue;
        solutionColors[3] = new Color(127f / 255f, 0f, 0f);
        solutionColors[4] = new Color(0f, 127f / 255f, 0f);
        solutionColors[5] = new Color(0f, 0f, 127f / 255f);

        // Shuffle the colors using the Fisher-Yates algorithm
        for (int i = solutionColors.Length - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            Color temp = solutionColors[i];
            solutionColors[i] = solutionColors[randomIndex];
            solutionColors[randomIndex] = temp;
        }

        int colorIndex = 0;

        for (int i = 0; i < colorButtons.Length; i++)
        {
            for (int j = 0; j < colorButtons[i].Length - 1; j++)
            {
                colorButtons[i][j].GetComponent<Image>().color = solutionColors[colorIndex];
                colorIndex++;
            }

            colorButtons[i][colorButtons[i].Length - 1].GetComponent<Image>().color = 
            AddColors(colorButtons[i][0].GetComponent<Image>().color, colorButtons[i][1].GetComponent<Image>().color);

            Debug.Log(colorButtons[i][0].name + " :" + colorButtons[i][0].GetComponent<Image>().color);
            Debug.Log(colorButtons[i][1].name + " :" + colorButtons[i][1].GetComponent<Image>().color);
            Debug.Log(colorButtons[i][2].name + " :" + colorButtons[i][2].GetComponent<Image>().color);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    Color AddColors(Color color1, Color color2)
    {
        float r = Mathf.Clamp01((color1.r + color2.r) / 2);
        float g = Mathf.Clamp01((color1.g + color2.g) / 2);
        float b = Mathf.Clamp01((color1.b + color2.b) / 2);

        return new Color(r, g, b);
    }
}
