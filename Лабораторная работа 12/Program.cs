using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

namespace Лабораторная_работа_12
{
    class Program
    {
        static void Main(string[] args)
        {
            Airline airline = new Airline();
            Airoport port = new Airoport("Минск-2");
            IPOP_NAOBOROT port1 = new Airoport("Минск-1");
            port1.show();
            //a
            Reflector.AssName(airline);
            Reflector.AssName(port);
            //b
            Reflector.PublicConstructorsInfo(airline);
            Reflector.PublicConstructorsInfo(port);
            //c
            IEnumerable<string> air_method_info = Reflector.CommonAvaliableMethods(airline);
            IEnumerable<string> port_method_info = Reflector.CommonAvaliableMethods(port);
            Console.WriteLine("\n\n");
            foreach (string el in air_method_info)
                Console.WriteLine(el);
            Console.WriteLine("\n\n");
            foreach (string el in port_method_info)
                Console.WriteLine(el);
            //d
            IEnumerable<string> air_method_and_prop = Reflector.InfoAboutMethodsAndProperties(airline);
            IEnumerable<string> port_method_and_prop = Reflector.InfoAboutMethodsAndProperties(port);
            foreach (string el in air_method_and_prop)
                Console.WriteLine(el);
            Console.WriteLine("\n\n");
            foreach (string el in port_method_and_prop)
                Console.WriteLine(el);
            Console.WriteLine("\n\n");
            //e            
            IEnumerable<string> air_interfaces = Reflector.InfoAboutInterfaces(airline);
            IEnumerable<string> port_interfaces = Reflector.InfoAboutInterfaces(port);
            foreach (string el in air_interfaces)
                Console.WriteLine(el);
            Console.WriteLine("\n\n");
            foreach (string el in port_interfaces)
                Console.WriteLine(el);
            Console.WriteLine("\n\n");
            //f
            IEnumerable<string> air_param_methods = Reflector.MethodsWithUserParametrsInfo(airline);
            IEnumerable<string> port_param_methods = Reflector.MethodsWithUserParametrsInfo(port);
            foreach (string el in air_param_methods)
                Console.WriteLine(el);
            Console.WriteLine("\n\n");
            foreach (string el in port_param_methods)
                Console.WriteLine(el);
            Console.WriteLine("\n\n");
            // g           
            StreamReader reader = new StreamReader("Reflector.txt");
            string fname = reader.ReadLine();
            string values = reader.ReadLine();
            Reflector.Invoke(typeof(Reflector), fname, (typeof(string), values));
            reader.Close();

            // 2

             var someClass = Reflector.Create<Airline>();
             someClass.showObjectInfo();
           
        }
        
    }  

    public static class Reflector
    {
       
        public static void AssName(object tested_class)
        {
            Type type = tested_class.GetType();
            StreamWriter stream = new StreamWriter($"Информация о классе {type.Name}.txt");
            stream.WriteLine($"===>Основная информация<===");
            stream.WriteLine($"Полное имя класса: {type.FullName} ");
            stream.WriteLine($"Информация о сборке: {type.Assembly} ");
            stream.WriteLine($"Globally Unique Identifier: {type.GUID}");
            stream.Close();
            
        }

        public static void PublicConstructorsInfo(object tested_class)
        {
            Type type = tested_class.GetType();
            StreamWriter stream = new StreamWriter($"Публичные конструкторы класса {type.Name}.txt");
            stream.WriteLine($"===>Основная информация<===");
            foreach (MemberInfo el in type.GetConstructors())
            {
                stream.WriteLine($" {el} ");
            }         
            stream.Close();
        }

        public static IEnumerable<string> CommonAvaliableMethods(object tested_class)
        {
            Type type = tested_class.GetType();
            StreamWriter stream = new StreamWriter($"Общедоступные методы класса {type.Name}.txt");
            stream.WriteLine($"===>Основная информация<===");
            IEnumerable<string> AvaliableMethods = type.GetMethods().Select(n => Convert.ToString(n));
            foreach (MemberInfo el in type.GetMethods())
            {
                stream.WriteLine($" {el} ");
            }
            stream.Close();

            return AvaliableMethods;
        }

        public static IEnumerable<string> InfoAboutMethodsAndProperties(object tested_class)
        {
            Type type = tested_class.GetType();
            StreamWriter stream = new StreamWriter($"Методы и свойства класса {type.Name}.txt");
            stream.WriteLine($"===>Информация о методах и свойствах класса  {type.Name} <===");
            foreach (MemberInfo el in type.GetMembers())
                stream.WriteLine(el);
            stream.Close();

            IEnumerable<string> MethodsAndProperties = type.GetMembers().Select(n => Convert.ToString(n));
            return MethodsAndProperties;

        }

        public static IEnumerable<string> InfoAboutInterfaces(object tested_class)
        {
            Type type = tested_class.GetType();
            StreamWriter stream = new StreamWriter($"Интерфейсы класса {type.Name}.txt");
            stream.WriteLine($"===>Интерфейсы класса {type.Name} <===");
            foreach (MemberInfo el in type.GetInterfaces())
                stream.WriteLine(el);
            stream.Close();

            IEnumerable<string> Interfaces = type.GetInterfaces().Select(n => Convert.ToString(n));
            return Interfaces;

        }

        public static IEnumerable<string> MethodsWithUserParametrsInfo(object tested_class)
        {
            Type type = tested_class.GetType();
            MethodInfo[] methodInfo = type.GetMethods();
            StreamWriter stream = new StreamWriter($"Методы с парам класса {type.Name}.txt");
            stream.WriteLine($"===>Основная информация<===");            
            IEnumerable<string> AvaliableMethods = methodInfo.Where(n => n.GetParameters().Length != 0).Select(n => Convert.ToString(n));
            foreach (MethodInfo el in type.GetMethods())
            {
                if (el.GetParameters().Length != 0)
                stream.WriteLine($" {el} ");
            }
            stream.Close();

            return AvaliableMethods;
        }

        public static void Invoke(Type type, string methodName, params (Type type, object value)[] paramTuples)
        {

            var paramsTypes = paramTuples.Select(item => item.type).ToArray();
            var paramsValues = paramTuples.Select(item => item.value).ToArray();
            var method = type.GetMethod(methodName, paramsTypes);
            if (method == null)
            {
                Console.WriteLine("Метод не найден");
                return;
            }
            method.Invoke(null, paramsValues);
        }
        public static void write(string str)
        {
            Console.WriteLine(str);
        }

        public static UserType Create<UserType>()
        {
            var type = typeof(UserType);
            Type[] types = new Type[0];
            var constructor = type.GetConstructor(types);
            return (UserType)constructor.Invoke(null);
        }

    }

    interface IPOP_NAOBOROT
    {
        void show();
    }
    public class Airoport : IPOP_NAOBOROT
    {
        string City;

        void IPOP_NAOBOROT.show()
        {
            Console.WriteLine($"Аэропорт находится в городе {City}");
        }
        public string city
        {
            get { return City; }
        }

        public void show(Airoport kek)
        {
            Console.WriteLine(kek.City);
        }
        public Airoport(string city) { City = city; }
    }

    public partial class Airline
    {
        private static int CurrentSize = 0;
        private const int ArchiveMaxSize = 100;
        private DateTime date = new DateTime(2020, 12, 04, 0, 0, 0);

        public DateTime Date
        {
            get
            {
                return date;
            }
        }


        private int dist;
        public int Dist
        {
            set
            {
                dist = value;
            }
            get
            {
                return dist;
            }
        }


        private String Punct;
        public String punct
        {
            set
            {
                Punct = value;
            }
            get
            {
                return Punct;
            }
        }

        private int flightNumber;
        public int FlightNumber
        {
            private set
            {

                flightNumber = value;

            }
            get { return flightNumber; }
        }
        private String planeType;

        public String PlaneType
        {
            set
            {
                planeType = value;
            }
            get
            {
                return planeType;
            }
        }

        private String depatureTime;

        public String DepartureTime
        {
            set
            {
                depatureTime = value;
            }
            get
            {
                return depatureTime;
            }
        }

        private String WeekDay;

        public String weekDay
        {
            set
            {
                WeekDay = value;
            }
            get
            {
                return WeekDay;
            }
        }


        private const String developer = "Palevoda POIT-4";


        Airline[] Archive = new Airline[ArchiveMaxSize];

        static string status = "Стркутура структура не использовалась";            //обычный конструктор
        static Airline()
        {
            status = "Был использован приватный конструктор";
        }

        public override bool Equals(object obj)
        {
            Airline flight = (Airline)obj;

            if (this.flightNumber == flight.flightNumber)
                return true;
            else return false;
        }

        public override string ToString()
        {
            return "Номер рейса: " + this.flightNumber + ", Пункт назначения: " + this.Punct + ", Время вылета: " + this.Date + " День вылета: " + this.weekDay + " дальность полета " + this.dist;
        }


        private int upgradeHashCodeRef(ref int hash)
        {
            hash /= 1000;
            return hash;
        }

        private void upgradeHashCodeOut(out int hash)
        {

            hash = 123456;
        }


        public Airline() { punct = "Неизвестно"; flightNumber = 0; planeType = "Неизвестно"; depatureTime = "Неизвестно"; weekDay = "Неизвестно"; CurrentSize++; }
        public Airline(String Punct, String DepatureTime) { punct = Punct; flightNumber = 0; planeType = "Неизвестно"; depatureTime = DepatureTime; weekDay = "Неизвестно"; CurrentSize++; }
        public Airline(String newPunct, ushort newFlightNumber, String newPlaneType, String newdepatureTime, String newWeekDay, int newdist)
        {
            punct = newPunct;
            //flightNumber = newFlightNumber; 
            this.flightNumber = GetHashCode();
            planeType = newPlaneType;
            depatureTime = newdepatureTime;
            weekDay = newWeekDay;
            //upgradeHashCodeOut(out this.flightNumber);
            upgradeHashCodeRef(ref this.flightNumber);
            dist = newdist;
            CurrentSize++;
            Random rand = new Random();
            date = date.AddHours(Convert.ToDouble(rand.Next(0, 24)));
            date = date.AddMinutes(Convert.ToDouble(rand.Next(0, 60)));
        }

        public void showObjectInfo()
        {
            Console.WriteLine("\n========================================");
            Console.WriteLine("::" + Airline.status);
            Console.WriteLine($"\nПункт назначения: {punct}  \nНомер рейса: {flightNumber} \nТип рейса: { planeType} \nВремя отправления: {date} \nДень отправления: {weekDay} \nДальность полета: {dist} ");
            Console.WriteLine($"=============={CurrentSize}==================");
        }

    }
}


