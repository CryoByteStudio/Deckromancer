using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    CardMaterials[] p1hand;
    CardMaterials[] p2hand;
    [SerializeField]
    CardMaterials attacker;
    [SerializeField]
    CardMaterials defender;

    public float phase_timer_fl = 5.0f;
    public bool is_declare_horde;
    public bool is_combat;
    public bool is_combat_completed;
    public bool is_attacker_win;
    public bool is_defender_win;


    // Start is called before the first frame update
    void Start()
    {
        attacker.cur_health = attacker.health;
        defender.cur_health = defender.health;
    }

    // Update is called once per frame
    void Update()
    {
    
        if(attacker && defender && attacker.cur_health>0 && defender.cur_health>0 )
        {
        Debug.Log("attacker health is" + attacker.cur_health);
        Debug.Log("defender health is" + defender.cur_health);
            Fight();
            Debug.Log("attacker health is" + attacker.cur_health);
            Debug.Log("defender health is" + defender.cur_health);
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
}
