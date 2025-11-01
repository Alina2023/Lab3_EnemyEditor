using Lab2_EnemyEditor.Game;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Lab2_EnemyEditor
{
    public partial class ClickerWindow : Window
    {
        private EnemyManager manager = new EnemyManager();
        private CPlayer player = new CPlayer();
        private CEnemy currentEnemy;
        private CIconList iconList = new CIconList(); 

        public ClickerWindow()
        {
            InitializeComponent();
            // попытка загрузить иконки из папки images (как в ЛР2)
            iconList.Load("images", 64, 64);
            UpdatePlayerUI();
        }

        private void BtnLoadTemplates_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog { DefaultExt = ".json", Filter = "JSON files (*.json)|*.json" };
            if (dlg.ShowDialog() == true)
            {
                try
                {
                    manager.LoadFromJson(dlg.FileName);
                    MessageBox.Show("Шаблоны загружены: " + manager.Templates.Count);
                    SpawnNextEnemy();
                }
                catch (Exception ex) { MessageBox.Show("Ошибка: " + ex.Message); }
            }
        }

        private void SpawnNextEnemy()
        {
            var tpl = manager.PickRandomTemplate();
            if (tpl == null) { MessageBox.Show("Нет шаблонов"); return; }
            // модификация по уровню игрока: используем lifeModifier^lvl (пример)
            double lifeMod = tpl.GetLifeModifier(); // реализуй доступ
            double goldMod = tpl.GetGoldModifier();

            // вычислим HP и Gold как int (округляем)
            double hpDouble = tpl.BaseLife * Math.Pow(lifeMod, player.Lvl - 1);
            double goldDouble = tpl.BaseGold * Math.Pow(goldMod, player.Lvl - 1);

            int hp = Math.Max(1, (int)Math.Round(hpDouble));
            int gold = Math.Max(0, (int)Math.Round(goldDouble));

            // найдём иконку по имени
            var ic = iconList.FindByName(tpl.IconName);
            BitmapImage bmp = null;
            if (ic != null) bmp = ic.ImageControl.Source as BitmapImage;

            // создаём CEnemy
            var imgControl = new Image();
            if (bmp != null) imgControl.Source = bmp;
            var enemy = new CEnemy(tpl, imgControl, BigNumber.FromInt(hp), BigNumber.FromInt(gold));
            currentEnemy = enemy;
            RefreshEnemyUI();
        }

        private void RefreshEnemyUI()
        {
            if (currentEnemy == null) { imgEnemy.Source = null; txtEnemyHP.Text = "HP: -"; txtEnemyName.Text = ""; return; }
            // показать иконку
            if (currentEnemy.IconImage != null && currentEnemy.IconImage.Source != null) imgEnemy.Source = currentEnemy.IconImage.Source;
            txtEnemyHP.Text = "HP: " + currentEnemy.HitPoints.ToString();
            txtEnemyName.Text = currentEnemy.Name;
            UpdatePlayerUI();
        }

        private void ImgEnemy_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (currentEnemy == null) return;
            // атакуем
            currentEnemy.TakeDamage(player.Damage);
            if (currentEnemy.IsDead())
            {
                // начисляем золото
                player.AddGold(currentEnemy.GoldReward);
                MessageBox.Show($"Враг побеждён! Получено золота: {currentEnemy.GoldReward}");
                SpawnNextEnemy();
            }
            else
            {
                RefreshEnemyUI();
            }
        }

        private void BtnUpgrade_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (player.CanPay(player.UpgradeCost))
                {
                    player.UpgradeDamage();
                    UpdatePlayerUI();
                    MessageBox.Show("Апгрейд выполнен");
                }
                else MessageBox.Show("Недостаточно золота");
            }
            catch (Exception ex) { MessageBox.Show("Ошибка апгрейда: " + ex.Message); }
        }

        private void UpdatePlayerUI()
        {
            txtPlayerGold.Text = player.Gold.ToString();
            txtPlayerDamage.Text = player.Damage.ToString();
            txtPlayerLevel.Text = player.Lvl.ToString();
        }

        private void BtnNextEnemy_Click(object sender, RoutedEventArgs e) => SpawnNextEnemy();
    }
}

