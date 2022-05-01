﻿using System;
using C971.Models.DatabaseModels;
using C971.Models.Enums;
using C971.Services;
using C971.Services.Implementations;
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

      bool Initial = Preferences.Get(nameof(Initial), true);
      if (Initial)
      {
        _syncConn.CreateTable<AcademicTerm>();
        _syncConn.CreateTable<Course>();
        _syncConn.CreateTable<Assessment>();
        _syncConn.CreateTable<Instructor>();
        _syncConn.CreateTable<Notification>();

        Initial = false;

        AcademicTerm term = new()
        {
          TermTitle = "Spring 2022",
          Start = new DateTime(2022, 1, 01, 12, 0, 0, DateTimeKind.Utc),
          End = new DateTime(2022, 6, 30, 11, 59, 59, DateTimeKind.Utc),
          NotificationId = 1
        };

        _syncConn.Insert(term);
        _syncConn.Insert(term.Notification());

        _syncConn.Insert(new Instructor()
        {
          Name = "Dr. Jan Hasbro",
          PhoneNumber = "877-435-7948 x6295",
          Email = "Janis.Hasbro@wgu.edu"
        });

        Course D191 = new()
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
          Status = CourseStatus.Completed,
          NotificationId = 2
        };

        _syncConn.Insert(D191);
        _syncConn.Insert(D191.Notification());

        Assessment paADM = new()
        {
          Name = "Pre-Assessment: Advanced Data Management (FJO1)",
          CourseId = 1,
          Start = new DateTime(2022, 1, 18, 12, 0, 0, DateTimeKind.Utc),
          End = new DateTime(2022, 1, 17, 12, 0, 0, DateTimeKind.Utc),
          NotificationId = 3
        };

        _syncConn.Insert(paADM);
        _syncConn.Insert(paADM.Notification());

        Assessment oaDM = new()
        {
          Name = "Objective Assessment: Advanced Data Management",
          CourseId = 1,
          Start = new DateTime(2022, 1, 19, 12, 0, 0, DateTimeKind.Utc),
          End = new DateTime(2022, 1, 20, 12, 0, 0, DateTimeKind.Utc),
          NotificationId = 4
        };

        _syncConn.Insert(oaDM);
        _syncConn.Insert(oaDM.Notification());

        _syncConn.Insert(new Instructor()
        {
          Name = "Dr. Clyford Nosborough ",
          PhoneNumber = "877-435-7948 xx8188",
          Email = "Clyford.Nosborough@wgu.edu"
        });

        Course C949 = new()
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
          Status = CourseStatus.Completed,
          NotificationId = 5
        };

        _syncConn.Insert(C949);
        _syncConn.Insert(C949.Notification());

        Assessment paDS = new()
        {
          Name = "Pre-Assessment: Data Structures and Algorithms I (GJO1)",
          CourseId = 2,
          Start = new DateTime(2022, 2, 23, 12, 0, 0, DateTimeKind.Utc),
          End = new DateTime(2022, 2, 24, 12, 0, 0, DateTimeKind.Utc),
          NotificationId = 6
        };

        _syncConn.Insert(paDS);
        _syncConn.Insert(paDS.Notification());

        Assessment oaDS = new()
        {
          Name = "Objective Assessment: Data Structures and Algorithms I",
          CourseId = 2,
          Start = new DateTime(2022, 2, 27, 12, 0, 0, DateTimeKind.Utc),
          End = new DateTime(2022, 3, 1, 11, 59, 59, DateTimeKind.Utc),
          NotificationId = 7
        };

        _syncConn.Insert(oaDS);
        _syncConn.Insert(oaDS.Notification());

        _syncConn.Insert(new Instructor()
        {
          Name = "Dr. Greylan Beerman ",
          PhoneNumber = "877-435-7948 8367",
          Email = "Greylan.Beerman@wgu.edu"
        });

        Course C971 = new()
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
          Status = CourseStatus.InProgress,
          NotificationId = 8
        };

        _syncConn.Insert(C971);
        _syncConn.Insert(C971.Notification());

        Assessment paMA = new()
        {
          Name = "Performance Assessment: Mobile Application Development Using C# (LAP1)",
          CourseId = 3,
          Start = new DateTime(2022, 4, 11, 12, 0, 0, DateTimeKind.Utc),
          End = new DateTime(2022, 4, 12, 11, 59, 59, DateTimeKind.Utc),
          NotificationId = 9
        };

        _syncConn.Insert(paMA);
        _syncConn.Insert(paMA.Notification());

        Assessment oaMA = new()
        {
          Name = "Objective Assessment: Mobile Application Development Using C#",
          CourseId = 3,
          Start = new DateTime(2022, 4, 14, 12, 0, 0, DateTimeKind.Utc),
          End = new DateTime(2022, 4, 15, 11, 59, 59, DateTimeKind.Utc),
          NotificationId = 10
        };

        _syncConn.Insert(oaMA);
        _syncConn.Insert(oaMA.Notification());

        _syncConn.Insert(new Instructor()
        {
          Name = "Dr. Andrew Beeman",
          PhoneNumber = "217-549-5995",
          Email = "abeema5@wgu.edu"
        });

        Course C868 = new()
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
          Status = CourseStatus.PlanToTake,
          NotificationId = 11
        };

        _syncConn.Insert(C868);
        _syncConn.Insert(C868.Notification());

        Assessment paSD = new()
        {
          Name = "Performance Assessment: Software Development Capstone (RYM2)",
          CourseId = 4,
          Start = new DateTime(2022, 6, 14, 5, 59, 59, DateTimeKind.Utc),
          End = new DateTime(2022, 6, 15, 5, 59, 59, DateTimeKind.Utc),
          NotificationId = 12
        };

        _syncConn.Insert(paSD);
        _syncConn.Insert(paSD.Notification());

        Assessment oaSD = new()
        {
          Name = "Objective Assessment: Software Development Capstone",
          CourseId = 4,
          Start = new DateTime(2022, 6, 28, 12, 0, 0, DateTimeKind.Utc),
          End = new DateTime(2022, 6, 30, 11, 59, 59, DateTimeKind.Utc),
          NotificationId = 13
        };

        _syncConn.Insert(oaSD);
        _syncConn.Insert(oaSD.Notification());

        _syncConn.Table<Notification>().ToList().ForEach(o => o.Insert());

        Preferences.Set(nameof(Initial), Initial);
      }
    }
  }
}
