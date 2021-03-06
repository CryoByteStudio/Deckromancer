﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class BoardGameManager : MonoBehaviour
{
    /* static BoardGameManager _instance = null;

     public static BoardGameManager instance
     {
         get { return _instance; }
         set { _instance = value; }
     }*/

    public GameObject startturnbuttons;
    public Location Deadzone;
    public Location Thrallville;
    public bool cantriggermortton=true;
    public bool cantriggerdeadzone=true;
    public GameObject Options;
    bool is_loaded = false;
    public Player player1;
    public Player player2;
    public List<CardMaterials> MonsterDeck;
    public List<CardMaterials> MonsterDiscard;
    public GameObject cman;
    public GameObject combatsystem;
    public bool gameinprogress=false;
    public List<string> ailocnames;
    public List<string> playerlocnames;
    public GameObject atkbutton;
    public GameObject cnclbutton;
    public bool isdeclaringattack = false;
    public Location defendinglocation;
    //Active location is the selected location, the one that is performing the attack.
    public Location activelocation;
    public int playerturn;
    List <Location> locations;
    public Text turndisplaytext;
    public List <Location> playerlocations;
    public List <Location> ailocations;
    // Start is called before the first frame update
    void Start()
    {
      /*  if (instance)
            DestroyImmediate(gameObject);
        else
        {
            DontDestroyOnLoad(this);
            instance = this;
        }*/
        locations = FindObjectsOfType<Location>().ToList();
        locations = locations.OrderBy(x => Random.value).ToList();
        SetupGame();
        gameinprogress = true;
     
    }
    public void CheckForBuffs()
    {

        foreach (Location loc in defendinglocation.adjacentlocations)
        {
            loc.ApplyBuff();
        }
        defendinglocation.ApplyBuff();

    }
    public void QuitApp()
    {
        Application.Quit();
    }
    //Attacker Wins Battle
 public void WinBattle()
    {
        defendinglocation.WinBattle();
        //disables combat
        //combatsystem.SetActive(false);
        cman.GetComponentInChildren<CombatManager>().ReturnHandsToPLayer();
        Destroy(cman);
    }

    public void ToggleOptionsMenuOn()
    {
        Options.SetActive(true);
    }
    public void ToggleOptionsMenuOff()
    {
        Options.SetActive(false);
    }
    //Attacker loses battle
    public void LoseBattle()
    {
      defendinglocation.LoseBattle();
        //disables combat
       //
        cman.GetComponentInChildren<CombatManager>().ReturnHandsToPLayer();
        Destroy(cman);
    }
    private void OnLevelWasLoaded(int level)
    {
        if (!is_loaded)
        {
            ShuffleDeck(MonsterDeck, MonsterDiscard);
            DrawCards(player1);
            DrawCards(player2);
            if(player1.cardsHand.Count>0)
                is_loaded = true;
        }
        
    }

    public void SaveLocations()
    {
        foreach (Location loc in playerlocations)
        {
            
            playerlocnames.Add(loc.name);
            
        }
        foreach (Location loc in ailocations)
        {
            ailocnames.Add(loc.name);
        }


    }

    //Enable combat
    public void StartBattle()
    {
        if (defendinglocation.name == "Riptide")
        {
            if (playerturn==1)
            {
                DiscardOneCard(player1);
                Debug.Log("removed random card from p1");
            }
            else if (playerturn == 2)
            {

                DiscardOneCard(player2);
                Debug.Log("removed random card from p2");

            }
            
        }
        Instantiate(combatsystem);
        CheckForBuffs();
        //combatsystem.SetActive(true);
    }
   
       
    public void UseThrallvilleEffect()
    {
        
        DiscardOneCard(Thrallville.Owner);
        DrawOneCard(Thrallville.Owner);
        DrawOneCard(Thrallville.Owner);
    }
    public void UseDarkObeliskEffect(Player player)
    {

    }
    public void UseDeadZoneEffect(Player player)
    {

    }
    public void UseDeadMansBluffEffect(Player player)
    {

    }


    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.V))
        {
            Debug.Log(player1.cardsHand.Count);
           // Debug.Log("drawing");
           // DrawOneCard(player1);
        }else if (Input.GetKeyDown(KeyCode.B))
        {
          //  Debug.Log("discarding");
           // DiscardOneCard(player1);
        }
        OnLevelWasLoaded(1);

        if(MonsterDeck.Count<= 10)
        {
            ShuffleDeck(MonsterDeck, MonsterDiscard);
        }

        if (Input.touchCount == 1)
        {
            Touch touch = Input.touches[0];
            RaycastHit hit;

            if (Physics.Raycast(touch.position, Vector3.down, out hit))
            {
                if (hit.collider != null)
                {
                    // Find the hit reciver (if existant) and call the method
                    var hitloc = hit.collider.gameObject.GetComponent<Location>();
                    if (hitloc != null)
                    {
                        hitloc.OnRayHit();
                    }
                }
            }
        }



    }

    public void SetupGame()
    { 
            //Determine first player
            if (Random.Range(0, 100) > 50)
            {
                playerturn = 1;

                turndisplaytext.text = "Player 1 Turn";
                turndisplaytext.color = Color.blue;
            }
            else
            {
                playerturn = 2;
                turndisplaytext.text = "Player 2 Turn";
                turndisplaytext.color = Color.red;
            }

            if (playerturn == 2)//if 2nd player has first turn, first player gets additional location.
            {
                for (int i = 0; i < locations.Count; i++)
                {
                    if (i == 0 || i % 2 == 0)//if even
                    {

                        playerlocations.Add(locations[i]);
                  //  Debug.Log("added locs");

                    }
                    else
                    {
                        ailocations.Add(locations[i]);
                    }
                }
            }
            else if (playerturn == 1)//if first player has first turn, 2nd player gets extra lcoation.
            {
                for (int i = 0; i < locations.Count; i++)
                {
                    if (i == 0 || i % 2 == 0)//if even
                    {
                        ailocations.Add(locations[i]);
                    }
                    else
                    {
                        playerlocations.Add(locations[i]);
                    }
                }
            }

    }

    public void LoadGameScene(string scenetoload)
    {
        SceneManager.LoadScene(scenetoload);
        SetLocationsInProgress();
    }
    public void DeclareAttack()
    {
        if (playerturn == activelocation.ownership)
        {
            Debug.Log("Declaring attack");
            isdeclaringattack = true;
        }
        else
        {
            Debug.Log("Pick one of your locations to attack from!");
        }
    }

    public void CancelAttack()
    {
        isdeclaringattack = false;
        atkbutton.SetActive(false);
        cnclbutton.SetActive(false);
       
        activelocation.uiImage.enabled = false;
        
    }
    public void EndTurn()
    {
        Debug.Log("ending turn");
        CancelAttack();
        
     
        atkbutton.gameObject.SetActive(false);
        cnclbutton.gameObject.SetActive(false);
       
        activelocation = null;
        defendinglocation = null;
        RefreshPlayerUI();
        cantriggerdeadzone = true;
        cantriggermortton = true;
        Debug.Log(player1.cardsHand.Count);
        Debug.Log(player2.cardsHand.Count);

        startturnbuttons.gameObject.SetActive(true);
    }
    
    public void StartTurn()
    {
        startturnbuttons.SetActive(false);
        if (playerturn == 1)
        {
            Debug.Log("Now player 2s turn");
            Debug.Log(player2.cardsHand.Count);
            if (player2.cardsHand.Count < player2.hordepoints)
            {
                Debug.Log("drawing player 2");
                Debug.Log(player2.hordepoints);
                DrawCards(player2);
            }
            playerturn = 2;
        }
        else
        {

            Debug.Log("Now player 1s turn");
            Debug.Log(player1.cardsHand.Count);
            if (player1.cardsHand.Count < player1.hordepoints)
            {
                Debug.Log("drawing player 1");
                Debug.Log(player1.hordepoints);
                DrawCards(player1);
                playerturn = 1;
            }
        }
        RefreshPlayerUI();
    }
    public void LoadVictoryScreen()
    {
        SceneManager.LoadScene("Victory");
    }
    public void RefreshPlayerUI()
    {
        foreach (Player player in FindObjectsOfType<Player>())
        {
            player.RefreshUI();
        }
        if (playerturn == 1)
        {
            turndisplaytext.color = Color.blue;
            turndisplaytext.text = "Player 1 Turn";
        }else if (playerturn == 2)
        {
            turndisplaytext.color = Color.red;
            turndisplaytext.text = "Player 2 Turn";
        }
    }
    public void SetLocationsInProgress()
    {
       
        {
            foreach (string name in playerlocnames)
            {
                GameObject reference = GameObject.Find(name);
                Debug.Log(reference);
                playerlocations.Add(reference.GetComponent<Location>());


            }
            foreach (string name in ailocnames)
            {
                GameObject reference = GameObject.Find(name);
                ailocations.Add(reference.GetComponent<Location>());


            }
        }
    }

    public void ShuffleDeck(List<CardMaterials> deck, List<CardMaterials> discardDeck)
    {
        while (discardDeck.Count > 0)
        {
                deck.Add(discardDeck[0]);
            discardDeck.RemoveAt(0);
        }
        for(int i = 0; i<= deck.Count-1; i++)
        {
            CardMaterials holdCard = deck[i];
            int randCard = Random.Range(0,deck.Count);
            deck[i] = deck[randCard];
            deck[randCard] = holdCard;
        }
    }

    void DrawCards(Player player)
    {
        for (int i = player.cardsHand.Count; i < player.hordepoints; i++)
        {
           
            player.cardsHand.Add(MonsterDeck[MonsterDeck.Count - 1]);
            MonsterDeck.RemoveAt(MonsterDeck.Count-1);
        }
    }
    void DrawOneCard(Player player)
    {
        player.cardsHand.Add(MonsterDeck[MonsterDeck.Count - 1]);
        MonsterDeck.RemoveAt(MonsterDeck.Count - 1);
    }
    void DiscardOneCard(Player player)
    {
        int randomnum = Random.Range(0, player.cardsHand.Count);
        if (cantriggerdeadzone)//discard and trigger dead zone
        {
            Debug.Log("Added sacrificed card to deadzone");
            //add card to deadzone owners hand
            Deadzone.Owner.cardsHand.Add(player.cardsHand[randomnum]);
             //remove card from current players hand
              player.cardsHand.RemoveAt(randomnum);

            
            cantriggerdeadzone = false;
        }
        else //regular discard
        {
            Debug.Log("regular discard");
            MonsterDiscard.Add(player.cardsHand[randomnum]);
            player.cardsHand.RemoveAt(randomnum);
        }
       
    }


}
