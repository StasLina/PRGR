
using System;
using System.Diagnostics;

using System.Linq.Expressions;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Collections;

using MatrixValue = double;
using System.Text.RegularExpressions;
public class MatrixRow : List<MatrixValue>
{
    public MatrixRow() : base() { }

    public MatrixRow(IEnumerable<MatrixValue> IList) : base(IList) { }

    public MatrixRow(int ColumnCount) : base(ColumnCount)
    {
        for (int curColIndex = 0; curColIndex < ColumnCount; ++curColIndex)
        {
            this.Add(0);
        }
    }
}
public class MatrixData : List<MatrixRow>
{
    public MatrixData() : base() { }

    public MatrixData(int RowCount) : base(RowCount) { }

    public MatrixData(int RowCount, int ColumnCount) : base()
    {
        for (int curRowIndex = 0; curRowIndex < RowCount; ++curRowIndex)
        {
            this.Add(new MatrixRow(ColumnCount));
        }
    }

    public MatrixData(IEnumerable<MatrixRow> IList) : base(IList) { }

    public MatrixData(IEnumerable<IEnumerable<MatrixValue>> IList)
    {
        int RowCount = IList.Count();
        for (int rowIndex = 0; rowIndex < RowCount; ++rowIndex)
        {
            this.Add(new MatrixRow(IList.ElementAt(rowIndex)));
        }
    }
}


class Application {
    public static SmartMatrix Matr1, Matr2;
    public static void CreateMatrix(out SmartMatrix matr1)
    {
        SmartMatrix matr = new SmartMatrix();

        Console.WriteLine("Создание матрицы");
        Console.WriteLine("Введите размер квадратной матрицы матрицы");

        int sizeMatrix;
        string sizeMatrixString = "";
        while (true)
        {
            try
            {
                sizeMatrixString = Console.ReadLine();
                sizeMatrix = int.Parse(sizeMatrixString);
                break;
            }
            catch
            {
                Console.WriteLine("Попробуйте ввести снова");
            }
        }

        MatrixData matrixData = new MatrixData(sizeMatrix, sizeMatrix);
        Console.WriteLine("1 - Заполнить рандомными значениями");
        Console.WriteLine("2 - Заполнить ручками");
        Console.WriteLine("3 - Заполнить 0");
        bool cycleHandling = true;
        while (cycleHandling)
        {
            ConsoleKey pressedKey = Console.ReadKey().Key;
            Console.WriteLine();
            switch (pressedKey)
            {
                case ConsoleKey.D1:
                    matr1 = new SmartMatrix(matrixData);
                    matr1.FillRandomValues();
                    Console.WriteLine();
                    return;
                case ConsoleKey.D2:
                    matrixData = FillMatrix(sizeMatrix);
                    matr1 = new SmartMatrix(matrixData);
                    Console.WriteLine();
                    return;
                case ConsoleKey.D3:
                    matr1 = new SmartMatrix(matrixData);
                    Console.WriteLine();
                    return;
            }
        }
        matr1 = new SmartMatrix(matrixData);
    }

    public static void FillRandomMatrix(int choose = 0)
    {
        Console.WriteLine("Заполнение матрицы рандомными значениями");
        ConsoleKey pressedKey;
        switch (choose)
        {
            case 1:
                pressedKey = ConsoleKey.D1;
                break;
            case 2:
                pressedKey = ConsoleKey.D2;
                break;
            default:
                Console.WriteLine();
                Console.WriteLine("1 - Основная матрица");
                Console.WriteLine("2 - Дополнительная матрица");
                pressedKey = Console.ReadKey().Key;
                Console.WriteLine();
                break;
        }
        switch (pressedKey)
        {
            case ConsoleKey.D1:
                if (Matr1 is null)
                {
                    CreateMatrix(out Matr1);
                }
                else
                {
                    Matr1.FillRandomValues();
                }
                break;
            case ConsoleKey.D2:
                if (Matr2 is null)
                {
                    CreateMatrix(out Matr2);
                }
                else
                {
                    Matr2.FillRandomValues();
                }
                break;
        }
    }

    public static MatrixData FillMatrix(int sizeMatrix)
    {
        MatrixData returnValue = new MatrixData(sizeMatrix, sizeMatrix);
        for (int indexRow = 0; indexRow < sizeMatrix; ++indexRow)
        {
            for (int indexColumn = 0; indexColumn < sizeMatrix; ++indexColumn)
            {
                while (true)
                {
                    try
                    {
                        Console.Write($"Строка {indexRow} Столбец {indexColumn} = ");
                        string inputString = Console.ReadLine();
                        returnValue[indexRow][indexColumn] = MatrixValue.Parse(inputString);
                        break;
                    }
                    catch
                    {
                        Console.WriteLine("Попробуйте снова");
                    }
                }
            }
        }
        return returnValue;
    }

    public static void ChooseFillMatrix()
    {
        Console.WriteLine("Заполнение матрицы значениями");
        Console.WriteLine("1 - Основная матрица");
        Console.WriteLine("2 - Дополнительная матрица");
        ConsoleKey pressedKey = Console.ReadKey().Key;
        Console.WriteLine();
        switch (pressedKey)
        {
            case ConsoleKey.D1:
                if (Matr1 is null)
                {
                    CreateMatrix(out Matr1);
                }
                else
                {
                    MatrixData newData = FillMatrix(Matr1.GetRowCount());
                    Matr1 = new SmartMatrix(newData);
                }
                break;
            case ConsoleKey.D2:
                if (Matr2 is null)
                {
                    CreateMatrix(out Matr2);
                }
                else
                {
                    MatrixData newData = FillMatrix(Matr2.GetRowCount());
                    Matr2 = new SmartMatrix(newData);
                }
                break;
        }
    }
    public static void PrintMatrix()
    {
        Console.WriteLine("Вывод матрицы");
        Console.WriteLine("1 - Основная матрица");
        Console.WriteLine("2 - Дополнительная матрица");
        ConsoleKey pressedKey = Console.ReadKey().Key;
        Console.WriteLine();
        switch (pressedKey)
        {
            case ConsoleKey.D1:
                if (Matr1 is null)
                {
                    Console.WriteLine("Матрица не создана");
                }
                else
                {
                    Console.WriteLine(Matr1.ToString());
                }
                break;
            case ConsoleKey.D2:
                if (Matr2 is null)
                {
                    Console.WriteLine("Матрица не создана");
                }
                else
                {
                    Console.WriteLine(Matr2.ToString());
                }
                break;
        }
    }

    public static void GetHashCode()
    {
        Console.WriteLine("Вывод хэш-кода матрицы");
        Console.WriteLine("1 - Основная матрица");
        Console.WriteLine("2 - Дополнительная матрица");
        ConsoleKey pressedKey = Console.ReadKey().Key;
        Console.WriteLine();
        switch (pressedKey)
        {
            case ConsoleKey.D1:
                if (Matr1 is null)
                {
                    Console.WriteLine("Матрица не создана");
                }
                else
                {
                    Console.WriteLine("Хэш-код основной матрицы: " + Matr1.GetHashCode());
                }
                break;
            case ConsoleKey.D2:
                if (Matr2 is null)
                {
                    Console.WriteLine("Матрица не создана");
                }
                else
                {
                    Console.WriteLine("Хэш-код дополнительной матрицы: " + Matr2.GetHashCode());
                }
                break;
        }
    }

    public static void PrintInverseMatrix()
    {
        Console.WriteLine("Вывод обратной матрицы");
        Console.WriteLine("1 - Основная матрица");
        Console.WriteLine("2 - Дополнительная матрица");
        SmartMatrix inverse;
        ConsoleKey pressedKey = Console.ReadKey().Key;
        Console.WriteLine();
        switch (pressedKey)
        {
            case ConsoleKey.D1:
                if (Matr1 is null)
                {
                    Console.WriteLine("Матрица не создана");
                }
                else
                {
                    inverse = Matr1.GetInverseMatrix();
                    Console.WriteLine(inverse.ToString());
                }
                break;
            case ConsoleKey.D2:
                if (Matr2 is null)
                {
                    Console.WriteLine("Матрица не создана");
                }
                else
                {
                    inverse = Matr2.GetInverseMatrix();
                    Console.WriteLine(inverse.ToString());
                }
                break;
        }
    }
    public static void ChooseCompareMatrix()
{
    if (Matr1 is null)
    {
        Console.WriteLine("Основная Матрица не создана");
        return;
    }
    if (Matr2 is null)
    {
        Console.WriteLine("Дополнительная Матрица не создана");
        return;
    }

    Console.WriteLine("1 - всё сравнение");
    Console.WriteLine("2 - выборочное сравнение");
    ConsoleKey pressedKey = Console.ReadKey().Key;
    Console.WriteLine();
    switch (pressedKey)
    {
        case ConsoleKey.D1:
            Console.WriteLine("1 - основная матрица меньше дополнительной: " + (Matr1 < Matr2 ? "Да" : "Нет"));
            Console.WriteLine("2 - основная матрица больше дополнительной: " + (Matr1 > Matr2 ? "Да" : "Нет"));
            Console.WriteLine("3 - основная матрица равна дополнительной: " + (Matr1 == Matr2 ? "Да" : "Нет"));
            Console.WriteLine("4 - объекты матриц равны: " + (Matr1.Equals(Matr2) ? "Да" : "Нет"));
            Console.WriteLine("5 - значения матриц равны: " + (Matr1.EqualsValues(Matr2) ? "Да" : "Нет"));
            break;
        case ConsoleKey.D2:
            Console.WriteLine("1 - основная матрица меньше дополнительной");
            Console.WriteLine("2 - основная матрица больше дополнительной");
            Console.WriteLine("3 - основная матрица равна дополнительной");
            Console.WriteLine("4 - объекты матриц равны");
            pressedKey = Console.ReadKey().Key;
            Console.WriteLine();
            switch (pressedKey)
            {
                case ConsoleKey.D1:
                    Console.WriteLine(Matr1 < Matr2 ? "Да" : "Нет");
                    break;
                case ConsoleKey.D2:
                    Console.WriteLine(Matr1 > Matr2 ? "Да" : "Нет");
                    break;
                case ConsoleKey.D3:
                    Console.WriteLine(Matr1 == Matr2 ? "Да" : "Нет");
                    break;
                case ConsoleKey.D4:
                    Console.WriteLine(Matr1.Equals(Matr2) ? "Да" : "Нет");
                    break;
            }
            break;
    }
}

    public static void Main(string[] args)
    {
        string functionality = "Функциональность\r\n1) - Создание Основной матрицы\r\n2) - Создание Дополнительной матрицы\r\n3) - Заполнить случайными значениями\r\n4) - Заполнить ручками\r\n5) - Вывести матрицу\r\n6) - Вывести обратную матрицу\r\n7) - Получить хэш код\r\n8) - Сравнить матрицы";

        while (true)
        {
            Console.Clear();
            Console.WriteLine(functionality);
            try
            {
                ConsoleKey pressedKey = Console.ReadKey().Key;
                Console.WriteLine();
                switch (pressedKey)
                {
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
                        ChooseCompareMatrix();
                        break;
                }
            }
            catch (SmartMatrixException ex)
            {
                Console.WriteLine(ex.ToString());
            }

            Console.WriteLine("Выполнено. Нажмите любую кнопку для продолжения");
            Console.ReadKey();
        }
    }

}

class Matrix : ICloneable
{
    protected MatrixData Data = new MatrixData();
    protected int ColumnCount = int.MaxValue;
    protected int RowCount = int.MaxValue;

    private void DetermineSizes(MatrixData SourceData, ref int rowCount, ref int columnCount)
    {
        rowCount = SourceData.Count();
        if (rowCount > 0)
        {
            columnCount = SourceData[0].Count();
        }
    }

    public Matrix() { }

    public Matrix(MatrixData SourceData)
    {
        Data = SourceData;
        DetermineSizes(Data, ref RowCount, ref ColumnCount);
    }

    public Matrix(IEnumerable<MatrixRow> IList)
    {
        Data = new MatrixData(IList);
        DetermineSizes(Data, ref RowCount, ref ColumnCount);
    }

    public Matrix(IEnumerable<IEnumerable<MatrixValue>> IList)
    {
        Data = new MatrixData(IList);
        DetermineSizes(Data, ref RowCount, ref ColumnCount);
    }

    public Matrix(Matrix BasedObject)
    {
        CloneRef(BasedObject, this);
    }

    public object Clone()
    {
        Matrix clonedObject;
        CloneOut(out clonedObject);
        return clonedObject;
    }

    public void CloneOut(out Matrix newObject)
    {
        newObject = new Matrix();
        CloneRef(this, newObject);
    }

    public void CloneRef(Matrix oldObject, Matrix newObject)
    {
        newObject.Data = new MatrixData(oldObject.Data);
        newObject.RowCount = oldObject.RowCount;
        newObject.ColumnCount = oldObject.ColumnCount;
    }

    // Returns:
    // 0 - если индексы валидны
    // 1 - не валиден индекс строки
    // 2 - не валиден индекс столбца
    // 3 - количество столбцов строки меньше глобального количества столбцов
    private int IsIndexValid(int rowIndex, int columnIndex)
    {
        if (rowIndex < 0 || rowIndex >= RowCount)
        {
            return 1;
        }
        if (columnIndex < 0 || columnIndex >= ColumnCount)
        {
            return 2;
        }
        if (columnIndex >= Data.Count)
        {
            return 3;
        }
        if (columnIndex >= Data[rowIndex].Count)
        {
            return 4;
        }
        return 0;
    }

    public int GetRowCount()
    {
        return RowCount;
    }

    public int GetColumnCount()
    {
        return ColumnCount;
    }

    public int GetValue(ref MatrixValue value, int rowIndex, int columnIndex)
    {
        if (IsIndexValid(rowIndex, columnIndex) != 0)
        {
            return 1;
        }
        value = Data[rowIndex][columnIndex];
        return 0;
    }

    // Unsafe
    private MatrixValue GetValue(int rowIndex, int columnIndex)
    {
        if (IsIndexValid(rowIndex, columnIndex) != 0)
        {
            return MatrixValue.MaxValue;
        }
        return Data[rowIndex][columnIndex];
    }

    public int SetValue(MatrixValue value, int rowIndex, int columnIndex)
    {
        if (IsIndexValid(rowIndex, columnIndex) != 0)
        {
            return 1;
        }
        Data[rowIndex][columnIndex] = value;
        return 0;
    }

    public void SetSize(int rowCount, int columnCount)
    {
        if (rowCount < 0)
        {
            rowCount = 0;
        }
        if (columnCount < 0)
        {
            columnCount = 0;
        }
        Data = new MatrixData(rowCount, columnCount);
    }
}

class MatrixMatrixUtils {
    public void PrintMatrix(Matrix inputMatrix)
    {
        string printedString = "";
        PrintMatrix(inputMatrix, ref printedString);
        Console.Write(printedString);
    }

    public void PrintMatrix(Matrix inputMatrix, ref string outputString)
    {
        int rowCount = inputMatrix.GetRowCount(), columnCount = inputMatrix.GetColumnCount(), currentRow, currentColumn;
        MatrixValue tempValue = 0;
        int retCode;
        for (currentRow = 0; currentRow < rowCount; ++currentRow)
        {
            for (currentColumn = 0; currentColumn < columnCount; ++currentColumn)
            {
                retCode = inputMatrix.GetValue(ref tempValue, currentRow, currentColumn);
                if (retCode != 0)
                {
                    throw new SmartMatrixExceptionControl($"Ошибка получения значения матрицы по индексам {currentRow} {currentColumn}");
                }
                outputString += tempValue + " ";
            }
            outputString += "\n";
        }
    }

    private Random randomInstance;
    Random GetRandomInstance()
    {
        if (randomInstance == null)
        {
            randomInstance = new Random();
        }
        return randomInstance;
    }

    public void GenerateRandomSize(Matrix inputMatrix, int maxRows, int maxColumns)
    {
        int columnCount = GenerateRandomNumber(1, maxColumns);
        int rowCount = GenerateRandomNumber(1, maxRows);
        inputMatrix.SetSize(rowCount, columnCount);
    }

    public void FillRandomValues(Matrix inputMatrix)
    {
        int rowCount = inputMatrix.GetRowCount(), columnCount = inputMatrix.GetColumnCount(), currentRow, currentColumn;
        MatrixValue newValue;
        for (currentRow = 0; currentRow < rowCount; ++currentRow)
        {
            for (currentColumn = 0; currentColumn < columnCount; ++currentColumn)
            {
                newValue = GetRandomMatrixValue(-100, 100);
                inputMatrix.SetValue(newValue, currentRow, currentColumn);
            }
        }
    }

    public int AddMatrix(Matrix matrixLeft, Matrix matrixRight)
    {
        if (!IsSizesEqual(matrixLeft, matrixRight))
        {
            return 1;
        }
        int rowCount = matrixLeft.GetRowCount(), columnCount = matrixLeft.GetColumnCount(), currentRow, currentColumn;
        MatrixValue leftValue = 0, rightValue = 0;
        int retCode = 0;

        for (currentRow = 0; currentRow < rowCount; ++currentRow)
        {
            for (currentColumn = 0; currentColumn < columnCount; ++currentColumn)
            {
                retCode = matrixLeft.GetValue(ref leftValue, currentRow, currentColumn);
                retCode |= matrixRight.GetValue(ref rightValue, currentRow, currentColumn);
                if (retCode != 0)
                {
                    throw new SmartMatrixExceptionControl($"Ошибка получения значения матрицы по индексам {currentRow} {currentColumn}");
                }
                leftValue += rightValue;
                matrixLeft.SetValue(leftValue, currentRow, currentColumn);
            }
        }
        return 0;
    }

    private bool IsSizesEqual(Matrix matrixLeft, Matrix matrixRight)
    {
        if (matrixLeft.GetColumnCount() == matrixRight.GetColumnCount() && matrixLeft.GetRowCount() == matrixRight.GetRowCount())
        {
            return true;
        }
        return false;
    }

    public bool IsMatrixEqual(Matrix matrixLeft, Matrix matrixRight)
    {
        if (matrixLeft.GetColumnCount() == matrixRight.GetColumnCount() && matrixLeft.GetRowCount() == matrixRight.GetRowCount())
        {
            MatrixValue leftValue = 0, rightValue = 0;
            int retCode = 0;
            int rowCount = matrixLeft.GetRowCount(), columnCount = matrixLeft.GetColumnCount(), currentRow, currentColumn;
            for (currentRow = 0; currentRow < rowCount; ++currentRow)
            {
                for (currentColumn = 0; currentColumn < columnCount; ++currentColumn)
                {
                    retCode = matrixLeft.GetValue(ref leftValue, currentRow, currentColumn);
                    retCode |= matrixRight.GetValue(ref rightValue, currentRow, currentColumn);
                    if (retCode != 0)
                    {
                        throw new SmartMatrixExceptionControl($"Ошибка получения значения матрицы по индексам {currentRow} {currentColumn}");
                    }
                    if (leftValue != rightValue)
                    {
                        return false;
                    }
                }
            }
        }
        return true;
    }

    public int GetMinorMatrix(Matrix sourceMatrix, int rowIndex, int columnIndex, ref Matrix outputMatrix)
    {
        int rowCount = sourceMatrix.GetRowCount(), columnCount = sourceMatrix.GetColumnCount(), currentRow, currentColumn;
        IEnumerable<IEnumerable<MatrixValue>> matrixCollection = new MatrixData();

        MatrixValue value = 0;
        if (sourceMatrix.GetColumnCount() == 1)
        {
            if (sourceMatrix.GetValue(ref value, 1, 1) != 0)
            {
                return 1;
            }
            matrixCollection = matrixCollection.Append(new MatrixRow() { value });
        }
        else
        {
            for (currentRow = 0; currentRow < rowCount; ++currentRow)
            {
                IEnumerable<MatrixValue> rowCollection = new MatrixRow();
                if (currentRow == rowIndex)
                {
                    continue;
                }
                for (currentColumn = 0; currentColumn < columnCount; ++currentColumn)
                {
                    if (currentColumn == columnIndex)
                    {
                        continue;
                    }
                    if (sourceMatrix.GetValue(ref value, currentRow, currentColumn) != 0)
                    {
                        return 1;
                    }
                    rowCollection = rowCollection.Append(value);
                }
                matrixCollection = matrixCollection.Append(rowCollection);
            }
        }
        outputMatrix = new Matrix(matrixCollection);
        return 0;
    }

    public int DetermineDeterminant(Matrix sourceMatrix, ref double outDeterminant)
    {
        if (sourceMatrix.GetColumnCount() != sourceMatrix.GetRowCount())
        {
            return 1;
        }

        outDeterminant = 0;
        double determinant = 0, curIndexValue = 0, curIndexMinorDeterminantValue = 0;
        int currentColumn, columnCount = sourceMatrix.GetColumnCount();
        Matrix minorMatrix = new Matrix();
        int retCode = 0;
        if (sourceMatrix.GetColumnCount() == 1)
        {
            sourceMatrix.GetValue(ref outDeterminant, 0, 0);
            return 0;
        }
        for (currentColumn = 0; currentColumn < columnCount; ++currentColumn)
        {
            retCode = sourceMatrix.GetValue(ref curIndexValue, 0, currentColumn);
            if (retCode != 0)
            {
                return 1;
            }
            if (GetMinorMatrix(sourceMatrix, 0, currentColumn, ref minorMatrix) != 0)
            {
                return 2;
            }
            if (minorMatrix.GetColumnCount() == 1)
            {
                retCode = minorMatrix.GetValue(ref curIndexMinorDeterminantValue, 0, 0);
                if (retCode != 0)
                {
                    return 1;
                }
            }
            else
            {
                DetermineDeterminant(minorMatrix, ref determinant);
                curIndexMinorDeterminantValue = determinant;
            }
            curIndexValue *= curIndexMinorDeterminantValue;
            if (currentColumn % 2 == 1)
            {
                curIndexValue *= -1;
            }
            outDeterminant += curIndexValue;
        }
        return 0;
    }


    public int GetInverseMatrix(Matrix sourceMatrix, Matrix outMatrix)
    {
        MatrixValue determinant = 0;
        if (DetermineDeterminant(sourceMatrix, ref determinant) != 0)
        {
            return 1;
        }
        outMatrix.CloneRef(outMatrix, sourceMatrix);

        int rowCount = sourceMatrix.GetRowCount(), columnCount = sourceMatrix.GetColumnCount()
        , currentRow, currentColumn;
        MatrixValue retrievedValue = 0, newValue;
        int retCode = 0;
        for (currentRow = 0; currentRow < rowCount; ++currentRow)
        {
            for (currentColumn = 0; currentColumn < columnCount; ++currentColumn)
            {
                retCode = sourceMatrix.GetValue(ref retrievedValue, currentRow, currentColumn);
                if (retCode != 0)
                {
                    return 1;
                }
                newValue = retrievedValue;
                newValue /= determinant;
                outMatrix.SetValue(newValue, currentRow, currentColumn);
            }
        }
        return 0;
    }

    public int MullMatrix(Matrix sourceMatrix, MatrixValue value)
    {
        int rowCount = sourceMatrix.GetRowCount(), columnCount = sourceMatrix.GetColumnCount()
        , currentRow, currentColumn, retCode;
        MatrixValue newValue = 0;
        for (currentRow = 0; currentRow < rowCount; ++currentRow)
        {
            for (currentColumn = 0; currentColumn < columnCount; ++currentColumn)
            {
                retCode = sourceMatrix.GetValue(ref newValue, currentRow, currentColumn);
                if (retCode != 0)
                {
                    return 1;
                }
                newValue *= value;
                sourceMatrix.SetValue(newValue, currentRow, currentColumn);
            }
        }
        return 0;
    }

    private MatrixValue GetRandomMatrixValue(MatrixValue minValue, MatrixValue maxValue)
    {
        return minValue + (GetRandomInstance().NextDouble() * (maxValue - minValue));
    }

    private int GenerateRandomNumber(int minValue, int maxValue)
    {
        return GetRandomInstance().Next(minValue, maxValue + 1);
    }

}

class SmartMatrix : Matrix, ICloneable, IComparable<SmartMatrix> {

    private MatrixMatrixUtils MatrixUtils = new MatrixMatrixUtils();
    public MatrixValue Determinant = 0;

    public SmartMatrix() : base() { }

    SmartMatrix(SmartMatrix basedObject)
    {
        SmartMatrix newObject = this;
        CloneRef(newObject, basedObject);
    }
    public new object Clone()
    {
        SmartMatrix clonedObject;
        CloneOut(out clonedObject);
        return clonedObject;
    }
    public void CloneOut(out SmartMatrix newObject)
    {
        newObject = new SmartMatrix();
        SmartMatrix basedObject = this;
        CloneRef(newObject, basedObject);
    }
    public void CloneRef(SmartMatrix newObject, SmartMatrix basedObject)
    {
        Matrix baseTypeThisObject = this;
        Matrix baseTypeNewObject = newObject;
        base.CloneRef(baseTypeThisObject, baseTypeNewObject);
        newObject.Determinant = basedObject.Determinant;
    }

    public SmartMatrix(MatrixData sourceData) : base(sourceData)
    {
        CalcDeterminant();
    }
    public void CalcDeterminant()
    {
        int retCode = MatrixUtils.DetermineDeterminant(this, ref this.Determinant);
        if (retCode != 0)
        {
            throw new SmartMatrixExceptionCalculating("Ошибка вычисления детерминанта");
        }
    }
    public void FillRandomValues()
    {
        Matrix baseTypeClass = (Matrix)this;
        MatrixUtils.FillRandomValues(baseTypeClass);
        this.CalcDeterminant();
    }

    public double GetDeterminant()
    {
        return Determinant;
    }

    public SmartMatrix GetInverseMatrix()
    {
        Matrix basedTypeObject = this;
        SmartMatrix outSmartMatrix = new SmartMatrix();
        Matrix outMatrix = outSmartMatrix;
        int retCode = MatrixUtils.GetInverseMatrix(basedTypeObject, outMatrix);
        if (retCode != 0)
        {
            throw new SmartMatrixException("Ошибка получения инверсонной матрицы");
        }
        outSmartMatrix.CalcDeterminant();
        return outSmartMatrix;
    }

    public SmartMatrix(IEnumerable<MatrixRow> iList) : base(iList)
    {
        CalcDeterminant();
    }

    public SmartMatrix(IEnumerable<IEnumerable<MatrixValue>> iList) : base(iList)
    {
        CalcDeterminant();
    }

    public static SmartMatrix operator +(SmartMatrix left, SmartMatrix right)
    {
        SmartMatrix returnMatrix = new SmartMatrix(left);
        Matrix leftMatrix = returnMatrix;
        Matrix rightMatrix = right;
        returnMatrix.MatrixUtils.AddMatrix(leftMatrix, rightMatrix);
        return returnMatrix;
    }

    public static SmartMatrix operator *(SmartMatrix left, MatrixValue value)
    {
        SmartMatrix returnMatrix = new SmartMatrix(left);
        Matrix leftMatrix = returnMatrix;
        returnMatrix.MatrixUtils.MullMatrix(leftMatrix, value);
        return returnMatrix;
    }

    public override string ToString()
    {
        string outputString = "";
        this.MatrixUtils.PrintMatrix(this, ref outputString);
        outputString += "Determinant: " + Determinant + "\n";
        return outputString;
    }

    public int CompareTo(SmartMatrix otherMatrix)
    {
        return this.Determinant.CompareTo(otherMatrix.Determinant);
    }

    public override bool Equals(object otherObject)
    {
        if (otherObject == null || GetType() != otherObject.GetType())
        {
            return false;
        }
        return true;
    }

    public bool EqualsValues(object otherObject)
    {
        if (Equals(otherObject))
        {
            SmartMatrix otherMatrix = (SmartMatrix)otherObject;
            if (this.Determinant == otherMatrix.Determinant)
            {
                Matrix thisObjectMatrixType = this;
                Matrix otherObjectMatrixType = otherMatrix;
                if (MatrixUtils.IsMatrixEqual(thisObjectMatrixType, otherObjectMatrixType))
                {
                    return true;
                }
            }
        }
        return false;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public static bool operator >(SmartMatrix left, SmartMatrix right)
    {
        return left.Determinant > right.Determinant;
    }

    public static bool operator <(SmartMatrix left, SmartMatrix right)
    {
        return left.Determinant < right.Determinant;
    }

    public static bool operator ==(SmartMatrix left, SmartMatrix right)
    {
        return left.Determinant == right.Determinant;
    }

    public static bool operator !=(SmartMatrix left, SmartMatrix right)
    {
        return left.Determinant != right.Determinant;
    }

    public static bool operator <=(SmartMatrix left, SmartMatrix right)
    {
        return !(left.Determinant > right.Determinant);
    }

    public static bool operator >=(SmartMatrix left, SmartMatrix right)
    {
        return !(left.Determinant < right.Determinant);
    }
}

public class SmartMatrixException : Exception {
    public SmartMatrixException(string message)
       : base(message) { }
    public override string ToString() {
        return base.ToString();
    }
}
public class SmartMatrixExceptionControl : SmartMatrixException {
    public SmartMatrixExceptionControl(string message)
       : base(message) { }
    public override string ToString() {
        return base.ToString();
    }
}

public class SmartMatrixExceptionCalculating : SmartMatrixException {
    public SmartMatrixExceptionCalculating(string message)
       : base(message) { }
    public override string ToString() {
        return base.ToString();
    }
}