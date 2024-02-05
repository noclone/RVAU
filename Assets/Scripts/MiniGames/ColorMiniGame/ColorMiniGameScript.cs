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

    public SerializableColor[] solutionColors;
    private SerializableColor[][] solutionColorPlacement;
    private int[] unknownColorIndices;
    private string colorButtonTag = "ColorButton";
    private bool victory = false;
    private bool isLoaded;

    public GameObject spawnPointPC;
    public GameObject spawnPointVR;
    public GameObject canvasVR;
    public GameObject canvasPC;

    void Start()
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber == 1)
        {
            GameObject playerPC = PhotonNetwork.Instantiate("Player", spawnPointPC.transform.position, Quaternion.identity);
            Camera camera = playerPC.GetComponent<Camera>();
            Canvas canvas = canvasPC.GetComponent<Canvas>();
            playerPC.GetComponent<NavigationPC>().enabled = false;
            canvas.worldCamera = camera;
            canvasPC.SetActive(true);
        }
        else
        {
            GameObject playerVR = PhotonNetwork.Instantiate("PlayerVR", spawnPointVR.transform.position, Quaternion.identity);
            playerVR.transform.GetChild(1).gameObject.SetActive(false);
            playerVR.GetComponent<Rigidbody>().useGravity = false;
            playerVR.GetComponent<Navigation>().enabled = false;
            Camera camera = playerVR.GetComponent<Camera>();
            Canvas canvas = canvasVR.GetComponent<Canvas>();
            canvas.worldCamera = camera;
            canvasVR.SetActive(true);
        }

        if (PhotonNetwork.IsMasterClient)
        {
            InitializeSolutionColors();
        }
    }

    void Update()
    {
        if (!isLoaded)
            return;
    }

    public void CheckVictoryConditions()
    {
        bool gameVictory = true;
        for (int i = 0; i < solutionColorPlacement.Length; i++)
        {
            for (int j = 0; j < solutionColorPlacement[i].Length; j++)
            {
                if (pcColorButtons[i][j].GetComponent<Image>().color != new Color(solutionColorPlacement[i][j].r, solutionColorPlacement[i][j].g, solutionColorPlacement[i][j].b))
                {
                    gameVictory = false;
                    break;
                }
            }
        }

        if (gameVictory)
        {
            // Set victory to true for all players
            PhotonView.Get(this).RPC("SetVictory", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    void SetVictory()
    {
        PhotonNetwork.LoadLevel("Game");
    }

    void InitializeSolutionColors()
    {
        // Arbitrary determined solution colors
        SerializableColor[] baseColors = new SerializableColor[6];
        baseColors[0] = new SerializableColor(Color.red.r, Color.red.g, Color.red.b);
        baseColors[1] = new SerializableColor(Color.green.r, Color.green.g, Color.green.b);
        baseColors[2] = new SerializableColor(Color.blue.r, Color.blue.g, Color.blue.b);
        baseColors[3] = new SerializableColor(127f / 255f, 0f, 0f);
        baseColors[4] = new SerializableColor(0f, 127f / 255f, 0f);
        baseColors[5] = new SerializableColor(0f, 0f, 127f / 255f);

        baseColors = ShuffleColors(baseColors);

        int colorIndex = 0;
        unknownColorIndices = new int[3];
        solutionColorPlacement = new SerializableColor[3][];
        for (int i = 0; i < 3; i++)
        {
            solutionColorPlacement[i] = new SerializableColor[3];
            unknownColorIndices[i] = Random.Range(0, 3);

            for (int j = 0; j < solutionColorPlacement[i].Length - 1; j++)
            {
                solutionColorPlacement[i][j] = baseColors[colorIndex];
                colorIndex++;
            }

            SerializableColor combinedColor = AddColors(solutionColorPlacement[i][0], solutionColorPlacement[i][1]);
            solutionColorPlacement[i][solutionColorPlacement[i].Length - 1] = combinedColor;
        }

        // Add the 3 combined colors and shuffle for Buttons sequences
        solutionColors = new SerializableColor[9];
        for (int i = 0; i < solutionColorPlacement.Length; i++)
        {
            for (int j = 0; j < solutionColorPlacement[i].Length; j++)
            {
                solutionColors[i * 3 + j] = solutionColorPlacement[i][j];
            }
        }

        solutionColors = ShuffleColors(solutionColors);

        // Send RPC to initialize colors on all clients
        PhotonView.Get(this).RPC("InitSolutionsColors", RpcTarget.AllBuffered, solutionColors, solutionColorPlacement, unknownColorIndices);
    }

    [PunRPC]
    void InitSolutionsColors(SerializableColor[] solutionColors, SerializableColor[][] solutionColorPlacement, int[] unknownColorIndices)
    {
        this.solutionColors = solutionColors;
        this.solutionColorPlacement = solutionColorPlacement;
        this.unknownColorIndices = unknownColorIndices;
        InitializeVRButtons();
        InitializePCButtons();
        isLoaded = true;
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
                    vrColorButtons[i][j].GetComponent<Image>().color = new Color(solutionColorPlacement[i][j].r, solutionColorPlacement[i][j].g, solutionColorPlacement[i][j].b);
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

    SerializableColor AddColors(SerializableColor color1, SerializableColor color2)
    {
        float r = Mathf.Clamp01((color1.r + color2.r) / 2);
        float g = Mathf.Clamp01((color1.g + color2.g) / 2);
        float b = Mathf.Clamp01((color1.b + color2.b) / 2);

        return new SerializableColor(r, g, b);
    }

    SerializableColor[] ShuffleColors(SerializableColor[] colors)
    {
        for (int i = colors.Length - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            (colors[i], colors[randomIndex]) = (colors[randomIndex], colors[i]);
        }

        return colors;
    }
}