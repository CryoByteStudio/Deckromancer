using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public List<CardMaterials> cardsHand = new List<CardMaterials>();
    int layerMask = 2;
    BoardGameManager gman;
    public int advantage;
    public int PlayerNumber;
    public Text hordetext;
    public Text powertext;
    public Text locationtext;
    public int hordepoints;
    public int powerpoints;
    public int locationpoints;
   
    public List <Location> OwnedLocations;
    // Start is called before the first frame update
    void Start()
    {
        gman = FindObjectOfType<BoardGameManager>();
        Invoke("InitializePlayer", 1);
    }

    void InitializePlayer()
    {
        if (gman.gameinprogress)
        {
            OwnedLocations.Clear();
        }
        else
        {

        }


        if (gman.gameinprogress)
        {
            OwnedLocations.Clear();
            gman.SetLocationsInProgress();
        }
        
        if (PlayerNumber == 1)
        {
            OwnedLocations = gman.playerlocations;

        }
        else if (PlayerNumber == 2)
        {
            OwnedLocations = gman.ailocations;
        }

        foreach (Location l in OwnedLocations)
        {
            
            l.UpdateOwnership(PlayerNumber, this);
        }
        if (gman.playerturn != PlayerNumber)
        {

        }
    }
    // Update is called once per frame
    void Update()
    {
        

        
    }


    public void RefreshUI()
    {
        hordetext.text = "Horde: " + hordepoints;
        powertext.text = "Power: " + powerpoints;
        locationtext.text = "Locations: " + locationpoints + "/11";
       
       
    }
}
