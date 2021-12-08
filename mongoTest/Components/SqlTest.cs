using System;
namespace mongoTest.Components
{
    public class SqlTest
    {
        private string name;
        private int charge;

        public SqlTest(string name, int charge)
        {
            this.name = name;
            this.charge = charge;
        }

        public SqlTest()
        {
            this.name = "fak";
            this.charge = 401;
        }
    }
}
