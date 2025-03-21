using System.Text;
using System;
using UnityEngine;
using WebSocketSharp;
using NUnit.Framework;
using System.Collections.Generic;

public class Multiplayer : MonoBehaviour
{
    public Transform player;
    public GameObject playerModel;
    WebSocket ws;
    int? id = null;
    List<GameObject> players = new();
    List<int> playerIds = new();
    Vector3 vector = new();
    void Start()
    {
        ws = new WebSocket("wss://localhost:7235/ws");
        ws.Connect();
        ws.OnMessage += (sender, e) =>
        {
            try
            {
                if (e.Data.Contains("Connected id : "))
                    id = int.Parse(e.Data.Split("Connected id : ")[1]);
                else
                {
                    if (id != int.Parse(e.Data.Split(";")[0].Split("id : ")[1]))
                    {
                        Debug.Log(e.Data);
                        if (!playerIds.Contains(int.Parse(e.Data.Split(";")[0].Split("id : ")[1])))
                        {
                            Debug.Log("Add player");

                            /*GameObject go = Instantiate(playerModel) as GameObject;
                            Debug.Log("Add player 1");
                            go.transform.position = new Vector3(0, 0, 0);
                            Debug.Log("Add player 2");
                            players.Add(go);
                            Debug.Log("Add player 3");*/

                        }
                        var t = float.Parse(e.Data.Split(";")[2].Split("y : ")[1]);
                        Debug.Log("Set position : " + float.Parse(e.Data.Split(";")[1].Split("x : ")[1]) +
                            float.Parse(e.Data.Split(";")[2].Split("y : ")[1]) +
                            float.Parse(e.Data.Split(";")[3].Split("z : ")[1]));

                        vector = new Vector3(
                            float.Parse(e.Data.Split(";")[1].Split("x : ")[1]),
                            float.Parse(e.Data.Split(";")[2].Split("y : ")[1]),
                            float.Parse(e.Data.Split(";")[3].Split("z : ")[1]));
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
            }
        };
    }

    void Update()
    {
        playerModel.transform.localPosition = vector;
     
        if (ws == null && id != null)
        {
            return;
        }
        double z = player.transform.localPosition.z;
        double y = player.transform.localPosition.y;
        double x = player.transform.localPosition.x;
        //Debug.Log($"id : {id}, x : {x}, y : {y}, z : {z}");
        //ws.Send($"id : {id}; x : {x}; y : {y}; z : {z}"); 
    }
}
