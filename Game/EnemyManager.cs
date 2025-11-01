using System;
using System.Collections.Generic;
using System.IO;
using Lab2_EnemyEditor;


namespace Lab2_EnemyEditor.Game
{
    public class EnemyManager
    {
        public List<CEnemyTemplate> Templates { get; private set; } = new List<CEnemyTemplate>();
        private Random rnd = new Random();

        public void LoadFromJson(string path)
        {
            var listHolder = new CEnemyTemplateList();
            listHolder.LoadFromFile(path); 
            Templates = listHolder.Enemies;
            NormalizeChances();
        }

        public void NormalizeChances()
        {
            double sum = 0;
            foreach (var t in Templates) sum += t.GetSpawnChanceAsDouble();
            if (sum == 0) return;
            for (int i = 0; i < Templates.Count; i++)
                Templates[i].SetNormalizedSpawnChance(Templates[i].GetSpawnChanceAsDouble() / sum);
        }

        public CEnemyTemplate PickRandomTemplate()
        {
            if (Templates.Count == 0) return null;
            double r = rnd.NextDouble();
            double sum = 0;
            foreach (var t in Templates)
            {
                sum += t.GetSpawnChanceAsDouble();
                if (r <= sum) return t;
            }
            return Templates[Templates.Count - 1];
        }
    }
}
