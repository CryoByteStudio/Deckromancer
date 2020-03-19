using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealDamage : Ability
{
    CardMaterials target;
    [SerializeField]
    int damage = 0;
    CombatManager combatMan;
    public override void Activate()
    {
        if (target)
        {
            target.cur_health -= damage;
        }
        else
        {
            GetTarget();
            Activate();
        }
    }

    public void GetTarget()
    {
        if (combatMan.is_card_selected)
        {
            target = combatMan.attacker;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        combatMan = FindObjectOfType<CombatManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
