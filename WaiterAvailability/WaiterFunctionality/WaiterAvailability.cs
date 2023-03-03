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

    var employee_id = row.Id;


    foreach (var day in selectedDays)
    {

      DateTime dates = DateTime.Parse(day);
      string weekDay = dates.ToString("dddd");

      var param = new { WeekDate = dates, Day = weekDay, employeeId = employee_id };
      connection.Execute(@"insert into schedule(WeekDay, Date, Employee_Id) values(@Day, @WeekDate, @employeeId)", param);

    }


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
    var sql = @"select firstname, weekday, date
    from employees
    inner join schedule
    on employees.id = employee_id
    order by date;
    ";
    var list = connection.Query<DayOfTheWeek>(sql);
    Dictionary<string, List<string>> employees = new Dictionary<string, List<string>>();



    var test = list.ToList().GroupBy(x => x.Date);

    foreach (var item in test)
    {

      employees.Add(item.Key.ToShortDateString(), new List<string>());

      foreach (var item2 in item)
      {
        employees[item.Key.ToShortDateString()].Add(item2.FirstName!);
      }
    }

    return employees;
  }

  public Dictionary<string, List<string>> WorkingEmployees()
  {
    foreach (var item in GetShiftOfWorkingEmployees())
    {
      workingEmployees.Add(item.Key, item.Value);
    }

    return workingEmployees;
  }

  public List<string> WeekDays(string firstName)
  {
    var sql = @"select weekday,date, firstname
    from employees
    inner join schedule
    on employees.id = employee_id
    ";

    var list = connection.Query<DayOfTheWeek>(sql);
    daysSelected.Clear();
    foreach (var item in list)
    {
      if (item.FirstName == firstName)
      {
        daysSelected.Add(item.Date.ToShortDateString());
      }
    }

    return daysSelected;
  }
  public List<string> GetWeekDays()
  {
    return daysSelected;
  }

  public List<string> WaiterDays(string firstName, int week)
  {
    List<string> waiterDays = new List<string>();

    foreach (var item in DaysOfTheWeek(today, week))
    {
      foreach (var items in WeekDays(firstName))
      {
        if (items.Contains(item.Key))
        {
          waiterDays.Add(item.Key);
        }

      }
    }
    return waiterDays;
  }


  public string UpdateWorkingDays(string firstName, List<string> selectedDays, int week)
  {

    string message = "";
    List<int> employeeDays = new List<int>();

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

    DateTime weekdays_Id;

    var parameter2 = new { employeeId = employee_id };

    var sql1 = @"select count(*) from schedule where employee_id = @employeeId;";

    var results = connection.QueryFirst(sql1, parameter2);

    if (results.count > 1)
    {
      List<string> waiterDays = new List<string>();

      foreach (var item in DaysOfTheWeek(today, week))
      {
        foreach (var items in WeekDays(firstName))
        {
          if (items.Contains(item.Key))
          {
            waiterDays.Add(item.Key);
          }

        }
      }
      foreach (var item in waiterDays)
      {
        var param = new { employeeId = employee_id, dates = DateTime.Parse(item) };

        connection.Execute(@"delete from schedule where employee_id = @employeeId AND date = @dates", param);

      }

      foreach (var item in selectedDays)
      {
        DateTime dates = DateTime.Parse(item);
        weekdays_Id = dates;
        string weekDay = dates.ToString("dddd");
        var parameter1 = new { employee1Id = employee_id, Days1Id = weekdays_Id, Day = weekDay };
        connection.Execute(@"insert into schedule(Date, Weekday, Employee_Id) values(@Days1Id, @Day, @employee1Id)", parameter1);

        message = "You have updated your days";

      }


    }

    return message;

  }
  public string ResetData(int week)
  {
    var sql = @"select firstname, weekday, date
    from employees
    inner join schedule
    on employees.id = employee_id
    order by date;
    ";
    var list = connection.Query<DayOfTheWeek>(sql);
    List<string> daysForEachWeek = new List<string>();

    foreach (var item in list)
    {
      foreach (var items in DaysOfTheWeek(today, week))
      {
        if (items.Key == item.Date.ToShortDateString())
        {
          daysForEachWeek.Add(item.Date.ToShortDateString());
        }

      }

    }
    foreach (var item in daysForEachWeek)
    {
      var parameter = new { days = DateTime.Parse(item) };
      connection.Execute(@"delete from schedule where date = @days", parameter);

    }


    return "The schedule is cleared";

  }

  // public void RemoveWaiter(string waiter)
  // {
  //   var parameter = new { userName = waiter };
  //   var row = connection.QueryFirst<Employees>(@"select * from employees where firstname = @UserName ", parameter);
  //   var employees_id = row.Id;

  //   var param = new { employeeId = employees_id };
  //   var sql = @"DELETE from workschedule where employees_id = @employeeId";
  //   var list = connection.Query<DayOfTheWeek>(sql, param);
  //   Console.WriteLine(list);


  // }
  public string CheckName(string userName)
  {
    var parameters = new { UserName = userName };

    var sql = @"select count (*) from employees where firstname = @UserName;";
    var count = connection.QuerySingle<int>(sql, parameters);

    if (count == 0)
    {
      return "Invalid user";
    }
    else
    {
      return userName;
    }

  }

  DateTime today = DateTime.Now;

  public Dictionary<string, DayOfWeek> DaysOfTheWeek(DateTime today, int week)
  {

    Dictionary<string, DayOfWeek> weekDaysDate = new Dictionary<string, DayOfWeek>();
    var days = DayOfWeek.Monday - today.DayOfWeek + week;
    var startDate = today.AddDays(days);

    for (int i = 0; i < 7; i++)
    {
      weekDaysDate.Add(DateOnly.FromDateTime(startDate.AddDays(i)).ToShortDateString(), startDate.AddDays(i).DayOfWeek);
    }


    return weekDaysDate;

  }

  Dictionary<string, DayOfWeek> weekDays = new Dictionary<string, DayOfWeek>();
  public Dictionary<string, DayOfWeek> GetDaysOfTheWeek()
  {
    foreach (var item in DaysOfTheWeek(today, 0))
    {
      weekDays.Add(item.Key, item.Value);

    }
    return weekDays;
  }



}







