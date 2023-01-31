namespace WaiterFunctionality.Tests;
using WaiterFunctionality;

public class WaiterTest
{
  WaiterAvailability waiter = new WaiterAvailability();
  [Fact]
  public void ItShouldBeAbleToRetunSelectedDays()
  {
    List<string> days = new List<string>() { "Monday", "Wednesday", "Friday" };


    Assert.Equal(waiter.daysSelected, waiter.GetWeekDays("Lulu"));

  }

  [Fact]
  public void ItShouldBeAbleToAddTheEmployeeWithTheWorkingDays()
  {
    List<string> days1 = new List<string>() { "Monday", "Wednesday", "Friday" };
    waiter.AddingSelectDays("Bongi", days1);
    List<string> days2 = new List<string>() { "Monday", "Tuesday", "Thursday" };
    waiter.AddingSelectDays("Xolani", days2);
    List<string> days3 = new List<string>() { "Thursday", "Friday", "Saturday" };
    waiter.AddingSelectDays("Thembisa", days3);
    List<string> days4 = new List<string>() { "Friday", "Saturday", "Sunday" };
    waiter.AddingSelectDays("Lulu", days4);

    Assert.Equal(waiter.GetShiftOfWorkingEmployees(), waiter.GetShiftOfWorkingEmployees());

  }

  [Fact]
  public void ItShouldBeAbleToGetTheShiftsOfTheEmployees()
  {
    List<string> days1 = new List<string>() { "Monday", "Wednesday", "Friday" };
    waiter.AddingSelectDays("Bongi", days1);
    List<string> days2 = new List<string>() { "Monday", "Tuesday", "Thursday" };
    waiter.AddingSelectDays("Xolani", days2);
    List<string> days3 = new List<string>() { "Thursday", "Friday", "Saturday" };
    waiter.AddingSelectDays("Thembisa", days3);




  }
}