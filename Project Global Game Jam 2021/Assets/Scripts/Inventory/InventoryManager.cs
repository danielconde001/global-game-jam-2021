using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
#region Singleton
    private static InventoryManager _instance;
    public static InventoryManager Instance
    {
        get 
        {
             if (_instance == null)
             {
                _instance = GameObject.FindObjectOfType<InventoryManager>();
             
                if (_instance == null)
                  {
                    GameObject container = new GameObject("Inventory Manager");
                    _instance = container.AddComponent<InventoryManager>();
                  }
            }

            return _instance;
        }
    }
#endregion

    private static bool hasBoltCutter;
    private static bool hasBoneSaw;
    private static bool hasFinger;

    public bool HasBoltCutter() { return hasBoltCutter; }
    public bool HasBoneSaw() { return hasBoneSaw; }
    public bool HasFinger() { return hasFinger; }

    public void AttainBoltCutter() { hasBoltCutter = true; }
    public void AttainBoneSaw() { hasBoneSaw = true; }
    public void AttainFinger() { hasFinger = true; }

    private void Awake() {
        _instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
}
