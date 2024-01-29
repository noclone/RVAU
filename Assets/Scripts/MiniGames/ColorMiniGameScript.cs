using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;

public class ColorMiniGameScript : MonoBehaviour
{
    private GameObject[][] vrColorButtons;
    public GameObject vrButtonsContainer;

    private GameObject[][] pcColorButtons;
    public GameObject pcButtonsContainer;

    public Color[] solutionColors;
    private Color[][] solutionColorPlacement;
    private int[] unknownColorIndices;
    private string colorButtonTag = "ColorButton";

    // Start is called before the first frame update
    void Start()
    {
        InitializeSolutionColors();
        InitializeVRButtons();
        InitializePCButtons();
    }

    // Update is called once per frame
    void Update()
    {
        CheckVictoryConditions();
    }

    void CheckVictoryConditions()
    {
        bool victory = true;
        for (int i = 0; i < solutionColorPlacement.Length; i++)
        {
            for (int j = 0; j < solutionColorPlacement[i].Length; j++)
            {
                if (pcColorButtons[i][j].GetComponent<Image>().color != solutionColorPlacement[i][j])
                {
                    victory = false;
                    break;
                }
            }
        }

        if (victory)
        {
            Debug.Log("Victory!");
            Time.timeScale = 0f;
        }
            
    }

    void InitializeSolutionColors()
    {
        // Arbitrary determined solution colors
        Color[] baseColors = new Color[6];
        baseColors[0] = Color.red;
        baseColors[1] = Color.green;
        baseColors[2] = Color.blue;
        baseColors[3] = new Color(127f / 255f, 0f, 0f);
        baseColors[4] = new Color(0f, 127f / 255f, 0f);
        baseColors[5] = new Color(0f, 0f, 127f / 255f);

        baseColors = ShuffleColors(baseColors);
        
        int colorIndex = 0;
        unknownColorIndices = new int[3];
        solutionColorPlacement = new Color[3][];
        for (int i = 0; i < 3; i++)
        {
            solutionColorPlacement[i] = new Color[3];
            unknownColorIndices[i] = Random.Range(0, 3);

            for (int j = 0; j < solutionColorPlacement[i].Length - 1; j++)
            {
                solutionColorPlacement[i][j] = baseColors[colorIndex];
                colorIndex++;
            }

            Color combinedColor = AddColors(solutionColorPlacement[i][0], solutionColorPlacement[i][1]);
            solutionColorPlacement[i][solutionColorPlacement[i].Length - 1] = combinedColor;
        }

        // Add the 3 combined colors and shuffle for Buttons sequences
        solutionColors = new Color[9];
        for (int i = 0; i < solutionColorPlacement.Length; i++)
        {
            for (int j = 0; j < solutionColorPlacement[i].Length; j++)
            {
                solutionColors[i * 3 + j] = solutionColorPlacement[i][j];
            }
        }

        solutionColors = ShuffleColors(solutionColors);
    }

    void InitializeVRButtons()
    {
        Transform vrButtonsContainerTransform = vrButtonsContainer.transform;
        GameObject[] vrButtonsWithTag = vrButtonsContainerTransform.GetComponentsInChildren<Transform>()
            .Where(t => t.CompareTag(colorButtonTag))
            .Select(t => t.gameObject)
            .ToArray();

        vrColorButtons = new GameObject[3][];

        for (int i = 0; i < 3; i++)
        {
            vrColorButtons[i] = new GameObject[3];
            for (int j = 0; j < 3; j++)
            {
                string buttonName = "Button" + i + j;

                vrColorButtons[i][j] = System.Array.Find(vrButtonsWithTag, b => b.name == buttonName);

                if (j == unknownColorIndices[i])
                {
                    vrColorButtons[i][j].GetComponent<Image>().color = Color.white;
                    vrColorButtons[i][j].GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "?";
                }
                else
                    vrColorButtons[i][j].GetComponent<Image>().color = solutionColorPlacement[i][j];
            }
        }
    }

    void InitializePCButtons()
    {
        Transform pcButtonsContainerTransform = pcButtonsContainer.transform;
        GameObject[] pcButtonsWithTag = pcButtonsContainerTransform.GetComponentsInChildren<Transform>()
            .Where(t => t.CompareTag(colorButtonTag))
            .Select(t => t.gameObject)
            .ToArray();

        pcColorButtons = new GameObject[3][];

        for (int i = 0; i < 3; i++)
        {
            pcColorButtons[i] = new GameObject[3];
            for (int j = 0; j < 3; j++)
            {
                string buttonName = "Button" + i + j;

                pcColorButtons[i][j] = System.Array.Find(pcButtonsWithTag, b => b.name == buttonName);
            }
        }
    }

    Color AddColors(Color color1, Color color2)
    {
        float r = Mathf.Clamp01((color1.r + color2.r) / 2);
        float g = Mathf.Clamp01((color1.g + color2.g) / 2);
        float b = Mathf.Clamp01((color1.b + color2.b) / 2);

        return new Color(r, g, b);
    }

    Color[] ShuffleColors(Color[] colors)
    {
        for (int i = colors.Length - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            Color temp = colors[i];
            colors[i] = colors[randomIndex];
            colors[randomIndex] = temp;
        }

        return colors;
    }
}
