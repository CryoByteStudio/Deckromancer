using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class BoardGameManager : MonoBehaviour
{
    static BoardGameManager _instance = null;

    public static BoardGameManager instance
    {
        get { return _instance; }
        set { _instance = value; }
    }

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
        if (instance)
            DestroyImmediate(gameObject);
        else
        {
            DontDestroyOnLoad(this);
            instance = this;
        }
        locations = FindObjectsOfType<Location>().ToList();
        locations = locations.OrderBy(x => Random.value).ToList();
        SetupGame();
        gameinprogress = true;
     
    }

    //Attacker Wins Battle
 public void WinBattle()
    {
        activelocation.WinBattle();
        //disables combat
        combatsystem.SetActive(false);
    }
    //Attacker loses battle
    public void LoseBattle()
    {
      activelocation.LoseBattle();
        //disables combat
        combatsystem.SetActive(false);
    }
    private void OnLevelWasLoaded(int level)
    {
        
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
        combatsystem.SetActive(true);
    }
   
       
    


    // Update is called once per frame
    void Update()
    {
        
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
                    Debug.Log("added locs");

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
        
        CancelAttack();
        if (playerturn == 1)
        {
            playerturn = 2;
        }else if (playerturn == 2)
        {
            playerturn = 1;
        }
        atkbutton.gameObject.SetActive(false);
        cnclbutton.gameObject.SetActive(false);
        Debug.Log("End turn");
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
}
