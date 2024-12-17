using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace AssemblyLoadTest.cs
{
    public class AssemblyLoadTest
    {
        public static void Main()
        {
            // TestLoadWithAssemblyName();
            // TestLoadWithAssemblyString();
            // TestLoadWithInvalidAssembly();
            // TestLoadFromWithValidPath();
            // TestLoadFromWithInvalidPath();
            // TestReflectionOnlyLoadFrom();
            // TestEmbeddedResourceResolution();
        }


        // Test Case 1: Loading an assembly using LoadFrom with a valid path
        public static void TestLoadFromWithValidPath()
        {
            string path = @"Ch01-1-SomeLibrary.dll";
            try
            {
                Assembly assembly = Assembly.LoadFrom(path);
                Console.WriteLine("Loaded assembly: " + assembly.FullName);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to load assembly: " + ex.Message);
            }
        }

        // Test Case 2: Attempting to load an assembly using LoadFrom with an invalid path
        public static void TestLoadFromWithInvalidPath()
        {
            string invalidPath = @"invalid_path.dll";
            try
            {
                Assembly.LoadFrom(invalidPath);
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine("Expected exception: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unexpected exception: " + ex.Message);
            }
        }

        // Test Case 3: Using ReflectionOnlyLoadFrom to load an assembly
        public static void TestReflectionOnlyLoadFrom()
        {
            string path = @"Ch01-1-SomeLibrary.dll";
            try
            {
                Assembly assembly = Assembly.ReflectionOnlyLoadFrom(path);
                Console.WriteLine("Loaded assembly in reflection-only mode: " + assembly.FullName);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to load assembly: " + ex.Message);
            }
        }

        // Test Case 4: Resolving embedded resources as assemblies
        public static void TestEmbeddedResourceResolution()
        {
            AppDomain.CurrentDomain.AssemblyResolve += ResolveEventHandler;

            try
            {
                string assemblyName = "embedded_assembly_name";
                Assembly assembly = Assembly.Load(assemblyName);
                Console.WriteLine("Loaded embedded assembly: " + assembly.FullName);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to load embedded assembly: " + ex.Message);
            }
        }

        private static Assembly ResolveEventHandler(object sender, ResolveEventArgs args)
        {
            string dllName = new AssemblyName(args.Name).Name + ".dll";
            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            string resourceName =
                executingAssembly.GetManifestResourceNames().FirstOrDefault(rn => rn.EndsWith(dllName));

            if (resourceName == null) return null;

            using (Stream stream = executingAssembly.GetManifestResourceStream(resourceName))
            {
                byte[] assemblyData = new byte[stream.Length];
                stream.Read(assemblyData, 0, assemblyData.Length);
                return Assembly.Load(assemblyData);
            }
        }

        // Test loading an assembly using AssemblyName
        private static void TestLoadWithAssemblyName()
        {
            try
            {
                Console.WriteLine("Testing Assembly.Load with AssemblyName...");
                var assemblyName =
                    new AssemblyName(
                        "Ch01-1-SomeLibrary");
                Assembly loadedAssembly = Assembly.Load(assemblyName);

                Console.WriteLine($"Successfully loaded: {loadedAssembly.FullName}");
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"Failed to load assembly: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }
        }

        // Test loading an assembly using a string
        private static void TestLoadWithAssemblyString()
        {
            try
            {
                Console.WriteLine("Testing Assembly.Load with assembly string...");
                string assemblyString =
                    "Ch01-1-SomeLibrary";
                Assembly loadedAssembly = Assembly.Load(assemblyString);

                Console.WriteLine($"Successfully loaded: {loadedAssembly.FullName}");
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"Failed to load assembly: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }
        }

        // Test loading an invalid assembly
        private static void TestLoadWithInvalidAssembly()
        {
            try
            {
                Console.WriteLine("Testing Assembly.Load with invalid assembly...");
                string invalidAssemblyString =
                    "NonExistentAssembly, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
                Assembly loadedAssembly = Assembly.Load(invalidAssemblyString);

                Console.WriteLine($"Unexpectedly loaded: {loadedAssembly.FullName}");
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"Expected failure: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}