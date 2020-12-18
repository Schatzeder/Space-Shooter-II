using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TIV_UIManager : MonoBehaviour
{
    [SerializeField]
    private Image _livesImage = null;
    [SerializeField]
    private Sprite[] _livesSprites = null;

    [SerializeField]
    private Image _thrusterVisual = null;

    public int int1;
    private int int2;
    public int int3;

    public void UpdateLivesVisual(int lives)
    {
        _livesImage.sprite = _livesSprites[lives];
    }

    public void UpdateThrusterVisual(float fillAmount)
    {
        _thrusterVisual.fillAmount = fillAmount;
    }
}
