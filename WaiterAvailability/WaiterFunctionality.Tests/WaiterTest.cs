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

  DateTime today = DateTime.Now;


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
    List<string> days = new List<string>() { "2023/02/01", "2023/02/02", "2023/02/03" };
    waiter.AddingSelectedDays("Lulu", days);
    List<string> day = new List<string>() { "2023/02/27", "2023/02/28" };

    Assert.Equal("You have updated your days", waiter.UpdateWorkingDays("Lulu", day, 0));

  }
  [Fact]
  public void ItShouldBeAbleToReturnWaiterAvailable()
  {
    List<string> days = new List<string>() { "2023/02/01", "2023/02/02", "2023/02/03" };
    waiter.AddingSelectedDays("Lulu", days);
    Assert.Equal("Lulu", waiter.CheckName("Lulu"));

  }
  public void ItShouldBeAbleToReturnErrorMessageIfWaiterIsNotAvailable()
  {
    List<string> days = new List<string>() { "2023/02/01", "2023/02/02", "2023/02/03" };
    waiter.AddingSelectedDays("Lulu", days);
    Assert.Equal("Invalid user", waiter.CheckName("Thembisa"));

  }








}