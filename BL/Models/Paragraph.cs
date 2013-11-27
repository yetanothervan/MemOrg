namespace Models
{
    public class Paragraph
    {
        public Paragraph()
        {
            Quote = false;
            Content = "";
        }
        public string Content { get; set; }
        public bool Quote { get; set; }
    }
}
