// Класс Student
using System.Runtime.Serialization.Formatters.Binary;

[Serializable]
public class Student
{
    public string Name { get; set; }
    public string Group { get; set; }
    public DateTime DateOfBirth { get; set; }
    public decimal AverageScore { get; set; }
}

class Program
{
    static void Main(string[] args)
    {
        // Путь к бинарному файлу
        string binaryFilePath = "students.dat";

        // Путь к папке для сохранения текстовых файлов
        string textFilesDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Students");

        // Считываем данные из бинарного файла
        List<Student> students = ReadStudentsFromBinaryFile(binaryFilePath);

        // Выводим информацию о папке до очистки
        long folderSizeBefore = GetFolderSize(textFilesDirectory);
        Console.WriteLine($"Размер папки до очистки: {folderSizeBefore} байт");

        // Создаем папку и сохраняем студентов в текстовые файлы
        CreateTextFiles(students, textFilesDirectory);

        // Выводим информацию об очистке
        int deletedFiles = Directory.GetFiles(textFilesDirectory).Length;
        long folderSizeAfter = GetFolderSize(textFilesDirectory);
        long freedSpace = folderSizeBefore - folderSizeAfter;
        Console.WriteLine($"Удалено {deletedFiles} файлов, освобождено {freedSpace} байт");

        // Выводим информацию о папке после очистки
        Console.WriteLine($"Размер папки после очистки: {folderSizeAfter} байт");
    }

    static List<Student> ReadStudentsFromBinaryFile(string filePath)
    {
        List<Student> students = new List<Student>();

        using (FileStream fs = new FileStream(filePath, FileMode.Open))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            students = (List<Student>)formatter.Deserialize(fs);
        }

        return students;
    }

    static void CreateTextFiles(List<Student> students, string directoryPath)
    {
        Directory.CreateDirectory(directoryPath);

        foreach (var student in students)
        {
            string fileName = $"{student.Group}.txt";
            string filePath = Path.Combine(directoryPath, fileName);

            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine($"{student.Name}, {student.DateOfBirth.ToString("dd.MM.yyyy")}, {student.AverageScore}");
            }
        }
    }

    static long GetFolderSize(string folderPath)
    {
        long size = 0;

        foreach (string file in Directory.GetFiles(folderPath))
        {
            size += new FileInfo(file).Length;
        }

        foreach (string dir in Directory.GetDirectories(folderPath))
        {
            size += GetFolderSize(dir);
        }

        return size;
    }
}
