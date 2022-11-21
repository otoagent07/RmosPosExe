namespace Pos.Getir.Class
{

    public class GetirStatik
    {

        public static string baseUri = "https://food-external-api-gateway.getirapi.com/";
        public static string baseUriTest = "https://food-external-api-gateway.development.getirapi.com/";
        public static string requestChangeLog = baseUri + "changelog";
        public static string requestLogin = baseUri + "auth/login";
        public static string requestMenu = baseUri + "restaurants/menu";
        public static string requestRestoran = baseUri + "restaurants";
        public static string requestOrder = baseUri + "food-orders/active";
        public static string requestOrderBase = baseUri + "food-orders";
        public static string requestPayment = baseUri + "payment-methods";
        public static string requestFoodOrderActive = baseUri + "food-orders/active";
        public static string requestFoodOrderVerify = baseUri + "food-orders/";


        public static void yenile()
        {
            requestChangeLog = baseUri + "changelog";
            requestLogin = baseUri + "auth/login";
            requestMenu = baseUri + "restaurants/menu";
            requestRestoran = baseUri + "restaurants";
            requestOrder = baseUri + "food-orders/active";
            requestOrderBase = baseUri + "food-orders";
            requestPayment = baseUri + "payment-methods";
            requestFoodOrderActive = baseUri + "food-orders/active";
            requestFoodOrderVerify = baseUri + "food-orders/";
        }
    }
}
