using Koppi.ViewModels;

namespace Koppi.Views;

public partial class HistoryPage : ContentPage
{
    public HistoryPage(HistoryViewModel historyViewModel)
    {
        BindingContext = historyViewModel;
        InitializeComponent();
    }

    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        notesCollection.SelectedItem = null;
    }
}
