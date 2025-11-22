using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardPair : MonoBehaviour
{
    public GameObject rewardA;
    public GameObject rewardB;

    bool cleaning = false;

    void Update()
    {
        if (cleaning) return;

        // A가 사라짐, B도 삭제
        if (rewardA == null && rewardB != null)
        {
            cleaning = true;
            Destroy(rewardB);
        }
        // B가 사라짐, A도 삭제
        else if (rewardB == null && rewardA != null)
        {
            cleaning = true;
            Destroy(rewardA);
        }

        /*
        // 둘 다 사라지면 자신도 삭제
        if (rewardA == null && rewardB == null)
        {
            Destroy(gameObject);
        }
        */
    }
}