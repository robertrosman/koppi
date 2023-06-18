namespace Koppi.Models;

internal class Clip
{
    public string Filename { get; set; }
    public string Text { get; set; }
    public DateTime Date { get; set; }

    public Clip()
    {
        Filename = $"{Path.GetRandomFileName()}.Koppi.txt";
        Date = DateTime.Now;
        Text = "";
    }

    public void Save() =>
        File.WriteAllText(Path.Combine(FileSystem.AppDataDirectory, Filename), Text);

    public void Delete() =>
        File.Delete(Path.Combine(FileSystem.AppDataDirectory, Filename));

    public static Clip Load(string filename)
    {
        filename = Path.Combine(FileSystem.AppDataDirectory, filename);

        if (!File.Exists(filename))
            throw new FileNotFoundException("Unable to find file on local storage.", filename);

        return
            new()
            {
                Filename = Path.GetFileName(filename),
                Text = File.ReadAllText(filename),
                Date = File.GetLastWriteTime(filename)
            };
    }

    public static IEnumerable<Clip> LoadAll()
    {
        // Get the folder where the notes are stored.
        string appDataPath = FileSystem.AppDataDirectory;

        // Use Linq extensions to load the *.Koppi.txt files.
        return Directory

                // Select the file names from the directory
                .EnumerateFiles(appDataPath, "*.Koppi.txt")

                // Each file name is used to load a note
                .Select(filename => Load(Path.GetFileName(filename)))

                // With the final collection of notes, order them by date
                .OrderByDescending(note => note.Date);
    }
}