using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject spawnPointPC;
    public GameObject spawnPointVR;
    public GameObject canvasPC;
    public GameObject canvasVR;
    public List<Bulb> bulbs;
    public List<ButtonVR> buttons;

    private List<int> buttonMapping;

    // Start is called before the first frame update
    void Start()
    {
        // if (PhotonNetwork.LocalPlayer.ActorNumber == 1)
        // {
        //     GameObject playerPC = PhotonNetwork.Instantiate("Player", spawnPointPC.transform.position, Quaternion.identity);
        //     Camera camera = playerPC.GetComponent<Camera>();
        //     Canvas canvas = canvasPC.GetComponent<Canvas>();
        //     canvas.worldCamera = camera;
        //     canvasPC.SetActive(true);
        // }
        // else
        // {
        //     GameObject playerVR = PhotonNetwork.Instantiate("PlayerVR", spawnPointVR.transform.position, Quaternion.identity);
        //     playerVR.transform.GetChild(1).gameObject.SetActive(false);
        //     playerVR.GetComponent<Rigidbody>().useGravity = false;
        //     playerVR.GetComponent<Navigation>().enabled = false;
        //     Camera camera = playerVR.GetComponent<Camera>();
        //     Canvas canvas = canvasVR.GetComponent<Canvas>();
        //     canvas.worldCamera = camera;
        //     canvasVR.SetActive(true);
        // }

        // if (PhotonNetwork.IsMasterClient)
        // {
            instance = this;
            buttonMapping = new List<int> {};

            System.Random random = new System.Random();

            int count = 0;

            for (int i = 0; i < buttons.Count; i++)
            {
                int randomIndex;

                do
                {
                    randomIndex = random.Next(0, bulbs.Count);
                } while (buttonMapping.Contains(randomIndex));

                buttonMapping.Add(randomIndex);

                if (random.Next(0, 2) == 0 || (count == 0 && i == buttons.Count - 1)) {
                    RPC_Toggle(buttons[i].GetInstanceID());
                    ++count;
                }
            }
       //}
    }

    // Update is called once per frame
    void Update()
    {
        // if (PhotonNetwork.IsMasterClient)
            if (bulbs.All(b => b.isOn))
                PhotonView.Get(this).RPC("SetVictory", RpcTarget.AllBuffered);
        // }
    }

    [PunRPC]
    void SetVictory()
    {
        PhotonNetwork.LoadLevel("Game");
    }


    public void Toggle(int buttonId)
    {
        PhotonView.Get(this).RPC("RPC_Toggle", RpcTarget.AllBuffered, buttonId);
    }


    [PunRPC]
    public void RPC_Toggle(int buttonId)
    {
        int index = buttons.FindIndex(s => s.GetInstanceID() == buttonId);
        int bulbIndex = buttonMapping[index];

        foreach (Bulb bulb in bulbs) {
            if (bulb.transform.position.x == bulbs[bulbIndex].transform.position.x || bulb.transform.position.y == bulbs[bulbIndex].transform.position.y)
                bulb.Toggle();
        }
    }
}