using WaiterFunctionality;
using Npgsql;
using Dapper;
namespace WaiterFunctionality.Tests;


public class WaiterTest : IDisposable
{

  static string connectionString = "Server=tiny.db.elephantsql.com ;Port=5432;Database=rjyxyjew;UserId=rjyxyjew;Password=T0xPrO3GmqOjoDyeCqz8q7H6FfmagVA8";

  IWaiterAvailability waiter = new WaiterAvailability(connectionString);



  public WaiterTest()
  {
    // ensure all the waiters are in the database

    // run the database script in the remote database

    var sql = File.ReadAllText("./sql/data.sql");

    // Console.WriteLine(sql);

    using (var connection = new NpgsqlConnection(connectionString))
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




}