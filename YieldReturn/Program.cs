using System;
using System.Collections;
using System.Collections.Generic;

namespace YieldReturn
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Antes de chamar 'Foo'...");
            var foo = Foo();
            Console.WriteLine("Depois de chamar 'Foo'...");

            using (var enumerator = foo.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    var element = enumerator.Current;
                    Console.WriteLine($"Antes de imprimir 'element' {element}");
                    Console.WriteLine(element);
                    Console.WriteLine($"Depois de imprimir 'element' {element}");
                }
            }

            // foreach (var element in foo)
            // {
            //     Console.WriteLine($"Antes de imprimir 'element' {element}");
            //     Console.WriteLine(element);
            //     Console.WriteLine($"Depois de imprimir 'element' {element}");
            // }
        }

        private static IEnumerable<int> Foo() => new MyEnumerable();
        
        // private static IEnumerable<int> Foo()
        // {
        //      Console.WriteLine("Antes de iniciar o 'loop for'...");
        //      for(int i = 0; i <= 4; i++)
        //      {
        //          Console.WriteLine("Antes do 'yield return'...");
        //          yield return i;
        //          Console.WriteLine("Depois do 'yield return'...");
        //      }
        //      Console.WriteLine("Depois de encerrar o 'loop for'...");
        // }
    }
    
    public class MyEnumerable : IEnumerable<int>, IDisposable
    {
        public IEnumerator<int> GetEnumerator() => new MyEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => new MyEnumerator();

        public void Dispose() { }
    }
    
    public class MyEnumerator : IEnumerator<int>
    {
        public MyEnumerator()
        {
            Console.WriteLine("Antes de iniciar o 'loop for'...");
        }
        
        private int _current = -1;
        public int Current => _current;

        object IEnumerator.Current => _current;

        public bool MoveNext()
        {
            if (_current > 0)
            {
                Console.WriteLine($"Depois do 'yield return {_current}'...");
            }
            
            if (_current >= 4)
            {
                return false;
            }
            
            _current++;
            Console.WriteLine($"Antes do 'yield return {_current}'...");

            return true;
        }

        public void Reset()
        {
            _current = -1;
        }

        public void Dispose()
        {
            Console.WriteLine("Depois de encerrar o 'loop for'...");
        }
    }
}