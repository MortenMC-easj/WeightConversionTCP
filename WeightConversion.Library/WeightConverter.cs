namespace WeightConversion
{

    public class ConversionResult
    {
        public double Value { get; set; }

        public string Unit { get; set; }


        public override string ToString()
        {
            return string.Format($"{Value} {Unit}");
        }
    }

    public enum ConversionType
    {
        TOGRAM,
        TOOUNCE
    }

    public static class WeightConverter
    {
       public static double ConvertToGram(double ounce)
       {
           return ounce * 28.34952;
       }
        public static double ConvertToOunce(double gram)
        {
            return gram * 0.03527396195;
        }


        public static ConversionResult Convert(ConversionType type, double value)
        {
            ConversionResult result = null;

            switch (type)
            {
                case ConversionType.TOGRAM:
                    result = new ConversionResult
                    {
                        Value = ConvertToGram(value),
                        Unit = "Gr."
                    };
                    break;
                case ConversionType.TOOUNCE:
                    result = new ConversionResult
                    {
                        Value = ConvertToOunce(value),
                        Unit = "Oz."
                    };
                    break;
            }
            // whatever you want to log

            return result;

        }








    }
}
