namespace TestISerializable
{
    using System;
    using System.IO;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Formatters.Binary;

// 实现 ISellerInfo 接口
    [Serializable]
    public class SellerInfo : ISerializable
    {
        public string SellerName { get; set; }
        public string SellerAddress { get; set; }

        // 构造函数
        public SellerInfo(string name, string address)
        {
            SellerName = name;
            SellerAddress = address;
        }

        // 实现 ISerializable 接口的 GetObjectData 方法
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            // 使用 AddValue 添加字段信息
            info.AddValue("SellerName", SellerName);
            info.AddValue("SellerAddress", SellerAddress);
        }

        // 构造函数用于反序列化
        protected SellerInfo(SerializationInfo info, StreamingContext context)
        {
            // 通过 AddValue 加载字段
            SellerName = info.GetString("SellerName");
            SellerAddress = info.GetString("SellerAddress");
        }
    }

// 包含 SellerInfo 类型的类
    [Serializable]
    public class ProductInfo : ISerializable
    {
        public string ProductName { get; set; }
        public double ProductPrice { get; set; }
        public SellerInfo SellerDetails { get; set; }

        // 构造函数
        public ProductInfo(string productName, double productPrice, SellerInfo sellerDetails)
        {
            ProductName = productName;
            ProductPrice = productPrice;
            SellerDetails = sellerDetails;
        }

        // 实现 ISerializable 接口的 GetObjectData 方法
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            // 使用 AddValue 添加字段信息，SellerDetails 是一个实现了 ISerializable 的类型
            info.AddValue("ProductName", ProductName);
            info.AddValue("ProductPrice", ProductPrice);
            info.AddValue("SellerDetails", SellerDetails); // 格式化器会自动调用 SellerInfo 的 GetObjectData
        }

        // 构造函数用于反序列化
        protected ProductInfo(SerializationInfo info, StreamingContext context)
        {
            // 通过 AddValue 加载字段
            ProductName = info.GetString("ProductName");
            ProductPrice = info.GetDouble("ProductPrice");
            SellerDetails = (SellerInfo)info.GetValue("SellerDetails", typeof(SellerInfo)); // 格式化器自动处理 SellerInfo 的反序列化
        }
    }

    class Program
    {
        static void Main()
        {
            // 创建 SellerInfo 和 ProductInfo 对象
            SellerInfo seller = new SellerInfo("John Doe", "123 Main St.");
            ProductInfo product = new ProductInfo("Laptop", 1200.50, seller);

            // 序列化对象到文件
            string filePath = "productInfo.dat";
            SerializeObject(product, filePath);

            // 反序列化对象
            ProductInfo deserializedProduct = DeserializeObject(filePath);

            // 输出反序列化后的内容
            Console.WriteLine($"Product Name: {deserializedProduct.ProductName}");
            Console.WriteLine($"Product Price: {deserializedProduct.ProductPrice}");
            Console.WriteLine($"Seller Name: {deserializedProduct.SellerDetails.SellerName}");
            Console.WriteLine($"Seller Address: {deserializedProduct.SellerDetails.SellerAddress}");
        }

        // 序列化对象
        static void SerializeObject(object obj, string filePath)
        {
            IFormatter formatter = new BinaryFormatter();
            using (Stream stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                formatter.Serialize(stream, obj);
            }

            Console.WriteLine("Object serialized successfully.");
        }

        // 反序列化对象
        static ProductInfo DeserializeObject(string filePath)
        {
            IFormatter formatter = new BinaryFormatter();
            using (Stream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                return (ProductInfo)formatter.Deserialize(stream);
            }
        }
    }
}