using System;

namespace CompanyB {
    public class BetterPhone : CompanyA.Phone {
        public new void Dial() {
            Console.WriteLine("BetterPhone.Dial");
            EstablishConnection();
            base.Dial();
        }

        protected new virtual void EstablishConnection() {
            Console.WriteLine("BetterPhone.EstablishConnection");
            // 在这里执行建立连接的操作
        }
    }
}
