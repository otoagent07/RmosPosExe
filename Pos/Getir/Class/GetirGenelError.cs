namespace Pos.Getir.Class
{
    public class GetirGenelError
    {
        public int code { get; set; }
        public string error { get; set; }
        public string message { get; set; }
        public Data data { get; set; }
        public string detail { get; set; }
    }
    public class Data
    {
        public string name { get; set; }
        public string message { get; set; }
    }

}
