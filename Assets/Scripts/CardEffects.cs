using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[System.Serializable]
public class CardEffects
{
    public enum TargetingType { None, Self, SingleEnemy, RandomEnemy, MultipleEnemies, AllEnemies };
    public enum BuffStatus {None, Armor};
    public enum DebuffStatus { None, Bleed, Fracture, Stun };
    public enum UniqueStatus {None };

    public TargetingType target;
    public int damage;
    public bool conditional;
    [ShowIf("conditional")]
    [AllowNesting]
    public ConditionalEffect conditionalStatement;

    public List<DebuffCommand> debuffs;
    public List<BuffCommand> buffs;

    [System.Serializable]
    public class DebuffCommand
    {
        public DebuffStatus debuff;
        public int count;
    }
    [System.Serializable]
    public class BuffCommand
    {
        public BuffStatus buff;
        public int count;
    }
    [System.Serializable]
    public class ConditionalEffect
    {
        public enum Conditionals { None, Equal, LessThan, GreaterThan };
        public enum ConTarget {None, Damage, Energy, Buff, Debuff, Unique }
        public TargetingType conditionTarget;
        public Conditionals condition;
        public int countOf;
        public ConTarget conTarget;
        [ShowIf("conTarget", ConTarget.Buff)]
        [AllowNesting]
        public BuffStatus buffStatus;
        [ShowIf("conTarget", ConTarget.Debuff)]
        [AllowNesting]
        public DebuffStatus debuffStatus;
        [ShowIf("conTarget", ConTarget.Unique)]
        [AllowNesting]
        public UniqueStatus uniqueStatus;
    }
}
