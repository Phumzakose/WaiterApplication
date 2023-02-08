using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WaiterFunctionality;

namespace Razor.Pages
{
  public class ManagerModel : PageModel
  {
    private IWaiterAvailability _waiter;
    private readonly ILogger<IndexModel> _logger;

    public ManagerModel(ILogger<IndexModel> logger, IWaiterAvailability waiter)
    {
      _logger = logger;
      _waiter = waiter;
    }

    [BindProperty]
    public Dictionary<string, List<string>> WorkingEmployees { get; set; }


    public void OnGet()
    {
      WorkingEmployees = _waiter.GetShiftOfWorkingEmployees();


    }

    public void OnPostClear()
    {
      _waiter.ResetData();
      TempData["AlertMessage"] = "Your Schedule has been cleared...!";
      WorkingEmployees = _waiter.GetShiftOfWorkingEmployees();
    }
  }
}
