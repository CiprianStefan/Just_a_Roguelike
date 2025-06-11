using UnityEngine;

public enum EmpowermentType
{
    Stat,
    Ability,
    Evolution,
}

public class LevelUpEmpowerment : ScriptableObject
{
    public string Name;
    public readonly EmpowermentType EmpowermentType;

    public LevelUpEmpowerment(EmpowermentType empowermentType)
    {
        EmpowermentType = empowermentType;
    }
}
