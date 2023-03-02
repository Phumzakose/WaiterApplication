using System.ComponentModel.DataAnnotations;


static public class WorkingDays
{

  static public int Week { get; set; }


  static public void IncreasedWeeks()
  {
    if (Week == 0)
    {
      Week += 7;

    }
  }

  static public void DecreasedWeeks()
  {
    if (Week == 7)
    {
      Week -= 7;

    }
  }

}

