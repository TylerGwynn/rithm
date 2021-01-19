﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chart : MonoBehaviour
{
    public songTimer timer;
    public songLoader songLoader;
    protected Song song ;
    protected GameObject songTop;
    private Material grid;

    Quaternion rot = new Quaternion(0,0,0,0);
    Vector3 chartTop;
    public Material quarterMaterial;
    public Material eighthMaterial;
    public Material sixteenthMaterial;

    
    // Start is called before the first frame update
    void Start()
    {
        Vector3 po = this.transform.position;
        chartTop = new Vector3(po.x, po.y + this.transform.localScale.y /2);
        songTop = Instantiate(new GameObject(), chartTop, rot);
        grid = this.GetComponent<Renderer>().material;
        //TODO make songPlayer superclass?
        if (timer == null){
            timer = (songTimer)GameObject.Find("songTimer").GetComponent(typeof(songTimer));
        }
        if (songLoader == null){
            print("No song detected for "+this.gameObject.name+", using placeholder song.");
            song = new Song("eighth","1 1 111 1111 11 ");
        }else{
            song = songLoader.song;
        }
        quarterMaterial = (quarterMaterial == null? (Material)Resources.Load("Assets/materials/Blue",typeof(Material)) : quarterMaterial);
        eighthMaterial = (eighthMaterial == null? (Material)Resources.Load("Red",typeof(Material)) : eighthMaterial);
        if (eighthMaterial == null){
            eighthMaterial = (Material)Resources.Load("Red",typeof(Material));
        }
        sixteenthMaterial = (sixteenthMaterial == null? (Material)Resources.Load("Green",typeof(Material)) : sixteenthMaterial);

        spawnNotes();
    }

    // Update is called once per frame
    void Update()
    {
        if (timer.playing){
            //TODO BPM math
            scrollAll(Time.deltaTime/2);
        }
    }

    void spawnNotes(){
        Vector3 notePos;
        float offset;
        float toChartTop = this.transform.localScale.y /2;
        
        GameObject curr;
        foreach (Note n in song.notes){
            offset = -(n.tick*(toChartTop/16));
            // TODO can change x pos, and add types here
            notePos = new Vector3(0, offset + toChartTop, -1) + this.transform.position;
            // print("tick: "+n.tick);
            // TODO optimize by only creating this bar + next... put in Update()...
            curr = Instantiate(GameObject.Find("cone"), notePos, rot);
            curr.transform.parent = songTop.transform;
            if (n.isQuarter()){
                curr.GetComponent<Renderer>().material = quarterMaterial;
            }else if(n.isEighth()){
                curr.GetComponent<MeshRenderer>().material = eighthMaterial;
            }else if(n.isSixteenth()){
                curr.GetComponent<Renderer>().material = sixteenthMaterial;
            }
        }
    }

    void scrollAll(float y){
        scrollTex(y);
        scrollNotes(y);
    }
    void scrollTex(float y){
        Vector2 currOffset = grid.GetTextureOffset("_MainTex");
        grid.SetTextureOffset("_MainTex",currOffset + new Vector2(0,-y));
    }
    void scrollNotes(float y){
        songTop.transform.Translate(0,y*this.transform.localScale.y/2,0);
    }

}
