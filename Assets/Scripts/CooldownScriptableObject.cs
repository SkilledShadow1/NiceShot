using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Cooldown", menuName = "ScriptableObjects/CooldownScriptableObject", order = 1)]
public class CooldownScriptableObject : ScriptableObject
{
    [SerializeField]
    private CooldownData cooldown;

    public CooldownData GetCooldownData()
    {
        return new CooldownData
        {
            timer = cooldown.timer,
            requiredTime = cooldown.requiredTime,
            type = cooldown.type,
        
        };
    }
}
