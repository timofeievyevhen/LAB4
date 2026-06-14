using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Lab4_2
{

    public struct Student
    {
        public string LastName;
        public string FirstName;
        public string Patronymic;
        public char Gender;
        public string BirthDate;
        public string MathGrade;
        public string PhysicsGrade;
        public string InformGrade;
        public int Scholarship;
    }

    public class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;

            string filePath = "input.txt";

            List<Student> students = ReadStudentsFromFile(filePath);

            Variant11(students);

            Console.ReadKey();
        }

        public static List<Student> ReadStudentsFromFile(string filePath)
        {
            List<Student> studentList = new List<Student>();

            string[] lines = File.ReadAllLines(filePath, System.Text.Encoding.UTF8);

            foreach (string line in lines)
            {

                string[] tokens = line.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                if (tokens.Length >= 9)
                {
                    Student student = new Student();
                    student.LastName = tokens[0];
                    student.FirstName = tokens[1];
                    student.Patronymic = tokens[2];
                    student.Gender = tokens[3][0];
                    student.BirthDate = tokens[4];
                    student.MathGrade = tokens[5];
                    student.PhysicsGrade = tokens[6];
                    student.InformGrade = tokens[7];
                    student.Scholarship = int.Parse(tokens[8]);

                    studentList.Add(student);
                }
            }

            return studentList;
        }

        public static double ParseGradeForProcessing(string gradeToken)
        {
            if (gradeToken == "-")
            {
                return 2.0;
            }
            if (double.TryParse(gradeToken, out double result))
            {
                return result;
            }
            return 2.0;
        }

        public static void Variant11(List<Student> students)
        {
            Console.WriteLine("Студенти, які мають середній бал більший ніж 4.5:");
            Console.WriteLine($"{"Прізвище",-20} | {"Ім'я",-12} | {"По батькові",-20} | {"Оцінка з фізики",-15}");

            bool found = false;

            foreach (var student in students)
            {
                double math = ParseGradeForProcessing(student.MathGrade);
                double physics = ParseGradeForProcessing(student.PhysicsGrade);
                double inform = ParseGradeForProcessing(student.InformGrade);

                double averageGrade = (math + physics + inform) / 3.0;

                if (averageGrade > 4.5)
                {
                    Console.WriteLine($"{student.LastName,-20} | {student.FirstName,-12} | {student.Patronymic,-20} | {student.PhysicsGrade,-15}");
                    found = true;
                }
            }

            if (!found)
            {
                Console.WriteLine("Студентів із середнім балом > 4.5 не знайдено.");
            }
        }
    }
}