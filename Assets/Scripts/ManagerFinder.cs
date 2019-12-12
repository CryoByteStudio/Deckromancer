using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ManagerFinder : MonoBehaviour
{
    public BoardGameManager gman;
    public Button button;
    public string scenetoload;
    public bool isloadlevel;
    public bool issaveloc;
    public bool isloadloc;
    public bool iswinbutton;
    public bool islosebutton;
    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        gman = FindObjectOfType<BoardGameManager>();
        button.onClick.AddListener(taskonclick);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void taskonclick()
    {
        if (isloadlevel) {
            gman.LoadGameScene(scenetoload);
        }
        else if(issaveloc)
        {
            gman.SaveLocations();
        }
        else if (isloadloc)
        {
            gman.SetLocationsInProgress();
        }
    }

    
}
