using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scriptLivesUI : MonoBehaviour
{
    // Start is called before the first frame update
    public float lives;
    [SerializeField] Sprite three;
    [SerializeField] Sprite two;
    [SerializeField] Sprite one;
    [SerializeField] Sprite none;

    void Start()
    {
        lives = 3;
        ChangeSprite(3);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeSprite(float nLives)
    {
        lives = nLives;
        Image image = gameObject.GetComponent<Image>();

        switch (lives)
        {
            case 3:
                image.sprite = three;
                break;
            case 2:
                image.sprite = two;
                break;
            case 1:
                image.sprite = one;
                break;
            case 0:
                image.sprite = none;
                break;
        }
    }
}
