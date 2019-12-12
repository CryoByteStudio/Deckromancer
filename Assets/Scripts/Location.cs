using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Location : MonoBehaviour
{
    int randomnum;
    public bool isattackable;
    bool isinitialized = false;
    public int hordevalue;
    public int powervalue;
    public Player Owner;
    public BoardGameManager gman;
    
    public Sprite cardimage;
    public Image uiImage;
    [SerializeField]
    Location[] adjacentlocations;
    public int ownership;
    Renderer rend;
    public Material[] mats;

    // Start is called before the first frame update
    void Start()
    {
        gman = FindObjectOfType<BoardGameManager>();
       
        rend = GetComponent<Renderer>();

    }
    
    

    //On battle won, will update the ownership of the location.
    public void UpdateOwnership(int newowner, Player newplayer)
    {
        Debug.Log(isinitialized);
        if (isinitialized)
        {
            Owner.hordepoints -= hordevalue;
            Owner.powerpoints -= powervalue;
            Owner.locationpoints--;
            

        }
        else
        {
            isinitialized = true;
        }
        
        Owner = newplayer;
        ownership = newowner;
        Debug.Log(ownership);
        if (gman.activelocation == this)
        {
            gman.defendinglocation.rend.material = mats[ownership - 1];
        }
        else{
            rend.material = mats[ownership - 1];
        }

        
        Owner.hordepoints += hordevalue;
        Owner.powerpoints += powervalue;
        Owner.locationpoints++;
        gman.RefreshPlayerUI();


    }

     /*//# May Have To Re Enable This VVVVVVVVVVVVVVVVVVVVVVVVVV
    //Handles Location selection by click

    private void HandleLocationSelect()
    {
        if (!gman.isdeclaringattack)
        {
            gman.activelocation = this;
            gman.atkbutton.gameObject.SetActive(true);
            gman.cnclbutton.gameObject.SetActive(true);
            uiImage.sprite = cardimage;
            if (uiImage.enabled == false)
            {

                uiImage.enabled = true;


            }
        }
        else
        {
            foreach(Location loc in adjacentlocations)
            {
                
                if ((gman.activelocation.name=="Dread Caverns"&&this.ownership!=gman.playerturn)||(loc == gman.activelocation&&this.ownership!=gman.playerturn))//checks adjacent locations to see if any were the attacker, also checks to make sure it is owned by another player.
                {
                    gman.defendinglocation = this;
                    isattackable = true;//#TODO transition into battle
                    randomnum = Random.Range(0, 100) ;
                    randomnum += gman.activelocation.Owner.advantage;
                    Debug.Log(randomnum);
                    if (randomnum > 50)
                    {
                        WinBattle();
                    }
                    else {
                        LoseBattle();
                    }




                }
            }
        }
    }*/

        public void WinBattle()
    {

        Debug.Log("Battle Won");
        UpdateOwnership(gman.playerturn, gman.defendinglocation.Owner);
        gman.RefreshPlayerUI();
        if (gman.activelocation.Owner.locationpoints >= 11)
        {
            gman.LoadVictoryScreen();
        }
        uiImage.enabled = false;
        gman.atkbutton.SetActive(false);
        gman.cnclbutton.SetActive(false);

        gman.EndTurn();
    }

    public void LoseBattle()
    {

        Debug.Log("Battle Lost");
        uiImage.enabled = false;
        gman.atkbutton.SetActive(false);
        gman.cnclbutton.SetActive(false);

        gman.EndTurn();
    }
    //Handles location selection by touch (ray)
    public void OnRayHit() {
        if (!gman.isdeclaringattack)
        {
            gman.activelocation = this;
            gman.atkbutton.gameObject.SetActive(true);
            gman.cnclbutton.gameObject.SetActive(true);
            uiImage.sprite = cardimage;
            if (uiImage.enabled == false)
            {

                uiImage.enabled = true;


            }
        }
        else
        {
            foreach (Location loc in adjacentlocations)
            {
                //Allows dread caverns to attack anything
                if ((gman.activelocation.name == "Dread Caverns" && this.ownership != gman.playerturn) || (loc == gman.activelocation && this.ownership != gman.playerturn))//checks adjacent locations to see if any were the attacker, also checks to make sure it is owned by another player.
                {

                    gman.defendinglocation = this;
                  isattackable = true;
                    gman.StartBattle();
                    //#TODO transition into battle
                                      /*
                                       *Old Combat system for debug
                                       * randomnum = Random.Range(0, 100);
                                       randomnum += gman.activelocation.Owner.advantage;
                                       Debug.Log(randomnum);
                                      if (randomnum > 50)
                                       {
                                           WinBattle();


                                       }
                                       else
                                       {
                                           LoseBattle();


                                       }*/





                }
            }
        }
    }
    //Handles Location Seleection by mouse (debugigng)
    private void OnMouseDown()
    {

        OnRayHit();
        
    }

}
