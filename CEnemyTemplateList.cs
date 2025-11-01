using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;

namespace Lab2_EnemyEditor
{
    public class CEnemyTemplateList
    {
        public List<CEnemyTemplate> Enemies { get; set; } = new List<CEnemyTemplate>();

        public void Add(CEnemyTemplate e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));
            Enemies.Add(e);
        }

        public bool Remove(CEnemyTemplate e) => Enemies.Remove(e);

        public IReadOnlyList<CEnemyTemplate> GetAll() => Enemies.AsReadOnly();

        public void SaveToFile(string path)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(Enemies, options);
            File.WriteAllText(path, json);
        }

        public void LoadFromFile(string path)
        {
            if (!File.Exists(path)) throw new FileNotFoundException(path);
            string jsonFromFile = File.ReadAllText(path);
            var list = JsonSerializer.Deserialize<List<CEnemyTemplate>>(jsonFromFile);
            Enemies = list ?? new List<CEnemyTemplate>();
        }
    }
}
