# MethodOfOptimalSetPartitioning
Опис проекту

Цей проект реалізує алгоритм розв'язання нелінійної задачі оптимального розбиття множин з відшуканням центрів підмножин за допомогою мови програмування C# та бібліотек System.Diagnostics, MathNet.Numerics.LinearAlgebra, і System.Windows.Forms.
Використані технології

    C# - об’єктно-орієнтована мова програмування зі строгою типізацією, використовується для розробки програм різного рівня складності.
    System.Diagnostics - бібліотека для проведення тестів, що використовується для фіксації швидкості роботи алгоритму.
    MathNet.Numerics.LinearAlgebra - бібліотека для роботи з лінійною алгеброю, підтримує операції з матрицями та векторами.
    System.Windows.Forms - простір імен для створення графічних інтерфейсів користувача (GUI) у настільних застосунках.

Структура проекту
CalculableMetric

    ChebyshevDistanceCalculator.cs - клас для обчислення відстані Чебишева.
    EuclideanDistanceCalculator.cs - клас для обчислення евклідової відстані.
    ManhattanDistanceCalculator.cs - клас для обчислення відстані Манхеттена.

Functionals

    AbstractFunctionalContainer.cs - абстрактний клас для роботи з функціоналами.
    FunctionalContainer.cs - реалізація абстрактного класу для створення функціонала.

Helpers

    CreateImageOfPartitional.cs - клас для створення та збереження зображень областей, розбитих на функціональні частини.
    VectorDimentions.cs - клас для представлення векторів з координатами точок у двовимірному просторі.

StepMethods

    AbstractStepMethod.cs - абстрактний клас для обчислення кроку у процесі мінімізації функціонала.
    DefaultMaxStep.cs - реалізація методу для максимального кроку.
    DefaultMinStep.cs - реалізація методу для мінімального кроку.

MinimizationsAlgoritms

    RAlgoritm.cs - клас для розв'язання задачі мінімізації.
    RData.cs - клас для зберігання результатів ітерацій алгоритму мінімізації.
    StepData.cs - клас для зберігання інформації про кожний крок у процесі мінімізації.

Shapes

    ShapeRectangle.cs - клас для представлення прямокутної області, розділеної на сітку з певним кроком.

Interfaces

    ICalculableMetric.cs - інтерфейс для обчислення метрик.

Опис основних класів та методів
ICalculableMetric.cs

Інтерфейс визначає методи для обчислення метрик:

    CalculeteFunction((double X, double Y) tau, (double X, double Y) point) - метод для обчислення функції метрики між двома точками.
    CalculeteNorm(Vector<double> vector) - метод для обчислення норми вектора.

FunctionalContainer.cs

Реалізація абстрактного класу для створення функціонала:

    CalculateFunctionale(Vector<double> vector) - обчислює значення функціонала для заданого вектора.
    CalculateSubgradientFunctionale(Vector<double> vector) - обчислює субградієнт функціонала для заданого вектора.

CreateImageOfPartitional.cs

Клас для створення та збереження зображень областей, розбитих на функціональні частини:

    GetBitmapOfPartitional(FunctionalContainer container, Vector<double> vector, double size, Vector<double> centers) - створює та повертає зображення області, розбитої на функціональні частини.
    SaveImageUseBitmap(Bitmap bitmap, string name, string directoryPath = @"Fotos") - зберігає зображення у вказаному каталозі.

RAlgoritm.cs

Клас для розв'язання задачі мінімізації:

    SolveMinizationTask - основний метод для розв'язання задачі мінімізації функціонала.



Цей проект надає потужний інструментарій для розв'язання задач оптимального розбиття множин з відшуканням центрів підмножин. Використовуючи різноманітні методи та алгоритми, він дозволяє ефективно виконувати задачі мінімізації, обчислення метрик та візуалізації результатів.
