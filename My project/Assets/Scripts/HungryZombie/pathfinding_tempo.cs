using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Graph graphinstance;
    // public Graph grapho;
    private LevelLoader levelLoader;
    private GameObject player;
    private void Start(){

        levelLoader = FindAnyObjectByType<LevelLoader>();
        graphinstance = new Graph();
        if(graphinstance!=null){
            graphinstance.CreateGraph(levelLoader.levelLines);
        }
    }

    private void Update()
    {
        player = GameObject.FindGameObjectsWithTag("Player")[0];
        Vector2 player_pos = player.transform.position;
        Vector2 pos_zombie = transform.position;
        List<Node> path = graphinstance.FindPath(pos_zombie, player_pos);
        foreach (Node node in path){
            Debug.Log(node.position);
            
        }
        pos_zombie = path[0].position;
        transform.position = pos_zombie;
}}