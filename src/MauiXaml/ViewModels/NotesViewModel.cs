using CommunityToolkit.Mvvm.Input;
using MauiXaml.Models;
using MauiXaml.Views;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace MauiXaml.ViewModels;

internal class NotesViewModel : IQueryAttributable
{
    public ObservableCollection<NoteViewModel> AllNotes { get; }
    public ICommand NewCommand { get; }
    public ICommand SelectNoteCommand { get; }

    public NotesViewModel()
    {
        AllNotes = new ObservableCollection<NoteViewModel>(Note.LoadAll().Select(n => new NoteViewModel(n)));
        NewCommand = new AsyncRelayCommand(NewNoteAsync);
        SelectNoteCommand = new AsyncRelayCommand<NoteViewModel>(SelectNoteAsync);
    }

    private async Task NewNoteAsync()
    {
        await Shell.Current.GoToAsync(nameof(NotePage));
    }

    private async Task SelectNoteAsync(NoteViewModel? note)
    {
        if (note != null)
            await Shell.Current.GoToAsync($"{nameof(NotePage)}?load={note.Identifier}");
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("deleted"))
        {
            var noteId = query["deleted"].ToString() ?? "";
            var matchedNote = AllNotes.Where((n) => n.Identifier == noteId).FirstOrDefault();
            if (matchedNote != null)
                AllNotes.Remove(matchedNote);
        }
        else if (query.ContainsKey("saved"))
        {
            var noteId = query["saved"].ToString() ?? "";
            var matchedNote = AllNotes.Where((n) => n.Identifier == noteId).FirstOrDefault();
            if (matchedNote != null)
            {
                matchedNote.Reload();
                AllNotes.Move(AllNotes.IndexOf(matchedNote), 0);
            }
            else
                AllNotes.Insert(0, new NoteViewModel(Note.Load(noteId)));
        }
    }
}