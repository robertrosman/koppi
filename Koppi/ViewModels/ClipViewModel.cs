using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace Koppi.ViewModels;

public class ClipViewModel : ObservableObject, IQueryAttributable
{
    private Models.Clip _clip;

    public string Text
    {
        get => _clip.Text;
        set
        {
            if (_clip.Text != value)
            {
                _clip.Text = value;
                OnPropertyChanged();
            }
        }
    }

    public DateTime Date => _clip.Date;

    public string Identifier => _clip.Filename;

    public ICommand SaveCommand { get; private set; }
    public ICommand DeleteCommand { get; private set; }

    public ClipViewModel()
    {
        _clip = new Models.Clip();
        SaveCommand = new AsyncRelayCommand(Save);
        DeleteCommand = new AsyncRelayCommand(Delete);
    }

    public ClipViewModel(Models.Clip note)
    {
        _clip = note;
        SaveCommand = new AsyncRelayCommand(Save);
        DeleteCommand = new AsyncRelayCommand(Delete);
    }

    private async Task Save()
    {
        _clip.Date = DateTime.Now;
        _clip.Save();
        await Shell.Current.GoToAsync($"..?saved={_clip.Filename}");
    }

    private async Task Delete()
    {
        _clip.Delete();
        await Shell.Current.GoToAsync($"..?deleted={_clip.Filename}");
    }

    void IQueryAttributable.ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("load"))
        {
            _clip = Models.Clip.Load(query["load"].ToString());
            RefreshProperties();
        }
    }

    public void Reload()
    {
        _clip = Models.Clip.Load(_clip.Filename);
        RefreshProperties();
    }

    private void RefreshProperties()
    {
        OnPropertyChanged(nameof(Text));
        OnPropertyChanged(nameof(Date));
    }
}