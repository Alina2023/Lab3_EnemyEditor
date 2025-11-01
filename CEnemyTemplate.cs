using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace Lab2_EnemyEditor
{
    public class CEnemyTemplate
    {
        // Публичные свойства 
        public string Name { get; set; }
        public string IconName { get; set; }
        public int BaseLife { get; set; }
        public double LifeModifier { get; set; }
        public int BaseGold { get; set; }
        public double GoldModifier { get; set; }
        public double SpawnChance { get; set; }

        // Пустой конструктор нужен для десериализации
        public CEnemyTemplate() { }

        public CEnemyTemplate(string name, string iconName, int baseLife, double lifeModifier, int baseGold, double goldModifier, double spawnChance)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            IconName = iconName ?? throw new ArgumentNullException(nameof(iconName));
            BaseLife = baseLife;
            LifeModifier = lifeModifier;
            BaseGold = baseGold;
            GoldModifier = goldModifier;
            SpawnChance = spawnChance;
        }

        // Совместимость с кодом, который вызывает методы
        public double GetSpawnChanceAsDouble() => SpawnChance;
        public void SetNormalizedSpawnChance(double v) => SpawnChance = v;
        public double GetLifeModifier() => LifeModifier;
        public double GetGoldModifier() => GoldModifier;

        public override string ToString() => $"{Name}";
    }
}
