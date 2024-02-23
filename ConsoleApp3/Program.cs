
using System;
using System.Diagnostics;

using System.Linq.Expressions;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Collections;

using MatrixValue = double;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
class MatrixRow : List<MatrixValue> {
    public MatrixRow() : base() { }
    public MatrixRow(IEnumerable<MatrixValue> IList) : base(IList) { }
    public MatrixRow(Int32 ColumnCount) : base(ColumnCount) {
        for (Int32 CurColIndex = 0; CurColIndex < ColumnCount; ++CurColIndex) {
            this.Add(0);
        }
    }
}
class MatrixData : List<MatrixRow> {
    public MatrixData() : base() { }
    public MatrixData(Int32 RowCount) : base(RowCount) { }
    public MatrixData(Int32 RowCount, Int32 ColumnCount) : base() {
        for (Int32 CurRowIndex = 0; CurRowIndex < RowCount; ++CurRowIndex) {
            this.Add(new MatrixRow(ColumnCount));
        }
    }
    public MatrixData(IEnumerable<MatrixRow> IList) : base(IList) { }
    public MatrixData(ref IEnumerable<IEnumerable<MatrixValue>> IList) {
        int RowCount = IList.Count();
        for (int RowIndex = 0; RowIndex < RowCount; ++RowIndex) { this.Add(new MatrixRow(IList.ElementAt(RowIndex))); }
    }
}


class Application {
    public static SmartMatrix Matr1, Matr2;
    public static void CreateMatrix(out SmartMatrix Matr1) {
        Console.WriteLine("Создание матрицы");
        Console.WriteLine("Ведеите размер квадратной матрицы матрицы");

        int SizeMatrix;
        string SizeMatrixString = "";
        while (true) {
            try {
                SizeMatrixString = Console.ReadLine();
                SizeMatrix = int.Parse(SizeMatrixString);
                break;
            }
            catch {
                Console.WriteLine("Попробуйте ввести снова");
            }
        }
        MatrixData MatrixData = new MatrixData(SizeMatrix, SizeMatrix);
        Console.WriteLine("1 - Заполнить рандомными значениями");
        Console.WriteLine("2 - Заполнить ручками");
        Console.WriteLine("3 - Заполнить 0");
        bool CycleHandilng = true;
        while (CycleHandilng) {
            ConsoleKey PressedKey = Console.ReadKey().Key;
            Console.WriteLine();
            switch (PressedKey) {
                case ConsoleKey.D1:
                    Matr1 = new SmartMatrix(ref MatrixData);
                    Matr1.FillRandomValues();
                    Console.WriteLine();
                    return;
                case ConsoleKey.D2:
                    MatrixData = FillMatrix(SizeMatrix);
                    Matr1 = new SmartMatrix(ref MatrixData);
                    Console.WriteLine();
                    return;
                case ConsoleKey.D3:
                    Matr1 = new SmartMatrix(ref MatrixData);
                    Console.WriteLine();
                    return;
            }
        }
        Matr1 = new SmartMatrix(ref MatrixData);
    }
    public static void FillRandomMatrix(int Choose = 0) {
        Console.WriteLine("Заполнение матрицы рандомными значениями");
        ConsoleKey PressedKey;;
        switch (Choose) {
            case 1:
                PressedKey = ConsoleKey.D1;
                break;
            case 2:
                PressedKey = ConsoleKey.D2;
                break;
            default:
                PressedKey = Console.ReadKey().Key;
                Console.WriteLine();
                Console.WriteLine("1 - Основаная матрица");
                Console.WriteLine("2 - Дополнительная матрица");
                PressedKey = Console.ReadKey().Key;
                Console.WriteLine();
                break;
        }
        switch (PressedKey) {
            case ConsoleKey.D1:
                if (Matr1 is null) {
                    CreateMatrix(out Matr1);
                }
                else {
                    Matr1.FillRandomValues();
                }
                break;
            case ConsoleKey.D2:
                if (Matr2 is null) {
                    CreateMatrix(out Matr2);
                }
                else {
                    Matr2.FillRandomValues();
                }
                break;
        }
    }

    public static MatrixData FillMatrix(int SizeMatrix) {
        MatrixData ReturnValue = new MatrixData(SizeMatrix, SizeMatrix);
        for (int IndexRow = 0; IndexRow < SizeMatrix; ++IndexRow) {
            for (int IndexColumn = 0; IndexColumn < SizeMatrix; ++IndexColumn) {
                while (true) {
                    try {
                        Console.Write($"Строка {IndexRow} Столбец {IndexColumn} = ");
                        string InputString = Console.ReadLine(); 
                        ReturnValue[IndexRow][IndexColumn] = MatrixValue.Parse(InputString);
                        break;
                    }
                    catch {
                        Console.WriteLine("Попробуйте снова");
                    }
                }
                
            }
        }
        return ReturnValue;
    }
    public static void ChooseFillMatrix() {
        Console.WriteLine("Заполнение матрицы значениями");
        Console.WriteLine("1 - Основаная матрица");
        Console.WriteLine("2 - Дополнительная матрица");
        ConsoleKey PressedKey = Console.ReadKey().Key;
        Console.WriteLine();
        switch (PressedKey) {
            case ConsoleKey.D1: {
                    if (Matr1 is null) {
                        CreateMatrix(out Matr1);
                    }
                    else {
                        MatrixData NewData = FillMatrix(Matr1.GetRowCount());
                        Matr1 = new SmartMatrix(ref NewData);
                    }
                    break;
                }
            case ConsoleKey.D2: {
                    if (Matr2 is null) {
                        CreateMatrix(out Matr2);
                    }
                    else {
                        MatrixData NewData = FillMatrix(Matr2.GetRowCount());
                        Matr2 = new SmartMatrix(ref NewData);
                    }
                    break;
                }
        }
    }
    public static void PrintMatrix() {
        Console.WriteLine("Вывод матрицы");
        Console.WriteLine("1 - Основаная матрица");
        Console.WriteLine("2 - Дополнительная матрица");
        ConsoleKey PressedKey = Console.ReadKey().Key;
        Console.WriteLine();
        switch (PressedKey) {
            case ConsoleKey.D1:
                if (Matr1 is null) {
                    Console.WriteLine("Матрица не создана");
                }
                else {
                    Console.WriteLine(Matr1.ToString());
                }
                break;
            case ConsoleKey.D2:
                if (Matr2 is null) {
                    Console.WriteLine("Матрица не создана");
                }
                else {
                    Console.WriteLine(Matr2.ToString());
                }
                break;
        }
    }

    public static void GetHashCode() {
        Console.WriteLine("Вывод матрицы");
        Console.WriteLine("1 - Основаная матрица");
        Console.WriteLine("2 - Дополнительная матрица");
        ConsoleKey PressedKey = Console.ReadKey().Key;
        Console.WriteLine();
        switch (PressedKey) {
            case ConsoleKey.D1:
                if (Matr1 is null) {
                    Console.WriteLine("Матрица не создана");
                }
                else {
                    Console.WriteLine(Matr1.GetHashCode());
                }
                break;
            case ConsoleKey.D2:
                if (Matr2 is null) {
                    Console.WriteLine("Матрица не создана");
                }
                else {
                    Console.WriteLine(Matr2.GetHashCode());
                }
                break;
        }
    }

    public static void PrintInverseMatrix() {
        Console.WriteLine("Вывод обратной матрицы");
        Console.WriteLine("1 - Основаная матрица");
        Console.WriteLine("2 - Дополнительная матрица");
        SmartMatrix Inverse;
        ConsoleKey PressedKey = Console.ReadKey().Key;
        Console.WriteLine();
        switch (PressedKey) {
            case ConsoleKey.D1:
                if (Matr1 is null) {
                    Console.WriteLine("Матрица не создана");
                }
                else {
                    Inverse = Matr1.GetInverseMatrix();
                    Console.WriteLine(Inverse.ToString());
                }
                break;
            case ConsoleKey.D2:
                if (Matr2 is null) {
                    Console.WriteLine("Матрица не создана");
                }
                else {
                    Inverse = Matr2.GetInverseMatrix();
                    Console.WriteLine(Inverse.ToString());
                }
                break;
        }
    }
    public static void ChooseComareMatrix() {
        if (Matr1 is null) {
            Console.WriteLine("Основаная Матрица не создана");
            return;
        }
        if (Matr2 is null) {
            Console.WriteLine("Дополнительная Матрица не создана");
            return;
        }
        Console.WriteLine("1 - всё сравнение");
        Console.WriteLine("2 - выборочное сравнение");
        ConsoleKey PressedKey = Console.ReadKey().Key;
        Console.WriteLine();
        switch (PressedKey) {
            case ConsoleKey.D1: {
                    Console.Write("1 - основная матрица меньше дополнительной");
                    if (Matr1 < Matr2) {
                        Console.WriteLine(" Да");
                    }
                    else {
                        Console.WriteLine(" Нет");
                    }
                    Console.Write("2 - основная матрица больше дополнительной");
                    if (Matr1 > Matr2) {
                        Console.WriteLine(" Да");
                    }
                    else {
                        Console.WriteLine(" Нет");
                    }
                    Console.Write("3 - основная матрица равна дополнительной");
                    if (Matr1 == Matr2) {
                        Console.WriteLine(" Да");
                    }
                    else {
                        Console.WriteLine(" Нет");
                    }
                    Console.Write("4 - объекты матриц равны");
                    if (Matr1.Equals(Matr2)) {
                        Console.WriteLine(" Да");
                    }
                    else {
                        Console.WriteLine(" Нет");
                    }
                    Console.Write("5 - значения матриц равны");
                    if (Matr1.EqualsValuse(Matr2)) {
                        Console.WriteLine(" Да");
                    }
                    else {
                        Console.WriteLine(" Нет");
                    }
                }
                break;
            case ConsoleKey.D2: {
                    Console.WriteLine("1 - основная матрица меньше дополнительной");
                    Console.WriteLine("2 - основная матрица больше дополнительной");
                    Console.WriteLine("3 - основная матрица равна дополнительной");
                    Console.WriteLine("4 - объекты матриц равны");
                    PressedKey = Console.ReadKey().Key;
                    Console.WriteLine();
                    switch (PressedKey) {
                        case ConsoleKey.D1:
                            if (Matr1 < Matr2) {
                                Console.WriteLine("Да");
                            }
                            else {
                                Console.WriteLine("Нет");
                            }
                            break;
                        case ConsoleKey.D2:
                            if (Matr1 > Matr2) {
                                Console.WriteLine("Да");
                            }
                            else {
                                Console.WriteLine("Нет");
                            }
                            break;
                        case ConsoleKey.D3:
                            if (Matr1 == Matr2) {
                                Console.WriteLine("Да");
                            }
                            else {
                                Console.WriteLine("Нет");
                            }
                            break;
                        case ConsoleKey.D4:
                            if (Matr1.Equals(Matr2)) {
                                Console.WriteLine("Да");
                            }
                            else {
                                Console.WriteLine("Нет");
                            }
                            break;
                    }
                }
                break;
        }
    }
        public static void Main(string[] args) {
        string functiality = "Функциональность\r\n1) - Создание Основной матрицы\r\n2) - Создание Дополнительной матрицы\r\n3) - Заполнить случайными значениями\r\n4) - Заполнить ручками\r\n5) - Вывести матрицу\r\n6) - Вывести обратную матрицу\r\n7) - Получить хэш код\r\n8) - Сравнить матрицы";
        while (true) {
            Console.Clear();
            Console.WriteLine(functiality);
            try {
                ConsoleKey PressedKey = Console.ReadKey().Key;
                Console.WriteLine();
                switch (PressedKey) {
                    case ConsoleKey.D1:
                        CreateMatrix(out Matr1);
                        break;
                    case ConsoleKey.D2:
                        CreateMatrix(out Matr2);
                        break;
                    case ConsoleKey.D3:
                        FillRandomMatrix();
                        break;
                    case ConsoleKey.D4:
                        ChooseFillMatrix();
                        break;
                    case ConsoleKey.D5:
                        PrintMatrix();
                        break;
                    case ConsoleKey.D6:
                        PrintInverseMatrix();
                        break;
                    case ConsoleKey.D7:
                        GetHashCode();
                        break;
                    case ConsoleKey.D8:
                        ChooseComareMatrix();
                        break;
                }
            }
            catch (SmartMatrixException except) {
                Console.WriteLine(except.ToString());
            }
            Console.WriteLine("Выполнено. Нажмите любую кнопку для продолжения");
            Console.ReadKey();
        }

    }
}
class Matrix : ICloneable {
    protected MatrixData Data = new MatrixData();
    protected int ColumnCount = int.MaxValue;
    protected int RowCount = int.MaxValue;
    private void DetermineSizes(ref MatrixData SourceDara, ref int RowCount, ref int ColumnCount) {
        RowCount = SourceDara.Count();
        ColumnCount = SourceDara[0].Count();
    }
    public Matrix GetThisTypeObject() {
        return this;
    }
    public Matrix() { }
    public Matrix(ref MatrixData SourceDara) {
        Data = SourceDara;
        DetermineSizes(ref Data, ref RowCount, ref ColumnCount);
    }
    public Matrix(ref IEnumerable<MatrixRow> IList) {
        Data = new MatrixData(IList);
        DetermineSizes(ref Data, ref RowCount, ref ColumnCount);
    }
    public Matrix(ref IEnumerable<IEnumerable<MatrixValue>> IList) {
        Data = new MatrixData(ref IList);
        DetermineSizes(ref Data, ref RowCount, ref ColumnCount);
    }

    public Matrix(ref Matrix BasedObject) {
        Matrix ThisObject = this;
        CloneRef(ref BasedObject, ref ThisObject);
    }
    public object Clone() {
        Matrix ClonnedObject;
        CloneOut(out ClonnedObject);
        return ClonnedObject;
    }
    public void CloneOut(out Matrix NewObject) {
        NewObject = new Matrix();
        Matrix ThisObject = this;
        CloneRef(ref NewObject, ref ThisObject);
    }
    public void CloneRef(ref Matrix NewObject, ref Matrix OldObject) {
        NewObject.Data = OldObject.Data;
        NewObject.RowCount = OldObject.RowCount;
        NewObject.ColumnCount = OldObject.ColumnCount;
    }
    ///return 0 если индексы валидны 1 - не валиден индекс строки 2 - не валиден индекс столбца 3 - количество столбцов строки меньше глобального количества столбцов
    int IsIndexValid(int RowIndex, int ColumnIndex) {
        if (RowIndex < 0 || RowIndex >= (RowCount)) {
            return 1;
        }
        if (ColumnIndex < 0 || ColumnIndex >= (ColumnCount)) {
            return 2;
        }
        if (ColumnIndex >= this.Data[RowIndex].Count) {
            return 3;
        }
        return 0;
    }

    public int GetRowCount() {
        return RowCount;
    }
    public int GetColumnCount() {
        return ColumnCount;
    }
    public int GetValue(ref MatrixValue Value, int RowIndex, int ColumnIndex) {
        if (IsIndexValid(RowIndex, ColumnIndex) != 0) {
            return 1;
        }
        Value = Data[RowIndex][ColumnIndex];
        return 0;
    }

    //Unsafe
    private MatrixValue GetValue(int RowIndex, int ColumnIndex) {
        if (IsIndexValid(RowIndex, ColumnIndex) != 0) {
            return MatrixValue.MaxValue;
        }
        return Data[RowIndex][ColumnIndex];
    }
    public int SetValue(ref MatrixValue Value, int RowIndex, int ColumnIndex) {
        if (IsIndexValid(RowIndex, ColumnIndex) != 0) {
            return 1;
        }
        Data[RowIndex][ColumnIndex] = Value;
        return 0;
    }
    public void SetSize(int RowCount, int ColumnCount) {
        if (RowCount < 0) {
            RowCount = 0;
        }
        if (ColumnCount < 0) {
            ColumnCount = 0;
        }
        Data = new MatrixData(RowCount, ColumnCount);
    }
};
class MatrixUtils {
    public void PrintMatrix(ref Matrix InputMatrix) {
        string PrintedString = "";
        PrintMatrix(ref InputMatrix, ref PrintedString);
        Console.Write(PrintedString);
    }

    public void PrintMatrix(ref Matrix InputMatrix, ref string OutputString) {
        int RowCount = InputMatrix.GetRowCount(), ColumnCount = InputMatrix.GetColumnCount()
    , CurrentRow, CurrentCollumn;
        MatrixValue TempValue = 0;
        int RetCode;
        for (CurrentRow = 0; CurrentRow < RowCount; ++CurrentRow) {
            for (CurrentCollumn = 0; CurrentCollumn < ColumnCount; ++CurrentCollumn) {
                RetCode = InputMatrix.GetValue(ref TempValue, CurrentRow, CurrentCollumn);
                if (RetCode != 0) {
                    throw new SmartMatrixException("0");   
                }
                OutputString += TempValue + " ";
            }
            OutputString +="\n";
        }
    }

    private Random RandomInstance;
    Random GetRandomInstance() {
        if (RandomInstance == null) {
            RandomInstance = new Random();
        }
        return RandomInstance;
    }
    public void GenearateRandomSize(ref Matrix InputMatrix, int MaxRows, int MaxColumns) {
        int ColumnCount = GenerateRandomNumber(1, MaxColumns);
        int RowCount = GenerateRandomNumber(1, MaxRows);
        InputMatrix.SetSize(RowCount, ColumnCount);
    }
    public void FillRandomValues(ref Matrix InputMatrix) {
        int RowCount = InputMatrix.GetRowCount(), ColumnCount = InputMatrix.GetColumnCount()
            , CurrentRow, CurrentCollumn;
        MatrixValue NewValue;
        for (CurrentRow = 0; CurrentRow < RowCount; ++CurrentRow) {
            for (CurrentCollumn = 0; CurrentCollumn < ColumnCount; ++CurrentCollumn) {
                NewValue = GetRandomMatrixValue(-100, 100);
                InputMatrix.SetValue(ref NewValue, CurrentRow, CurrentCollumn);
            }
        }
    }

    public int AddMatrix(ref Matrix MatrixLeft, ref Matrix MatrixRight) {
        if (!IsSizesEqual(ref MatrixLeft, ref MatrixRight)) {
            return 1;
        }
        int RowCount = MatrixLeft.GetRowCount(), ColumnCount = MatrixLeft.GetColumnCount()
            , CurrentRow, CurrentCollumn;
        MatrixValue LeftValue = 0, RightValue = 0;
        int RetCode = 0;

        for (CurrentRow = 0; CurrentRow < RowCount; ++CurrentRow) {
            for (CurrentCollumn = 0; CurrentCollumn < ColumnCount; ++CurrentCollumn) {
                RetCode = MatrixLeft.GetValue(ref LeftValue, CurrentRow, CurrentCollumn);
                RetCode |= MatrixRight.GetValue(ref RightValue, CurrentRow, CurrentCollumn);
                if (RetCode != 0) {
                    throw new SmartMatrixException("AddMatrix");
                }
                LeftValue += RightValue;
                MatrixLeft.SetValue(ref LeftValue, CurrentRow, CurrentCollumn);
            }
        }
        return 0;
    }
    private bool IsSizesEqual(ref Matrix MatrixLeft, ref Matrix MatrixRight) {
        if (MatrixLeft.GetColumnCount() == MatrixRight.GetColumnCount() && MatrixLeft.GetRowCount() == MatrixRight.GetRowCount()) {
            return true;
        }
        return false;
    }
    public bool IsMatrixEqual(ref Matrix MatrixLeft, ref Matrix MatrixRight) {
        if (MatrixLeft.GetColumnCount() == MatrixRight.GetColumnCount() && MatrixLeft.GetRowCount() == MatrixRight.GetRowCount()) {
            MatrixValue LeftValue = 0, RightValue = 0;
            int RetCode = 0;
            int RowCount = MatrixLeft.GetRowCount(), ColumnCount = MatrixLeft.GetColumnCount(), CurrentRow, CurrentCollumn;
            for (CurrentRow = 0; CurrentRow < RowCount; ++CurrentRow) {
                for (CurrentCollumn = 0; CurrentCollumn < ColumnCount; ++CurrentCollumn) {
                    RetCode = MatrixLeft.GetValue(ref LeftValue, CurrentRow, CurrentCollumn);
                    RetCode |= MatrixRight.GetValue(ref RightValue, CurrentRow, CurrentCollumn);
                    if (RetCode != 0) {
                        throw new SmartMatrixException("IsMatrixEqual");
                    }
                    if (LeftValue  != RightValue) ;
                    return false;
                }
            }
        }
        return true;
    }

    int GetMinorMatrix(ref Matrix SourceMatrix, int RowIndex, int ColumnIndex, ref Matrix OutputMatrix) {
        int RowCount = SourceMatrix.GetRowCount(), ColumnCount = SourceMatrix.GetColumnCount(), CurrentRow, CurrentCollumn;
        IEnumerable<IEnumerable<MatrixValue>> MatrixCollection = new MatrixData();

        MatrixValue Value = 0;
        if (SourceMatrix.GetColumnCount() == 1) {
            if(SourceMatrix.GetValue(ref Value , 1, 1) != 0) {
                return 1;
            }
            MatrixCollection = MatrixCollection.Append(new MatrixRow() { Value });
        }
        else {
            for (CurrentRow = 0; CurrentRow < RowCount; ++CurrentRow) {
                IEnumerable<MatrixValue> RowCollection = new MatrixRow();
                if (CurrentRow == RowIndex) {
                    continue;
                }
                for (CurrentCollumn = 0; CurrentCollumn < ColumnCount; ++CurrentCollumn) {
                    if (CurrentCollumn == ColumnIndex) {
                        continue;
                    }
                    if (SourceMatrix.GetValue(ref Value, CurrentRow, CurrentCollumn) != 0) {
                        return 1;
                    }
                    RowCollection = RowCollection.Append(Value);
                }
                MatrixCollection = MatrixCollection.Append(RowCollection);
            }
        }
        OutputMatrix = new Matrix(ref MatrixCollection);
        return 0;
    }
    public int DetermineDeterminant(ref Matrix SourceMatrix, ref double OutDeterminant) {
        if (SourceMatrix.GetColumnCount() != SourceMatrix.GetRowCount()) {
            return 1;
        }
        
        OutDeterminant = 0;
        double Determinant = 0, CurIndexValue = 0, CurIndexMinorDeterminantValue = 0;
        int CurrentColumn, ColumnCount = SourceMatrix.GetColumnCount();
        Matrix MinorMatrix = new Matrix();
        int RetCode = 0;
        if (SourceMatrix.GetColumnCount() == 1) {
            SourceMatrix.GetValue(ref OutDeterminant, 0, 0);
            return 0;
        }
        for (CurrentColumn = 0; CurrentColumn < ColumnCount; ++CurrentColumn) {
            RetCode = SourceMatrix.GetValue(ref CurIndexValue, 0, CurrentColumn);
            if (RetCode != 0) {
                return 1;
            }
            if (GetMinorMatrix(ref SourceMatrix, 0, CurrentColumn, ref MinorMatrix) != 0) {
                return 2;
            }
            if (MinorMatrix.GetColumnCount() == 1) {
                RetCode = MinorMatrix.GetValue(ref CurIndexMinorDeterminantValue, 0, 0);
                if (RetCode != 0) {
                    return 1;
                }

            }
            else {
                DetermineDeterminant(ref MinorMatrix, ref Determinant);
                CurIndexMinorDeterminantValue = Determinant;
            }
            //Console.WriteLine($"{CurIndexValue} {CurIndexMinorDeterminantValue} {CurIndexMinorDeterminantValue *CurIndexValue}");
            CurIndexValue *= CurIndexMinorDeterminantValue;
            if (CurrentColumn % 2 == 1) {
                CurIndexValue *= -1;
            }
            OutDeterminant += CurIndexValue;
        }
        return 0;
    }

    public int GetInverseMatrix(ref Matrix SourceMatrix, ref Matrix OutMatrix) {
        MatrixValue Determinant = 0;
        if (DetermineDeterminant(ref SourceMatrix, ref Determinant) != 0) {
            return 1;
        }
        OutMatrix = SourceMatrix;

        int RowCount = SourceMatrix.GetRowCount(), ColumnCount = SourceMatrix.GetColumnCount()
    , CurrentRow, CurrentCollumn;
        MatrixValue NewValue = 0;
        int RetCode = 0;
        for (CurrentRow = 0; CurrentRow < RowCount; ++CurrentRow) {
            for (CurrentCollumn = 0; CurrentCollumn < ColumnCount; ++CurrentCollumn) {
                RetCode = SourceMatrix.GetValue(ref NewValue, CurrentRow, CurrentCollumn);
                if (RetCode != 0) {
                    return 1;
                }
                NewValue /= Determinant;
                OutMatrix.SetValue(ref NewValue, CurrentRow, CurrentCollumn);
            }
        }
        return 0;
    }

    public int  MullMatrix(ref Matrix SourceMatrix, MatrixValue Value) {
        int RowCount = SourceMatrix.GetRowCount(), ColumnCount = SourceMatrix.GetColumnCount()
    , CurrentRow, CurrentCollumn, RetCode;
        MatrixValue NewValue = 0;
        for (CurrentRow = 0; CurrentRow < RowCount; ++CurrentRow) {
            for (CurrentCollumn = 0; CurrentCollumn < ColumnCount; ++CurrentCollumn) {
                RetCode = SourceMatrix.GetValue(ref NewValue, CurrentRow, CurrentCollumn);
                if (RetCode != 0) {
                    return 1;
                }
                NewValue *= Value;
                SourceMatrix.SetValue(ref NewValue, CurrentRow, CurrentCollumn);
            }
        }
        return 0;
    }
    private MatrixValue GetRandomMatrixValue(MatrixValue MinValue, MatrixValue MaxValue) {
        return MinValue + (GetRandomInstance().NextDouble() * (MaxValue - MinValue));
    }
    private int GenerateRandomNumber(int minValue, int maxValue) {
        return GetRandomInstance().Next(minValue, maxValue + 1);
    }
}

class SmartMatrix : Matrix, ICloneable, IComparable<SmartMatrix> {

    private MatrixUtils Utils = new MatrixUtils();
    public MatrixValue Determinant = 0;

    public SmartMatrix() : base() { }

    SmartMatrix(ref SmartMatrix BasedObject) {
        SmartMatrix NewObject = this;
        CloneRef(ref NewObject, ref BasedObject);
    }
    public new object Clone() {
        SmartMatrix ClonnedObject;
        CloneOut(out ClonnedObject);
        return ClonnedObject;
    }
    public void CloneOut(out SmartMatrix NewObject) {
        NewObject = new SmartMatrix();
        SmartMatrix BasedObject = this;
        CloneRef(ref NewObject, ref BasedObject);
    }
    public void CloneRef(ref SmartMatrix NewObject, ref SmartMatrix BasedObject) {
        Matrix BaseTypeThisObject = this;
        Matrix BaseTypeNewObject = NewObject;
        base.CloneRef(ref BaseTypeThisObject, ref BaseTypeNewObject);
        NewObject.Determinant = BasedObject.Determinant;
    }

    public SmartMatrix(ref MatrixData SourceDara) : base(ref SourceDara) {
        CalcDeterminant();
    }
    public void CalcDeterminant() {
        Matrix BaseTypeClass = (Matrix)this;
        int RetCode = 0;
        RetCode  = Utils.DetermineDeterminant(ref BaseTypeClass, ref this.Determinant);
        if(RetCode != 0) {
            throw new SmartMatrixException("Error calc determinant");
        }
    }
    public void FillRandomValues() {
        Matrix BaseTypeClass = (Matrix)this;
        int RetCode = 0;
        Utils.FillRandomValues(ref BaseTypeClass);
        this.CalcDeterminant();
    }
    public double GetDeterminant() {
        return Determinant;
    }
    public SmartMatrix GetInverseMatrix() {
        Matrix BasedTypeObject = this;
        SmartMatrix OutSmartMatrix = new SmartMatrix();
        Matrix OutMatrix = OutSmartMatrix;
        int RetCode = 0;
        RetCode  = Utils.GetInverseMatrix(ref BasedTypeObject,  ref OutMatrix);
        if (RetCode != 0) {
            throw new 
                SmartMatrixException("Error calc determinant");
        }
            OutSmartMatrix.CalcDeterminant();
        return OutSmartMatrix;
    }
    public SmartMatrix(ref IEnumerable<MatrixRow> IList) : base(ref IList) {
        CalcDeterminant();
    }
    public SmartMatrix(ref IEnumerable<IEnumerable<MatrixValue>> IList) : base(ref IList) {
        CalcDeterminant();
    }
    public static SmartMatrix operator +(SmartMatrix Left, SmartMatrix Right) {
        SmartMatrix ReturnMatrix = new SmartMatrix(ref Left);
        Matrix LeftMatrix = ReturnMatrix;
        Matrix RightMatrix = Right;
        ReturnMatrix.Utils.AddMatrix(ref LeftMatrix, ref RightMatrix);
        return ReturnMatrix;
    }

    public static SmartMatrix operator *(SmartMatrix Left, MatrixValue Value) {
        SmartMatrix ReturnMatrix = new SmartMatrix(ref Left);
        Matrix LeftMatrix = ReturnMatrix;
        ReturnMatrix.Utils.MullMatrix(ref LeftMatrix, Value);
        return ReturnMatrix;
    }

    public override string ToString() {
        Matrix BasedTypeObject = this;
        string OutputString = "";
        this.Utils.PrintMatrix(ref BasedTypeObject, ref OutputString);
        OutputString += "Determinant: " + Determinant + "\n";
        return OutputString;
    }
    public int CompareTo(SmartMatrix OtherMatrix) {
        return this.Determinant.CompareTo(OtherMatrix.Determinant);
    }
    public override bool Equals(object OtherObjet) {
        if (OtherObjet == null || GetType() != OtherObjet.GetType()) {
            return false;
        }
        return true;
    }
    public bool EqualsValuse(object OtherObjet) {
        if (Equals(OtherObjet)){
            SmartMatrix OtherMatrix = (SmartMatrix)OtherObjet;
            if (this.Determinant == OtherMatrix.Determinant) {
                Matrix ThisObjectMatrixType = this,
                        OtherObjectMatrixType = OtherMatrix;
                if (Utils.IsMatrixEqual(ref ThisObjectMatrixType, ref OtherObjectMatrixType)) {
                    return true;
                }
            }
        }
        return false;
    }
    public override int GetHashCode() {
        return base.GetHashCode();
    }

public static bool operator > (SmartMatrix Left, SmartMatrix Right) {
        return Left.Determinant > Right.Determinant;
    }

    public static bool operator <(SmartMatrix Left, SmartMatrix Right) {
        return Left.Determinant < Right.Determinant;
    }
    public static bool operator == (SmartMatrix Left, SmartMatrix Right) {
        
        return Left.Determinant == Right.Determinant;
    }
    public static bool operator != (SmartMatrix Left, SmartMatrix Right) {
        return Left.Determinant != Right.Determinant;
    }

    public static bool operator <=(SmartMatrix Left, SmartMatrix Right) {
        return !(Left.Determinant > Right.Determinant);
    }
    public static bool operator >=(SmartMatrix Left, SmartMatrix Right) {
        return !(Left.Determinant < Right.Determinant);
    }
}

public class SmartMatrixException : Exception {
    public SmartMatrixException(string message)
       : base(message) { }
    public override  string ToString() {
        return base.ToString();
    }
}