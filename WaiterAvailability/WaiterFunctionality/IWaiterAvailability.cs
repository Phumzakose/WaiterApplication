namespace WaiterFunctionality;

public interface IWaiterAvailability
{
  void AddingSelectDays(string userName, List<string> selectedDays);
  List<string> GetWeekDays(string firstName);
  Dictionary<string, List<string>> GetShiftOfWorkingEmployees();
  public void UpdateWorkingDays(string firstName);


}