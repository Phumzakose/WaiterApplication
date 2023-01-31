using Dapper;
using Npgsql;
namespace WaiterFunctionality;

public class WaiterAvailability : IWaiterAvailability
{
  string connectionString = "Server=tiny.db.elephantsql.com ;Port=5432;Database=qnnpfxju;UserId=qnnpfxju;Password=YIn1V7tPQXB9tIf9EprMpR5KpSXvDGtm";

  public List<int> selectedDays = new List<int>();

  public List<string> daysSelected = new List<string>();


  public void AddingSelectDays(string userName, List<string> selectedDays)
  {
    using var connection = new NpgsqlConnection(connectionString);
    connection.Open();

    var parameters = new { UserName = userName };


    var sql = @"select count (*) from employees where firstname = @UserName;";
    var result = connection.QueryFirst(sql, parameters);

    int employees_id = 0;
    if (result.count == 1)
    {
      var rows = connection.Query<Employees>(@"select * from employees where firstname = @UserName ", parameters);

      foreach (var id in rows)
      {
        employees_id = id.Id;

      }
    }

    foreach (var day in selectedDays)
    {
      var parameter = new { UserDays = day };
      var list = connection.Query<Shifts>(@"select * from weekdays where weekday = @UserDays order by id", parameter);

      int weekdays_Id = 0;

      foreach (var days in list)
      {
        weekdays_Id = days.Id;
      }
      var parameter1 = new { employeeId = employees_id, DaysId = weekdays_Id };
      connection.Execute(@"insert into workschedule values(@employeeId, @DaysId)", parameter1);
    }

  }

  public Dictionary<string, List<string>> GetShiftOfWorkingEmployees()
  {
    using var connection = new NpgsqlConnection(connectionString);
    connection.Open();

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
        workingDays[item.WeekDay] = monday;
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
    Console.WriteLine(workingDays);
    return workingDays;

  }

  public List<string> GetWeekDays(string firstName)
  {
    using var connection = new NpgsqlConnection(connectionString);
    connection.Open();

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

  public void UpdateWorkingDays(string firstName)
  {
    using var connection = new NpgsqlConnection(connectionString);
    connection.Open();

    var parameters = new { UserName = firstName };

    var sql = @"select count (*) from employees where firstname = @UserName;";
    var result = connection.QueryFirst(sql, parameters);

    int employee_id = 0;
    if (result.count == 1)
    {
      var rows = connection.Query<Employees>(@"select * from employees where firstname = @UserName ", parameters);

      foreach (var id in rows)
      {
        employee_id = id.Id;
      }

      var parameter1 = new { employeeId = employee_id };
      connection.Execute(@"delete from workschedule where employees_id = @employeeId", parameter1);

    }

  }


  public void ResetData()
  {
    using var connection = new NpgsqlConnection(connectionString);
    connection.Open();
    var list = connection.Query<DayOfTheWeek>(@"delete from workschedule");
  }
















}







