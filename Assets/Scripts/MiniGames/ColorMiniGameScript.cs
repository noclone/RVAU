using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ColorMiniGameScript : MonoBehaviour
{
    public GameObject[][] colorButtons;
    public Color[][] solutionColorPlacement;
    public GameObject pcButtonsContainer;
    public GameObject vrButtonsContainer;
    public string colorButtonTag = "ColorButton";

    // Start is called before the first frame update
    void Start()
    {
        InitializeSolutionColors();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void InitializeSolutionColors()
    {
        Transform vrButtonsContainerTransform = vrButtonsContainer.transform;
        GameObject[] vrButtonsWithTag = vrButtonsContainerTransform.GetComponentsInChildren<Transform>()
            .Where(t => t.CompareTag(colorButtonTag))
            .Select(t => t.gameObject)
            .ToArray();

        colorButtons = new GameObject[3][];
        solutionColorPlacement = new Color[3][];

        for (int i = 0; i < 3; i++)
        {
            colorButtons[i] = new GameObject[3];
            solutionColorPlacement[i] = new Color[3];
            for (int j = 0; j < 3; j++)
            {
                string buttonName = "Button" + i + j;

                colorButtons[i][j] = System.Array.Find(vrButtonsWithTag, b => b.name == buttonName);
            }
        }

        // Arbitrary determined solution colors
        Color[] solutionColors = new Color[6];
        solutionColors[0] = Color.red;
        solutionColors[1] = Color.green;
        solutionColors[2] = Color.blue;
        solutionColors[3] = new Color(127f / 255f, 0f, 0f);
        solutionColors[4] = new Color(0f, 127f / 255f, 0f);
        solutionColors[5] = new Color(0f, 0f, 127f / 255f);

        for (int i = solutionColors.Length - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            Color temp = solutionColors[i];
            solutionColors[i] = solutionColors[randomIndex];
            solutionColors[randomIndex] = temp;
        }
        
        int colorIndex = 0;
        int[] unknownColorIndices = new int[3];
        for (int i = 0; i < colorButtons.Length; i++)
        {
            unknownColorIndices[i] = Random.Range(0, 3);

            for (int j = 0; j < colorButtons[i].Length - 1; j++)
            {
                if (j == unknownColorIndices[i])
                {
                    colorButtons[i][j].GetComponent<Image>().color = Color.white;
                    colorButtons[i][j].GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "?";
                }
                else
                    colorButtons[i][j].GetComponent<Image>().color = solutionColors[colorIndex];

                solutionColorPlacement[i][j] = solutionColors[colorIndex];
                colorIndex++;
            }

            Color combinedColor = AddColors(colorButtons[i][0].GetComponent<Image>().color, colorButtons[i][1].GetComponent<Image>().color);
            if (2 == unknownColorIndices[i])
                {
                    colorButtons[i][2].GetComponent<Image>().color = Color.white;
                    colorButtons[i][2].GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "?";
                }
                else
                    colorButtons[i][2].GetComponent<Image>().color = combinedColor;

            solutionColorPlacement[i][colorButtons[i].Length - 1] = combinedColor;
        }
    }

    Color AddColors(Color color1, Color color2)
    {
        float r = Mathf.Clamp01((color1.r + color2.r) / 2);
        float g = Mathf.Clamp01((color1.g + color2.g) / 2);
        float b = Mathf.Clamp01((color1.b + color2.b) / 2);

        return new Color(r, g, b);
    }
}
