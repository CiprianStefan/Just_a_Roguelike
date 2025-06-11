using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoulContainerDatabase", menuName = "SoulContainerDatabase", order = 1)]
public class SoulContainerDatabase : ScriptableObject
{
    public List<SoulContainer> soulContainers;
}
