using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Must be added if using SceneManager functions
using UnityEngine.SceneManagement;
// Must be added if using UI functions
using UnityEngine.UI;

public class CombatManager : MonoBehaviour
{
    /* static CombatManager _instance = null;

     public static CombatManager instance
     {
         get { return _instance; }
         set { _instance = value; }
     }*/
    public int P1BuffDamage;
    public int P1BuffHealth;
    public int P2BuffDamage;
    public int P2BuffHealth;

    public List<CardMaterials> p1_cards = new List<CardMaterials>();
    public List<CardMaterials> p2_cards = new List<CardMaterials>();
    [SerializeField]
    CardMaterials attacker;
    [SerializeField]
    CardMaterials defender;
    [SerializeField]
    Canvas p1_hand_canvas;
    [SerializeField]
    Canvas p2_hand_canvas;
    [SerializeField]
    Canvas fight_canvas;

 
    public float turndelaytimer=2f;
    public BoardGameManager gman;
    public Stack<CardMaterials> p1_stack_left = new Stack<CardMaterials>();
    public CardMaterials p1_stack_mid;
    public Stack<CardMaterials> p1_stack_right = new Stack<CardMaterials>();
    public Stack<CardMaterials> p2_stack_left = new Stack<CardMaterials>();
    public CardMaterials p2_stack_mid;
    public Stack<CardMaterials> p2_stack_right = new Stack<CardMaterials>();
    [SerializeField]
    Image[] p1_card_images_arr;
    [SerializeField]
    Image[] p2_card_images_arr;
    [SerializeField]
    Image atk_image;
    [SerializeField]
    Image def_image;
    [SerializeField]
    Text atk_health;
    [SerializeField]
    Text def_health;

    public float phase_timer_fl = 3.0f;
    float combat_timer =0;
    public bool is_declare_horde;
    public bool is_combat;
    public bool is_combat_completed;
    public bool is_attacker_win;
    public bool is_defender_win;
    public bool is_p1_turn;
    public bool is_card_selected;


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
        gman = FindObjectOfType<BoardGameManager>();
        p1_cards = gman.player1.cardsHand;
        p2_cards = gman.player2.cardsHand;
        gman=FindObjectOfType<BoardGameManager>();
        gman.cman = transform.parent.gameObject;
        if (gman.playerturn == 1)
            is_p1_turn = true;
        else
            is_p1_turn = false;
        is_declare_horde = true;
        is_card_selected = false;
        fight_canvas.gameObject.SetActive(false);
        for (int i = p1_cards.Count -1; i > 1; i--)
        {
            p1_stack_right.Push(p1_cards[i]);
        }
        p1_stack_mid = p1_cards[1];
        p1_stack_left.Push(p1_cards[0]);
        for (int i = p2_cards.Count - 1; i > 1; i--)
        {
            p2_stack_right.Push(p2_cards[i]);
        }
        p2_stack_mid = p2_cards[1];
        p2_stack_left.Push(p2_cards[0]);

        //show cards on buttons
        Debug.Log("Select Attacker");
    }

    // Update is called once per frame
    void Update()
    {
        if (is_declare_horde)
        {
        DisplayP1Hand();
        DisplayP2Hand();
            if (!attacker)
            {
                if (is_p1_turn && is_card_selected)
                {
                    attacker = p1_stack_mid;
                    Debug.Log("Select Defender");
                    is_card_selected = false;
                    if (p1_stack_right.Count > 0)
                    {
                        p1_stack_mid = p1_stack_right.Pop();
                    }
                    else if (p1_stack_left.Count > 0)
                    {
                        p1_stack_mid = p1_stack_left.Pop();
                    }
                    is_p1_turn = false;
                    attacker.cur_health = attacker.health + P1BuffHealth;
                    attacker.add_dmg = attacker.add_dmg + P1BuffDamage;
                    DisplayP1Hand();
                    DisplayP2Hand();

                }
                else if (!is_p1_turn && is_card_selected)
                {
                    attacker = p2_stack_mid;
                    Debug.Log("Select Defender");
                    is_card_selected = false;
                    if (p2_stack_right.Count > 0)
                    {
                        p2_stack_mid = p2_stack_right.Pop();
                    }
                    else if (p2_stack_left.Count > 0)
                    {
                        p2_stack_mid = p2_stack_left.Pop();
                    }
                    is_p1_turn = true;
                    attacker.cur_health = attacker.health + P2BuffHealth;
                    attacker.add_dmg = attacker.add_dmg + P2BuffDamage;
                    DisplayP2Hand();
                    DisplayP1Hand();
                }
            }
            else if (!defender)
            {
                if (is_p1_turn && is_card_selected)
                {
                    defender = p1_stack_mid;
                    is_card_selected = false;
                    if (p1_stack_right.Count > 0)
                    {
                        p1_stack_mid = p1_stack_right.Pop();
                    }
                    else if (p1_stack_left.Count > 0)
                    {
                        p1_stack_mid = p1_stack_left.Pop();
                    }
                    is_p1_turn = false;
                    defender.cur_health = defender.health + P1BuffHealth;
                    defender.add_dmg = defender.add_dmg + P1BuffDamage;
                    DisplayP1Hand();
                    DisplayP2Hand();
                }
                else if (!is_p1_turn && is_card_selected)
                {
                    defender = p2_stack_mid;
                    is_card_selected = false;
                    if (p2_stack_right.Count > 0)
                    {
                        p2_stack_mid = p2_stack_right.Pop();
                    }
                    else if (p2_stack_left.Count > 0)
                    {
                        p2_stack_mid = p2_stack_left.Pop();
                    }
                    is_p1_turn = true;
                    defender.cur_health = defender.health + P2BuffHealth;
                    defender.add_dmg = defender.add_dmg + P2BuffDamage;
                    DisplayP2Hand();
                    DisplayP1Hand();
                }
            }
            else if (attacker && defender)
            {
                is_declare_horde = false;
                is_combat = true;
                p1_hand_canvas.gameObject.SetActive(false);
                p2_hand_canvas.gameObject.SetActive(false);
                fight_canvas.gameObject.SetActive(true);
                atk_image.sprite = attacker.front_sprite;
                def_image.sprite = defender.front_sprite;
                atk_health.text = attacker.cur_health.ToString();
                def_health.text = defender.cur_health.ToString();
            }
        }
        if (is_combat)
        {
            if (combat_timer <= phase_timer_fl)
            {
                combat_timer += Time.deltaTime;
            }
            else
            {
                if (attacker.cur_health > 0 && defender.cur_health > 0)
                {
                    Debug.Log("attacker health is" + attacker.cur_health);
                    Debug.Log("defender health is" + defender.cur_health);
                    atk_health.text = attacker.cur_health.ToString();
                    def_health.text = defender.cur_health.ToString();
                    Fight();
                    Debug.Log("attacker health is" + attacker.cur_health);
                    Debug.Log("defender health is" + defender.cur_health);
                    atk_health.text = attacker.cur_health.ToString();
                    def_health.text = defender.cur_health.ToString();
                    combat_timer = 0;
                }
                else if (attacker.cur_health <= 0 && defender.cur_health > 0)
                {
                    is_combat = false;
                    is_combat_completed = true;
                    is_defender_win = true;
                    if (attacker.cur_health <= 0)
                    {
                        gman.MonsterDiscard.Add(attacker);
                        attacker = null;
                    }
                }
                else
                {
                    is_combat = false;
                    is_combat_completed = true;
                    is_attacker_win = true;
                    if (attacker.cur_health <= 0)
                    {
                        gman.MonsterDiscard.Add(attacker);
                        attacker = null;
                    }
                    if (defender.cur_health <= 0)
                    {
                        gman.MonsterDiscard.Add(defender);
                        defender = null;
                    }
                }
            }
        }
        if (is_combat_completed)
        {
            if (combat_timer <= phase_timer_fl)
            {
                combat_timer += Time.deltaTime;
            }
            else
            {
                if (is_defender_win)
                {
                    //defender win
                    is_declare_horde = true;
                    is_combat_completed = false;
                    is_p1_turn = true;
                    gman.LoseBattle();

                }
                else
                {
                    //attacker win
                    is_declare_horde = true;
                    is_combat_completed = false;
                    is_p1_turn = true;
                    gman.WinBattle();
                }
            }
        }
    }

    void Fight()
    {

        defender.cur_health -= DealDamage(attacker.die_dmg) + attacker.add_dmg;
        attacker.cur_health -= DealDamage(defender.die_dmg) + defender.add_dmg;
        
    }

    int DealDamage(CardMaterials.Die_Type die_Type)
    {
        int dmg;

        if (die_Type is CardMaterials.Die_Type.d2)
        {
            dmg = 2;
        }else if(die_Type is CardMaterials.Die_Type.d4)
        {
            dmg = 4;
        }else if(die_Type is CardMaterials.Die_Type.d6)
        {
            dmg = 6;
        }
        else
        {
            dmg = 8;
        }

        return Random.Range(1, dmg);
    }

    public void ShuffleLeft()
    {
        if (is_p1_turn && p1_stack_left.Count > 0)
        {
            p1_stack_right.Push(p1_stack_mid);
            p1_stack_mid = p1_stack_left.Pop();
            DisplayP1Hand();
        }
        else if (!is_p1_turn && p2_stack_left.Count > 0)
        {
            p2_stack_right.Push(p2_stack_mid);
            p2_stack_mid = p2_stack_left.Pop();
            DisplayP2Hand();
        }
    }

    public void ShuffleRight()
    {
        if (is_p1_turn && p1_stack_right.Count > 0)
        {
            p1_stack_left.Push(p1_stack_mid);
            p1_stack_mid = p1_stack_right.Pop();
            DisplayP1Hand();
        }
        else if (!is_p1_turn && p2_stack_right.Count > 0)
        {
            p2_stack_left.Push(p2_stack_mid);
            p2_stack_mid = p2_stack_right.Pop();
            DisplayP2Hand();
        }
    }

     public void SelectCard()
    {
        is_card_selected = true;
    }

    void DisplayP1Hand()
    {
        if (is_p1_turn) 
        { 
            p1_hand_canvas.gameObject.SetActive(true);
            p2_hand_canvas.gameObject.SetActive(false);
        }
        if (is_combat)
        {
            p1_hand_canvas.gameObject.SetActive(false);
            p2_hand_canvas.gameObject.SetActive(false);
        }
        if (p1_stack_left.Count > 0)
        {
            if (!p1_card_images_arr[0].isActiveAndEnabled)
            {
                p1_card_images_arr[0].enabled = true;
            }
            p1_card_images_arr[0].sprite = p1_stack_left.Peek().front_sprite;
        }
        else
        {
            p1_card_images_arr[2].enabled = false;
        }
        if (p1_stack_mid)
            p1_card_images_arr[1].sprite = p1_stack_mid.front_sprite;
        if (p1_stack_right.Count > 0)
        {
            if (!p1_card_images_arr[2].isActiveAndEnabled)
            {
                p1_card_images_arr[2].enabled = true;
            }
            p1_card_images_arr[2].sprite = p1_stack_right.Peek().front_sprite;
        }
        else
        {
            p1_card_images_arr[2].enabled = false;
        }
    }

    void DisplayP2Hand()
    {
        if (!is_p1_turn)
        {
            p2_hand_canvas.gameObject.SetActive(true);
            p1_hand_canvas.gameObject.SetActive(false);
        }
        if (is_combat)
        {
            p1_hand_canvas.gameObject.SetActive(false);
            p2_hand_canvas.gameObject.SetActive(false);
        }
        if (p2_stack_left.Count > 0)
        {
            if (!p2_card_images_arr[0].isActiveAndEnabled)
            {
                p2_card_images_arr[0].enabled = true;
            }
            p2_card_images_arr[0].sprite = p2_stack_left.Peek().front_sprite;
        }
        else
        {
            p2_card_images_arr[0].enabled = false;
        }
        if (p2_stack_mid)
            p2_card_images_arr[1].sprite = p2_stack_mid.front_sprite;
        if (p2_stack_right.Count > 0)
        {
            if (!p2_card_images_arr[2].isActiveAndEnabled)
            {
                p2_card_images_arr[2].enabled = true;
            }
            p2_card_images_arr[2].sprite = p2_stack_right.Peek().front_sprite;
        }
        else
        {
            p2_card_images_arr[2].enabled = false;
        }
    }

}
