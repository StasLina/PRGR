//v1.2024_02_27_2
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Diagnostics;
#pragma warning disable SYSLIB0011

[Serializable]
public class TextFileContent
{
    public string FileName { get; set; }
    public string FileContent { get; set; }

    // Бинарная сериализация
    public void SerializeBinary(string filePath)
    {
        using FileStream currentFileStream = new FileStream(filePath, FileMode.Create);
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        binaryFormatter.Serialize(currentFileStream, this);
    }

    // Бинарная десериализация
    public void DeserializeBinary(string filePath)
    {
        using FileStream currentFileStream = new FileStream(filePath, FileMode.Open);
        var binaryFormatter = new BinaryFormatter();
        TextFileContent newContent = (TextFileContent)binaryFormatter.Deserialize(currentFileStream);
        this.FileContent = newContent.FileContent;
    }

    // XML сериализация
    public void SerializeXml(string filePath)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(TextFileContent));
        using StreamWriter writer = new StreamWriter(filePath);
        serializer.Serialize(writer, this);
    }
    // XML десериализация
    public void DeserializeXml(string filePath)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(TextFileContent));
        using StreamReader reader = new StreamReader(filePath);
        TextFileContent newContent = (TextFileContent)serializer.Deserialize(reader);
        this.FileContent = newContent.FileContent;
    }

    public class Application
    {
        public static void Main(string[] args)
        {
            //Тест_Кейс 1
            //TextFileContent FileContent = new TextFileContent();
            //FileContent.FileName = "1234.txt";
            //FileContent.FileContent = "";
            //FileContent.SerializeXml("1234.xml");
            //FileContent.DeserializeXml("1234.xml");
            ////Получаем директорую
            //string[] keyWords = new string[] {
            //    "Git",
            //    "sql"
            //};
            //DocumentManager docManager = new DocumentManager("C:\\Users\\sthoz\\Downloads\\test_folder", keyWords);

            DocumentManager docManager = new DocumentManager();
            docManager.ChooseDocument();
        }
    }
    public class DocumentManager
    {
        private int elementMaxIndex
        {
            get
            {
                if (countElements > 0)
                {
                    return countElements - 1;
                }
                return 0;
            }
        }
        private int currentElementIndex = 0, countElements, elementIndexCategory = 0;
        Dictionary<int, string> curKeywords = new Dictionary<int, string>();
        private string fileDirectory;
        TextFileSearcher.SmartDictionary curDictionary;

        void SetUpFileDirectory()
        {
            Console.WriteLine("Введите файловую дирректорию для индексации");
            while (true)
            {
                fileDirectory = System.Console.ReadLine();
                if (fileDirectory != null && fileDirectory != "\r\n")
                {
                    if (Directory.Exists(fileDirectory))
                    {
                        return;
                    }
                    else
                    {
                        Console.WriteLine("Директория установлена. ");
                        break;
                    }
                }
                else
                {
                    Console.WriteLine("Повторите ввод");
                }
            }
        }

        void SetUpKewWords(string[] listKeyWords)
        {
            if (listKeyWords is null)
            {
                string inputKeyWord;
                int inputHashCode;
                while (true)
                {
                    Console.WriteLine("Введите ключевое слово");
                    while (true)
                    {
                        inputKeyWord = System.Console.ReadLine();
                        if (inputKeyWord != null && inputKeyWord != "\r\n")
                        {
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Повторите ввод");
                        }
                    }
                    inputHashCode = inputKeyWord.GetHashCode();
                    if (curKeywords.ContainsKey(inputHashCode) == false)
                    {
                        curKeywords.Add(inputHashCode, inputKeyWord);
                    }
                    Console.WriteLine("Продолжить? Enter - продолжить");
                    ConsoleKeyInfo key = Console.ReadKey(true);
                    if (key.Key != ConsoleKey.Enter)
                    {
                        break;
                    }
                }
            }
            else
            {
                int inputHashCode;
                for (int indexKeyWord = 0; indexKeyWord < listKeyWords.Length; indexKeyWord++)
                {
                    inputHashCode = listKeyWords[indexKeyWord].GetHashCode();
                    if (curKeywords.ContainsKey(inputHashCode) == false)
                    {
                        curKeywords.Add(inputHashCode, listKeyWords[indexKeyWord]);
                    }
                    ++indexKeyWord;
                }
            }
        }

        void InitDictionary()
        {
            TextFileSearcher newSearcher = new TextFileSearcher(fileDirectory);
            curDictionary = newSearcher.SearchFiles(curKeywords.Values.ToArray());
            if (curDictionary.GetCount() > 0)
            {
                KeyValuePair<int, List<string>> AtElment = curDictionary.ElementAt(this.elementIndexCategory);
                this.countElements = AtElment.Value.Count;
            }
            else
            {
                this.countElements = 0;
            }
            
        }
        public DocumentManager()
        {
            SetUpFileDirectory();
            SetUpKewWords(null);
            InitDictionary();
        }

        public DocumentManager(string fileDirectory, string[] keyWords)
        {
            this.fileDirectory = fileDirectory;
            SetUpKewWords(keyWords);
            InitDictionary();
        }
        void DrawDirectory()
        {
            Console.Clear();
            Console.WriteLine("Текущая дирректория: {0}", fileDirectory);
            int keyCategoryHashCode = curDictionary.ElementAt(elementIndexCategory).Key;
            if (keyCategoryHashCode != 0) {
                Console.WriteLine("Текущая категория: {0}", curKeywords[keyCategoryHashCode]);
            }
            else
            {
                //Нерассортированные элементы
                Console.WriteLine("Текущая категория: безключевые");

            }
            int currentArrayIndex = 0;
            while (currentArrayIndex < currentElementIndex) {
                Console.WriteLine(curDictionary.GetElementByKeyAndIndex(curDictionary.ElementAt(elementIndexCategory).Key, currentArrayIndex++));
            }
            Console.WriteLine($"{curDictionary.GetElementByKeyAndIndex(curDictionary.ElementAt(elementIndexCategory).Key, currentArrayIndex++)}<----");
            while (currentArrayIndex < countElements) {
                Console.WriteLine(curDictionary.GetElementByKeyAndIndex(curDictionary.ElementAt(elementIndexCategory).Key, currentArrayIndex++));
            }
        }
        void DrawEditor()
        {
            System.Console.Clear();
            //Передаём управление в текстовый редактор
            TextEditor newEditor = new TextEditor(curDictionary.ElementAt(elementIndexCategory).Value[currentElementIndex]);
            newEditor.Edit();
        }
        void MoveUp()
        {
            if (currentElementIndex != 0)
            {
                --currentElementIndex;
            }
        }
        void MoveDouwn()
        {
            if (currentElementIndex != elementMaxIndex)
            {
                ++currentElementIndex;
            }
        }
        void MoveNextCategory() {
            currentElementIndex = 0;
            if (this.elementIndexCategory + 1 < curDictionary.GetCount()) {
                this.elementIndexCategory += 1;
                KeyValuePair<int, List<string>> AtElment = curDictionary.ElementAt(this.elementIndexCategory);
                this.countElements = AtElment.Value.Count;
            }
        }
        void MovePreviousCategory()
        {
            currentElementIndex = 0;
            if (this.elementIndexCategory - 1 >= 0)
            {
                this.elementIndexCategory -= 1;
                KeyValuePair<int, List<string>> AtElment = curDictionary.ElementAt(this.elementIndexCategory);
                this.countElements = AtElment.Value.Count;
            }
        }
        public void ChooseDocument()
        {
            while (true)
            {
                DrawDirectory();
                switch (Console.ReadKey().Key)
                {
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


    public class TextFileSearcher
    {
        public string DirectoryPath { get; }

        public TextFileSearcher(string DirectoryPath)
        {
            this.DirectoryPath = DirectoryPath;
        }
        public SmartDictionary SearchFiles(string[] keywords) {
            SmartDictionary currentSmartDictionary = new SmartDictionary();
            List<string> listKeyWord;

            if (!Directory.Exists(DirectoryPath))
            {
                Console.WriteLine("Директория не найдена. ");
                return currentSmartDictionary;
            }

            foreach (string filePath in Directory.GetFiles(DirectoryPath, "*.txt", SearchOption.AllDirectories))
            {
                Console.WriteLine(filePath);
                listKeyWord = ContainsKeywords(filePath, keywords);
                if (listKeyWord.Count > 0) {
                    foreach (string keyword in listKeyWord) {
                        currentSmartDictionary.Add(keyword.GetHashCode(), filePath);
                    }
                }
                else {
                    currentSmartDictionary.Add(0, filePath);
                }
            }
            return currentSmartDictionary;
        }


        public class SmartDictionary {
            private Dictionary<int, List<string>> keyValuePairs = new Dictionary<int, List<string>>();
            public void Add(int key, string value)
            {
                if (keyValuePairs.ContainsKey(key))
                {
                    keyValuePairs[key].Add(value);
                }
                else
                {
                    keyValuePairs.Add(key, new List<string>() { value });
                }
            }
            public void Remove(int key, string value)
            {
                if (keyValuePairs.ContainsKey(key))
                {
                    if (keyValuePairs[key].Contains(value))
                    {
                        keyValuePairs[key].Remove(value);
                    }
                }
            }
            public List<string> GetListByKey(int key)
            {
                if (keyValuePairs.ContainsKey(key))
                {
                    return keyValuePairs[key];
                }
                else
                {
                    return new List<string>();
                }
            }

            public string GetElementByKeyAndIndex(int key, int index)
            {
                if (keyValuePairs.ContainsKey(key))
                {
                    return keyValuePairs[key][index];
                }
                else
                {
                    return "";
                }
            }

            public int GetCountByKey(int key)
            {
                if (keyValuePairs.ContainsKey(key))
                {
                    return keyValuePairs[key].Count;
                }
                return 0;
            }
            public int GetCount()
            {
                return keyValuePairs.Count;
            }
            public KeyValuePair<int, List<string>> ElementAt(int element)
            {
                if (element < keyValuePairs.Count) {
                    return keyValuePairs.ElementAt(element);
                }
                return new KeyValuePair<int, List<string>>();
            }
        }

        private List<string> ContainsKeywords(string filePath, string[] keywords)
        {
            List<string> fileKeyWords = new List<string>();
            string FileContent = File.ReadAllText(filePath);
            foreach (string keyword in keywords)
            {
                //Сравниваем по бинарному поиску без учёта регистра.
                if (FileContent.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                {
                    fileKeyWords.Add(keyword);
                }
            }
            return fileKeyWords;
        }
    }

    public class CareTaker
    {
        private Dictionary<int, object> AssClassMemento = new Dictionary<int, object>();
        private static CareTaker instance;
        public static CareTaker Instance
        {
            get 
            {
                if (instance == null)
                {
                    instance = new CareTaker();
                }
                return instance; 
            }
        }

        public void SaveState(AbstractTextFile originator)
        {
            int hashCodeOriginator = originator.GetHashCode();
            if (AssClassMemento.ContainsKey(hashCodeOriginator))
            {
                AssClassMemento[hashCodeOriginator] = originator.GetMemento();
            }
            else
            {
                AssClassMemento.Add(hashCodeOriginator, originator.GetMemento());
            }
        }
        public object GetMemento(AbstractTextFile originator)
        {
            int hashCodeOriginator = originator.GetHashCode();
            if (AssClassMemento.ContainsKey(hashCodeOriginator))
            {
                return AssClassMemento[hashCodeOriginator];
            }
            return null;
        }
        public void RestoreState(AbstractTextFile originator)
        {
            int hashCodeOriginator = originator.GetHashCode();
            if (AssClassMemento.ContainsKey(hashCodeOriginator))
            {
                originator.SetMeneto(AssClassMemento[hashCodeOriginator]);
            }
        }
    }
    public abstract class AbstractTextFile
    {
        abstract public object  GetMemento();

        abstract public void SetMeneto(object textFileMemento);
    }
    public class TextFile : AbstractTextFile
    {
        TextFileContent FileContentInstance = new TextFileContent();
        //TextFileMemento backupContent;
        string FileName
        {
            get
            {
                return FileContentInstance.FileName;
            }
            set
            {
                FileContentInstance.FileName = value;
            }
        }
        string FileContent
        {
            get
            {
                return FileContentInstance.FileContent;
            }
            set
            {
                FileContentInstance.FileContent = value;
            }
        }


        public TextFile(string FileName)
        {
            this.FileName = FileName;
            Load();
            //Реализовать Instance
            var CareTakerInstance = CareTaker.Instance;
            CareTakerInstance.SaveState(this);
        }

        public void SerializeBinary(string FileName)
        {
            this.FileContentInstance.SerializeBinary(FileName);
        }
        public void DeserializeBinary(string FileName)
        {
            this.FileContentInstance.DeserializeBinary(FileName);
        }
        public void SerializeXml(string FileName)
        {
            this.FileContentInstance.SerializeXml(FileName);
        }
        public void DeserializeXml(string FileName)
        {
            this.FileContentInstance.DeserializeXml(FileName);
        }

        public void Load()
        {
            this.FileContent = File.ReadAllText(this.FileName);
        }

        public void Save()
        {
            File.WriteAllText(FileName, FileContent);
        }

        public void Undo()
        {
            CareTaker.Instance.RestoreState(this);
        }
        
        override public TextFileMemento GetMemento()
        {
            return new TextFileMemento(FileContentInstance.FileContent);
        }

        override public void SetMeneto(object textFileMemento)
        {
            FileContent = (textFileMemento as TextFileMemento).GetState();
        }

        public void SetContent(string newContent)
        {
            FileContent = newContent;
        }

        public string GetContent()
        {
            return FileContent;
        }


        public string GetFileName()
        {
            return this.FileName;
        }
        public string GetTitle()
        {
            string fileTitle = Path.GetFileName(this.FileName);
            return fileTitle;
        }
    }

    public class TextFileMemento
    {
        private string state;

        public TextFileMemento(string state)
        {
            this.state = state;
        }

        public string GetState()
        {
            return state;
        }
    }


    public class TextEditor
    {
        private TextFile textFile;

        public TextEditor(string filePath)
        {
            textFile = new TextFile(filePath);
        }
        void SerializeBinary()
        {
            Console.WriteLine("Введите имя для нового файла, который будет бинарно серелизован");
            string inputFile = Console.ReadLine();
            textFile.SerializeBinary(inputFile);
        }
        void DeserializeBinary()
        {
            Console.WriteLine("Введите имя файла, контент которого будет бинарно десерелизован");
            string inputFile = Console.ReadLine();
            textFile.DeserializeBinary(inputFile);
        }
        void SerializeXML()
        {
            Console.WriteLine("Введите имя для нового файла, который будет XML серелизован");
            string inputFile = Console.ReadLine();
            textFile.SerializeXml(inputFile);
        }
        void DeserializeXML()
        {
            Console.WriteLine("Введите имя файла, контент которого будет XML десерелизован");
            string inputFile = Console.ReadLine();
            textFile.DeserializeXml(inputFile);
        }
        public void Serilization()
        {
            string startMessage = "Команды серелизатора:\n" +
            "1. Бинарно серилизовать\n" +
            "2. Бинарно десерилизовать\n" +
            "3. XML серелизовать\n" +
            "4. XML десерелизовать\n" +
            "5. Закрыть серелизатор\n" +
            "Введите номер команды: ";
            while (true)
            {
                Console.Clear();
                Console.Write(startMessage);
                int choice = int.Parse(Console.ReadLine());
                switch (choice)
                {
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
        public void Edit()
        {
            string startMessage =
            "Текстовый команды редактора:\n" +
            "1. Перезаписать контент\n" +
            "2. Сохранить\n" +
            "3. Отменить изменения\n" +
            "4. Закрыть редактор\n" +
            "5. Открыть в системном редакторе\n" +
            "6. Интерфэйс серилизации\\десерилизации\n";
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Файл: {0}", textFile.GetTitle());
                Console.WriteLine("Конетент: ");
                var FileContent = textFile.GetContent();
                Console.WriteLine(FileContent);
                Console.Write(startMessage);
                Console.Write("Введите номер команды: ");

                int choice = int.Parse(Console.ReadLine());

                switch (choice)
                {
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
                    case 5:
                        {
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
                    case 6:
                        {
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