using System;

namespace WebApiTest.Common
{
    public static class AgeHelper
    {
        public static int GetAge(DateTime date)
        {
            return (new DateTime(1, 1, 1) + (DateTime.Now.Date - date)).Year - 1;
        }

        public static int GetAproximateAge(int birthYear, int birthMonth)
        {
            var birth = new DateTime(birthYear, birthMonth, 1);
            return (new DateTime(1, 1, 1) + (DateTime.Now.Date - birth)).Year - 1;
        }

    }
}
