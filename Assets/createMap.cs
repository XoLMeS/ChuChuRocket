using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class createMap : MonoBehaviour {

    public Sprite sprite;

    // Max size 12x10
    private int map_x = 12;
    private int map_y = 9;

    private int block_size = 100;
    private float block_scale = 1f;

    private Color color_1 = Color.yellow;
    private Color color_2 = Color.magenta;

    private Sprite border_h;
    private Sprite border_v;

    private int mouse_id = 0;
    private int rocket_id = 0;
    private int cat_id = 0;

    private int selected_map = 0;

    public static int arrows_l = 0;
    public static int arrows_r = 0;
    public static int arrows_u = 0;
    public static int arrows_d = 0;

    // Use this for initialization
    void Start () {
        border_h = Resources.Load<Sprite>("border_h");
        border_v = Resources.Load<Sprite>("border_v");
    }

    public void Activate()
    {
        Status.active = !Status.active;

        if (Status.active)
        {
            GameObject.Find("StartReset").GetComponentInChildren<Text>().text = "Reset";
        }
        else
        {
            GameObject.Find("StartReset").GetComponentInChildren<Text>().text = "Start";
            Create(selected_map);
        }
    }

    public void Create(int map_id)
    {
        Status.active = false;

        Status.selected_arrrow = "";

        GameObject.Find("Arrow_L").GetComponent<Image>().color = Color.blue;
        GameObject.Find("Arrow_R").GetComponent<Image>().color = Color.blue;
        GameObject.Find("Arrow_D").GetComponent<Image>().color = Color.blue;
        GameObject.Find("Arrow_U").GetComponent<Image>().color = Color.blue;

        selected_map = map_id;

        block_scale = block_size / 64;

        mouse_id = 0;
        rocket_id = 0;
        cat_id = 0;

    // Delete all blocks
        GameObject[] blocks = GameObject.FindGameObjectsWithTag("block");
        if (blocks != null)
        {
            for (int i =0; i < blocks.Length; i++)
            {
                Destroy(blocks[i]);
            }
        }   

    // Create texture with color_1
        Texture2D texture = new Texture2D(block_size, block_size, TextureFormat.ARGB32, false);
        var colors = new Color[block_size*block_size];
        for(int i = 0; i < colors.Length; i++)
        {
            colors[i] = color_1;
        }
        
        texture.SetPixels(0, 0, block_size, block_size, colors, 0);
        texture.Apply();

    // Create texture with color_2
        Texture2D texture2 = new Texture2D(block_size, block_size, TextureFormat.ARGB32, false);
        var colors2 = new Color[block_size * block_size];
        for (int i = 0; i < colors.Length; i++)
        {
            colors2[i] = color_2;
        }

        texture2.SetPixels(0, 0, block_size, block_size, colors2, 0);
        texture2.Apply();

    // Create default map map_x*map_y
        float x = (float) map_x/2 - 2 * block_scale;
        float y = -(float) map_y/2 + block_scale;
        bool switch_color = true;
        for (int i = 0; i < map_y; i++)
        {
            for (int j = 0; j < map_x; j++)
            {
                int block_id = j + i * map_x;

                if (switch_color)
                {
                    sprite = Sprite.Create(texture, new Rect(0, 0, block_size, block_size), new Vector2(0.5f, 0.5f));
                }
                 else
                {
                    sprite = Sprite.Create(texture2, new Rect(0, 0, block_size, block_size), new Vector2(0.5f, 0.5f));
                }

                GameObject go = new GameObject("sprite_" + (j + i*map_x));
                var collider = go.AddComponent<BoxCollider>();
                collider.transform.position = new Vector3(-x + 0.5f,-y + 0.5f, 0f);
                go.AddComponent<TapScript>();
                go.tag = "block";
                SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
                sr.sprite = sprite;

                // Draw default border
                // Top border
                    if (block_id < map_x)
                    {
                        string name = "border_" + (block_id) + "_h";
                        AddBorder(-x + 0.5f, -y + 1, name, go, false);
                    }
                // Left border
                    if (block_id % map_x == 0)
                    {
                        string name = "border_" + (block_id) + "_v";
                        AddBorder(-x , -y + 0.5f, name, go, true);
                    }
                // Right border
                    if (block_id != 0 && (block_id + 1) % (map_x) == 0)
                    {
                        string name = "border_" + (block_id) + "_v";
                        AddBorder(-x + 1f, -y + 0.5f, name, go, true);
                    }
                // Bottom border
                    if (block_id >= map_x * map_y - map_x)
                    {
                        string name = "border_" + (block_id) + "_h";
                        AddBorder(-x + 0.5f, -y, name, go, false);
                       
                    }

                x -= block_scale;
                switch_color = !switch_color;
            }
            if (map_x%2==0)
            {
                switch_color = !switch_color;
            }
            x = (float)map_x / 2 - 2 * block_scale;
            y += block_scale;
        }

        // Modify default map
        Create2();
       
    }

    public void Create2()
    {
        // Get map from DB (Map structure below)
        // map[] = DB.getMap(map);
        string[] map = map_01;
        if (selected_map == 1)
        {
            map = map_01;
        }

        if (selected_map == 2)
        {
            map = map_02;
        }

        // Get general map info
        map_x = Int32.Parse(map[0]);
        map_y = Int32.Parse(map[1]);
        arrows_l = Int32.Parse(map[2]);
        GameObject.Find("Arrow_L").GetComponentInChildren<Text>().text = "←" + "x" + arrows_l;
        arrows_r = Int32.Parse(map[3]);
        GameObject.Find("Arrow_R").GetComponentInChildren<Text>().text = "→" + "x" + arrows_r;
        arrows_u = Int32.Parse(map[4]);
        GameObject.Find("Arrow_U").GetComponentInChildren<Text>().text = "↑" + "x" + arrows_u;
        arrows_d = Int32.Parse(map[5]);
        GameObject.Find("Arrow_D").GetComponentInChildren<Text>().text = "↓" + "x" + arrows_d;

        // Create map sprites
        for (int i = 6; i < map.Length; i++)
        {
            string block = map[i];
            int block_id = i - 6;

            // Delete marked blocks
            if (block.Contains("E"))
            {
                GameObject to_destroy = GameObject.Find("sprite_" + block_id);
                Destroy(to_destroy);
            }

            // Add mouse
            if (block.Contains("M"))
            {
                SpawnMouse(block_id%map_x,block_id/map_x,block);
                mouse_id++;
            }

            // Add cat
            if (block.Contains("C"))
            {
                SpawnCat(block_id % map_x, block_id / map_x,block);
                cat_id++;
            }

            // Add rocket
            if (block.Contains("RT"))
            {
                AddRocket(block_id % map_x, block_id / map_x);
                rocket_id++;
            }

            // Add border
            // Left
                if (block.Contains("BL"))
                {
                    GameObject parent = GameObject.Find("sprite_" + block_id);
                    AddBorder(block_id % map_x - (float)map_x/2 + 2 * block_scale, 0.5f + (float)map_y / 2 - block_scale - block_id / map_x, "border_" + block_id + "v", parent, true);
                }
            // Right
                if (block.Contains("BR"))
                {
                    GameObject parent = GameObject.Find("sprite_" + block_id);
                    AddBorder(block_id % map_x - (float)map_x / 2 + 3 * block_scale, 0.5f + (float)map_y / 2 - block_scale - block_id / map_x, "border_" + block_id + "v", parent, true);
                }
            // Up
                if (block.Contains("BU"))
                {
                    GameObject parent = GameObject.Find("sprite_" + block_id);
                    AddBorder(block_id % map_x - (float)map_x / 2 + 2 * block_scale +0.5f, 2*0.5f + (float)map_y / 2 - block_scale - block_id / map_x, "border_" + block_id + "h", parent, false);
                }
            // Down
                if (block.Contains("BD"))
                {
                    GameObject parent = GameObject.Find("sprite_" + block_id);
                    AddBorder(block_id % map_x - (float)map_x / 2 + 2 * block_scale + 0.5f, (float)map_y / 2 - block_scale - block_id / map_x, "border_" + block_id + "h", parent, false);
                }

        }
    }

    public void SpawnMouse(float x, float y, string block)
    {
        GameObject mouse = (GameObject) Instantiate(Resources.Load("mouse"));
        mouse.name = "mouse_"+mouse_id;
        mouse.tag = "block";
        mouse.layer = 8;

        mouse.transform.position = new Vector3(0.5f - (float)map_x / 2 + 2 * block_scale + x, 0.5f + (float)map_y / 2 - block_scale - y, -0.5f);

        var movement = mouse.AddComponent<Movement>();
        movement._speed = 6;

        mouse.transform.Rotate(new Vector3(0, 90, -90));

        if (block.Contains("MU"))
        {
            mouse.transform.Rotate(new Vector3(0, 90, 0));
            movement._x = 0;
            movement._y = 1;
        }
        if (block.Contains("MR"))
        {
            mouse.transform.Rotate(new Vector3(0, 180, 0));
            movement._x = 1;
            movement._y = 0;
        }
        if (block.Contains("MD"))
        {
            mouse.transform.Rotate(new Vector3(0, -90, 0));
            movement._x = 0;
            movement._y = -1;
        }

        mouse.transform.localScale = new Vector3(4, 4, 4);

        var rigid = mouse.AddComponent<Rigidbody>();
        rigid.useGravity = false;
        rigid.isKinematic = true;

        var collider = mouse.AddComponent<BoxCollider>();
        collider.isTrigger = true;
        collider.size = new Vector3(0.15f, 0.15f, 0.15f);

        
        var collisions = mouse.AddComponent<BorderCollision>();

    }

    public void SpawnCat(float x, float y, string block)
    {
        GameObject cat = (GameObject)Instantiate(Resources.Load("cat"));
        cat.name = "cat_" + cat_id;
        cat.tag = "block";
        cat.layer = 8;

        cat.transform.position = new Vector3(0.5f - (float)map_x / 2 + 2 * block_scale + x, 0.5f + (float)map_y / 2 - block_scale - y, -0.5f);

        var movement = cat.AddComponent<Movement>();

        cat.transform.Rotate(new Vector3(0, 90, -90));

        if (block.Contains("CU"))
        {
            cat.transform.Rotate(new Vector3(0, 90, 0));
            movement._x = 0;
            movement._y = 1;
        }
        if (block.Contains("CR"))
        {
            cat.transform.Rotate(new Vector3(0, 180, 0));
            movement._x = 1;
            movement._y = 0;
        }
        if (block.Contains("CD"))
        {
            cat.transform.Rotate(new Vector3(0, -90, 0));
            movement._x = 0;
            movement._y = -1;
        }


        cat.transform.localScale = new Vector3(4, 4, 4);

        var rigid = cat.AddComponent<Rigidbody>();
        rigid.useGravity = false;
        rigid.isKinematic = true;

        var collider = cat.AddComponent<BoxCollider>();
        collider.size = new Vector3(0.15f, 0.15f, 0.15f);

      
        var collisions = cat.AddComponent<BorderCollision>();
        var collisions2 = cat.AddComponent<CatCollision>();
    }

    public void AddRocket(float x, float y)
    {
        GameObject rocket = (GameObject)Instantiate(Resources.Load("rocket"));
        rocket.name = "rocket_"+rocket_id;
        rocket.tag = "block";
        rocket.layer = 10;

        rocket.transform.position = new Vector3(0.5f - (float)map_x / 2 + 2 * block_scale + x, 0.5f + (float)map_y / 2 - block_scale - y, -0.5f);

        rocket.transform.Rotate(new Vector3(0, 90, -90));

        rocket.transform.localScale = new Vector3(1, 1, 1);

        var rigid = rocket.AddComponent<Rigidbody>();
        rigid.useGravity = false;
        rigid.isKinematic = true;

        var collider = rocket.AddComponent<BoxCollider>();
        collider.isTrigger = true;
        collider.size = new Vector3(1f, 1f, 1f);

        var collisions = rocket.AddComponent<RocketCollision>();

        var anumation = rocket.AddComponent<RocketAnimation>();
    }

    public void AddBorder(float x, float y, string name, GameObject parent, bool vert)
    {
        GameObject obj = (GameObject)Instantiate(Resources.Load("border"));
        obj.name = name;
        obj.tag = "block";
        obj.transform.SetParent(parent.transform);
        obj.layer = 10;

        obj.transform.position = new Vector3(x, y, -0.5f);

        if (!vert)
        {
            obj.transform.Rotate(new Vector3(0, 90, 90));
        }
        else
            obj.transform.Rotate(new Vector3(90, 0, 0));

        var rigid = obj.AddComponent<Rigidbody>();
        rigid.useGravity = false;

        var collider = obj.AddComponent<BoxCollider>();
        collider.isTrigger = true;

        var size = collider.size;
        size.x = 0.3f;
        collider.size = size;
    }


    public void SelectArrow(int arrow)
    {
        if (arrow == 0 && arrows_l > 0)
        {
            Status.selected_arrrow = "left";
            GameObject.Find("Arrow_L").GetComponent<Image>().color = Color.green;

            GameObject.Find("Arrow_R").GetComponent<Image>().color = Color.blue;
            GameObject.Find("Arrow_D").GetComponent<Image>().color = Color.blue;
            GameObject.Find("Arrow_U").GetComponent<Image>().color = Color.blue;

        }

        if (arrow == 1 && arrows_r > 0)
        {
            Status.selected_arrrow = "right";

            GameObject.Find("Arrow_R").GetComponent<Image>().color = Color.green;

            GameObject.Find("Arrow_L").GetComponent<Image>().color = Color.blue;
            GameObject.Find("Arrow_D").GetComponent<Image>().color = Color.blue;
            GameObject.Find("Arrow_U").GetComponent<Image>().color = Color.blue;
        }

        if (arrow == 2 && arrows_d > 0)
        {
            Status.selected_arrrow = "down";

            GameObject.Find("Arrow_D").GetComponent<Image>().color = Color.green;

            GameObject.Find("Arrow_L").GetComponent<Image>().color = Color.blue;
            GameObject.Find("Arrow_R").GetComponent<Image>().color = Color.blue;
            GameObject.Find("Arrow_U").GetComponent<Image>().color = Color.blue;
        }

        if (arrow == 3 && arrows_u > 0)
        {
            Status.selected_arrrow = "up";

            GameObject.Find("Arrow_U").GetComponent<Image>().color = Color.green;

            GameObject.Find("Arrow_L").GetComponent<Image>().color = Color.blue;
            GameObject.Find("Arrow_D").GetComponent<Image>().color = Color.blue;
            GameObject.Find("Arrow_R").GetComponent<Image>().color = Color.blue;
        }
    }

    string[] map_01 = {
            "12", "9", "0","0","1","0",
            "", "", "", "", "", "", "", "", "", "MR", "", "",          // 1
            "BD", "", "", "", "", "", "", "", "", "", "BR", "",          // 2
            "", "", "", "", "", "", "", "", "", "", "", "",          // 3
            "", "", "CR", "", "", "", "", "", "", "CD", "BR BL", "BU",          // 4
            "BD", "", "", "", "", "", "", "", "", "", "", "",          // 5
            "BD", "", "", "", "", "", "", "", "", "", "", "",          // 6
            "", "", "", "", "", "", "", "", "", "", "", "",          // 7
            "", "", "CU", "", "", "", "", "", "", "CL", "", "BU",          // 8
            "", "", "", "", "", "", "", "", "", "BU", "BU", "RT",          // 9
            "", "", "", "", "", "", "", "", "", "", "", "",          // 10
        };

    string[] map_02 = {
            "12", "9", "1","1","0","0",
            "RT", "", "MR", "", "ML", "", "ML", "", "MR", "", "MU", "",          // 1
            "", "MR", "", "MU", "", "MD", "BL", "ML", "", "MU", "", "MR",          // 2
            "MU", "", "ML", "", "ML", "", "BL MU", "", "MD", "", "MD", "",          // 3
            "", "MU", "", "MR", "", "MR", "BD BL", "BD MR", "BD", "ML BD", "BD", "MR BD",          // 4
            "MR", "", "MU", "", "ML", "", "MU", "", "ML", "", "MR", "",          // 5
            "BU", "BU MD", "BU", "BU MR", "BU", "BU BR MR", "", "ML", "", "MR", "", "MU",          // 6
            "MD", "", "MR", "", "MR", "BR", "MD", "", "MU", "", "ML", "",          // 7
            "", "MU", "", "MD", "", "BR MD", "", "MU", "", "MU", "", "MU",          // 8
            "ML", "", "MR", "", "MU", "", "MR", "", "MD", "", "MD", "RT",          // 9
            "", "", "", "", "", "", "", "", "", "", "", "",          // 10
        };



    // Map Structure
    // X - number of columns
    // Y - number of rows
    // RN - number of move-right arrows
    // LN - number of move-left arrows
    // UN - number of move-up arrows
    // DN - number of move-down arrows
    // E - enmpty background (no sprite on this spot)
    // B(L|R|U|D) - border left|right|up|down
    // M(L|R|U|D) - mouse and movement
    // C(L|R|U|D) - cat and movement
    // RT - rocket, exit for mouse

    // [
    //  X, Y, LN, RN, UN, DN,
    //  'B | ( B(L|R|U|D) & ( M(L|R|U|D) | C(L|R|U|D) ) )',  (X*Y times)
    // ]
}
