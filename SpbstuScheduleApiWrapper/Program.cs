using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace SpbstuScheduleApiWrapper
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.Write("group id: ");
            string groupid = Console.ReadLine();
            Console.Write("date (year-month-day): ");
            string date = Console.ReadLine();
            using (HttpClient client = new HttpClient())
            {
                var response = client.GetAsync(string.Format("https://ruz.spbstu.ru/api/v1/ruz/scheduler/{0}?date={1}", groupid, date)).Result;
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var buffer = response.Content.ReadAsByteArrayAsync().Result;
                    var strings = Encoding.UTF8.GetString(buffer);
                    ApiResponse res = JsonConvert.DeserializeObject<ApiResponse>(strings);
                    foreach (var day in res.Days)
                    {
                        Console.WriteLine("date: " + day.Date);
                        foreach (var lesson in day.Lessons)
                        {
                            Console.WriteLine("Lesson:" + lesson.Subject);
                            var auditory = lesson.Auditories.FirstOrDefault();
                            if (auditory != null)
                            {
                                Console.WriteLine("Auditory: " + auditory.Name + ", building: " + auditory.Building.Name);
                            }
                        }
                    }
                }
                else
                {
                    throw new Exception();
                }
            }
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }

    public partial class ApiResponse
    {
        public Week Week { get; set; }
        public List<Day> Days { get; set; }
        public Group Group { get; set; }
    }

    public partial class Day
    {
        public long Weekday { get; set; }
        public DateTimeOffset Date { get; set; }
        public List<Lesson> Lessons { get; set; }
    }

    public partial class Lesson
    {
        public string Subject { get; set; }
        public string SubjectShort { get; set; }
        public long Type { get; set; }
        public string AdditionalInfo { get; set; }
        public string TimeStart { get; set; }
        public string TimeEnd { get; set; }
        public long Parity { get; set; }
        public Faculty TypeObj { get; set; }
        public List<Group> Groups { get; set; }
        public List<Teacher> Teachers { get; set; }
        public List<Auditory> Auditories { get; set; }
        public string WebinarUrl { get; set; }
        public string LmsUrl { get; set; }
    }

    public partial class Auditory
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public Faculty Building { get; set; }
    }

    public partial class Faculty
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Abbr { get; set; }
        public string Address { get; set; }
    }

    public partial class Group
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long Level { get; set; }
        public string Type { get; set; }
        public long Kind { get; set; }
        public string Spec { get; set; }
        public long Year { get; set; }
        public Faculty Faculty { get; set; }
    }

    public partial class Teacher
    {
        public long Id { get; set; }
        public long Oid { get; set; }
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Grade { get; set; }
        public string Chair { get; set; }
    }

    public partial class Week
    {
        public string DateStart { get; set; }
        public string DateEnd { get; set; }
        public bool IsOdd { get; set; }
    }
}
