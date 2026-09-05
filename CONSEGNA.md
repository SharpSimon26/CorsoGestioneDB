# Esercitazione: Importazione, pulizia e analisi di un database vendite

## Scenario

Una società di vendita omnicanale ha esportato 15.000 record di ordini in un file CSV. Il file contiene dati realistici ma anche errori intenzionali: valori mancanti, formati incoerenti, duplicati, date impossibili, valori fuori intervallo e incongruenze tra campi.

## Obiettivi formativi

1. Importare un CSV in Microsoft SQL Server tramite SQL Server Management Studio (SSMS).
2. Progettare una tabella di staging e una struttura relazionale pulita.
3. Sviluppare uno script C# per validare, normalizzare e correggere i dati.
4. Registrare gli scarti e motivare le regole applicate.
5. Analizzare il database consolidato con query SQL.

## Parte 1 — Importazione in SQL Server

- Creare un database chiamato `CorsoGestioneDB`.
- Creare una tabella `StagingOrders` con colonne inizialmente permissive (`NVARCHAR`) per evitare che l'importazione si interrompa sugli errori.
- Importare `ordini_clienti_sporchi_15000.csv` con separatore `;` e codifica UTF-8.
- Verificare che siano state caricate esattamente 15.000 righe.
- Conservare il file originale e non modificarlo manualmente.

## Parte 2 — Profilazione e pulizia in C#

Realizzare una console application .NET che legga da `StagingOrders`, validi ogni record e scriva:

- i record validi nelle tabelle definitive;
- i record non correggibili in `RejectedRows`;
- le anomalie rilevate in `DataQualityLog`.

### Regole minime obbligatorie

- Rimuovere spazi iniziali/finali e uniformare maiuscole/minuscole.
- Validare e normalizzare email e telefono.
- Convertire date nei tipi SQL corretti; rifiutare date impossibili.
- `Quantity` deve essere compresa tra 1 e 20.
- `UnitPrice` deve essere numerico e maggiore di zero.
- `DiscountPct` deve essere compreso tra 0 e 100.
- `DeliveryDate` non può precedere `OrderDate`.
- Normalizzare valori categorici (stato, pagamento, regione, canale).
- Individuare duplicati tramite `OrderID` e tramite confronto dei campi principali.
- Ricalcolare `Revenue = Quantity * UnitPrice * (1 - DiscountPct/100) + ShippingCost`; segnalare differenze superiori a 0,01 euro.
- Usare transazioni e query parametrizzate.

## Modello dati consigliato

- `Customers(CustomerID, FirstName, LastName, Email, Phone, City, Province, Region, SignupDate)`
- `Products(ProductCode, ProductName, Category)`
- `Orders(OrderID, OrderDate, CustomerID, PaymentMethod, SalesChannel, OrderStatus, DeliveryDate)`
- `OrderLines(OrderID, ProductCode, Quantity, UnitPrice, DiscountPct, ShippingCost, Revenue)`
- `RejectedRows(RejectID, SourceRowNumber, RawData, Reason, RejectedAt)`
- `DataQualityLog(LogID, SourceRowNumber, FieldName, OriginalValue, CleanValue, RuleCode, LoggedAt)`

## Parte 3 — Analisi SQL

Produrre query, viste o stored procedure per rispondere almeno alle seguenti domande:

1. Fatturato mensile e variazione percentuale mese su mese.
2. Top 10 prodotti per fatturato e per quantità.
3. Fatturato e numero ordini per regione.
4. Valore medio ordine per canale di vendita.
5. Tasso di reso e annullamento per categoria.
6. Clienti con maggiore spesa totale e frequenza di acquisto.
7. Tempo medio di consegna per regione e canale.
8. Impatto degli sconti sul fatturato e sul volume.
9. Individuazione di possibili outlier di prezzo o quantità.
10. Confronto tra fatturato importato e fatturato ricalcolato.

## Consegna richiesta

- Script SQL di creazione database e tabelle.
- Progetto C# compilabile.
- Script SQL delle analisi.
- Breve relazione (2-4 pagine) con regole di pulizia, numero di anomalie per tipo, record scartati e principali risultati dell'analisi.

## Valutazione (100 punti)

- Importazione e struttura SQL: 20
- Qualità e robustezza del codice C#: 30
- Controlli di qualità e tracciabilità: 20
- Analisi SQL: 20
- Documentazione e chiarezza: 10
