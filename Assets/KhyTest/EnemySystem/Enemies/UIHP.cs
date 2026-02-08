using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIHP : MonoBehaviour
{
    [SerializeField] private BossEnemy bossEnemy;
    [SerializeField] private Slider HP;
    [SerializeField] private TextMeshProUGUI textHp;

    private void Update()
    {
        if (HP != null)
        {
            HP.value = Utils.Percent(bossEnemy.CurrentHp, bossEnemy.MaxHP);
        }

        if (textHp != null)
        {
            textHp.text = $"{bossEnemy.CurrentHp:F0}/{bossEnemy.MaxHP:F0}";
        }

    }

}
