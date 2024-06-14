using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.UIElements;
using UnityEngine.UI;


public class BackgroundChanger : MonoBehaviour
{
    Dictionary <string, Color> colors = new Dictionary<string, Color>();
    public string status = "Night";
     public Image image;
    void Start()
    {
        colors.Add("Night", new Color(0.182f, 0.535f, 0.638f, 1f));
        colors.Add("Day", new Color(204f, 193f, 83f, 0f));
        image =  GetComponent<Image>();
        StartCoroutine(ExecuteEveryMinute());
    }

    IEnumerator ExecuteEveryMinute()
    {
        status = "Night";
        while (true)
        {
            if(status == "Night") status = "Day";
            else status = "Night";
            image.color = colors[status];
            yield return new WaitForSeconds(5f);
        }
    }
}
