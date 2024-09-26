using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HexObjects", menuName = "ScriptableObjects/HexObjectsSO", order = 1)]
public class HexGridObjectsSO : ScriptableObject
{
    public Transform prefabHex;
    public Transform prefabAttack;
    public Transform prefabStar;
    
}
