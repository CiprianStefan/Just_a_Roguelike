using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelUpEmpowermentDatabase", menuName = "Database/LevelUpEmpowermentDatabase")]
public class LevelUpEmpowermentDatabase : ScriptableObject
{
    public List<LevelUpEmpowerment> levelUpEmpowerments;
}
