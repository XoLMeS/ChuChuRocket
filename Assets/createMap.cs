using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class createMap : MonoBehaviour {

    public Sprite sprite;

    // Max size 12x10
    private int map_x = 12;
    private int map_y = 10;

    private int block_size = 64 + 64/2;
    private float block_scale = 1f;

    private Color color_1 = Color.yellow;
    private Color color_2 = Color.magenta;

	// Use this for initialization
	void Start () {
      
	}

    public void create()
    {
        block_scale = block_size / 64;

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
        create2(0);
       
    }

    public void create2(int map_id)
    {
        // Get map from DB (Map structure below)
        // map[] = DB.getMap(map);
        string[] map = {
            ""+map_x, ""+map_y, "2","2","2","2",
            "B", "", "", "", "", "", "", "", "", "", "", "",          // 1
            "", "B", "", "", "", "", "", "", "", "", "", "",          // 2
            "", "", "B", "", "", "", "", "", "", "", "", "",          // 3
            "", "", "", "B", "", "", "", "", "", "", "", "",          // 4
            "", "", "", "", "B", "", "", "", "", "", "", "",          // 5
            "", "", "", "", "", "B", "", "", "", "", "", "",          // 6
            "", "", "", "", "", "", "B", "", "", "", "", "",          // 7
            "", "", "", "", "", "", "", "B", "", "", "", "",          // 8
            "", "", "", "", "", "", "", "", "B", "", "", "",          // 9
            "", "", "", "", "", "", "", "", "", "B", "", "",          // 10
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

            if (block.Contains("B"))
            {
                GameObject to_destroy = GameObject.Find("sprite_" + block_id);
                Destroy(to_destroy);
            }

          
        }
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
        // B - background (no sprite on this spot)
        // B(L|R|U|D) - border left|right|up|down
        // M(L|R|U|D) - mouse and movement
        // C(L|R|U|D) - cat and movement
        // E - exit for mouse

        // [
            //  X, Y, LN, RN, UN, DN,
            //  'B | ( B(L|R|U|D) & ( M(L|R|U|D) | C(L|R|U|D) ) )',  (X*Y times)
        // ]
}
