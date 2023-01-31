using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WaiterFunctionality;

namespace Razor.Pages
{
  public class DaysModel : PageModel
  {
    private IWaiterAvailability _waiters;
    private readonly ILogger<IndexModel> _logger;

    public DaysModel(ILogger<IndexModel> logger, IWaiterAvailability getWorkingDays)
    {
      _logger = logger;
      _waiters = getWorkingDays;
    }


    [BindProperty(SupportsGet = true)]
    public WorkingDays Data { get; set; }

    [BindProperty]
    public List<string> Day { get; set; }
    [BindProperty]
    public string Handler { get; set; }




    public void OnGet()
    {
      Day = _waiters.GetWeekDays(Data.FirstName!);

    }

    public void OnPostEnter()
    {
      if (Handler == "Enter")
      {
        Day = _waiters.GetWeekDays(Data.FirstName!);
      }
    }
  }
}
