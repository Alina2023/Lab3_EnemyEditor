using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab2_EnemyEditor;


namespace Lab2_EnemyEditor.Game
{
    public class CPlayer
    {
        public int Lvl { get; private set; } = 1;
        public BigNumber Gold { get; private set; } = new BigNumber(0);
        public BigNumber Damage { get; private set; } = new BigNumber(1);

        private double damageModifier = 1.5; // как возрастает урон при апгрейде
        private double upgradeModifier = 1.7; // как увеличивается стоимость апгрейда
        public BigNumber UpgradeCost { get; private set; } = new BigNumber(10);

        public void AddGold(BigNumber amount)
        {
            Gold = Gold.Add(amount);
        }

        public bool CanPay(BigNumber cost)
        {
            return Gold.CompareTo(cost) >= 0;
        }

        public void Pay(BigNumber cost)
        {
            Gold = Gold.Subtract(cost);
        }

        public void UpgradeDamage()
        {
            // списываем деньги, увеличиваем уровень, урон и стоимость
            if (!CanPay(UpgradeCost)) throw new InvalidOperationException("Not enough gold");
            Pay(UpgradeCost);
            Lvl++;
            // Damage = Damage * damageModifier (approximate by multiply by int factor)
            int factor = (int)Math.Ceiling(damageModifier);
            Damage = Damage.MultiplyByInt(factor);
            int costFactor = (int)Math.Ceiling(upgradeModifier);
            UpgradeCost = UpgradeCost.MultiplyByInt(costFactor);
        }
    }
}
