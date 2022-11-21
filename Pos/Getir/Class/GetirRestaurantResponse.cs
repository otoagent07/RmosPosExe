namespace Pos.Getir.Class
{
    public class GetirRestaurantResponse
    {
        public string id { get; set; }
        public int averagePreparationTime { get; set; }
        public int status { get; set; }
        public bool isCourierAvailable { get; set; }
        public string name { get; set; }
    }

}
