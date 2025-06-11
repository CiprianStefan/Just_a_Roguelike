using UnityEngine;

[CreateAssetMenu(fileName = "EvolutionEmpowerment", menuName = "Empowerment/EvolutionEmpowerment")]
public class EvolutionEmpowerment : LevelUpEmpowerment
{
    public string AbilityReplaceName;
    public GameObject abilityHolder;
    
    public SkillContainer SkillContainer
    {
        get
        {
            return abilityHolder != null ? abilityHolder.GetComponent<AbilityHolder>().skillContainer : null;
        }
    }

    public EvolutionEmpowerment() : base(EmpowermentType.Evolution)
    {
    }
}
