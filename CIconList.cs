using System.Collections.Generic;
using System.IO;
using System.Windows.Controls;

namespace Lab2_EnemyEditor
{
    public class CIconList
    {
        public List<CIcon> Icons { get; private set; } = new List<CIcon>();

        public void Load(string pathOrFolder, int iconWidth = 64, int iconHeight = 64)
        {
            string folder = pathOrFolder;
            if (!Directory.Exists(folder))
            {
                string tryFolder = Path.Combine(Directory.GetCurrentDirectory(), pathOrFolder);
                if (Directory.Exists(tryFolder)) folder = tryFolder;
            }

            if (!Directory.Exists(folder)) return;

            string[] files = Directory.GetFiles(folder, "*.png");
            foreach (string file in files)
            {
                CIcon icon = new CIcon(file, iconWidth, iconHeight);
                Icons.Add(icon);
            }
        }

        public CIcon FindByName(string name) => Icons.Find(ic => ic.Name.Equals(name, System.StringComparison.OrdinalIgnoreCase));
    }
}
