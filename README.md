Código:

````c#
Console.WriteLine("Antes de chamar 'Foo'...");
var foo = Foo();
Console.WriteLine("Depois de chamar 'Foo'...");

foreach (var element in foo)
{
     Console.WriteLine($"Antes de imprimir 'element' {element}");
     Console.WriteLine(element);
     Console.WriteLine($"Depois de imprimir 'element' {element}");
}

private static IEnumerable<int> Foo()
 {
      Console.WriteLine("Antes de iniciar o 'loop for'...");
      for(int i = 0; i <= 4; i++)
      {
          Console.WriteLine("Antes do 'yield return'...");
          yield return i;
          Console.WriteLine("Depois do 'yield return'...");
      }
      Console.WriteLine("Depois de encerrar o 'loop for'...");
}
````

Retorno:
````
Antes de chamar 'Foo'...
Depois de chamar 'Foo'...
Antes de iniciar o 'loop for'...
Antes do 'yield return 0'...
Antes de imprimir 'element' 0
0
Depois de imprimir 'element' 0
Antes do 'yield return 1'...
Antes de imprimir 'element' 1
1
Depois de imprimir 'element' 1
Depois do 'yield return 1'...
Antes do 'yield return 2'...
Antes de imprimir 'element' 2
2
Depois de imprimir 'element' 2
Depois do 'yield return 2'...
Antes do 'yield return 3'...
Antes de imprimir 'element' 3
3
Depois de imprimir 'element' 3
Depois do 'yield return 3'...
Antes do 'yield return 4'...
Antes de imprimir 'element' 4
4
Depois de imprimir 'element' 4
Depois do 'yield return 4'...
Depois de encerrar o 'loop for'...
````

Quando as pessoas leem o código, normalmente esperam esse retorno:
````
Antes de chamar 'Foo'...
Antes de iniciar o 'loop for'...
Antes do 'yield return 0'...
Depois do 'yield return 0'...
Antes do 'yield return 1'...
Depois do 'yield return 1'...
Antes do 'yield return 2'...
Antes do 'yield return 2'...
Depois do 'yield return 3'...
Antes do 'yield return 3'...
Depois do 'yield return 4'...
Antes do 'yield return 4'...
Depois de encerrar o 'loop for'...
Antes de imprimir 'element' 0
0
Depois de imprimir 'element' 0
Antes de imprimir 'element' 1
1
Depois de imprimir 'element' 1
Antes de imprimir 'element' 2
2
Depois de imprimir 'element' 2
Antes de imprimir 'element' 3
3
Depois de imprimir 'element' 3
Antes de imprimir 'element' 4
4
Depois de imprimir 'element' 4
````

Código escrito de igual ao código gerado pelo compilador do c#:

````c#
Console.WriteLine("Antes de chamar 'Foo'...");
var foo = Foo();
Console.WriteLine("Depois de chamar 'Foo'...");

// Implementação do foreach conforme o compilador faria
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

private static IEnumerable<int> Foo() => new MyEnumerable();

// Implementação do IEnumerable
public class MyEnumerable : IEnumerable<int>, IDisposable
{
    public IEnumerator<int> GetEnumerator() => new MyEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => new MyEnumerator();

    public void Dispose() { }
}

// Implementação do IEnumerator
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
````

### O que a gente ganha com essa maneira de implementação:

Dessa maneira podemos trabalhar com lotes de dados e obter resultados parciais sem precisar esperar a execução de um método inteiro para ai sim começar a trabalhar com esses dados. Perceba que o print do elementos acontece sem que o for termine, assim que for retorna um dado, já printamos ele, agora imagine isso em um serviço mais complexo, poderiamos pegar dados e ja executar, ao invés de esperar todos os dados e só então executar, gerando mais performance.