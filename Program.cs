using System;
using System.Linq.Expressions;
using System.Numerics;
using System.Runtime.CompilerServices;

// Базовый класс для всех документов
public class Document {
    public string Title { get; set; }
    public string Content { get; set; }
    public string Author { get; set; }
    public List<string> Keywords { get; set; }
    public string Theme { get; set; }
    public string FilePath { get; set; }

    public string GetKeyWords() {
        string ReturnValue = "[";
        if (Keywords.Count>0) {
            var LatElement = Keywords.Count - 1;
            int CurElement = 0;
            while(CurElement != LatElement) {
                ReturnValue += Keywords.ElementAt(CurElement) + ", ";
                ++CurElement;
            }
            ReturnValue += Keywords.ElementAt(LatElement);
        }
        ReturnValue += "]";
        return ReturnValue;
    }
    public Document(string NewTitle, string NewContent) {
        Title = NewTitle;
        Content = NewContent;
        Author = "Станислав Хозяенок";
        Keywords = new List<string>() { "Учёба",  "Файл" };
        Theme = "Старание и труд все перебьют";
        FilePath = "C:/КиберКотлетыЛучшеВсех/";
    }
    public virtual string GetTitle() {
        return Title;
    }
    public virtual void GetDocumentInfo() {}
}

// Класс для документов MS Word
public class WordDocument : Document {
    public uint CountPages{ get; set; }

    public WordDocument(string Title, string Content) : base(Title, Content) {
        CountPages = 3;
    }

    public override void GetDocumentInfo() {
        Console.WriteLine("Word документ: " + Title);
        Console.WriteLine($"Автор: {Author}");
        Console.WriteLine($"Ключевые слова {GetKeyWords()}");
        Console.WriteLine($"Тема: {Theme}");
        Console.WriteLine($"Путь файла: {FilePath}");
        Console.WriteLine($"Количество страниц: {CountPages}");
        Console.WriteLine("Содержимое ");
        Console.WriteLine(Content);
    }
}

// Класс для документов PDF
public class PdfDocument : Document {
    public string FormFactor { get; set; }
    public PdfDocument(string Title, string Content) : base(Title, Content) {
        FormFactor = "A4";
    }
    public override void GetDocumentInfo() {
        Console.WriteLine("PDF документ: " + Title);
        Console.WriteLine($"Автор: {Author}");
        Console.WriteLine($"Ключевые слова {GetKeyWords()}");
        Console.WriteLine($"Тема: {Theme}");
        Console.WriteLine($"Путь файла: {FilePath}");
        Console.WriteLine($"Форм фактор: {FormFactor}");
        Console.WriteLine("Содержимое: ");
        Console.WriteLine(Content);
    }
}

// Класс для документов MS Excel
public class ExcelDocument : Document {
    public uint CountSheets { get; set; }

    public ExcelDocument(string Title, string Content) : base(Title, Content) {
        CountSheets = 5;
    }

    public override void GetDocumentInfo() {
        Console.WriteLine("Excel документ: " + Title);
        Console.WriteLine($"Автор: {Author}");
        Console.WriteLine($"Ключевые слова {GetKeyWords()}");
        Console.WriteLine($"Тема: {Theme}");
        Console.WriteLine($"Путь файла: {FilePath}");
        Console.WriteLine($"Количетсво листов: {CountSheets}");
        Console.WriteLine("Содержимое: ");
        Console.WriteLine(Content);
    }
}

// Класс для текстовых документов TXT
public class TxtDocument : Document {
    public uint CountChars { get; set; }

    public TxtDocument(string Title, string Content) : base(Title, Content) {
        CountChars = 123;
    }

    public override void GetDocumentInfo() {
        Console.WriteLine("TXT документ: " + Title);
        Console.WriteLine($"Автор: {Author}");
        Console.WriteLine($"Ключевые слова {GetKeyWords()}");
        Console.WriteLine($"Тема: {Theme}");
        Console.WriteLine($"Путь файла: {FilePath}");
        Console.WriteLine($"Количество символов: {CountChars}");
        Console.WriteLine("Содержимое: ");
        Console.WriteLine(Content);
    }
}

// Класс для документов HTML
public class HtmlDocument : Document {
    public string PublishDateString { get; set; }

    public HtmlDocument(string Title, string Content) : base(Title, Content) {
        PublishDateString = "2024.02.12 05:23";
    }

    public override void GetDocumentInfo() {
        Console.WriteLine("HTML документ: " + Title);
        Console.WriteLine($"Автор: {Author}");
        Console.WriteLine($"Ключевые слова {GetKeyWords()}");
        Console.WriteLine($"Тема: {Theme}");
        Console.WriteLine($"Путь файла: {FilePath}");
        Console.WriteLine($"Дата публикации: {PublishDateString}");
        Console.WriteLine("Содержимое: ");
        Console.WriteLine(Content);
    }
}

public class DocumentManager {
    private int CurrElementIndex, ElementMaxIndex, CountElements;
    private List<Document> ListDocuments;
    private static DocumentManager Instance;

    private DocumentManager() { }

    public static DocumentManager GetInstance() {
        if (Instance == null) {
            Instance = new DocumentManager();
        }
        return Instance;
    }

    void DrawDirectory() {
        Console.Clear();
        Console.WriteLine("Текущая дирректория:");
        int CurrArrayIndex = 0;
        while(CurrArrayIndex != CurrElementIndex) {
            Console.WriteLine(ListDocuments.ElementAt(CurrArrayIndex++).GetTitle());
        }
        Console.WriteLine($"{ListDocuments.ElementAt(CurrArrayIndex++).GetTitle()}<----");
        while (CurrArrayIndex != CountElements) {
            Console.WriteLine(ListDocuments.ElementAt(CurrArrayIndex++).GetTitle());
        }
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
    public void ChooseDocument(List<Document> InputListDocuments){
        ListDocuments = InputListDocuments;
        CountElements = ListDocuments.Count;
        CurrElementIndex = 0;
        ElementMaxIndex = CountElements-1;
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
                case ConsoleKey.Enter:
                    ShowDocumentInfo(ListDocuments.ElementAt(CurrElementIndex));
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
    public void ShowDocumentInfo(Document InputDocument) {
        Console.Clear();
        Console.WriteLine("Document Information:");
        InputDocument.GetDocumentInfo();
        Console.ReadKey();
    }
}

class Program {
    static void Main(string[] Args) {
        List<Document> ListDocuments = new List<Document>();
        ListDocuments.Add(new WordDocument("Полезные команды.docx", "Это Word документ."));
        ListDocuments.Add(new PdfDocument("Дипломная работа.pdf", "Это PDF документ."));
        ListDocuments.Add(new ExcelDocument("Моя таблица.xlsx", "Это Excel документ."));
        ListDocuments.Add(new TxtDocument("Git комманды.txt", "Это TXT документ."));
        ListDocuments.Add(new HtmlDocument("Моя первая страница.html", "<html><body><h1>This is an HTML документ.</h1></body></html>"));
        DocumentManager DocManager = DocumentManager.GetInstance();
        DocManager.ChooseDocument(ListDocuments);
    }
}
