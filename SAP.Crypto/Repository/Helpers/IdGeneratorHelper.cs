namespace SAP.Crypto.Services.Helpers
{
    public static class IdGeneratorHelper
    {
        private const int Min = 0xA0000;
        private const int Max = 0xFFFF9;
        private static int _value = Min - 1;

        public static int NextId()
        {
            if (_value < Max)
            {
                _value++;
            }
            else
            {
                _value = Min;
            }
            return _value;
        }
    }
}
