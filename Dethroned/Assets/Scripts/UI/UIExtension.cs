using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIExtension : MonoBehaviour
{
    public bool openLeft = true;
    public void moveUIObject(GameObject obj)
    {
        var objR = obj.GetComponent<RectTransform>();
        var width = Mathf.Abs(objR.rect.width);
        bool closed = (openLeft && (width == Mathf.Abs( objR.anchoredPosition.x))) || (!openLeft && (width != Mathf.Abs(objR.anchoredPosition.x)));
        Debug.Log($"closed:{closed}\nopenLeft:{openLeft}\nwidth&transform.pos.x:{width}:{Mathf.Abs(objR.anchoredPosition.x)}");
        if (closed)
        {
            if (openLeft)
                objR.anchoredPosition += new Vector2(-width, 0);
            else
                objR.anchoredPosition += new Vector2(width, 0);
        }
        else
        {
            if(openLeft)
                objR.anchoredPosition += new Vector2(width, 0);
            else
                objR.anchoredPosition += new Vector2(-width, 0);
        }
    }
}
