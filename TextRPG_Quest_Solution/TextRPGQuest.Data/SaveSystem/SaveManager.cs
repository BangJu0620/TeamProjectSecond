using System.IO;

namespace TextRPGQuest.SaveSystem
{
    public static class SaveManager
    {
        public static void Save(string path, string json)
        {
            File.WriteAllText(path, json);
        }

        public static string Load(string path)
        {
            return File.Exists(path) ? File.ReadAllText(path) : null;
        }
    }
}
