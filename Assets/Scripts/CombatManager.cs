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
    public CardMaterials attacker;
    public CardMaterials defender;
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
    [SerializeField]
    Text atk_damage;
    [SerializeField]
    Text def_damage;
    [SerializeField]
    Button p1_atk_but;
    [SerializeField]
    Button p1_sur_but;
    [SerializeField]
    Button p2_atk_but;
    [SerializeField]
    Button p2_sur_but;

    public float phase_timer_fl = 3.0f;
    float combat_timer =0;
    public bool is_declare_horde;
    public bool is_combat;
    public bool is_combat_completed;
    public bool is_attacker_win;
    public bool is_defender_win;
    public bool is_p1_turn;
    public bool is_card_selected;
    bool is_p1_atk;
    bool is_p1_sur;
    bool is_p2_atk;
    bool is_p2_sur;
    bool is_p1_attacker;


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
        {
            is_p1_turn = true;
            is_p1_attacker = true;
        }
        else
        {
            is_p1_turn = false;
            is_p1_attacker = false;
        }
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
        if (!is_p1_atk)
        {
            p1_hand_canvas.gameObject.SetActive(true);
            p2_hand_canvas.gameObject.SetActive(false);
            p1_atk_but.gameObject.SetActive(true);
            p1_sur_but.gameObject.SetActive(true);
        }
        else if (!is_p2_atk)
        {
            p2_hand_canvas.gameObject.SetActive(true);
            p1_hand_canvas.gameObject.SetActive(false);
            p2_atk_but.gameObject.SetActive(true);
            p2_sur_but.gameObject.SetActive(true);
        }
        if (is_p1_attacker && is_p1_sur)
        {
            gman.LoseBattle();
        }else if(is_p1_attacker && is_p2_sur)
        {
            gman.WinBattle();
        }
        else if (!is_p1_attacker && is_p1_sur)
        {
            gman.WinBattle();
        }
        else if (!is_p1_attacker && is_p2_sur)
        {
            gman.LoseBattle();
        }
        if (is_declare_horde && is_p1_atk && is_p2_atk)
        {
            atk_damage.text = "0";
            def_damage.text = "0";
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
                    else if (p1_stack_left.Count == 0 && p1_stack_right.Count == 0)
                    {
                        p1_stack_mid = null;
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
                    else if (p2_stack_left.Count == 0 && p2_stack_right.Count == 0)
                    {
                        p2_stack_mid = null;
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
                    else if (p1_stack_left.Count == 0 && p1_stack_right.Count == 0)
                    {
                        p1_stack_mid = null;
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
                    }else if(p2_stack_left.Count==0 && p2_stack_right.Count == 0)
                    {
                        p2_stack_mid = null;
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
                    combat_timer = 0;
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
                    if (is_p1_attacker)
                    {
                        is_p1_turn = true;
                    }else { is_p1_turn = false; }
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
                    if (is_p1_attacker)
                    {
                        is_p1_turn = true;
                    }
                    else { is_p1_turn = false; }
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
                    combat_timer = 0;
                    //defender win
                    is_p1_atk = false;
                    is_p2_atk = false;
                    if (!is_p1_atk && is_p1_attacker)
                    {
                        p1_hand_canvas.gameObject.SetActive(true);
                        p2_hand_canvas.gameObject.SetActive(false);
                        p1_atk_but.gameObject.SetActive(true);
                        p1_sur_but.gameObject.SetActive(true);
                        is_declare_horde = true;
                        is_combat_completed = false;
                    }
                    else if (!is_p2_atk && !is_p1_attacker)
                    {
                        p2_hand_canvas.gameObject.SetActive(true);
                        p1_hand_canvas.gameObject.SetActive(false);
                        p2_atk_but.gameObject.SetActive(true);
                        p2_sur_but.gameObject.SetActive(true);
                        is_declare_horde = true;
                        is_combat_completed = false;
                    }
                    if (is_p1_attacker && (is_p1_sur || p1_cards.Count==0))
                    {
                        gman.LoseBattle();
                    }
                    else if (is_p1_attacker && is_p2_sur)
                    {
                        gman.WinBattle();
                    }
                    else if (!is_p1_attacker && is_p1_sur)
                    {
                        gman.WinBattle();
                    }
                    else if (!is_p1_attacker && is_p2_sur)
                    {
                        gman.LoseBattle();
                    }

                }
                else
                {
                    combat_timer = 0;
                    //attacker win
                    is_p1_atk = false;
                    is_p2_atk = false;
                    if (!is_p1_atk && is_p1_attacker)
                    {
                        p1_hand_canvas.gameObject.SetActive(true);
                        p2_hand_canvas.gameObject.SetActive(false);
                        p1_atk_but.gameObject.SetActive(true);
                        p1_sur_but.gameObject.SetActive(true);
                        is_declare_horde = true;
                        is_combat_completed = false;
                    }
                    else if (!is_p2_atk && !is_p1_attacker)
                    {
                        p2_hand_canvas.gameObject.SetActive(true);
                        p1_hand_canvas.gameObject.SetActive(false);
                        p2_atk_but.gameObject.SetActive(true);
                        p2_sur_but.gameObject.SetActive(true);
                        is_declare_horde = true;
                        is_combat_completed = false;
                    }
                    if (is_p1_attacker && (is_p1_sur || !p1_stack_mid))
                    {
                        gman.LoseBattle();
                    }
                    else if (is_p1_attacker && (is_p2_sur || !p2_stack_mid))
                    {
                        gman.WinBattle();
                    }
                    else if (!is_p1_attacker && (is_p1_sur || !p1_stack_mid))
                    {
                        gman.WinBattle();
                    }
                    else if (!is_p1_attacker && (is_p2_sur || !p2_stack_mid))
                    {
                        gman.LoseBattle();
                    }
                    
                }
            }
        }
    }

    void Fight()
    {
        int def_dmg = DealDamage(defender.die_dmg);
        int atk_dmg = DealDamage(attacker.die_dmg);
        defender.cur_health -= atk_dmg + attacker.add_dmg;
        attacker.cur_health -= def_dmg + defender.add_dmg;
        atk_damage.text = (atk_dmg + attacker.add_dmg).ToString();
        def_damage.text = (def_dmg + defender.add_dmg).ToString();
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

    public void ReturnHandsToPLayer()
    {
        gman.player1.cardsHand.RemoveRange(0, gman.player1.cardsHand.Count);
        for(int i = 0; i < p1_stack_left.Count; i++)
        {
            gman.player1.cardsHand.Add(p1_stack_left.Pop());
        }
        gman.player1.cardsHand.Add(p1_stack_mid);
        for (int i = 0; i < p1_stack_right.Count; i++)
        {
            gman.player1.cardsHand.Add(p1_stack_right.Pop());
        }
        gman.player2.cardsHand.RemoveRange(0, gman.player2.cardsHand.Count);
        for (int i = 0; i < p2_stack_left.Count; i++)
        {
            gman.player2.cardsHand.Add(p2_stack_left.Pop());
        }
        gman.player2.cardsHand.Add(p2_stack_mid);
        for (int i = 0; i < p2_stack_right.Count; i++)
        {
            gman.player2.cardsHand.Add(p2_stack_right.Pop());
        }
    }
    void DisplayP1Hand()
    {
        p1_atk_but.gameObject.SetActive(false);
        p1_sur_but.gameObject.SetActive(false);
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
            p1_card_images_arr[0].enabled = false;
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
        p2_atk_but.gameObject.SetActive(false);
        p2_sur_but.gameObject.SetActive(false);
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

    public void P1Attack()
    {
        is_p1_atk = true;
    }

    public void P1Surrender()
    {
        is_p1_sur = true;
    }
    public void P2Attack()
    {
        is_p2_atk = true;
    }
    public void P2Surrender()
    {
        is_p2_sur = true;
    }
}
