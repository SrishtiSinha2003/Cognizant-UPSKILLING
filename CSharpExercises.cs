// ============================================================
// C# and ADO.NET Exercises – All 30 Programs
// ============================================================

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Web;         // for HttpUtility – add reference to System.Web

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("=== C# Exercises ===\n");

        Exercise01_HelloWorld();
        Exercise02_ValueVsReference();
        Exercise03_PrimaryConstructors();
        Exercise04_TypeInference();
        Exercise05_GradeCalculation();
        Exercise06_ArrayLoops();
        Exercise07_MethodOverloading();
        Exercise08_RefOutIn();
        Exercise09_LocalFunctions();
        Exercise10_OOPConstructors();
        Exercise11_AccessModifiers();
        Exercise12_AutoProperties();
        Exercise13_Records();
        Exercise14_Inheritance();
        Exercise15_AbstractAndInterface();
        Exercise16_NullHandling();
        Exercise17_NullConditionalChaining();
        Exercise18_RequiredModifier();
        Exercise19_ListsAndDictionaries();
        Exercise20_LINQ();
        Exercise21_PatternMatching();
        Exercise22_Tuples();
        await Exercise23_AsyncFileUpload();
        Exercise24_JsonSerialization();
        Exercise25_Streams();
        Exercise26_RaceCondition();
        Exercise27_Deadlock();
        Exercise28_TraceLogging();
        Exercise29_SanitizeInput();
        Exercise30_AdoNetCRUD();
    }


    // ── Exercise 1: Hello World ──────────────────────────────
    static void Exercise01_HelloWorld()
    {
        Console.WriteLine("--- Exercise 1: Hello World ---");
        Console.WriteLine("Hello World");
        Console.WriteLine();
    }


    // ── Exercise 2: Value vs Reference Types ────────────────
    static void Exercise02_ValueVsReference()
    {
        Console.WriteLine("--- Exercise 2: Value vs Reference Types ---");

        int num = 10;
        string text = "original";
        var obj = new SimpleData { Value = 100 };

        Console.WriteLine($"Before – num: {num}, text: {text}, obj.Value: {obj.Value}");

        ModifyValue(num);
        ModifyReference(obj);

        Console.WriteLine($"After  – num: {num}, text: {text}, obj.Value: {obj.Value}");
        Console.WriteLine();
    }

    static void ModifyValue(int n) { n = 999; }
    static void ModifyReference(SimpleData d) { d.Value = 999; }

    class SimpleData { public int Value { get; set; } }


    // ── Exercise 3: Primary Constructors (C# 12) ────────────
    static void Exercise03_PrimaryConstructors()
    {
        Console.WriteLine("--- Exercise 3: Primary Constructors ---");
        var person = new PersonPrimary("Alice", 30, "alice@example.com");
        person.DisplayInfo();
        Console.WriteLine();
    }

    class PersonPrimary(string name, int age, string email)
    {
        public string Name  { get; } = name;
        public int    Age   { get; } = age;
        public string Email { get; } = email;

        public void DisplayInfo() =>
            Console.WriteLine($"Name: {Name}, Age: {Age}, Email: {Email}");
    }


    // ── Exercise 4: Type Inference with var and new() ────────
    static void Exercise04_TypeInference()
    {
        Console.WriteLine("--- Exercise 4: Type Inference ---");

        var num    = 42;
        var text   = "Hello";
        var list   = new List<int> { 1, 2, 3 };
        SimpleData obj = new() { Value = 77 };

        Console.WriteLine($"num  ({num.GetType().Name}): {num}");
        Console.WriteLine($"text ({text.GetType().Name}): {text}");
        Console.WriteLine($"list ({list.GetType().Name}): [{string.Join(", ", list)}]");
        Console.WriteLine($"obj  ({obj.GetType().Name}): Value = {obj.Value}");
        Console.WriteLine();
    }


    // ── Exercise 5: Grade Calculation ───────────────────────
    static void Exercise05_GradeCalculation()
    {
        Console.WriteLine("--- Exercise 5: Grade Calculation ---");

        int score = 78;
        Console.WriteLine($"Score: {score}");

        // if-else
        string grade;
        if      (score >= 90) grade = "A";
        else if (score >= 80) grade = "B";
        else if (score >= 70) grade = "C";
        else if (score >= 60) grade = "D";
        else                  grade = "F";
        Console.WriteLine($"Grade (if-else): {grade}");

        // switch with pattern matching
        string gradeSwitch = score switch
        {
            >= 90 => "A",
            >= 80 => "B",
            >= 70 => "C",
            >= 60 => "D",
            _     => "F"
        };
        Console.WriteLine($"Grade (switch):  {gradeSwitch}");
        Console.WriteLine();
    }


    // ── Exercise 6: Array Loops ──────────────────────────────
    static void Exercise06_ArrayLoops()
    {
        Console.WriteLine("--- Exercise 6: Array Loops ---");
        int[] arr = { 1, 2, 3, 4, 5, 6, 7 };

        Console.Write("for:      ");
        for (int i = 0; i < arr.Length; i++)
        {
            if (arr[i] == 5) break;
            Console.Write(arr[i] + " ");
        }
        Console.WriteLine("(stopped at 5)");

        Console.Write("foreach:  ");
        foreach (int n in arr)
        {
            if (n % 2 == 0) continue;
            Console.Write(n + " ");
        }
        Console.WriteLine("(odd only)");

        Console.Write("while:    ");
        int w = 0;
        while (w < arr.Length) { Console.Write(arr[w] + " "); w++; }
        Console.WriteLine();

        Console.Write("do-while: ");
        int d = 0;
        do { Console.Write(arr[d] + " "); d++; } while (d < arr.Length);
        Console.WriteLine("\n");
    }


    // ── Exercise 7: Method Overloading ──────────────────────
    static void Exercise07_MethodOverloading()
    {
        Console.WriteLine("--- Exercise 7: Method Overloading ---");
        Console.WriteLine(CalculateTotal(3, 5));
        Console.WriteLine(CalculateTotal(1.5, 2.5, 3.0));
        Console.WriteLine(CalculateTotal(10, 20, 30));
        Console.WriteLine();
    }

    static int    CalculateTotal(int a, int b)                    => a + b;
    static double CalculateTotal(double a, double b, double c)    => a + b + c;
    static int    CalculateTotal(int a, int b, int c)             => a + b + c;


    // ── Exercise 8: ref, out, in ─────────────────────────────
    static void Exercise08_RefOutIn()
    {
        Console.WriteLine("--- Exercise 8: ref, out, in ---");

        int refVal = 10;
        Console.WriteLine($"Before ref: {refVal}");
        UseRef(ref refVal);
        Console.WriteLine($"After  ref: {refVal}");

        UseOut(out int outVal);
        Console.WriteLine($"out value:  {outVal}");

        int inVal = 50;
        UseIn(in inVal);
        Console.WriteLine($"in value unchanged: {inVal}");
        Console.WriteLine();
    }

    static void UseRef(ref int x) { x *= 2; }
    static void UseOut(out int x) { x = 99; }
    static void UseIn(in int x)   { Console.WriteLine($"  in parameter received: {x}"); }


    // ── Exercise 9: Local Functions ─────────────────────────
    static void Exercise09_LocalFunctions()
    {
        Console.WriteLine("--- Exercise 9: Local Functions ---");
        Console.WriteLine($"5! = {CalculateFactorial(5)}");
        Console.WriteLine();
    }

    static long CalculateFactorial(int n)
    {
        long Factorial(int x) => x <= 1 ? 1 : x * Factorial(x - 1);
        return Factorial(n);
    }


    // ── Exercise 10: OOP Constructors ───────────────────────
    static void Exercise10_OOPConstructors()
    {
        Console.WriteLine("--- Exercise 10: OOP Constructors ---");
        var car1 = new Car();
        var car2 = new Car("Toyota", "Corolla", 2022);
        Console.WriteLine(car1);
        Console.WriteLine(car2);
        Console.WriteLine();
    }

    class Car
    {
        public string Make  { get; set; }
        public string Model { get; set; }
        public int    Year  { get; set; }

        public Car() { Make = "Unknown"; Model = "Unknown"; Year = 2000; }
        public Car(string make, string model, int year) { Make = make; Model = model; Year = year; }

        public override string ToString() => $"{Year} {Make} {Model}";
    }


    // ── Exercise 11: Access Modifiers ───────────────────────
    static void Exercise11_AccessModifiers()
    {
        Console.WriteLine("--- Exercise 11: Access Modifiers ---");
        var obj = new DerivedClass();
        obj.ShowAll();
        Console.WriteLine();
    }

    class BaseClass
    {
        public    string PublicMember    = "public";
        private   string privateMember   = "private";
        protected string protectedMember = "protected";
        public string GetPrivate() => privateMember;
    }

    class DerivedClass : BaseClass
    {
        public void ShowAll()
        {
            Console.WriteLine($"Public:    {PublicMember}");
            Console.WriteLine($"Protected: {protectedMember}");
            Console.WriteLine($"Private (via method): {GetPrivate()}");
        }
    }


    // ── Exercise 12: Auto-Properties and Backing Fields ──────
    static void Exercise12_AutoProperties()
    {
        Console.WriteLine("--- Exercise 12: Auto-Properties ---");
        var p = new Product { Name = "Widget" };
        p.Price = 19.99m;
        Console.WriteLine($"Name: {p.Name}, Price: {p.Price}");

        p.Price = -5;   // validation prevents negative
        Console.WriteLine($"After negative set – Price: {p.Price}");
        Console.WriteLine();
    }

    class Product
    {
        public string Name { get; set; }
        private decimal _price;
        public decimal Price
        {
            get => _price;
            set { if (value >= 0) _price = value; else Console.WriteLine("  Price cannot be negative."); }
        }
    }


    // ── Exercise 13: Records with init ──────────────────────
    static void Exercise13_Records()
    {
        Console.WriteLine("--- Exercise 13: Records ---");
        var emp  = new Employee { Name = "Bob", Department = "IT",    Salary = 60000 };
        var emp2 = emp with { Department = "HR", Salary = 65000 };

        Console.WriteLine($"Original: {emp}");
        Console.WriteLine($"Modified: {emp2}");
        Console.WriteLine();
    }

    record Employee
    {
        public required string Name       { get; init; }
        public required string Department { get; init; }
        public required decimal Salary    { get; init; }
    }


    // ── Exercise 14: Inheritance and Overriding ──────────────
    static void Exercise14_Inheritance()
    {
        Console.WriteLine("--- Exercise 14: Inheritance ---");
        Shape[] shapes = { new Circle(), new Rectangle() };
        foreach (var s in shapes) s.Draw();
        Console.WriteLine();
    }

    class Shape   { public virtual  void Draw() => Console.WriteLine("Drawing a shape"); }
    class Circle    : Shape { public override void Draw() => Console.WriteLine("Drawing a Circle"); }
    class Rectangle : Shape { public override void Draw() => Console.WriteLine("Drawing a Rectangle"); }


    // ── Exercise 15: Abstract Class and Interface ────────────
    static void Exercise15_AbstractAndInterface()
    {
        Console.WriteLine("--- Exercise 15: Abstract Class & Interface ---");
        IDrivable vehicle = new CarDrive();
        vehicle.Start();
        ((Vehicle)vehicle).Drive();
        Console.WriteLine();
    }

    abstract class Vehicle          { public abstract void Drive(); }
    interface IDrivable             { void Start(); }
    class CarDrive : Vehicle, IDrivable
    {
        public override void Drive() => Console.WriteLine("Car is driving.");
        public void Start()          => Console.WriteLine("Car has started.");
    }


    // ── Exercise 16: Null Handling ───────────────────────────
    static void Exercise16_NullHandling()
    {
        Console.WriteLine("--- Exercise 16: Null Handling ---");
        PersonNull? person = null;
        string name = person?.Name ?? "Unknown";
        Console.WriteLine($"Name: {name}");

        person = new PersonNull { Name = "Alice" };
        Console.WriteLine($"Name: {person?.Name ?? "Unknown"}");
        Console.WriteLine();
    }

    class PersonNull { public string? Name { get; set; } }


    // ── Exercise 17: Null-Conditional Chaining ───────────────
    static void Exercise17_NullConditionalChaining()
    {
        Console.WriteLine("--- Exercise 17: Null-Conditional Chaining ---");
        Contact? c1 = null;
        Contact? c2 = new Contact { Name = "Charlie", PhoneNumber = "555-1234" };

        Console.WriteLine($"Contact1 name: {c1?.Name ?? "No contact"}");
        Console.WriteLine($"Contact2 name: {c2?.Name ?? "No contact"}");
        Console.WriteLine();
    }

    class Contact { public string? Name { get; set; } public string? PhoneNumber { get; set; } }


    // ── Exercise 18: required Modifier ──────────────────────
    static void Exercise18_RequiredModifier()
    {
        Console.WriteLine("--- Exercise 18: required Modifier ---");
        // Uncommenting the line below causes a compile-time error (CS9035):
        // var s = new Student();

        var student = new Student { Name = "Diana", StudentId = "S001" };
        Console.WriteLine($"Student: {student.Name}, ID: {student.StudentId}");
        Console.WriteLine();
    }

    class Student
    {
        public required string Name      { get; init; }
        public required string StudentId { get; init; }
    }


    // ── Exercise 19: Lists and Dictionaries ─────────────────
    static void Exercise19_ListsAndDictionaries()
    {
        Console.WriteLine("--- Exercise 19: Lists and Dictionaries ---");

        var cities = new List<string> { "New York", "Chicago", "Los Angeles" };
        cities.Add("Houston");
        cities.Remove("Chicago");
        Console.Write("Cities: ");
        foreach (var c in cities) Console.Write(c + "  ");

        var scores = new Dictionary<int, string> { { 1, "Alice" }, { 2, "Bob" } };
        scores[3] = "Charlie";
        scores.Remove(1);
        Console.WriteLine("\nScores:");
        foreach (var kv in scores) Console.WriteLine($"  {kv.Key}: {kv.Value}");
        Console.WriteLine();
    }


    // ── Exercise 20: LINQ ────────────────────────────────────
    static void Exercise20_LINQ()
    {
        Console.WriteLine("--- Exercise 20: LINQ ---");

        var orders = new List<Order>
        {
            new Order { OrderId = 1, CustomerName = "Alice", TotalAmount = 250 },
            new Order { OrderId = 2, CustomerName = "Bob",   TotalAmount = 75  },
            new Order { OrderId = 3, CustomerName = "Carol", TotalAmount = 500 },
        };

        var result = orders
            .Where(o => o.TotalAmount > 100)
            .Select(o => new { o.OrderId, o.CustomerName, o.TotalAmount });

        foreach (var r in result)
            Console.WriteLine($"  Order {r.OrderId}: {r.CustomerName} – ${r.TotalAmount}");
        Console.WriteLine();
    }

    class Order { public int OrderId { get; set; } public string CustomerName { get; set; } public decimal TotalAmount { get; set; } }


    // ── Exercise 21: Pattern Matching ───────────────────────
    static void Exercise21_PatternMatching()
    {
        Console.WriteLine("--- Exercise 21: Pattern Matching ---");
        CheckType(42);
        CheckType("hello");
        CheckType(3.14);
        CheckType(new Car("Honda", "Civic", 2021));
        Console.WriteLine();
    }

    static void CheckType(object obj)
    {
        string result = obj switch
        {
            int    n => $"Integer: {n}",
            string s => $"String: {s}",
            double d => $"Double: {d}",
            Car    c => $"Car: {c}",
            _        => "Unknown type"
        };
        Console.WriteLine("  " + result);
    }


    // ── Exercise 22: Tuples ──────────────────────────────────
    static void Exercise22_Tuples()
    {
        Console.WriteLine("--- Exercise 22: Tuples ---");
        var (id, name) = GetUserInfo();
        Console.WriteLine($"Id: {id}, Name: {name}");
        Console.WriteLine();
    }

    static (int Id, string Name) GetUserInfo() => (101, "Eve");


    // ── Exercise 23: Async File Upload ──────────────────────
    static async Task Exercise23_AsyncFileUpload()
    {
        Console.WriteLine("--- Exercise 23: Async File Upload ---");
        try
        {
            string result = await SimulateUploadAsync("report.pdf");
            Console.WriteLine(result);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Upload failed: {ex.Message}");
        }
        Console.WriteLine();
    }

    static async Task<string> SimulateUploadAsync(string fileName)
    {
        await Task.Delay(3000);
        return $"'{fileName}' uploaded successfully.";
    }


    // ── Exercise 24: JSON Serialization ─────────────────────
    static void Exercise24_JsonSerialization()
    {
        Console.WriteLine("--- Exercise 24: JSON Serialization ---");
        var user = new UserJson { Name = "Frank", Age = 28, Email = "frank@example.com" };

        string json = JsonSerializer.Serialize(user, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText("user.json", json);
        Console.WriteLine("Serialized JSON:\n" + json);

        string fileContent = File.ReadAllText("user.json");
        var loaded = JsonSerializer.Deserialize<UserJson>(fileContent);
        Console.WriteLine($"Deserialized – Name: {loaded!.Name}, Age: {loaded.Age}, Email: {loaded.Email}");
        Console.WriteLine();
    }

    class UserJson { public string Name { get; set; } public int Age { get; set; } public string Email { get; set; } }


    // ── Exercise 25: FileStream and MemoryStream ─────────────
    static void Exercise25_Streams()
    {
        Console.WriteLine("--- Exercise 25: Streams ---");

        // Write a test file first
        File.WriteAllText("test.txt", "Hello from FileStream!");

        // FileStream – read
        using var fs = new FileStream("test.txt", FileMode.Open, FileAccess.Read);
        var buffer = new byte[fs.Length];
        fs.Read(buffer, 0, buffer.Length);
        Console.WriteLine("FileStream read: " + Encoding.UTF8.GetString(buffer));

        // MemoryStream – write
        using var ms = new MemoryStream();
        byte[] data = Encoding.UTF8.GetBytes("MemoryStream data");
        ms.Write(data, 0, data.Length);
        Console.WriteLine($"MemoryStream bytes written: {ms.Length}");
        Console.WriteLine();
    }


    // ── Exercise 26: Race Condition ──────────────────────────
    static void Exercise26_RaceCondition()
    {
        Console.WriteLine("--- Exercise 26: Race Condition ---");
        var counter = new SafeCounter();
        var threads = new Thread[5];
        for (int i = 0; i < threads.Length; i++)
            threads[i] = new Thread(() => { for (int j = 0; j < 100; j++) counter.Increment(); });

        foreach (var t in threads) t.Start();
        foreach (var t in threads) t.Join();
        Console.WriteLine($"Expected: 500, Got: {counter.Value}");
        Console.WriteLine();
    }

    class SafeCounter
    {
        private int _value;
        private readonly object _lock = new();
        public void Increment() { lock (_lock) { _value++; } }
        public int Value => _value;
    }


    // ── Exercise 27: Deadlock Simulation and Resolution ──────
    static void Exercise27_Deadlock()
    {
        Console.WriteLine("--- Exercise 27: Deadlock Resolution ---");
        var lock1 = new object();
        var lock2 = new object();

        var t1 = new Thread(() =>
        {
            if (Monitor.TryEnter(lock1, 500))
            {
                try
                {
                    Thread.Sleep(100);
                    if (Monitor.TryEnter(lock2, 500))
                    {
                        try   { Console.WriteLine("Thread 1 acquired both locks."); }
                        finally { Monitor.Exit(lock2); }
                    }
                    else Console.WriteLine("Thread 1 could not acquire lock2 – deadlock avoided.");
                }
                finally { Monitor.Exit(lock1); }
            }
        });

        var t2 = new Thread(() =>
        {
            if (Monitor.TryEnter(lock2, 500))
            {
                try
                {
                    Thread.Sleep(100);
                    if (Monitor.TryEnter(lock1, 500))
                    {
                        try   { Console.WriteLine("Thread 2 acquired both locks."); }
                        finally { Monitor.Exit(lock1); }
                    }
                    else Console.WriteLine("Thread 2 could not acquire lock1 – deadlock avoided.");
                }
                finally { Monitor.Exit(lock2); }
            }
        });

        t1.Start(); t2.Start();
        t1.Join();  t2.Join();
        Console.WriteLine();
    }


    // ── Exercise 28: Trace Logging ───────────────────────────
    static void Exercise28_TraceLogging()
    {
        Console.WriteLine("--- Exercise 28: Trace Logging ---");
        var listener = new TextWriterTraceListener("app.log");
        Trace.Listeners.Add(listener);
        Trace.AutoFlush = true;

        Trace.WriteLine("Application started.");
        Trace.WriteLine("User logged in.");
        Trace.WriteLine("Application stopped.");

        Console.WriteLine("Log written to app.log");
        Trace.Listeners.Remove(listener);
        listener.Close();
        Console.WriteLine();
    }


    // ── Exercise 29: Sanitize Input / Prevent XSS ────────────
    static void Exercise29_SanitizeInput()
    {
        Console.WriteLine("--- Exercise 29: Input Sanitization ---");
        string malicious = "<script>alert('xss')</script>Hello";
        string safe      = System.Net.WebUtility.HtmlEncode(malicious);
        Console.WriteLine($"Raw input:       {malicious}");
        Console.WriteLine($"Sanitized input: {safe}");
        Console.WriteLine();
    }


    // ── Exercise 30: ADO.NET CRUD ────────────────────────────
    static void Exercise30_AdoNetCRUD()
    {
        Console.WriteLine("--- Exercise 30: ADO.NET CRUD ---");

        // Replace with your actual connection string
        string connStr = "Server=localhost;Database=TestDB;Integrated Security=True;";

        try
        {
            using var conn = new SqlConnection(connStr);
            conn.Open();

            // CREATE table if not exists
            var createCmd = new SqlCommand(@"
                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Employees' AND xtype='U')
                CREATE TABLE Employees (
                    Id   INT IDENTITY PRIMARY KEY,
                    Name VARCHAR(100) NOT NULL,
                    Role VARCHAR(100) NOT NULL
                );", conn);
            createCmd.ExecuteNonQuery();

            // INSERT
            var insertCmd = new SqlCommand(
                "INSERT INTO Employees (Name, Role) VALUES (@Name, @Role)", conn);
            insertCmd.Parameters.AddWithValue("@Name", "Alice");
            insertCmd.Parameters.AddWithValue("@Role", "Developer");
            insertCmd.ExecuteNonQuery();
            Console.WriteLine("Inserted: Alice – Developer");

            // READ
            var readCmd = new SqlCommand("SELECT Id, Name, Role FROM Employees", conn);
            using var reader = readCmd.ExecuteReader();
            Console.WriteLine("Employees:");
            while (reader.Read())
                Console.WriteLine($"  {reader["Id"]}: {reader["Name"]} – {reader["Role"]}");
            reader.Close();

            // UPDATE
            var updateCmd = new SqlCommand(
                "UPDATE Employees SET Role = @Role WHERE Name = @Name", conn);
            updateCmd.Parameters.AddWithValue("@Role", "Senior Developer");
            updateCmd.Parameters.AddWithValue("@Name", "Alice");
            updateCmd.ExecuteNonQuery();
            Console.WriteLine("Updated Alice to Senior Developer");

            // DELETE
            var deleteCmd = new SqlCommand(
                "DELETE FROM Employees WHERE Name = @Name", conn);
            deleteCmd.Parameters.AddWithValue("@Name", "Alice");
            deleteCmd.ExecuteNonQuery();
            Console.WriteLine("Deleted Alice");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"DB Error: {ex.Message}");
            Console.WriteLine("(Update the connection string in Exercise30 to run ADO.NET queries)");
        }

        Console.WriteLine();
    }
}
