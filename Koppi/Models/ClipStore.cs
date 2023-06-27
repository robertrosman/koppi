namespace Koppi.Models
{
    public class ClipStore
    {
        public List<Clip> clips { get; private set; } = new List<Clip>();

        ClipStore()
        {
        }

        public void AddClip(Clip clip)
        {
            clips.Add(clip);
        }

    }
}
