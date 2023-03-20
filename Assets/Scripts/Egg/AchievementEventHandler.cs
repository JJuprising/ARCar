using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AchievementEventHandler
{
    public static event Action<int> MoneyUpdateEvent;

    public static void CallMoneyUpdateEvent(int money)
    {
        if (MoneyUpdateEvent != null)
        {
            MoneyUpdateEvent(money);
        }
    }

}
