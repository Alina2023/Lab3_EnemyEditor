using System.Windows.Controls;
using Lab2_EnemyEditor;


namespace Lab2_EnemyEditor.Game
{
    public class CEnemy
    {
        public string Name { get; private set; }
        public BigNumber HitPoints { get; private set; }
        public BigNumber GoldReward { get; private set; }
        public Image IconImage { get; private set; }
        public CEnemyTemplate Template { get; private set; }

        public CEnemy(CEnemyTemplate template, Image icon, BigNumber hpOverride = null, BigNumber goldOverride = null)
        {
            Template = template;
            Name = template.Name;
            IconImage = icon;
            HitPoints = hpOverride ?? new BigNumber(template.BaseLife);
            GoldReward = goldOverride ?? new BigNumber(template.BaseGold);
        }

        public void TakeDamage(BigNumber dmg)
        {
            if (dmg == null) return;
            if (dmg.CompareTo(HitPoints) >= 0) HitPoints = new BigNumber(0);
            else HitPoints = HitPoints.Subtract(dmg);
        }

        public bool IsDead() => HitPoints.IsZero();
    }
}
