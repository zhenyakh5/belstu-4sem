using GREPO;
using System.Text.Json;

namespace DALJSONXG
{
    public class JSONContext
    {
        private readonly string _fileName;
        public List<WSRef> WSRefs { get; private set; } = new();

        public List<Comment> Comments =>
            WSRefs.SelectMany(wsref => wsref.Comments ?? new List<Comment>()).ToList();

        private JSONContext(string fileName)
        {
            _fileName = fileName;
            Load();
        }

        public static JSONContext Create(string fileName) => new(fileName);

        private void Load()
        {
            if (File.Exists(_fileName))
            {
                string json = File.ReadAllText(_fileName);
                WSRefs = JsonSerializer.Deserialize<List<WSRef>>(json) ?? new List<WSRef>();
            }
            else
            {
                WSRefs = new List<WSRef>();
                SaveChanges();
            }
        }

        public int SaveChanges()
        {
            string json = JsonSerializer.Serialize(WSRefs, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_fileName, json);
            return 1;
        }
    }

}
