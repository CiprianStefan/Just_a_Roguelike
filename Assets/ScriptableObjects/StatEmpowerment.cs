using UnityEngine;

[CreateAssetMenu(fileName = "StatEmpowerment", menuName = "Empowerment/StatEmpowerment")]
public class StatEmpowerment : LevelUpEmpowerment
{
    [TextArea]
    public string Description;
    public StatType StatType;
    public int Value;

    public StatEmpowerment() : base(EmpowermentType.Stat)
    {
    }
}
