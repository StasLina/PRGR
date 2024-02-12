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
        //foreach(string Keyword in Keywords) {
        //    ReturnValue += Keyword + " ";
        //}
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
    public Document(string title, string content) {
        Title = title;
        Content = content;
        Author = "Станислав хозяенок";
        Keywords = new List<string>() { "Работа",  "файл" };
        Theme = "Старание и труд все перебьют";
        FilePath = "C:/КиберКотлетыЛучшеВсех/";
    }

    public virtual void Open() {
        Console.WriteLine("Opening документ: " + Title);
    }

    public virtual string GetTitle() {
        return Title;
    }
    public virtual void GetDocumentInfo() {}
}

// Класс для документов MS Word
public class WordDocument : Document {
    public WordDocument(string title, string content) : base(title, content) {
    }

    public override void GetDocumentInfo() {
        Console.WriteLine("Word документ: " + Title);
        Console.WriteLine($"Автор: {Author}");
        Console.WriteLine($"Ключевые слова {GetKeyWords()}");
        Console.WriteLine($"Тема: {Theme}");
        Console.WriteLine($"Путь файла: {FilePath}");
        Console.WriteLine("Содержимое ");
        Console.WriteLine(Content);
    }
}

// Класс для документов PDF
public class PdfDocument : Document {
    public PdfDocument(string title, string content) : base(title, content) {
    }
    public override void GetDocumentInfo() {
        Console.WriteLine("PDF документ: " + Title);
        Console.WriteLine($"Автор: {Author}");
        Console.WriteLine($"Ключевые слова {GetKeyWords()}");
        Console.WriteLine($"Тема: {Theme}");
        Console.WriteLine($"Путь файла: {FilePath}");
        Console.WriteLine("Содержимое ");
        Console.WriteLine(Content);
    }
}

// Класс для документов MS Excel
public class ExcelDocument : Document {
    public ExcelDocument(string title, string content) : base(title, content) {
    }

    public override void GetDocumentInfo() {
        Console.WriteLine("Excel документ: " + Title);
        Console.WriteLine($"Автор: {Author}");
        Console.WriteLine($"Ключевые слова {GetKeyWords()}");
        Console.WriteLine($"Тема: {Theme}");
        Console.WriteLine($"Путь файла: {FilePath}");
        Console.WriteLine("Содержимое ");
        Console.WriteLine(Content);
    }
}

// Класс для текстовых документов TXT
public class TxtDocument : Document {
    public TxtDocument(string title, string content) : base(title, content) {
    }

    public override void GetDocumentInfo() {
        Console.WriteLine("TXT документ: " + Title);
        Console.WriteLine($"Автор: {Author}");
        Console.WriteLine($"Ключевые слова {GetKeyWords()}");
        Console.WriteLine($"Тема: {Theme}");
        Console.WriteLine($"Путь файла: {FilePath}");
        Console.WriteLine("Содержимое ");
        Console.WriteLine(Content);
    }
}

// Класс для документов HTML
public class HtmlDocument : Document {
    public HtmlDocument(string title, string content) : base(title, content) {
    }

    public override void GetDocumentInfo() {
        Console.WriteLine("HTML документ: " + Title);
        Console.WriteLine($"Автор: {Author}");
        Console.WriteLine($"Ключевые слова {GetKeyWords()}");
        Console.WriteLine($"Тема: {Theme}");
        Console.WriteLine($"Путь файла: {FilePath}");
        Console.WriteLine("Содержимое ");
        Console.WriteLine(Content);
    }
}

public class DocumentManager {
    private int CurrElementIndex, ElementMaxIndex, CountElements;
    private List<Document> ListDocuments;
    private static DocumentManager instance;

    private DocumentManager() { }

    public static DocumentManager GetInstance() {
        if (instance == null) {
            instance = new DocumentManager();
        }
        return instance;
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
                default:
                    Console.WriteLine("Нажмите один или два");
                    break;
            };
        }
    }
    public void ShowDocumentInfo(Document document) {
        Console.Clear();
        Console.WriteLine("Document Information:");
        document.GetDocumentInfo();
        Console.ReadKey();
    }
}

class Program {
    static void Main(string[] args) {
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
