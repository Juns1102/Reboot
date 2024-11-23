using NUnit.Framework;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InventoryManager : Singleton<InventoryManager> {
    public List<GameObject> items = new List<GameObject>();
}
