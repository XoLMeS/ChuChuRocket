using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class createMap : MonoBehaviour {

    public Sprite sprite;

    // Max size 12x10
    private int map_x = 12;
    private int map_y = 10;

    private int block_size = 100;
    private float block_scale = 1f;

    private Color color_1 = Color.yellow;
    private Color color_2 = Color.magenta;

    private Sprite border_h;
    private Sprite border_v;

    private int mouse_id = 0;
    private int rocket_id = 0;
    private int cat_id = 0;


    // Use this for initialization
    void Start () {
        border_h = Resources.Load<Sprite>("border_h");
        border_v = Resources.Load<Sprite>("border_v");
    }

    public void create()
    {
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
                    sprite = Sprite.Create(texture, new Rect(0, 0, block_size, block_size), new Vector2(x, y));
                }
                 else
                {
                    sprite = Sprite.Create(texture2, new Rect(0, 0, block_size, block_size), new Vector2(x, y));
                }

                GameObject go = new GameObject("sprite_" + (j + i*map_x));
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
        Create2(0);
       
    }

    public void Create2(int map_id)
    {
        // Get map from DB (Map structure below)
        // map[] = DB.getMap(map);
        
        string[] map = {
            ""+map_x, ""+map_y, "2","2","2","2",
            "", "", "", "", "", "", "", "", "", "", "", "",          // 1
            "", "", "", "", "", "", "", "", "", "", "", "",          // 2
            "M", "", "", "", "", "", "", "", "", "", "", "",          // 3
            "", "MBL", "", "", "", "", "", "", "", "", "", "",          // 4
            "", "", "MBR", "", "", "", "", "", "", "", "", "",          // 5
            "", "", "", "MBU", "", "", "", "", "", "", "", "",          // 6
            "", "", "", "", "MBD BL", "", "", "", "", "", "", "",          // 7
            "", "", "", "", "", "", "", "", "", "", "", "",          // 8
            "", "", "", "", "", "", "", "", "", "", "", "",          // 9
            "RT", "", "", "", "", "", "", "", "", "", "", "",          // 10
        };

        // Get general map info
        map_x = Int32.Parse(map[0]);
        map_y = Int32.Parse(map[1]);
        int arrows_l = Int32.Parse(map[2]);
        int arrows_r = Int32.Parse(map[3]);
        int arrows_u = Int32.Parse(map[4]);
        int arrows_d = Int32.Parse(map[5]);

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
                SpawnMouse(block_id%map_x,block_id/map_x);
                mouse_id++;
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

    public void SpawnMouse(float x, float y)
    {
        GameObject mouse = (GameObject) Instantiate(Resources.Load("mouse"));
        mouse.name = "mouse_"+mouse_id;
        mouse.tag = "block";
        mouse.layer = 8;

        var pos = new Vector3(0.5f - (float)map_x/2 + 2 * block_scale + x, 0.5f + (float)map_y /2 - block_scale - y, -0.5f);
        mouse.transform.position = pos;

        mouse.transform.Rotate(new Vector3(0, 90, -90));

        var scale = new Vector3(4, 4, 4);
        mouse.transform.localScale = scale;

        var rigid = mouse.AddComponent<Rigidbody>();
        rigid.useGravity = false;
        rigid.isKinematic = true;

        var collider = mouse.AddComponent<BoxCollider>();
        var size = new Vector3(0.15f, 0.15f, 0.15f);
        collider.size = size;

        var movement = mouse.AddComponent<Movement>();
        var collisions = mouse.AddComponent<BorderCollision>();

    }

    public void AddRocket(float x, float y)
    {
        GameObject rocket = (GameObject)Instantiate(Resources.Load("rocket"));
        rocket.name = "rocket_"+rocket_id;
        rocket.tag = "block";
        rocket.layer = 10;

        var pos = new Vector3(0.5f - (float)map_x / 2 + 2 * block_scale + x, 0.5f + (float)map_y / 2 - block_scale - y, -0.5f);
        rocket.transform.position = pos;

        rocket.transform.Rotate(new Vector3(0, 90, -90));

        var scale = new Vector3(1, 1, 1);
        rocket.transform.localScale = scale;

        var rigid = rocket.AddComponent<Rigidbody>();
        rigid.useGravity = false;
        rigid.isKinematic = true;

        var collider = rocket.AddComponent<BoxCollider>();
        collider.isTrigger = true;
        var size = new Vector3(0.5f, 0.5f, 0.5f);
        collider.size = size;

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

        var pos = new Vector3(x, y, -0.5f);
        obj.transform.position = pos;

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
           
	
	// Update is called once per frame
	void Update () {
		
	}

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
