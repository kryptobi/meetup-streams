
# 🔄 .NET Streams – Schreiben, Lesen, Puffern & Verwalten

Dieses Projekt zeigt die Verwendung von **Streams** in C# mit einem Fokus auf:

- effizientes Schreiben und Lesen von Daten
- Verwendung von Puffern
- den Lebenszyklus von Streams über `using` und `IDisposable`
- Unterscheidung von Managed vs. Unmanaged Ressourcen

---

## 📚 Was sind Streams?

Streams ermöglichen das Lesen und Schreiben von Daten, **ohne den gesamten Inhalt im Speicher zu halten**. Stattdessen wird blockweise gearbeitet – effizient und ressourcenschonend.

### 🔍 Eigenschaften von Streams:

- Zugriff auf Datenquelle ohne Kenntnis des gesamten Inhalts
- Informationen über:
  - **Länge der Daten**
  - **Aktuelle Position im Stream**
  - **Lesen/Schreiben erlaubt?**
  - **Kapazität** (intern reservierter Speicher)

### 🔌 Typen von Streams

Streams können viele Datenquellen kapseln:

- `MemoryStream` – Arbeit im RAM
- `FileStream` – Zugriff auf Dateien
- `NetworkStream` – Netzwerkdaten
- Weitere: `CryptoStream`, `BufferedStream`, `GZipStream`, ...

Alle Streams erben von der Basisklasse `System.IO.Stream`.

---

## 🧪 Beispiel: Arbeiten mit einem `MemoryStream`

```csharp
using var stream = new MemoryStream();
Console.WriteLine("Before Write Position: " + stream.Position);
Console.WriteLine("Before Write Length: " + stream.Length);
Console.WriteLine("Before Write Capacity: " + stream.Capacity);

var data = "Hallo Tobias Janssen"u8.ToArray();
stream.Write(data, 0, data.Length);

Console.WriteLine("After Write Position: " + stream.Position);
Console.WriteLine("After Write Length: " + stream.Length);
Console.WriteLine("After Write Capacity: " + stream.Capacity);

// Lesen
stream.Seek(0, SeekOrigin.Begin);
var buffer = new byte[stream.Length];
var numberOfBytesRead = stream.Read(buffer, 0, buffer.Length);
var readString = Encoding.UTF8.GetString(buffer);
Console.WriteLine("Gelesener String: " + readString);
```

---

## 📦 Was ist ein **Puffer**?

Streams verwenden einen **internen RAM-Puffer**, um Zugriffe effizienter zu machen.

### 📈 Vorteile:

- Weniger Systemaufrufe → bessere Performance
- Beispiel: Statt 1.000x `WriteByte()` → 1x gebündelt schreiben

### 🔄 Schreib-Vorgang im 4-Byte-Puffer (Visualisiert):

```
[Stream.Write("ABCDEFGH")]

1. Buffer leer:           [    ] (0/4 Bytes)
2. Write "ABCD" → Buffer: [ABCD] (4/4 Bytes)
3. Buffer ist voll → Flush to File
4. Buffer leer:           [    ]
5. Write "EFGH" → Buffer: [EFGH]
6. Stream.Close() → Flush restlicher Buffer
```

### 📍 Speicherort:

- Der Puffer liegt im **Managed Heap**
- Verwaltet vom Garbage Collector – **aber der Inhalt selbst kann unmanaged sein** (z. B. File-Handles, Netzwerkressourcen)

---

## ☂️ `using` & `IDisposable`

Streams verwenden `IDisposable`, da sie **unmanaged Ressourcen** verwalten.

```csharp
using var stream = new FileStream(...); // impliziert stream.Dispose()
```

### 🧼 Warum `Dispose()` wichtig ist:

- Gibt Dateihandles, Speicherbereiche etc. wieder frei
- Verhindert Leaks bei langen Laufzeiten
- `using` ruft automatisch `Dispose()` auf, auch bei Exceptions

---

## 🧨 Unterschied: Managed vs Unmanaged

| Typ                 | Beschreibung                                       |
|---------------------|----------------------------------------------------|
| **Managed**         | Wird vom GC automatisch aufgeräumt                 |
| **Unmanaged**       | Muss manuell aufgeräumt werden (`Dispose`)         |

Unmanaged Beispiele:  
- Datei-Handles  
- Netzwerkverbindungen  
- Datenbankverbindungen  
- Streams!

---

## 🧑‍💻 Beispiel für eigenes `IDisposable`-Objekt:

```csharp
class MyDisposable : IDisposable
{
    public void DoSomething() => Console.WriteLine("Doing work...");

    public void Dispose() => Console.WriteLine("Cleanup done.");
}

using var myDisposable = new MyDisposable();
myDisposable.DoSomething();
```

### 🪚 Ausgabe:

```
Doing work...
Cleanup done.
```

---

## ✅ Fazit

- Verwende Streams, um effizient große Daten zu verarbeiten
- Nutze `using` oder `Dispose()`, um Ressourcen korrekt zu beenden
- Pufferung ist entscheidend für Performance
- Achte auf Unterschiede zwischen Managed und Unmanaged Ressourcen

---

> 🔗 Weitere Infos: [.NET Docs – Streams](https://learn.microsoft.com/en-us/dotnet/standard/io/)

