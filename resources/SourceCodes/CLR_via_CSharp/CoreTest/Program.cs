namespace CoreTest
{

    public class Program
    {
        {
            const int a = 100000;

            for (int i = 0; i < 100; i++)
            {

                using (new PerformanceTester(nameof(TestPoint), TestPoint, a))
                {

                };
                using (new PerformanceTester(nameof(TestPoint1), TestPoint1, a))
                {

                };
            }
        }
    }
}
