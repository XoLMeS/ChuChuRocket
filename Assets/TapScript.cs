using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TapScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {

                //Debug.Log("I'm hitting " + hit.collider.name);

                if (hit.collider.name.Contains("sprite"))
                {
                    if (!Status.selected_arrrow.Equals(""))
                    {
                        var block = GameObject.Find(hit.collider.name);

                        block.GetComponent<BoxCollider>().isTrigger = true;
                        block.GetComponent<BoxCollider>().size = new Vector3(1.3f, 1.3f, 1.3f);

                        if (Status.selected_arrrow.Equals("right") && createMap.arrows_r>0)
                        {
                            block.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("arrow_r");
                            block.name = "arrow_R";
                            createMap.arrows_r--;
                            GameObject.Find("Arrow_R").GetComponentInChildren<Text>().text = "→" + "x" + createMap.arrows_r;
                        }

                        if (Status.selected_arrrow.Equals("left") && createMap.arrows_l > 0)
                        {
                            block.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("arrow_l");
                            block.name = "arrow_L";
                            createMap.arrows_l--;
                            GameObject.Find("Arrow_L").GetComponentInChildren<Text>().text = "←" + "x" + createMap.arrows_l;
                        }

                        if (Status.selected_arrrow.Equals("down") && createMap.arrows_d > 0)
                        {
                            block.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("arrow_d");
                            block.name = "arrow_D";
                            createMap.arrows_d--;
                            GameObject.Find("Arrow_D").GetComponentInChildren<Text>().text = "↓" + "x" + createMap.arrows_d;
                        }

                        if (Status.selected_arrrow.Equals("up") && createMap.arrows_u > 0)
                        {
                            block.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("arrow_u");
                            block.name = "arrow_U";
                            createMap.arrows_u--;
                            GameObject.Find("Arrow_U").GetComponentInChildren<Text>().text = "↑" + "x" + createMap.arrows_u;
                        }

                    }
                }

            }
        }
    }
 }
