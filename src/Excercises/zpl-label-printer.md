# Drukarka etykiet ZPL

## Opis
Firma produkcyjna zakupiła drukarkę etykiet i potrzebuje biblioteki do drukowania etykiet z poziomu C#. Drukarka obsługuje popularny format ZPL (Zebra Programming Language).


Przykładowy kod w C# do drukowania w sieci po protokole TCP wygląda następująco:

``` csharp
 TcpClient tcpClient = new TcpClient();
 tcpClient.Connect(ipAdress, port);

 var stream = new StreamWriter(tcpClient.GetStream());
 stream.Write(content);
 stream.Flush(); 
 stream.Close();
 tcpClient.Close();
```

Niestety nie mamy fizycznego dostępu do drukarki.



## Zadanie

Przygotuj bibliotekę `LabelGenerator`, która umożliwi drukowanie etykiet w formacie _ZPL_.



## Wymagania

1. **Metoda do drukowania pola na etykiecie** - umożliwia wydruk tekstu

- Kod ZPL
```
^XA
^FDHello World^FS
^XZ
```

2. **Pusty tekst** powinien rzucać wyjątek `InvalidArgumentException`.
3. **Metoda do ustawienia położenia pola** - umożliwia określenie pozycji x, y 

- Kod ZPL
```
^XA
^FO50,50
^FDHello World^FS
^XZ
```
4. **Przekroczenie limitów** (np. położenie poza obszarem etykiety) powinno rzucać wyjątek.
5. **Metoda do separatora pola** Należy dodać wsparcie dla separatora `^FS`
6. **Zmiana rozmiaru czcionki**

- Kod ZPL
```
^XA
^ADN,36,20^FDYour Name^FS
^XZ
```

7. **Metoda do drukowania kodu kreskowego w formacie Code 39**

- Kod ZPL
```
^XA
^B3N,N,100,Y,N
^FD123456^FS
^XZ
```

## Wskazówka:
- Można skorzystać z Online Viewer https://labelary.com/viewer.html
