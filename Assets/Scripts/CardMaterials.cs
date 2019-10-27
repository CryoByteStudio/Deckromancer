using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardMaterials : MonoBehaviour
{
    public enum Card_Type { Location, Power, Horde};
    public Card_Type card_type;
    public Material back_mat;
    public Material front_mat;

    public int add_power_draw_int = 0;
    public int add_horde_draw_int = 0;

    public int health =0;
    public int cur_health;
    public enum Die_Type { d2 =2, d4 = 4, d6 = 6, d8=8 };
    public Die_Type die_dmg;
    public int add_dmg = 0;

    public enum sacrifice_type { horde, power};
    public int sacrifice_amount = 0;

    // Start is called before the first frame update
    void Start()
    {
        MeshRenderer[] card_meshs = GetComponentsInChildren<MeshRenderer>();
        card_meshs[0].material = back_mat;
        card_meshs[1].material = front_mat;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
