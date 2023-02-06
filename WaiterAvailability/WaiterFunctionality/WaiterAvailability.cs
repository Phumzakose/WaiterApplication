using Dapper;
using Npgsql;
namespace WaiterFunctionality;

public class WaiterAvailability : IWaiterAvailability
{

  private NpgsqlConnection connection;
  public WaiterAvailability(string connectionString)
  {
    connection = new NpgsqlConnection(connectionString);
    connection.Open();
  }

  List<string> daysSelected = new List<string>();
  List<string> employees = new List<string>();

  Dictionary<string, List<string>> workingEmployees = new Dictionary<string, List<string>>();

  public void AddingSelectedDays(string userName, List<string> selectedDays)
  {
    var parameters = new { UserName = userName };

    var sql = @"select count (*) from employees where firstname = @UserName;";
    var count = connection.QuerySingle<int>(sql, parameters);

    if (count == 0)
    {
      throw new Exception($"Invalid username : {userName}");
    }


    var row = connection.QueryFirst<Employees>(@"select * from employees where firstname = @UserName ", parameters);

    var employees_id = row.Id;


    foreach (var day in selectedDays)
    {
      var parameter = new { UserDays = day };
      var list = connection.Query<Shifts>(@"select * from weekdays where weekday = @UserDays order by id", parameter);

      int weekdays_Id = 0;
      foreach (var days in list)
      {

        weekdays_Id = days.Id;
      }
      // var param = new { days = weekdays_Id };
      // Console.WriteLine(param);
      // var sql2 = @"select count(*) from workschedule where weekdays_id = @days";
      // var results = connection.QuerySingle(sql2, param);
      // if (results.count < 3)
      // {
      var parameter1 = new { employeeId = employees_id, DaysId = weekdays_Id };
      connection.Execute(@"insert into workschedule values(@employeeId, @DaysId)", parameter1);

      // }

    }


  }

  public string Count(List<string> selectedDays)
  {
    string message = "";
    foreach (var day in selectedDays)
    {
      var parameter = new { UserDays = day };
      var list = connection.Query<Shifts>(@"select * from weekdays where weekday = @UserDays order by id", parameter);

      int weekdays_Id = 0;
      foreach (var days in list)
      {

        weekdays_Id = days.Id;
      }

      var param = new { days = weekdays_Id };
      var sql2 = @"select count(*) from workschedule where weekdays_id = @days";
      var results = connection.QuerySingle(sql2, param);

      if (results.count < 3)
      {
        message = "You have successfully added your days";
      }
      else if (results.count == 3)
      {
        message = param + " " + "is fully booked";
      }
    }

    return message;

  }


  public List<string> GetWorkingEmployees()
  {
    var sql = @"select firstname, weekday
    from employees
    inner join workschedule
    on employees.id = employees_id
    inner join weekdays
    on weekdays.id = weekdays_id 
    group by 
    firstname, weekday
    ";
    var list = connection.Query<DayOfTheWeek>(sql);
    foreach (var employee in list)
    {
      employees.Add(employee.FirstName!);
    }

    return employees;

  }

  public Dictionary<string, List<string>> GetShiftOfWorkingEmployees()
  {
    //joining the tables
    var sql = @"select firstname, weekday
    from employees
    inner join workschedule
    on employees.id = employees_id
    inner join weekdays
    on weekdays.id = weekdays_id 
    group by 
    firstname, weekday
    ";
    var list = connection.Query<DayOfTheWeek>(sql);
    List<string> monday = new List<string>();
    List<string> tuesday = new List<string>();
    List<string> wednesday = new List<string>();
    List<string> thursday = new List<string>();
    List<string> friday = new List<string>();
    List<string> saturday = new List<string>();
    List<string> sunday = new List<string>();

    Dictionary<string, List<string>> workingDays = new Dictionary<string, List<string>>() { { "Monday", monday }, { "Tuesday", tuesday }, { "Wednesday", wednesday }, { "Thursday", thursday }, { "Friday", friday }, { "Saturday", saturday }, { "Sunday", sunday } };


    foreach (var item in list)
    {
      if (item.WeekDay == "Monday")
      {
        monday.Add(item.FirstName!);
        workingDays[item.WeekDay!] = monday;
      }
      else if (item.WeekDay == "Tuesday")
      {
        tuesday.Add(item.FirstName!);
        workingDays[item.WeekDay] = tuesday;
      }
      else if (item.WeekDay == "Wednesday")
      {
        wednesday.Add(item.FirstName!);
        workingDays[item.WeekDay] = wednesday;
      }
      else if (item.WeekDay == "Thursday")
      {
        thursday.Add(item.FirstName!);
        workingDays[item.WeekDay] = thursday;
      }
      else if (item.WeekDay == "Friday")
      {
        friday.Add(item.FirstName!);
        workingDays[item.WeekDay] = friday;
      }
      else if (item.WeekDay == "Saturday")
      {
        saturday.Add(item.FirstName!);
        workingDays[item.WeekDay] = saturday;
      }
      else if (item.WeekDay == "Sunday")
      {
        sunday.Add(item.FirstName!);
        workingDays[item.WeekDay] = sunday;
      }
    }
    return workingDays;

  }

  public Dictionary<string, List<string>> WorkingEmployees()
  {
    //GetShiftOfWorkingEmployees();
    foreach (var item in GetShiftOfWorkingEmployees())
    {
      workingEmployees.Add(item.Key, item.Value);
    }

    return workingEmployees;
  }

  public List<string> WeekDays(string firstName)
  {
    var sql = @"select weekday, firstname
    from employees
    inner join workschedule
    on employees.id = employees_id
    inner join weekdays
    on weekdays.id = weekdays_id 
    ";

    var list = connection.Query<DayOfTheWeek>(sql);
    daysSelected.Clear();
    foreach (var item in list)
    {
      if (item.FirstName == firstName)
      {

        daysSelected.Add(item.WeekDay!);
      }
    }

    return daysSelected;
  }
  public List<string> GetWeekDays()
  {
    return daysSelected;
  }


  public string UpdateWorkingDays(string firstName, List<string> selectedDays)
  {

    string message = "";
    List<int> day1 = new List<int>();

    var name = new { UserName = firstName };

    var sql = @"select count (*) from employees where firstname = @UserName;";
    var result = connection.QueryFirst(sql, name);

    int employee_id = 0;
    if (result.count == 1)
    {
      var rows = connection.Query<Employees>(@"select * from employees where firstname = @UserName ", name);

      foreach (var id in rows)
      {
        employee_id = id.Id;
      }

    }

    int weekdays_Id = 0;

    foreach (var day in selectedDays)
    {
      var days = new { UserDays = day };

      var list = @"select count(*) from weekdays where weekday = @UserDays";
      var res = connection.QueryFirst(list, days);

      if (res.count == 1)
      {
        var weekday = connection.Query<Shifts>(@"select * from weekdays where weekday = @UserDays order by id", days);

        foreach (var item in weekday)
        {
          day1.Add(item.Id);
        }
      }

    }

    var parameter2 = new { employeeId = employee_id };

    var sql1 = @"select count(*) from workschedule where employees_id = @employeeId;";

    var results = connection.QueryFirst(sql1, parameter2);

    if (results.count > 1)
    {
      connection.Execute(@"delete from workschedule where employees_id = @employeeId", parameter2);
      foreach (var item in day1)
      {
        weekdays_Id = item;
        var parameter1 = new { employee1Id = employee_id, Days1Id = weekdays_Id };
        connection.Execute(@"insert into workschedule values(@employee1Id, @Days1Id)", parameter1);

        message = "You have updated your days";

      }

    }
    else
    {
      foreach (var items in day1)
      {
        weekdays_Id = items;
        var param = new { employeeId = employee_id, DaysId = weekdays_Id };
        connection.Execute(@"insert into workschedule values(@employeeId, @DaysId)", param);
      }
    }

    return message;

  }
  public void ResetData()
  {
    var list = connection.Query<DayOfTheWeek>(@"truncate table workschedule");
  }




}







