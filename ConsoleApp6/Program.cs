﻿using System.Text.RegularExpressions;

class ErroneousWordsDictionary : System.Collections.Generic.Dictionary<string, string>
{
    public ErroneousWordsDictionary() : base()
    {
    }
    public new void Add(string trueWord, string falseWord)
    {
        if(base.ContainsKey(falseWord)==false) 
            base.Add(falseWord, trueWord);
    }
}
class Apllication
{
    static ErroneousWordsDictionary ErroneousWordsDictionary = new ErroneousWordsDictionary();

    public static void Main(string[] args)
    {
        // Словарь ошибочных слов
        ErroneousWordsDictionary.Add("привет", "првиет");
        ErroneousWordsDictionary.Add("привет", "првиет");
        ErroneousWordsDictionary.Add("привет", "правиет");
        ErroneousWordsDictionary.Add("связаться", "сзвяться");

        ConsoleInterface.Get.Run();
        Console.WriteLine("Приятно помочь, досвидание.");
    }
    class ConsoleInterface
    {
        static ConsoleInterface instance;
        static public ConsoleInterface Get
        {
            get
            {
                if(instance == null)
                {
                    instance = new ConsoleInterface();
                }
                return instance;
            }
        }
        public void Run()
        {
            while (true)
            {
                try
                {
                    // Директория, содержащая текстовые файлы
                    //string directoryPath = "C:\\Users\\sthoz\\source\\repos\\C#\\ConsoleApp6\\ConsoleApp6";
                    Console.Write("Введите путь до дирректории: ");
                    string directoryPath = Console.ReadLine();
                    // Проходим по всем файлам в указанной директории
                    foreach (string filePath in Directory.GetFiles(directoryPath, "*.txt"))
                    {
                        FixErrorsInFile(filePath, ErroneousWordsDictionary);
                    }
                    Console.WriteLine("Нажмите enter для завершения.");
                    ConsoleKey pressedKey = Console.ReadKey(false).Key;
                    if (pressedKey == ConsoleKey.Enter)
                    {
                        break;
                    }
                    Console.Clear();
                }
                catch {
                    Console.WriteLine("Что-то страшное, попробуйте снова");
                }
            }

        }

    }

    static void FixErrorsInFile(string filePath, ErroneousWordsDictionary erroneousWords)
    {
        try
        {
            // Считываем текст из файла
            string text = File.ReadAllText(filePath), newText;
            bool isFileReplaces = false;
            // Исправляем ошибочные слова
            foreach (var pair in erroneousWords)
            {
                newText = text.Replace(pair.Key, pair.Value);
                if(isFileReplaces==false && newText != text)
                {
                    isFileReplaces = true;
                }
                text = newText;
            }

            // Заменяем номера телефонов
            newText = Regex.Replace(text, @"\((\d{3})\)\s*(\d{3})-(\d{2})-(\d{2})", "+380 $1 $2 $3 $4");
            if (isFileReplaces == false && newText != text)
            {
                isFileReplaces = true;
            }
            text = newText;

            // Записываем исправленный текст обратно в файл
            if (isFileReplaces)
            {
                string newFilePath = filePath.Remove(filePath.Length - 4);
                newFilePath += "_fixed.txt";
                File.WriteAllText(newFilePath, text);
                Console.WriteLine($"Файл {filePath} не верен, исправленный файл - {newFilePath}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при обработке файла {filePath}: {ex.Message}");
        }
    }
}