// See https://aka.ms/new-console-template for more information

/*
  Streams erlauben es, Daten zu lesen und zu schreiben,
  ohne dass der gesamte Inhalt im Speicher gehalten werden muss.
 
  Dabei wird der Inhalt in Blöcken gelesen und geschrieben,
  ohne den Inhalt zu kennen
 
  Streams beinhalten Daten über die Datenquelle wie z.B.
  - Länge der Daten
  - Die Position im Stream
  - Ob der Stream schreibgeschützt ist und wir lesen oder schreiben dürfen
  - Menge (Capacity) an Speicherplatz, die für den Stream reserviert ist.
    - Größe des internen Puffers, der die Daten speichert. Wird mehr benötigt,
      wird der Puffer automatisch vergrößert.
 
  Alle Streams haben mindestens diese Properties, da sie alle von Stream erben
  Es gibt unterschiedliche Streams für unterschiedliche Datenquellen
  - MemomryStream
  - FileStream
  - NetworkStream
  - usw.
 * 
 */


//Schreiben in einen MemoryStream
using var stream = new MemoryStream();
Console.WriteLine("Before Write Position: " + stream.Position);
Console.WriteLine("Before Write Length: " + stream.Length);
Console.WriteLine("Before Write Capacity: " + stream.Capacity);

var data = "Hallo Tobias Janssen"u8.ToArray();
stream.Write(data, 0, data.Length);

Console.WriteLine("After Write Position: " + stream.Position);
Console.WriteLine("Before Write Length: " + stream.Length);
Console.WriteLine("Before Write Capacity: " + stream.Capacity);

//Lesen aus einem MemoryStream

//Wichtig ist, das wir zum lesen an der richtigen Position sind
stream.Seek(0, SeekOrigin.Begin);
Console.WriteLine("Before Read Position: " + stream.Position);

var buffer = new byte[stream.Length];
var numberOfBytesRead = stream.Read(buffer, 0, buffer.Length);

Console.WriteLine("After Read Position: " + stream.Position);
Console.WriteLine("Anzahl der Bytes: " + numberOfBytesRead);

var readString = Encoding.UTF8.GetString(buffer);
Console.WriteLine("Gelesener String: " + readString);


/* USING
  Using ruft am Ende der Nutzung Dispose auf
  Alle Streams implementieren IDisposable (unmanaged Ressource)
  Dadurch wird der reservierte Speicher wieder freigegeben
*/ 

/* PUFFER
 Streams verwenden einen Puffer um Daten quasi zwischenspeichern
 
 - Der Puffer steigert die effizienz, da nicht jedes Byte einzeln gelesen oder geschrieben wird
 - Dadurch steigt auch die Performance, da bei Write und Read keine teuren Zugriffe benötigt werden
 
 Beispiel: Stell dir vor, du willst 1.000 einzelne Bytes in eine Datei schreiben:
 Ohne Puffer:
   Jeder Aufruf von WriteByte() macht sofort einen Systemaufruf = 1.000 langsame IO-Operationen.
   Mit Puffer: Die Bytes landen erstmal im RAM-Puffer (zB. 4 KB groß).
               Erst wenn der voll ist (oder Flush() aufgerufen wird), schreibt der Stream gebündelt in die Datei = weniger IO-Aufwand.
 
 Das bedeutet fürs Lesen, dass der Stream ein Puffer füllt und dann der Puffer ausgelesen wird
   nie die Quelle selbst
 
 Der Puffer wird geleert (geflushed), wenn:
 - Flush() aufgerufen wird
  - Der Stream geschlossen wird
  - Der Puffer einfach voll ist
 
 Speicherort des Puffers ist der HEAP wird aber unterschiedlich verwaltet
 und vom Garbage Collector überwacht bzw. verwaltet

 Beispiel wie der Puffer funktioniert (visuell):
 [Stream.Write("ABCDEFGH")]
   
   1. Buffer leer:          [    ] (0/4 Bytes)
   2. Write "ABCD" → Buffer: [ABCD] (4/4 Bytes)
   3. Buffer ist voll → Flush to File
   4. Buffer leer:          [    ]
   5. Write "EFGH" → Buffer: [EFGH]
   6. Stream.Close() → Flush restlicher Buffer 
*/

/* IDISPOSABLE
 kommen wir auf das Wort unmanged Ressource zurück
 - Managed Ressourcen:
    Dies sind Ressourcen, die vom Garbage Collector (GC) der .NET-Runtime verwaltet werden.
    z.B. Klassen, die der GC verfolgen und automatisch aufräumen.
- unmanaged Ressourcen:
   z.B. dieses Thema hier, der GC hat keine Ahnung wie der Stream aufgeräumt wird

Beispiel 
*/

using var myDisposable = new MyDisposable();
myDisposable.DoSomething();