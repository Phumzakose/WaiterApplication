using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WaiterFunctionality;

namespace Razor.Pages
{
  public class DaysModel : PageModel
  {
    private IWaiterAvailability _waiter;
    private readonly ILogger<IndexModel> _logger;

    public DaysModel(ILogger<IndexModel> logger, IWaiterAvailability getWorkingDays)
    {
      _logger = logger;
      _waiter = getWorkingDays;
    }


    [BindProperty(SupportsGet = true)]
    public WorkingDays Data { get; set; }

    [BindProperty]
    public List<string> Day { get; set; }

    [BindProperty]
    public Dictionary<string, List<string>> WorkingEmployees { get; set; }
    [BindProperty]
    public string Handler { get; set; }

    public string Count { get; set; }



    public void OnGet()
    {
      Day = _waiter.WeekDays(Data.FirstName!);
      WorkingEmployees = _waiter.GetShiftOfWorkingEmployees();

    }

    public void OnPostSubmit()
    {
      _waiter.AddingSelectedDays(Data.FirstName!, Day);
      WorkingEmployees = _waiter.GetShiftOfWorkingEmployees();
      Count = _waiter.Count(Day);
      TempData["AlertMessage"] = "Your days have been submitted successfully..!";
      TempData["ErrorMessage"] = "The days you selected are full";

    }

    public void OnPostUpdate(string name)
    {

      _waiter.UpdateWorkingDays(Data.FirstName!, Day);
      WorkingEmployees = _waiter.GetShiftOfWorkingEmployees();
      TempData["AlertMessage"] = "Your days have been updated successfully..!";

    }
  }
}
