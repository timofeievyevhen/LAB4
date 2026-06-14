using MyTime = (int hour, int min, int sec);

namespace ConsoleApp7
{
    internal class Program
    {

        static int SecInDay = 86400;
        public static string MyTimeToString(MyTime t)
        {
            return $"{t.hour}:{t.min:D2}:{t.sec:D2}";
        }

        public static MyTime Normalize(MyTime t)
        {
            int totalSec = ToSecSinceMidnight(t);
            return FromSecSinceMidnight(totalSec);
        }

        public static int ToSecSinceMidnight(MyTime t)
        {
            return t.hour * 3600 + t.min * 60 + t.sec;
        }

        public static MyTime FromSecSinceMidnight(int t)
        {
            t %= SecInDay;
            if (t < 0) t += SecInDay;

            int h = t / 3600;
            int m = (t / 60) % 60;
            int s = t % 60;

            return (h, m, s);
        }

        public static MyTime AddOneSecond(MyTime t) 
        {
            return AddSeconds(t, 1);
        }
        public static MyTime AddOneMinute(MyTime t) 
        {
            return AddSeconds(t, 60);
        } 
        public static MyTime AddOneHour(MyTime t)  
        { 
            return AddSeconds(t, 3600); 
        }

        public static MyTime AddSeconds(MyTime t, int s)
        {
            int totalSec = ToSecSinceMidnight(t) + s;
            return FromSecSinceMidnight(totalSec);
        }

        public static int Difference(MyTime t1, MyTime t2)
        {
            int a = ToSecSinceMidnight(t1);
            int b = ToSecSinceMidnight(t2);

            return a - b;
        }

        public static string WhatLesson(MyTime t)
        {
            int currentSec = ToSecSinceMidnight(Normalize(t));

            var schedule = new (int start, int end, string name)[]
            {
            (ToSec(8, 30, 0),  ToSec(9, 50, 0),  "1-а пара"),
            (ToSec(9, 50, 0),  ToSec(10, 0, 0),  "перерва між 1-ю та 2-ю парами"),
            (ToSec(10, 0, 0),  ToSec(11, 20, 0), "2-а пара"),
            (ToSec(11, 20, 0), ToSec(11, 40, 0), "перерва між 2-ю та 3-ю парами"),
            (ToSec(11, 40, 0), ToSec(13, 0, 0),  "3-я пара"),
            (ToSec(13, 0, 0),  ToSec(13, 20, 0), "перерва між 3-ю та 4-ю парами"),
            (ToSec(13, 20, 0), ToSec(14, 40, 0), "4-а пара"),
            (ToSec(14, 40, 0), ToSec(14, 50, 0), "перерва між 4-ю та 5-ю парами"),
            (ToSec(14, 50, 0), ToSec(16, 10, 0), "5-а пара"),
            (ToSec(16, 10, 0), ToSec(16, 20, 0), "перерва між 5-ю та 6-ю парами"),
            (ToSec(16, 20, 0), ToSec(17, 40, 0), "6-а пара")
            };

            if (currentSec < schedule[0].start) return "пари ще не почалися";

            foreach (var slot in schedule)
            {
                if (currentSec >= slot.start && currentSec < slot.end)
                    return slot.name;
            }

            return "пари вже скінчилися";

            int ToSec(int h, int m, int s) => h * 3600 + m * 60 + s;
        }
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;
            int choise = 1;
            do
            {
                Console.WriteLine("MyTime");
                Console.Write("Введіть час (hh:mm:ss): ");
                string[] input = Console.ReadLine().Trim().Split(':');
                int h = int.Parse(input[0]);
                int m = int.Parse(input[1]);
                int s = int.Parse(input[2]);

                MyTime userTime = (h, m, s);

                Console.WriteLine($"Рядкове подання (як є): {MyTimeToString(userTime)}");

                MyTime normalized = Normalize(userTime);
                Console.WriteLine($"Нормалізований час: {MyTimeToString(normalized)}");

                int totalSeconds = ToSecSinceMidnight(normalized);
                Console.WriteLine($"Секунд від початку доби: {totalSeconds}");

                Console.WriteLine($"Додали 1 секунду: {MyTimeToString(AddOneSecond(normalized))}");

                Console.WriteLine($"Додали 1 хвилину: {MyTimeToString(AddOneMinute(normalized))}");

                Console.WriteLine($"Додали 1 годину: {MyTimeToString(AddOneHour(normalized))}");

                Console.Write("Введіть скільки секунд додати: ");
                int sec = int.Parse(Console.ReadLine());
                MyTime afterAdding = AddSeconds(normalized, sec);
                Console.WriteLine($"Час після додавання секунд: {MyTimeToString(afterAdding)}");

                Console.Write("Введіть другий час для порівняння (hh:mm:ss): ");
                string[] input2 = Console.ReadLine().Trim().Split(':');
                int h2 = int.Parse(input2[0]);
                int m2 = int.Parse(input2[1]);
                int s2 = int.Parse(input2[2]);

                MyTime userTime2 = (h2, m2, s2);
                MyTime normalized2 = Normalize(userTime2);

                int diff = Difference(normalized, normalized2);
                Console.WriteLine($"Різниця між першим і другим часом: {diff} сек.");


                string lessons = WhatLesson(normalized);
                Console.WriteLine($"За розкладом: {lessons}");

                Console.WriteLine("Продовжуємо?(введыть 0 щоб припинити)");
                choise = int.Parse(Console.ReadLine());


            } while (choise != 0);


        }
    }
}
