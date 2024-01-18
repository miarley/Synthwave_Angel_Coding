using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class CharacterEvent
{
    public static UnityAction<GameObject,int> characterDamaged;
    public static UnityAction<GameObject, int> characterHealed;
    public static UnityAction<GameObject> characterBroken;
    public static UnityAction<GameObject> characterDead;
}
