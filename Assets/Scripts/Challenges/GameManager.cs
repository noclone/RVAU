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
    public List<Switch> switches;
    
    private List<int> switchMapping;

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
            switchMapping = new List<int> {};

            System.Random random = new System.Random();

            int count = 0;

            for (int i = 0; i < switches.Count; i++)
            {
                int randomIndex;

                do
                {
                    randomIndex = random.Next(0, bulbs.Count);
                } while (switchMapping.Contains(randomIndex));

                switchMapping.Add(randomIndex);

                if (random.Next(0, 2) == 0 || (count == 0 && i == switches.Count - 1)) {
                    Toggle(switches[i].GetInstanceID());
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


    [PunRPC]
    public void Toggle(int switchId)
    {
        int index = switches.FindIndex(s => s.GetInstanceID() == switchId);
        int bulbIndex = switchMapping[index];

        foreach (Bulb bulb in bulbs) {
            if (bulb.transform.position.x == bulbs[bulbIndex].transform.position.x || bulb.transform.position.y == bulbs[bulbIndex].transform.position.y)
                bulb.Toggle();
        }
    }
}
