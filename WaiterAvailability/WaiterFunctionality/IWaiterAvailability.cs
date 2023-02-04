namespace WaiterFunctionality;

public interface IWaiterAvailability
{
  string AddingSelectedDays(string userName, List<string> selectedDays);
  List<string> WeekDays(string firstName);
  Dictionary<string, List<string>> GetShiftOfWorkingEmployees();
  Dictionary<string, List<string>> WorkingEmployees();
  string UpdateWorkingDays(string firstName, List<string> selectedDays);
  List<string> GetWeekDays();
  List<string> GetWorkingEmployees();
  void ResetData();
  string Message();



}