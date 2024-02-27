using System;
using System.IO;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Reflection.Metadata;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Diagnostics;
#pragma warning disable SYSLIB0011

[Serializable]
public class TextFileContent {
    public string fileName { get; set; }
    public string fileContent { get; set; }

    // Бинарная сериализация
    public void SerializeBinary(string filePath) {
        using (FileStream CurrentFileStream = new FileStream(filePath, FileMode.Create)) {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            binaryFormatter.Serialize(CurrentFileStream, this);
        }
    }

    // Бинарная десериализация
    public void DeserializeBinary(string filePath) {
        using (FileStream fs = new FileStream(filePath, FileMode.Open)) {
            var binaryFormatter = new BinaryFormatter();
            TextFileContent newContent = (TextFileContent)binaryFormatter.Deserialize(fs);
            this.fileContent = newContent.fileContent;
        }
    }

    // XML сериализация
    public void SerializeXml(string filePath) {
        XmlSerializer serializer = new XmlSerializer(typeof(TextFileContent));
        using (StreamWriter writer = new StreamWriter(filePath)) {
            serializer.Serialize(writer, this);
        }
    }
    // XML десериализация
    public void  DeserializeXml(string filePath) {
        XmlSerializer serializer = new XmlSerializer(typeof(TextFileContent));
        using (StreamReader reader = new StreamReader(filePath)) {
            TextFileContent newContent = (TextFileContent)serializer.Deserialize(reader);
            this.fileContent = newContent.fileContent;
        }
    }

    public class Application {
        public static void Main(string[] args) {
            TextFileContent fileContent= new TextFileContent();
            fileContent.fileName = "1234.txt";
            fileContent.fileContent = "Жук банак кртофель";
            fileContent.SerializeXml("1234.xml");
            fileContent.DeserializeXml("1234.xml");
            //Получаем директорую
            string[] keyWords = new string[] {
                "Git",
                "sql",
                "обучен"
            };
            DocumentManager docManager = new DocumentManager("C:\\Users\\sthoz\\Downloads\\test_folder", keyWords);
            docManager.ChooseDocument();
            return;
        }
    }
    public class DocumentManager {
        private int ElementMaxIndex {
            get {
                if (CountElements > 0) {
                    return CountElements - 1;
                }
                return 0;
            }
        }
        private int CurrElementIndex = 0, CountElements, ElementIndexCategory = 0;
        //private List<Document> ListDocuments;

        private static DocumentManager Instance;
        Dictionary<int, string> curKeywords = new Dictionary<int, string>();
        Dictionary<int, TextFile> listFiles = new Dictionary<int, TextFile>();
        private string fileDirectory;
        TextFileSearcher.SmartDictinary curDictinary;

        void SetUpFileDirectory() {
            Console.WriteLine("Введите файловую дирректорию для индексации");
            while (true) {
                fileDirectory = System.Console.ReadLine();
                if (fileDirectory != null && fileDirectory != "\r\n") {
                    if (Directory.Exists(fileDirectory)) {
                        Console.WriteLine("Директория не найдена. ");
                    }
                    else {
                        Console.WriteLine("Директория установлена. ");
                        break;
                    }
                }
                else {
                    Console.WriteLine("Повторите ввод");
                }
            }
        }

        void SetUpKewWords(string[] listKeyWords) {
            if (listKeyWords is null) {
                Console.WriteLine("Введите ключевое слово");
                string inputKeyWord;
                int inputHashCode;
                while (true) {
                    ConsoleKeyInfo key = Console.ReadKey();

                    if (key.Key == ConsoleKey.Escape) {
                        break;
                    }
                    while (true) {
                        inputKeyWord = System.Console.ReadLine();
                        if (inputKeyWord != null && inputKeyWord != "\r\n") {
                            break;
                        }
                        else {
                            Console.WriteLine("Повторите ввод");
                        }
                    }
                    inputHashCode = inputKeyWord.GetHashCode();
                    if (curKeywords.ContainsKey(inputHashCode) == false) {
                        curKeywords.Add(inputHashCode, inputKeyWord);
                    }
                }
            }
            else {
                int inputHashCode;
                for (int indexKeyWord = 0; indexKeyWord < listKeyWords.Length; indexKeyWord++) {
                    inputHashCode = listKeyWords[indexKeyWord].GetHashCode();
                    if (curKeywords.ContainsKey(inputHashCode) == false) {
                        curKeywords.Add(inputHashCode, listKeyWords[indexKeyWord]);
                    }
                    ++indexKeyWord;
                }
            }
        }

        void InitDictinary() {
            TextFileSearcher newSearcher = new TextFileSearcher(fileDirectory);
            curDictinary = newSearcher.SearchFiles(curKeywords.Values.ToArray());
            KeyValuePair<int, List<string>> AtElment = curDictinary.ElementAt(this.ElementIndexCategory);
            this.CountElements = AtElment.Value.Count;
        }
        public DocumentManager() {
            SetUpFileDirectory();
            SetUpKewWords(null);
            InitDictinary();
            //DrawDirectory();
        }

        public DocumentManager(string fileDirectory, string[] keyWords) {
            this.fileDirectory = fileDirectory;
            SetUpKewWords(keyWords);
            InitDictinary();
            //DrawDirectory();
        }
        public static DocumentManager GetInstance() {
            if (Instance == null) {
                Instance = new DocumentManager();
            }
            return Instance;
        }

        void DrawDirectory() {
            Console.Clear();
            Console.WriteLine("Текущая дирректория: {0}", fileDirectory);
            int keyCategoryHashCode = curDictinary.ElementAt(ElementIndexCategory).Key;
            if (keyCategoryHashCode != 0) {
                Console.WriteLine("Текущая категория: {0}", curKeywords[keyCategoryHashCode]);
            }
            else {
                //Нерассортированные элементы
                Console.WriteLine("Текущая категория: безключевые");

            }
            int CurrArrayIndex = 0;
            while (CurrArrayIndex != CurrElementIndex) {
                Console.WriteLine(curDictinary.GetElementByKeyAndIndex(curDictinary.ElementAt(ElementIndexCategory).Key, CurrArrayIndex++));
            }
            Console.WriteLine($"{curDictinary.GetElementByKeyAndIndex(curDictinary.ElementAt(ElementIndexCategory).Key, CurrArrayIndex++)}<----");
            while (CurrArrayIndex != CountElements) {
                Console.WriteLine(curDictinary.GetElementByKeyAndIndex(curDictinary.ElementAt(ElementIndexCategory).Key, CurrArrayIndex++));
            }
        }
        void DrawEditor() {
            System.Console.Clear();
            //Передаём управление в текстовый редактор
            TextEditor newEditor = new TextEditor(curDictinary.ElementAt(ElementIndexCategory).Value[CurrElementIndex]);
            newEditor.Edit();
        }
        void MoveUp() {
            if (CurrElementIndex != 0) {
                --CurrElementIndex;
            }
        }
        void MoveDouwn() {
            if (CurrElementIndex != ElementMaxIndex) {
                ++CurrElementIndex;
            }
        }
        void MoveNextCategory() {
            CurrElementIndex = 0;
            if (this.ElementIndexCategory + 1 < curDictinary.GetCount()) {
                this.ElementIndexCategory += 1;
            }
            KeyValuePair<int, List<string>> AtElment = curDictinary.ElementAt(this.ElementIndexCategory);
            this.CountElements = AtElment.Value.Count;
        }
        void MovePreviousCategory() {
            CurrElementIndex = 0;
            if (this.ElementIndexCategory - 1 >= 0) {
                this.ElementIndexCategory -= 1;
            }
            KeyValuePair<int, List<string>> AtElment = curDictinary.ElementAt(this.ElementIndexCategory);
            this.CountElements = AtElment.Value.Count;
        }
        public void ChooseDocument() {
            while (true) {
                DrawDirectory();
                switch (Console.ReadKey().Key) {
                    case ConsoleKey.W:
                    case ConsoleKey.UpArrow:
                        MoveUp();
                        break;
                    case ConsoleKey.S:
                    case ConsoleKey.DownArrow:
                        MoveDouwn();
                        break;
                    case ConsoleKey.A:
                    case ConsoleKey.LeftArrow:
                        MovePreviousCategory();
                        break;
                    case ConsoleKey.D:
                    case ConsoleKey.RightArrow:
                        MoveNextCategory();
                        break;
                    case ConsoleKey.Enter:
                        //Окно редактора
                        DrawEditor();
                        break;
                    case ConsoleKey.Q:
                    case ConsoleKey.Escape:
                    case ConsoleKey.Spacebar:
                        return;
                    default:
                        break;
                };
            }
        }

    }


    public class TextFileSearcher {
        public string DirectoryPath { get; }

        public TextFileSearcher(string directoryPath) {
            DirectoryPath = directoryPath;
        }
        public SmartDictinary SearchFiles(string[] keywords) {
            SmartDictinary CurrSmartDictinary = new SmartDictinary();
            List<string> listKeyWord;

            if (!Directory.Exists(DirectoryPath)) {
                Console.WriteLine("Директория не найдена. ");
                return CurrSmartDictinary;
            }

            foreach (string filePath in Directory.GetFiles(DirectoryPath, "*.txt", SearchOption.AllDirectories)) {
                Console.WriteLine(filePath);
                listKeyWord = ContainsKeywords(filePath, keywords);
                if (listKeyWord.Count > 0) {
                    foreach (string keyword in listKeyWord) {
                        CurrSmartDictinary.Add(keyword.GetHashCode(), filePath);
                        
                    }
                }
                else {
                    CurrSmartDictinary.Add(0, filePath);
                }
            }
            return CurrSmartDictinary;
        }
        public class SmartDictinary {
            private Dictionary<int, List<string>> keyValuePairs = new Dictionary<int, List<string>>();
            public void Add(int key, string value) {
                if (keyValuePairs.ContainsKey(key)) {
                    keyValuePairs[key].Add(value);
                }
                else {
                    keyValuePairs.Add(key, new List<string>() { value });
                }
            }
            public void Remove(int key, string value) {
                if (keyValuePairs.ContainsKey(key)) {
                    if (keyValuePairs[key].Contains(value)) {
                        keyValuePairs[key].Remove(value);
                    }
                }
            }
            public List<string> GetListByKey(int key) {
                if (keyValuePairs.ContainsKey(key)) {
                    return keyValuePairs[key];
                }
                else {
                    return new List<string>();
                }
            }

            public string GetElementByKeyAndIndex(int key, int index) {
                if (keyValuePairs.ContainsKey(key)) {
                    return keyValuePairs[key][index];
                }
                else {
                    return "";
                }
            }

            public int GetCountByKey(int key) {
                if (keyValuePairs.ContainsKey(key)) {
                    return keyValuePairs[key].Count;
                }
                return 0;
            }
            public int GetCount() {
                return keyValuePairs.Count;
            }
            public KeyValuePair<int, List<string>> ElementAt(int element) {
                return keyValuePairs.ElementAt(element);
            }
        }


        private List<string> ContainsKeywords(string filePath, string[] keywords) {
            List<string> fileKeyWords = new List<string>();
            string fileContent = File.ReadAllText(filePath);
            foreach (string keyword in keywords) {
                //Сравниваем по бинарному поиску без учёта регистра.
                if (fileContent.Contains(keyword, StringComparison.OrdinalIgnoreCase)) {
                    fileKeyWords.Add(keyword);
                }
            }
            return fileKeyWords;
        }

    }

    public class TextFile  {
        TextFileContent fileContentInstance = new TextFileContent();
        TextFileMemento backupContent;
        string fileName {
            get {
                return fileContentInstance.fileName;
            }
            set {
                fileContentInstance.fileName = value;
            }
        }
        string fileContent {
            get {
                return fileContentInstance.fileContent;
            }
            set {
                fileContentInstance.fileContent = value;
            }
        }
        
        public TextFile(string fileName) {
            this.fileName = fileName;
            Load();
            backupContent = new TextFileMemento(this.fileContent);
        }

        public void SerializeBinary(string fileName) {
            this.fileContentInstance.SerializeBinary(fileName);
        }
        public void DeserializeBinary(string fileName) {
            this.fileContentInstance.DeserializeBinary(fileName);
        }
        public void SerializeXml(string fileName) {
            this.fileContentInstance.SerializeXml(fileName);
        }
        public void DeserializeXml(string fileName) {
            this.fileContentInstance.DeserializeXml(fileName);
        }

        public void Load() {
            this.fileContent = File.ReadAllText(this.fileName);
        }

        public void Save() {
            File.WriteAllText(fileName, fileContent);
        }

        public void Undo() {
            fileContent = backupContent.GetState();
        }

        public void SetContent(string newContent) {
            fileContent = newContent;
        }

        public string GetContent() {
            return fileContent;
        }

        public TextFileMemento CreateMemento() {
            return new TextFileMemento(fileContent);
        }

        public void RestoreMemento(TextFileMemento memento) {
            fileContent = memento.GetState();
        }
        public string GetFileName() {
            return this.fileName;
        }
        public string GetTitle() {
            string fileTitle = Path.GetFileName(this.fileName);
            return fileTitle;
        }
    }
    
    
public class TextFileMemento {
    private string state;

    public TextFileMemento(string state) {
        this.state = state;
    }

    public string GetState() {
        return state;
    }
}

public class TextEditor {
        private TextFile textFile;

        public TextEditor(string filePath) {
            textFile = new TextFile(filePath);
        }
        void SerializeBinary() {
            Console.WriteLine("Введите имя для нового файа, который будет бинарно серелизован");
            string inputFile = Console.ReadLine();
            textFile.SerializeBinary(inputFile);
        }
        void DeserializeBinary() {
            Console.WriteLine("Введите имя файа, контент которого будет бинарно десерелизован");
            string inputFile = Console.ReadLine();
            textFile.DeserializeBinary(inputFile);
        }
        void SerializeXML() {
            Console.WriteLine("Введите имя для нового файа, который будет XML серелизован");
            string inputFile = Console.ReadLine();
            textFile.SerializeXml(inputFile);
        }
        void DeserializeXML() {
            Console.WriteLine("Введите имя файа, контент которого будет XML десерелизован");
            string inputFile = Console.ReadLine();
            textFile.DeserializeXml(inputFile);
        }
        public void Serilization() {
            string startMessage ="Команды серелизатора:\n" +
            "1. Бинарно серилизовать\n" +
            "2. Бинарно десерилизовать\n" +
            "3. XML серелизовать\n" +
            "4. XML десерелизовать\n" +
            "5. Закрыть серелизатор\n"+
            "Введите номер команды: ";
            while (true) {
                Console.Clear();
                Console.Write(startMessage);
                int choice = int.Parse(Console.ReadLine());
                switch (choice) {
                    case 1:
                        SerializeBinary();
                        break;
                    case 2:
                        DeserializeBinary();
                        break;
                    case 3:
                        SerializeXML();
                        break;
                    case 4:
                        DeserializeXML();
                        break;
                    case 5:
                        return;
                }
            }
        }
        public void Edit() {
            string startMessage =
            "Текстовый команды редактора:\n" +
            "1. Перезаписать контент\n" +
            "2. Сохранить\n" +
            "3. Отменить изменения\n" +
            "4. Завершить работу\n" +
            "5. Открыть в системном редакторе\n" +
            "6. Интерфэйс серилизации\\десерилизации\n";
            while (true) {
                Console.Clear();
                Console.Write(startMessage);
                Console.WriteLine("Файл: {0}", textFile.GetTitle());
                Console.WriteLine("Конетент: ");
                var fileContent = textFile.GetContent();
                Console.WriteLine(fileContent);
                Console.Write("Введите номер команды: ");

                int choice = int.Parse(Console.ReadLine());

                switch (choice) {
                    case 1:
                        Console.Write("Введите новый контент: ");
                        string newContent = Console.ReadLine();
                        textFile.SetContent(newContent);
                        break;
                    case 2:
                        textFile.Save();
                        Console.WriteLine("Сохранить новый контент.");
                        break;
                    case 3:
                        textFile.Undo();
                        Console.WriteLine("Изменения отменены.");
                        break;
                    case 4:
                        return;
                    case 5: {
                            Process process = new Process();
                            // Установка параметров запуска
                            process.StartInfo.FileName = "notepad.exe"; 
                            process.StartInfo.Arguments = textFile.GetFileName(); 
                            // Запуск процесса
                            process.Start();

                            // Ожидание завершения процесса (необязательно)
                            process.WaitForExit();
                        }
                        break;
                    case 6: {
                          this.Serilization();
                            break;
                    }
                    default:
                        Console.WriteLine("Неправильный выбор, попытайтесь снова");
                        break;
                }
            }
        }
    }
}