using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class speftbase : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator clickanimator;//特效动画控制器

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        clickanimation();
    }

    void clickanimation()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            clickanimator.SetTrigger("click");
        }
    }
}
