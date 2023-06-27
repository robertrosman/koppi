using CommunityToolkit.Mvvm.Input;
using Koppi.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Koppi.ViewModels;

public class HistoryViewModel : IQueryAttributable
{
    public ObservableCollection<ClipViewModel> ClipHistory { get; }
    public ICommand NewCommand { get; }
    public ICommand SelectNoteCommand { get; }
    private readonly ClipStore _clipStore;

    public HistoryViewModel(ClipStore clipStore)
    {
        _clipStore = clipStore;
        ClipHistory = new ObservableCollection<ClipViewModel>(clipStore.clips.Select(n => new ClipViewModel(n)));
        NewCommand = new AsyncRelayCommand(NewNoteAsync);
        SelectNoteCommand = new AsyncRelayCommand<ClipViewModel>(SelectNoteAsync);

        Clipboard.Default.ClipboardContentChanged += Clipboard_ClipboardContentChanged;
    }

    private async Task NewNoteAsync()
    {
        await Shell.Current.GoToAsync(nameof(Views.ClipPage));
    }

    private async Task SelectNoteAsync(ClipViewModel note)
    {
        if (note != null)
            await Shell.Current.GoToAsync($"{nameof(Views.ClipPage)}?load={note.Identifier}");
    }

    void IQueryAttributable.ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("deleted"))
        {
            string noteId = query["deleted"].ToString();
            ClipViewModel matchedNote = ClipHistory.Where((n) => n.Identifier == noteId).FirstOrDefault();

            // If note exists, delete it
            if (matchedNote != null)
            {
                ClipHistory.Remove(matchedNote);
            }
        }
        else if (query.ContainsKey("saved"))
        {
            string noteId = query["saved"].ToString();
            ClipViewModel matchedNote = ClipHistory.Where((n) => n.Identifier == noteId).FirstOrDefault();

            // If note is found, update it
            if (matchedNote != null)
            {

                matchedNote.Reload();
                ClipHistory.Move(ClipHistory.IndexOf(matchedNote), 0);
            }

            // If note isn't found, it's new; add it.
            else
                ClipHistory.Insert(0, new ClipViewModel(Models.Clip.Load(noteId)));
        }
    }


    private async void Clipboard_ClipboardContentChanged(object sender, EventArgs e)
    {
        Models.Clip clip = new Models.Clip();
        clip.Text = await Clipboard.Default.GetTextAsync();
        _clipStore.AddClip(clip);
        ClipHistory.Insert(0, new ClipViewModel(clip));
    }

}
