using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Wall : MonoBehaviour
{
    public RectTransform rectTr;
    void Start()
    {
        rectTr = UiManager.instance.toolTipRectTransform;
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "ItemToolTip")
        {
            print("벽과 충돌함");
            rectTr.anchoredPosition += new Vector2(0, 300);
        }
    }
}
