using System;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Runtime;

static class GlobalVars{
public
    static String AppVersion = "v1.2023_02_12_1";
public
    static String StartMessage = "Добро пожловать, Администратор.Введите следующее\n 1) Возведение числа в степень \n 2) Перенос второй цифпы в конец циферного ряда";
}

public class MathLibrary{
    public static int CalculatePow(int SourseNumber,uint NumberCountDegree){
        if (NumberCountDegree == 0) return 1;

        int ResultNumber = SourseNumber;
        
        for(uint CurrentIteration = 1; CurrentIteration!=NumberCountDegree; ++CurrentIteration){
            ResultNumber *= SourseNumber;
        }
        return ResultNumber;
    }
    public static String SwapSecondaryValue(String InputString){
        String ResultStr = "";
        ResultStr += InputString[0];
        ResultStr += InputString.Substring(2);
        ResultStr += InputString[1];
        return ResultStr;
    }
}

class Application{
    static void HandleCalcPow(){
        System.Console.Write("\n");
        int SourceNumber;
        uint CountDegree;
        System.Console.WriteLine("Введите исходное число ");
        SourceNumber = Int32.Parse(System.Console.ReadLine());
        System.Console.WriteLine("Введите степень ");
        CountDegree = UInt32.Parse(System.Console.ReadLine());
        System.Console.WriteLine("Результат");
        System.Console.WriteLine(MathLibrary.CalculatePow(SourceNumber, CountDegree));
    }
    static void HandleSwapSecondaryValue(){
        System.Console.Write("\n");
        String SourceNumber = "";
        System.Console.WriteLine("Введите исходное число ");
        SourceNumber = System.Console.ReadLine();
        System.Console.WriteLine("Результат");
        System.Console.WriteLine(MathLibrary.SwapSecondaryValue(SourceNumber));
    }
    public static void Main(){
        Console.WriteLine(GlobalVars.AppVersion);
        Console.WriteLine(GlobalVars.StartMessage);

        switch (Console.ReadKey().Key) { 
            case ConsoleKey.D1:
                HandleCalcPow();
                break;
            case ConsoleKey.D2:
                HandleSwapSecondaryValue();
                break;
            default:
                Console.WriteLine("Нажмите один или два");
                break;
        };
        Console.Write("Выход");
    }
}