using WaiterFunctionality;
using Npgsql;
using Dapper;
namespace WaiterFunctionality.Tests;


public class WaiterTest : IDisposable
{

  static string connectionString = "Server=tiny.db.elephantsql.com ;Port=5432;Database=rjyxyjew;UserId=rjyxyjew;Password=T0xPrO3GmqOjoDyeCqz8q7H6FfmagVA8";

  //private static string connectionString = "Host = localhost; Username=waiter;Password=waiter123;Database= waiter_application";


  static string GetConnectionString()
  {
    var theCN = Environment.GetEnvironmentVariable("connectionString");
    if (theCN == "" || theCN == null)
    {
      theCN = connectionString;
    }
    return theCN;
  }
  IWaiterAvailability waiter = new WaiterAvailability(GetConnectionString());



  public WaiterTest()
  {
    // ensure all the waiters are in the database

    // run the database script in the remote database

    var sql = File.ReadAllText("./sql/data.sql");

    // Console.WriteLine(sql);

    using (var connection = new NpgsqlConnection(GetConnectionString()))
    {
      connection.Execute(sql);
    }

  }


  public void Dispose()
  {
    // Console.WriteLine("... done!");
  }



  [Fact]
  public void ItShouldBeAbleToRetunSelectedDays()
  {


    Assert.Equal(waiter.GetWeekDays(), waiter.WeekDays("Bongi"));

  }

  [Fact]
  public void ItShouldBeAbleToReturnAllWorkingEmployeesAndDays()
  {

    Assert.Equal(waiter.WorkingEmployees(), waiter.GetShiftOfWorkingEmployees());

  }
  [Fact]
  public void ItShouldBeAbleToUpdateTheWorkingDays()
  {
    List<string> days = new List<string>() { "Monday", "Tuesday", "Wednesday" };
    waiter.AddingSelectedDays("Lulu", days);
    List<string> day = new List<string>() { "Monday", "Tuesday", "Friday" };

    Assert.Equal("You have updated your days", waiter.UpdateWorkingDays("Lulu", day));

  }
  [Fact]
  public void ItShouldBeAbleToReturnAmessageWhenTheScheduleIsCleared()
  {
    List<string> days = new List<string>() { "Monday", "Tuesday", "Wednesday" };
    waiter.AddingSelectedDays("Lulu", days);
    List<string> day = new List<string>() { "Monday", "Tuesday", "Friday" };
    Assert.Equal("The schedule is cleared", waiter.ResetData());
  }
  [Fact]
  public void ItShouldBeAbleToReturnTheNameOfTheWaiter()
  {
    List<string> days = new List<string>() { "Monday", "Tuesday", "Wednesday" };
    waiter.AddingSelectedDays("Bongi", days);
    Assert.Equal("Bongi", waiter.CheckName("Bongi"));

  }





}