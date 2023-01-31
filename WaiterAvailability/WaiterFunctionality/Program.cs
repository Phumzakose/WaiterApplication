
using WaiterFunctionality;


WaiterAvailability waiter = new WaiterAvailability();


// List<string> days2 = new List<string>() { "Monday", "Tuesday", "Thursday" };
// waiter.AddingSelectDays("Soso", days2);
// List<string> days3 = new List<string>() { "Thursday", "Friday", "Saturday" };
// waiter.AddingSelectDays("Bongi", days3);

//waiter.UpdateWorkingDays("Bongi");

foreach (var item in waiter.GetShiftOfWorkingEmployees())
{
  foreach (var days in item.Value)
  {
    Console.WriteLine(item.Key + days);
  }
}
//waiter.GetShiftOfWorkingEmployees();
























