using System.IO;

public class FileManager
{
    public void SaveToFile(string path, string data)
    {
        File.WriteAllText(path, data);
    }

    public string ReadFromFile(string path)
    {
        return File.ReadAllText(path);
    }
}