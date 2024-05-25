using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class KnifeStats : ScriptableObject
{
    public GameObject Knife;
    public int Damage;
    public int freeze;
    public int speed;
    public int MaxKinfeCount;
    public int CurrentKinfeCount;
}
