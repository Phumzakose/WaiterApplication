using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WaiterFunctionality;

namespace Razor.Pages;

public class IndexModel : PageModel
{
  private IWaiterAvailability _waiter;
  private readonly ILogger<IndexModel> _logger;

  public IndexModel(ILogger<IndexModel> logger, IWaiterAvailability waiter)
  {
    _logger = logger;
    _waiter = waiter;
  }
  [BindProperty(SupportsGet = true)]
  public WorkingDays Data { get; set; }

  [BindProperty]
  public string FirstName { get; set; }

  [BindProperty]
  public List<string> Day { get; set; }


  [BindProperty]
  public Dictionary<string, List<string>> WorkingEmployees { get; set; }


  public void OnGet()
  {

    Day = _waiter.WeekDays(Data.FirstName!);
    WorkingEmployees = _waiter.GetShiftOfWorkingEmployees();

  }

  public void OnPostSubmit()
  {
    _waiter.AddingSelectedDays(Data.FirstName!, Day);
    WorkingEmployees = _waiter.GetShiftOfWorkingEmployees();
    TempData["AlertMessage"] = "Your days have been submitted successfully...!";


  }
  public void OnPostUpdate()
  {
    _waiter.UpdateWorkingDays(Data.FirstName!, Day);
    WorkingEmployees = _waiter.GetShiftOfWorkingEmployees();
    TempData["AlertMessage"] = "Your days have been updated successfully..!";


  }









}
