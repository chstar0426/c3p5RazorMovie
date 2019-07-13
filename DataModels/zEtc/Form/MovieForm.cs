namespace DataModels
{
    public class MovieForm
    {
        public FrmType FrmType { get; set; }
        public string  ErrorMessage { get; set; }
        public Movie Movie { get; set; }
        public string ReturnUrl { get; set; }

    }
}
