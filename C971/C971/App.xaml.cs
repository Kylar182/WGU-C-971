using System;
using C971.Models.DatabaseModels;
using C971.Models.Enums;
using C971.Services;
using C971.Services.Implementations;
using Plugin.LocalNotifications;
using SQLite;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace C971
{
  public partial class App : Application
  {
    public App(string dbPath)
    {
      InitializeComponent();
      InitializeDB(dbPath);

      DBConn conn = new()
      {
        ConnectionString = dbPath
      };

      DependencyService.RegisterSingleton(conn);
      DependencyService.Register<IAcademicTermService, AcademicTermService>();
      DependencyService.Register<IAssessmentService, AssessmentService>();
      DependencyService.Register<ICourseService, CourseService>();
      DependencyService.Register<IInstructorService, InstructorService>();
      MainPage = new AppShell();
    }

    protected override void OnStart()
    {
    }

    protected override void OnSleep()
    {
    }

    protected override void OnResume()
    {
    }

    private void InitializeDB(string dbPath)
    {
      SQLiteConnection _syncConn = new(dbPath);
      _syncConn.CreateTable<AcademicTerm>();
      _syncConn.CreateTable<Course>();
      _syncConn.CreateTable<Assessment>();
      _syncConn.CreateTable<Instructor>();

      bool Initial = Preferences.Get(nameof(Initial), true);
      if (Initial)
      {
        Initial = false;

        _syncConn.Insert(new AcademicTerm()
        {
          TermTitle = "Spring 2022",
          Start = new DateTime(2022, 1, 01, 12, 0, 0, DateTimeKind.Utc),
          End = new DateTime(2022, 6, 30, 11, 59, 59, DateTimeKind.Utc),
        });

        CrossLocalNotifications.Current.Show("Spring 2022", "Spring 2022 starts today!", 1, 
                            new DateTime(2022, 1, 01, 12, 0, 0, DateTimeKind.Utc));

        _syncConn.Insert(new Instructor()
        {
          Name = "Dr. Jan Hasbro",
          PhoneNumber = "877-435-7948 x6295",
          Email = "Janis.Hasbro@wgu.edu"
        });

        _syncConn.Insert(new Course()
        {
          Name = "D191 - Advanced Data Management",
          Description = "Advanced Data Management enables organizations to extract and analyze raw data. " +
          "Skillful data management allows organizations to discover and explore data in ways that uncover trends, " +
          "issues, and their root causes. In turn, businesses are better equipped to capitalize on opportunities " +
          "and more accurately plan for the future. As organizations continue to extract larger and more detailed " +
          "volumes of data, the need is rapidly growing for IT professionals possessing data management skills. " +
          "These skills include performing advanced relational data modeling as well as designing data marts, " +
          "lakes, and warehouses. This course will empower software developers with the skills to build business " +
          "logic at the database layer to employ more stability and higher data-processing speeds. Data analysts " +
          "will gain the ability to automate common tasks to summarize and integrate data as they prepare it for " +
          "analysis. Data Management is a prerequisite for this course.",
          Notes = "SQL Performance Assessment",
          AcademicTermId = 1,
          InstructorId = 1,
          PerfAssessmentId = 1,
          ObjAssessmentId = 2,
          Start = new DateTime(2022, 1, 1, 12, 0, 0, DateTimeKind.Utc),
          End = new DateTime(2022, 1, 20, 11, 59, 59, DateTimeKind.Utc),
          Status = CourseStatus.Completed
        });

        CrossLocalNotifications.Current.Show("D191 - Advanced Data Management", 
                                                "D191 - Advanced Data Management starts today!", 1, 
                                                        new DateTime(2022, 1, 1, 12, 0, 0, DateTimeKind.Utc));

        _syncConn.Insert(new Assessment()
        {
          Name = "Pre-Assessment: Advanced Data Management (FJO1)",
          CourseId = 1,
          Start = new DateTime(2022, 1, 18, 12, 0, 0, DateTimeKind.Utc),
          End = new DateTime(2022, 1, 17, 12, 0, 0, DateTimeKind.Utc)
        });

        CrossLocalNotifications.Current.Show("Pre-Assessment: Advanced Data Management (FJO1)",
                                                    "Pre-Assessment: Advanced Data Management (FJO1) starts today!", 1,
                                                        new DateTime(2022, 1, 18, 12, 0, 0, DateTimeKind.Utc));

        _syncConn.Insert(new Assessment()
        {
          Name = "Objective Assessment: Advanced Data Management",
          CourseId = 1,
          Start = new DateTime(2022, 1, 19, 12, 0, 0, DateTimeKind.Utc),
          End = new DateTime(2022, 1, 20, 12, 0, 0, DateTimeKind.Utc)
        });

        CrossLocalNotifications.Current.Show("Objective Assessment: Advanced Data Management",
                                                "Objective Assessment: Advanced Data Management starts today!", 2,
                                                  new DateTime(2022, 1, 19, 12, 0, 0, DateTimeKind.Utc));

        _syncConn.Insert(new Instructor()
        {
          Name = "Dr. Clyford Nosborough ",
          PhoneNumber = "877-435-7948 xx8188",
          Email = "Clyford.Nosborough@wgu.edu"
        });

        _syncConn.Insert(new Course()
        {
          Name = "C949 - Data Structures and Algorithms I",
          Description = "Data Structures and Algorithms I covers the fundamentals of dynamic data structures, " +
          "such as bags, lists, stacks, queues, trees, hash tables, and their associated algorithms. With Python " +
          "software as the basis, the course discusses object-oriented design and abstract data types as a design " +
          "paradigm. The course emphasizes problem solving and techniques for designing efficient, maintainable " +
          "software applications. Students will implement simple applications using the techniques learned.",
          AcademicTermId = 1,
          InstructorId = 2,
          PerfAssessmentId = 3,
          ObjAssessmentId = 4,
          Start = new DateTime(2022, 1, 20, 12, 0, 0, DateTimeKind.Utc),
          End = new DateTime(2022, 3, 1, 11, 59, 59, DateTimeKind.Utc),
          Status = CourseStatus.Completed
        });

        CrossLocalNotifications.Current.Show("C949 - Data Structures and Algorithms I",
                                                "C949 - Data Structures and Algorithms I starts today!", 1,
                                                        new DateTime(2022, 1, 20, 12, 0, 0, DateTimeKind.Utc));

        _syncConn.Insert(new Assessment()
        {
          Name = "Pre-Assessment: Data Structures and Algorithms I (GJO1)",
          CourseId = 2,
          Start = new DateTime(2022, 2, 23, 12, 0, 0, DateTimeKind.Utc),
          End = new DateTime(2022, 2, 24, 12, 0, 0, DateTimeKind.Utc)
        });

        CrossLocalNotifications.Current.Show("Pre-Assessment: Data Structures and Algorithms I (GJO1)",
                                                "Pre-Assessment: Data Structures and Algorithms I (GJO1) starts today!", 1,
                                                        new DateTime(2022, 2, 23, 12, 0, 0, DateTimeKind.Utc));

        _syncConn.Insert(new Assessment()
        {
          Name = "Objective Assessment: Data Structures and Algorithms I",
          CourseId = 2,
          Start = new DateTime(2022, 2, 27, 12, 0, 0, DateTimeKind.Utc),
          End = new DateTime(2022, 3, 1, 11, 59, 59, DateTimeKind.Utc)
        });

        CrossLocalNotifications.Current.Show("Objective Assessment: Data Structures and Algorithms I",
                                                "Objective Assessment: Data Structures and Algorithms I starts today!", 1,
                                                        new DateTime(2022, 2, 27, 12, 0, 0, DateTimeKind.Utc));

        _syncConn.Insert(new Instructor()
        {
          Name = "Dr. Greylan Beerman ",
          PhoneNumber = "877-435-7948 8367",
          Email = "Greylan.Beerman@wgu.edu"
        });

        _syncConn.Insert(new Course()
        {
          Name = "C971 - Mobile Application Development Using C#",
          Description = "Mobile Application Development Using C# introduces students to programming for mobile devices. " +
          "Building on students’ previous knowledge of programming in C#, this course investigates Xamarin.Forms and " +
          "how it can be used to build a mobile application. This course explores a broad range of topics, " +
          "including mobile user interface design and development; building applications that adapt to different " +
          "mobile devices and platforms; managing data using a local database; and consuming REST-based web services. " +
          "There are several prerequisites for this course: Software I and II, and UI Design.",
          AcademicTermId = 1,
          InstructorId = 3,
          PerfAssessmentId = 5,
          ObjAssessmentId = 6,
          Start = new DateTime(2022, 3, 1, 12, 0, 0, DateTimeKind.Utc),
          End = new DateTime(2022, 4, 15, 11, 59, 59, DateTimeKind.Utc),
          Status = CourseStatus.InProgress
        });

        CrossLocalNotifications.Current.Show("C971 - Mobile Application Development Using C#",
                                                "C971 - Mobile Application Development Using C# starts today!", 1,
                                                        new DateTime(2022, 3, 1, 12, 0, 0, DateTimeKind.Utc));

        _syncConn.Insert(new Assessment()
        {
          Name = "Performance Assessment: Mobile Application Development Using C# (LAP1)",
          CourseId = 3,
          Start = new DateTime(2022, 4, 11, 12, 0, 0, DateTimeKind.Utc),
          End = new DateTime(2022, 4, 12, 11, 59, 59, DateTimeKind.Utc)
        });

        CrossLocalNotifications.Current.Show("Performance Assessment: Mobile Application Development Using C# (LAP1)",
                                                "Performance Assessment: Mobile Application Development Using C# (LAP1) starts today!", 1,
                                                        new DateTime(2022, 4, 11, 12, 0, 0, DateTimeKind.Utc));

        _syncConn.Insert(new Assessment()
        {
          Name = "Objective Assessment: Mobile Application Development Using C#",
          CourseId = 3,
          Start = new DateTime(2022, 4, 14, 12, 0, 0, DateTimeKind.Utc),
          End = new DateTime(2022, 4, 15, 11, 59, 59, DateTimeKind.Utc)
        });

        CrossLocalNotifications.Current.Show("Objective Assessment: Mobile Application Development Using C#",
                                                "Objective Assessment: Mobile Application Development Using C# starts today!", 1,
                                                        new DateTime(2022, 4, 14, 12, 0, 0, DateTimeKind.Utc));

        _syncConn.Insert(new Instructor()
        {
          Name = "Dr. Andrew Beeman",
          PhoneNumber = "217-549-5995",
          Email = "abeema5@wgu.edu"
        });

        _syncConn.Insert(new Course()
        {
          Name = "C868 - Software Development Capstone",
          Description = "The capstone assessment challenges students to demonstrate mastery of all the " +
          "BSITSW program outcomes. Students will develop a software application to solve a problem of " +
          "their choice constrained only by the technology requirements provided in the assessment DRF.",
          AcademicTermId = 1,
          InstructorId = 4,
          PerfAssessmentId = 7,
          ObjAssessmentId = 8,
          Start = new DateTime(2022, 4, 15, 12, 0, 0, DateTimeKind.Utc),
          End = new DateTime(2022, 6, 30, 11, 59, 59, DateTimeKind.Utc),
          Status = CourseStatus.PlanToTake
        });

        CrossLocalNotifications.Current.Show("C868 - Software Development Capstone",
                                                "C868 - Software Development Capstone# starts today!", 1,
                                                        new DateTime(2022, 4, 15, 12, 0, 0, DateTimeKind.Utc));

        _syncConn.Insert(new Assessment()
        {
          Name = "Performance Assessment: Software Development Capstone (RYM2)",
          CourseId = 4,
          Start = new DateTime(2022, 6, 14, 5, 59, 59, DateTimeKind.Utc),
          End = new DateTime(2022, 6, 15, 5, 59, 59, DateTimeKind.Utc)
        });

        CrossLocalNotifications.Current.Show("Performance Assessment: Software Development Capstone (RYM2)",
                                                "Performance Assessment: Software Development Capstone (RYM2) starts today!", 1,
                                                        new DateTime(2022, 6, 14, 12, 0, 0, DateTimeKind.Utc));

        _syncConn.Insert(new Assessment()
        {
          Name = "Objective Assessment: Software Development Capstone",
          CourseId = 4,
          Start = new DateTime(2022, 6, 28, 12, 0, 0, DateTimeKind.Utc),
          End = new DateTime(2022, 6, 30, 11, 59, 59, DateTimeKind.Utc)
        });


        CrossLocalNotifications.Current.Show("Objective Assessment: Software Development Capstone",
                                                "Objective Assessment: Software Development Capstone starts today!", 1,
                                                        new DateTime(2022, 6, 28, 12, 0, 0, DateTimeKind.Utc));

        Preferences.Set(nameof(Initial), Initial);
      }
    }
  }
}
