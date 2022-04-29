using System.Collections;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using NaughtyAttributes;

[System.Serializable]
public class CardEffects
{
    public enum TargetingType { None, Self, SingleEnemy, RandomEnemy, MultipleEnemies, AllEnemies };
    public enum AmountType { None, Fixed, AllEnergy, AllBuff, AllDebuff, BasedOnEnergy, BasedOnBuff, BasedOnDebuff, BasedOnEnemyBuff, BasedOnEnemyDebuff, BasedOnOtherCards };


    public TargetingType target;
    public bool conditional;
    [ShowIf("conditional")]
    [AllowNesting]
    public ConditionalEffect conditionalStatement;
    public AmountType damageType;
    [ShowIf("damageType", AmountType.Fixed)]
    [AllowNesting]
    public int damage;
    
    [HorizontalLine(color: NaughtyAttributes.EColor.Red)]
    public DamageModifier damageMod;
    [HorizontalLine(color: NaughtyAttributes.EColor.Violet)]
    public CardModifier cardMod;
    
    [HorizontalLine(color: NaughtyAttributes.EColor.Blue)]
    public List<DebuffCommand> debuffs;
    [HorizontalLine(color: NaughtyAttributes.EColor.Green)]
    public List<BuffCommand> buffs;

    [System.Serializable]
    public class DebuffCommand
    {
        [Dropdown("GetStatuses")]
        public string debuff;
        public int count;
        private DropdownList<string> GetStatuses()
        {
            return CardEffects.GetStatuses();
        }
    }
    [System.Serializable]
    public class BuffCommand
    {
        [Dropdown("GetStatuses")]
        public string buff;
        public int count;
        private DropdownList<string> GetStatuses()
        {
            return CardEffects.GetStatuses();
        }
    }
    [System.Serializable]
    public class UniqueCommand
    {
        [Dropdown("GetStatuses")]
        public string buff;
        public int count;
        private DropdownList<string> GetStatuses()
        {
            return CardEffects.GetStatuses();
        }
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
        [Dropdown("GetStatuses")]
        public string buffStatus;
        [ShowIf("conTarget", ConTarget.Debuff)]
        [Dropdown("GetStatuses")]
        public string debuffStatus;
        [ShowIf("conTarget", ConTarget.Unique)]
        [Dropdown("GetStatuses")]
        public string uniqueStatus;
        private DropdownList<string> GetStatuses()
        {
            return CardEffects.GetStatuses();
        }
    }
    [System.Serializable]
    public class DamageModifier
    {
        public enum ModifierTypeD { None, PlusX, MinusX, TimesX, DivideX};
        public ModifierTypeD modType;
        public AmountType amountType;
        [ShowIf("amountType", AmountType.Fixed)]
        [AllowNesting]
        public int modAmount;
        public bool modConditional;
        [ShowIf("modConditional")]
        [AllowNesting]
        public ConditionalEffect conditionalStatement;
    }
    [System.Serializable]
    public class CardModifier
    {
        public enum ModifierTypeC { None, BaseStat, Cost};
        public ModifierTypeC modType;
        public AmountType amountType;
        [ShowIf("amountType", AmountType.Fixed)]
        [AllowNesting]
        public int modAmount;
        public bool modConditional;
        [ShowIf("modConditional")]
        [AllowNesting]
        public ConditionalEffect conditionalStatement;
    }
    public static DropdownList<string> GetStatuses()
    {
        NaughtyAttributes.DropdownList<string> stringList = new DropdownList<string>();
        var assem = (
                from domainAssembly in System.AppDomain.CurrentDomain.GetAssemblies()
                    // alternative: from domainAssembly in domainAssembly.GetExportedTypes()
                    from assemblyType in domainAssembly.GetTypes()
                where typeof(Status).IsAssignableFrom(assemblyType) && !assemblyType.IsAbstract
                    // alternative: where assemblyType.IsSubclassOf(typeof(B))
                    select assemblyType).ToArray();
        for (int i = 0; i < assem.Count(); i++)
        {
            stringList.Add(assem[i].ToString(), assem[i].FullName);
        }
        return stringList;
    }
}
