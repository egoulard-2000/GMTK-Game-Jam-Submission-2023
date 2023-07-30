using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SockCharacterAnim : MonoBehaviour
{

    private SpriteRenderer sr;

    [Header("All Sprites")]
    [SerializeField] private Sprite normal;

    // Start is called before the first frame update
    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        ChangeToSprite(normal);
        gameObject.SetActive(false);
    }

    public void ChangeToSprite(Sprite sprite) { sr.sprite = sprite; }
}
