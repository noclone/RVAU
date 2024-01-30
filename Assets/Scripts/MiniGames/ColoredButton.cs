using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColoredButtons : MonoBehaviour
{
    public GameObject scriptHolder;
    private SerializableColor[][] solutionColorPlacement;
    private SerializableColor[] colorList;
    private Button button;
    private int currentColorIndex = 0;
    private ColorMiniGameScript gameScript;

    void Start()
    {
        gameScript = scriptHolder.GetComponent<ColorMiniGameScript>();
        button = GetComponent<Button>();
        button.onClick.AddListener(ChangeColorOnClick);
    }

    // On click, change color of button by the next color in the list and loop
    void ChangeColorOnClick()
    {
        if (colorList == null)
            colorList = gameScript.solutionColors;
        button.image.color = new Color(colorList[currentColorIndex].r, colorList[currentColorIndex].g, colorList[currentColorIndex].b);
        currentColorIndex = (currentColorIndex + 1) % colorList.Length;
    }
}