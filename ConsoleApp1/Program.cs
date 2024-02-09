using System;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Runtime;

static class GlobalVars{
public
    static String AppVersion = "v1.2023_02_07_1";
public
    static String StartMessage = "Добро пожловать, Администратор.Введите следующее\n 1) Возведение числа в степень \n 2) Перенос второй цифпы в конец циферного ряда";
}

public class MathLibrary
{
    public static int CalculatePow(int sourse_number,uint number_degree){
        if (number_degree == 0) return 1;

        int result_number = sourse_number;
        
        for(uint current_iteration = 1; current_iteration!=number_degree; ++current_iteration){
            result_number *= sourse_number;
        }
        return result_number;
    }
    public static String SwapSecondaryValue(String input_string){
        String res_str = "";
        res_str += input_string[0];
        res_str += input_string.Substring(2);
        res_str += input_string[1];
        return res_str;
    }
}

class Application{
    static void HandleCalcPow(){
        System.Console.Write("\n");
        int source_number;
        uint degree;
        System.Console.WriteLine("Введите исходное число ");
        source_number = Int32.Parse(System.Console.ReadLine());
        System.Console.WriteLine("Введите степень ");
        degree = UInt32.Parse(System.Console.ReadLine());
        System.Console.WriteLine("Результат");
        System.Console.WriteLine(MathLibrary.CalculatePow(source_number, degree));
    }
    static void HandleSwapSecondaryValue(){
        System.Console.Write("\n");
        String source_number = "";
        System.Console.WriteLine("Введите исходное число ");
        source_number = System.Console.ReadLine();
        System.Console.WriteLine("Результат");
        System.Console.WriteLine(MathLibrary.SwapSecondaryValue(source_number));
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