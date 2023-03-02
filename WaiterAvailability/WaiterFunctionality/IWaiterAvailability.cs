namespace WaiterFunctionality;

public interface IWaiterAvailability
{
  void AddingSelectedDays(string userName, List<string> selectedDays);
  List<string> WeekDays(string firstName);
  Dictionary<string, List<string>> GetShiftOfWorkingEmployees();
  Dictionary<string, List<string>> WorkingEmployees();
  string UpdateWorkingDays(string firstName, List<string> selectedDays, int week);
  List<string> GetWeekDays();
  List<string> GetWorkingEmployees();
  String ResetData(int week);
  string CheckName(string userName);
  Dictionary<string, DayOfWeek> DaysOfTheWeek(DateTime today, int week);
  Dictionary<string, DayOfWeek> GetDaysOfTheWeek();





}