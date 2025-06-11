using UnityEngine;

[CreateAssetMenu(fileName = "AbilityEmpowerment", menuName = "Empowerment/AbilityEmpowerment")]
public class AbilityEmpowerment : LevelUpEmpowerment
{
    public GameObject abilityHolder;
    public SkillContainer SkillContainer
    {
        get
        {
            return abilityHolder != null ? abilityHolder.GetComponent<AbilityHolder>().skillContainer : null;
        }
    }

    public AbilityEmpowerment() : base(EmpowermentType.Ability)
    {
    }
}
