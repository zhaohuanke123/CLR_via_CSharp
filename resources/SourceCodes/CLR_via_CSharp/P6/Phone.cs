using System;

namespace CompanyA
{
    public class Phone
    {
        public void Dial()
        {
            Console.WriteLine("Phone.Dial");
            EstablishConnection();
            // 在这里执行拨号操作
        }

        protected virtual void EstablishConnection()
        {
            Console.WriteLine("Phone.EstablishConnection");
            // 在这里执行建立连接的操作
        }
    }
}