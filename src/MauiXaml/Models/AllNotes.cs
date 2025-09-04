using System.Collections.ObjectModel;

namespace MauiXaml.Models;

internal class AllNotes
{
    public ObservableCollection<Note> Notes { get; set; } = new();

    public AllNotes() => LoadNotes();

    public void LoadNotes()
    {
        Notes.Clear();

        var notes = Directory
            .EnumerateFiles(FileSystem.AppDataDirectory, "*.notes.txt")
            .Select(filename => new Note()
            {
                Filename = filename,
                Text = File.ReadAllText(filename),
                Date = File.GetLastWriteTime(filename)
            })
            .OrderBy(note => note.Date);

        foreach (Note note in notes)
            Notes.Add(note);
    }
}
