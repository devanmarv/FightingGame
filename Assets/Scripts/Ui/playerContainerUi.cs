using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class playerContainerUi : MonoBehaviour
{

    public TextMeshProUGUI scoreText;
    public Image healthBarFill;

    public void Initialize(Color color)
    {
        scoreText.color = color;
        healthBarFill.color = color;

        scoreText.text = "O";
        healthBarFill.fillAmount = 1.0f;
    }

    public void updateScoreText(int score)
    {
        scoreText.text = score.ToString();
    }

    public void updateHealthBarFill(int curHp, int maxHp)
    {
        healthBarFill.fillAmount = (float)curHp / (float)maxHp;
    }


}
