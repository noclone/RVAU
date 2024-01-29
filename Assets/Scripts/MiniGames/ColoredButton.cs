using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColoredButtons : MonoBehaviour
{
    public GameObject scriptHolder;
    private Color[][] solutionColorPlacement;
    private Color[] colorList;
    private Button button;
    private int currentColorIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        ColorMiniGameScript gameScript = scriptHolder.GetComponent<ColorMiniGameScript>();

        if (gameScript != null)
            colorList = gameScript.solutionColors;

        button = GetComponent<Button>();
        button.onClick.AddListener(ChangeColorOnClick);
    }

    // Update is called once per frame
    void Update()
    {

    }

    // On click, change color of button by the next color in the list and loop
    void ChangeColorOnClick()
    {
        Debug.Log("Changing color");
        button.image.color = colorList[currentColorIndex];
        currentColorIndex = (currentColorIndex + 1) % colorList.Length;
    }
}
