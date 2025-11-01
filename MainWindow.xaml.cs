using Microsoft.Win32;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Lab2_EnemyEditor
{
    public partial class MainWindow : Window
    {
        private CIconList iconList = new CIconList();
        private CEnemyTemplateList enemyList = new CEnemyTemplateList();

        public MainWindow()
        {
            InitializeComponent();
            LoadIconsToUI();
            RefreshEnemyListUI();
        }

        private void LoadIconsToUI()
        {
            try
            {
                string imagesFolder = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "images");
                iconList.Load(imagesFolder, 64, 64);
            }
            catch
            {
                iconList.Load("images", 64, 64);
            }

            iconsWrapPanel.Children.Clear();

            foreach (var ic in iconList.Icons)
            {
                var btn = new Button { Width = 100, Height = 100, Margin = new Thickness(6), Tag = ic.Name };
                var img = new Image { Source = ic.ImageControl.Source, Width = 80, Height = 80, Stretch = System.Windows.Media.Stretch.Uniform };
                btn.Content = img;
                btn.Click += IconButton_Click;
                iconsWrapPanel.Children.Add(btn);
            }
        }

        // Клик по иконке справа
        private void IconButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is string name)
            {
                txtIconName.Text = name;
                var ic = iconList.FindByName(name);
                if (ic != null) previewImage.Source = ic.ImageControl.Source;
            }
        }

        // Добавить врага
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            // Валидация полей
            string iconName = txtIconName.Text.Trim();
            string enemyName = txtEnemyName.Text.Trim();
            if (string.IsNullOrWhiteSpace(iconName))
            {
                MessageBox.Show("Выберите иконку справа или укажите Icon name.");
                return;
            }
            if (string.IsNullOrWhiteSpace(enemyName))
            {
                MessageBox.Show("Введите имя врага (Enemy Name).");
                return;
            }

            if (!int.TryParse(txtBaseLife.Text.Trim(), out int baseLife))
            {
                MessageBox.Show("Base life должен быть целым числом.");
                return;
            }
            if (!double.TryParse(txtLifeModifier.Text.Trim(), out double lifeModifier))
            {
                MessageBox.Show("Life modifier должен быть числом (например, 1.0).");
                return;
            }
            if (!int.TryParse(txtBaseGold.Text.Trim(), out int baseGold))
            {
                MessageBox.Show("Base gold должен быть целым числом.");
                return;
            }
            if (!double.TryParse(txtGoldModifier.Text.Trim(), out double goldModifier))
            {
                MessageBox.Show("Gold modifier должен быть числом.");
                return;
            }
            if (!double.TryParse(txtSpawnChance.Text.Trim(), out double spawnChance))
            {
                MessageBox.Show("Spawn chance должен быть числом (1-100).");
                return;
            }

            var enemy = new CEnemyTemplate(enemyName, iconName, baseLife, lifeModifier, baseGold, goldModifier, spawnChance);
            enemyList.Add(enemy);
            RefreshEnemyListUI();
            ClearFieldsAfterAdd();
        }

        private void ClearFieldsAfterAdd()
        {
            txtEnemyName.Text = "";
            txtBaseLife.Text = "";
            txtLifeModifier.Text = "";
            txtBaseGold.Text = "";
            txtGoldModifier.Text = "";
            txtSpawnChance.Text = "";
        }

        // Удалить выделенного врага
        private void BtnRemove_Click(object sender, RoutedEventArgs e)
        {
            int idx = enemyListBox.SelectedIndex;
            if (idx < 0) { MessageBox.Show("Выберите врага в списке слева."); return; }
            var toRemove = enemyList.GetAll()[idx];
            enemyList.Remove(toRemove);
            RefreshEnemyListUI();
        }

        // Сохранение в JSON
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new SaveFileDialog { DefaultExt = ".json", Filter = "JSON files (*.json)|*.json", FileName = "enemies" };
            if (dlg.ShowDialog() == true)
            {
                try
                {
                    enemyList.SaveToFile(dlg.FileName);
                    MessageBox.Show("Сохранено: " + dlg.FileName);
                }
                catch (Exception ex) { MessageBox.Show("Ошибка при сохранении: " + ex.Message); }
            }
        }

        // Загрузка из JSON
        private void BtnLoad_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog { DefaultExt = ".json", Filter = "JSON files (*.json)|*.json" };
            if (dlg.ShowDialog() == true)
            {
                try
                {
                    enemyList.LoadFromFile(dlg.FileName);
                    RefreshEnemyListUI();
                    MessageBox.Show("Загружено врагов: " + enemyList.GetAll().Count);
                }
                catch (Exception ex) { MessageBox.Show("Ошибка при загрузке: " + ex.Message); }
            }
        }

        // Обновляем ListBox слева
        private void RefreshEnemyListUI()
        {
            enemyListBox.Items.Clear();
            var all = enemyList.GetAll();
            for (int i = 0; i < all.Count; i++)
            {
                var et = all[i];
                var sp = new StackPanel { Orientation = Orientation.Horizontal };
                var idxBlock = new TextBlock { Text = (i + 1).ToString(), Width = 28, VerticalAlignment = VerticalAlignment.Center };
                var nameBlock = new TextBlock { Text = et.Name, VerticalAlignment = VerticalAlignment.Center };
                sp.Children.Add(idxBlock);
                sp.Children.Add(nameBlock);
                var lbi = new ListBoxItem { Content = sp };
                enemyListBox.Items.Add(lbi);
            }
        }

        // При выборе элемента в списке — заполняем поля
        private void EnemyListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int idx = enemyListBox.SelectedIndex;
            if (idx < 0) return;
            var et = enemyList.GetAll()[idx];

            txtIconName.Text = et.IconName;
            txtEnemyName.Text = et.Name;
            txtBaseLife.Text = et.BaseLife.ToString();
            txtLifeModifier.Text = et.LifeModifier.ToString();
            txtBaseGold.Text = et.BaseGold.ToString();
            txtGoldModifier.Text = et.GoldModifier.ToString();
            txtSpawnChance.Text = et.SpawnChance.ToString();

            var ic = iconList.FindByName(et.IconName);
            if (ic != null) previewImage.Source = ic.ImageControl.Source;
            else previewImage.Source = null;
        }

        private void BtnRunGame_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Откроет окно игры. Show() — не блокирует основное окно.
                var gw = new ClickerWindow();
                gw.Owner = this; // опционально: назначаем владельца
                gw.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось открыть окно игры: " + ex.Message);
            }
        }
    }
}
