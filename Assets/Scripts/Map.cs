using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class Map : MonoBehaviour
{
    public GameObject displayedMap;
    public GameObject displayedPlayer;
    public Transform player;
    private bool isMapOpen = false;


    void Start()
    {
        displayedMap.SetActive(false);
    }
    void Update()
    {
        double y = player.transform.position.z / 2500 *4 -2;
        double x = player.transform.position.x / 2500 * 5.5 - 2.25;
        displayedPlayer.transform.localRotation = Quaternion.Euler(0, 0, player.eulerAngles.y * -1);
        displayedPlayer.transform.localPosition = new Vector3((float)(x), (float)(y), -1);
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (isMapOpen)
                CloseMap();
            else
                OpenMap();
        }
    }

    // Functions to display the map.
    private void OpenMap()
    {
        displayedMap.SetActive(true);
        isMapOpen = true;
    }

    private void CloseMap()
    {
        displayedMap.SetActive(false);
        isMapOpen = false;
    }
}
