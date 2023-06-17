namespace Koppi.Models;

internal class Clip
{
    public string Text { get; set; }
    public DateTime Date { get; set; }

    public Clip()
    {
        Date = DateTime.Now;
        Text = "";
    }

}