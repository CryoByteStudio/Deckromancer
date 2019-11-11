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

    private Vector3 startPosition;
    private Vector3 currentPosition;
    private float width;
    private float height;

    void Awake()
    {
        width = (float)Screen.width / 2.0f;
        height = (float)Screen.height / 2.0f;

        // Position used for the cube.
        startPosition = this.gameObject.transform.position;
        currentPosition = startPosition;
    }

    // Start is called before the first frame update
    void Start()
    {
        MeshRenderer[] card_meshs = GetComponentsInChildren<MeshRenderer>();
        card_meshs[0].material = back_mat;
        card_meshs[1].material = front_mat;
    }

    //void OnGUI()
    //{
    //    // Compute a fontSize based on the size of the screen width.
    //    GUI.skin.label.fontSize = (int)(Screen.width / 25.0f);

    //    GUI.Label(new Rect(20, 20, width, height * 0.25f),
    //        "x = " + startPosition.x.ToString("f2") +
    //        ", y = " + startPosition.y.ToString("f2"));
    //}

    // Update is called once per frame
    void Update()
    {
        //// Handle screen touches.
        //if (Input.touchCount > 0)
        //{
        //    Touch touch = Input.GetTouch(0);

        //    // Move the cube if the screen has the finger moving.
        //    if (touch.phase == TouchPhase.Moved)
        //    {
        //        Vector2 pos = touch.position;
        //        pos.x = (pos.x - width) / width;
        //        pos.y = (pos.y - height) / height;
        //        currentPosition = new Vector3(-pos.x, pos.y, 0.0f);

        //        // Position the cube.
        //        transform.position = currentPosition;
        //    }

        //    if (Input.touchCount == 2)
        //    {
        //        touch = Input.GetTouch(1);

        //        if (touch.phase == TouchPhase.Began)
        //        {
        //            // Halve the size of the cube.
        //            transform.position = new Vector3(startPosition.x, startPosition.y+5.0f, startPosition.z);
        //        }

        //        if (touch.phase == TouchPhase.Ended)
        //        {
        //            // Restore the regular size of the cube.
        //            transform.position = new Vector3(startPosition.x, startPosition.y, startPosition.z);
        //        }
        //    }
        //}
    }
}
