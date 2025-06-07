namespace RTCoffeeAPI.Utilitys
{
    public static class Constants
    {
        //200 Response Messages
        public static string SuccessHotCoffMessage = "Your piping hot coffee is ready";

        public static string SuccessColCofFMessage = "Your refreshing iced coffee is ready";
        //**********************************************************//

       

        //******************ERROR START****************************//

        //Error Messages
        public static string APIKeyMissing = "API Key is missing";
        public static string WeatherServError = "Error encountered when connecting to weather service";
        public static string BrewCoffeeError = "Error encountered when trying to brew coffee";

        //*****************ERROR END *****************************//

        //Constants
        public static string ConfigAPIKey = "OpenWeather:ApiKey";
        public static string ConfigAPILocation = "Melbourne,AU";
        public static string DefaultLocation = "Melbourne,AU";
        //ISO Formatter Constant for Date Time 
        public static string RTDateTimeFormat = "yyyy-MM-dd'T'HH:mm:sszzz";
    }
}
