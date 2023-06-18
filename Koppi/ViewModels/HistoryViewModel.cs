using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Koppi.ViewModels;

internal class HistoryViewModel : IQueryAttributable
{
    public ObservableCollection<ClipViewModel> AllNotes { get; }
    public ICommand NewCommand { get; }
    public ICommand SelectNoteCommand { get; }

    public HistoryViewModel()
    {

        /* Unmerged change from project 'App (net7.0-android)'
        Before:
                AllNotes = new ObservableCollection<ClipViewModel>(Models.Clip.LoadAll().Select(n => new ClipViewModel(n)));
        After:
                AllNotes = new ObservableCollection<ClipViewModel>(Clip.LoadAll().Select(n => new ClipViewModel(n)));
        */
        AllNotes = new ObservableCollection<ClipViewModel>(Models.Clip.LoadAll().Select(n => new ClipViewModel(n)));
        NewCommand = new AsyncRelayCommand(NewNoteAsync);
        SelectNoteCommand = new AsyncRelayCommand<ClipViewModel>(SelectNoteAsync);
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
            ClipViewModel matchedNote = AllNotes.Where((n) => n.Identifier == noteId).FirstOrDefault();

            // If note exists, delete it
            if (matchedNote != null)
            {
                AllNotes.Remove(matchedNote);
            }
        }
        else if (query.ContainsKey("saved"))
        {
            string noteId = query["saved"].ToString();
            ClipViewModel matchedNote = AllNotes.Where((n) => n.Identifier == noteId).FirstOrDefault();

            // If note is found, update it
            if (matchedNote != null)
            {

                matchedNote.Reload();
                AllNotes.Move(AllNotes.IndexOf(matchedNote), 0);
            }

            // If note isn't found, it's new; add it.
            else

                /* Unmerged change from project 'App (net7.0-android)'
                Before:
                                AllNotes.Insert(0, new ClipViewModel(Models.Clip.Load(noteId)));
                After:
                                AllNotes.Insert(0, new ClipViewModel(Clip.Load(noteId)));
                */
                AllNotes.Insert(0, new ClipViewModel(Models.Clip.Load(noteId)));
        }
    }

}
