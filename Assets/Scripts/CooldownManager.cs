using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using UnityEngine;

public class CooldownManager : MonoBehaviour
{
    private static CooldownManager instance;

    public static CooldownManager Instance { get { return instance; } }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

        //Initializes list of cooldowns
        for (int i = 0; i < scriptableObjects.Length; i++)
            cooldowns.Add(scriptableObjects[i].GetCooldownData());
    }

    public CooldownData FindCooldown(CooldownData.CooldownType desiredType)
    {
        for (int i = 0; i < cooldowns.Count; i++)
        {
            if (cooldowns[i].type == desiredType)
            {
                CooldownData shotGunCooldown = cooldowns[i];
                return shotGunCooldown;
            }
        }

        Debug.Log("TypeIsMissing");
        return null;
    }

    [SerializeField]
    private CooldownScriptableObject[] scriptableObjects;

    public List<CooldownData> cooldowns = new List<CooldownData>();

    private void Start()
    {

    }

    private void Update()
    {
        for (int i = 0; i < cooldowns.Count; i++) 
        {
            if(cooldowns[i].timer <= cooldowns[i].requiredTime) 
            {
                cooldowns[i].timer += Time.deltaTime;
            }
        }
    }


}

[System.Serializable]
public class CooldownData 
{
    public enum CooldownType 
    { 
        DEFAULT = 0,
        SHOTGUN = 1,
        FRICTION = 2,
        MACHINEGUN = 3,
        ENEMYJUMP = 4
    
    }

    void Awake() 
    {
        timer = requiredTime;
    }

    [HideInInspector] public float timer = 0;
    public float requiredTime;
    public CooldownType type;

    public bool IsReady()
    {
        return timer >= requiredTime;
    }


}

