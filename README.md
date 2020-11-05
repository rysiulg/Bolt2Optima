Bolt2Optima (c) MARM.pl Sp. z o.o.

Konwerter danych z systemu Bolt (plik .csv) do Optima (.xml) dla Księgowości.


Wybrac identyfikatory ksiegowosci i nadawcy -te dane musza byc zgodne.

czasem pojawiaja sie nipy -stad opcja usuniecia "-" z nip

Zdarza się że brakuje adresów dla kontrahentów w pliku źródłowym *ulica, nr, miejscowoć, Kraj) -dlatego zrobione jest domyslnie aby dla takich przypadków wybrać kod i miasto. Jeśli tego nie chcemy -należy te pola po prostu wyczyścić -usunąć przed konwersją.
plik zrodlowy wybieramy po wcisnieciu OK lub dwa razy klikniemy w górne okno programu lub Drag&Drop -przeniesiemy plik csv na okno programu.

Po konwersji otwarte zostanie okno skąd był wczytywany plik do konwersji, w tym folderze będzie plik z nazwa na koncu -OPTIMA i rozszerzeniem .xml -to plik przekonwertowany dla Optimy oraz utworzone zostaje archiwum .zip zawierajace plik źródłowy i wynikowy ;) -oryginalnych tu nie usuwam.


Podczas importu:
DATA_WYSTAWIENIA jako Data
DATA_SPRZEDAZY jako Data_przejazdu
                (są rozbieżności min. 20-30minut, więc gdy zaczepi o północ...)

Nie przypisuję pól hash źródła więc optima ma kłopoty z identyfikacją więc przy ponownych i kolejnych importach:
-Kontrahenci sie nie duplikuja i też nie aktualizuje kontrahentów, więc przy kolejnej próbie importu do optimy jest błąd że kontrahent istnieje.
-gdy drugi raz puścimy import do optimy w rejestrze wskocza ponownie te same operacje itd.
-pewnie też powinny Tworzyć się płatności
Jak zajdzie potrzeba to powyższe poprawię, ale wstecz nie zadziała -nie znam algorytmu i nie są opublikowane schematy definicji generowania hash Optimy ;(

Są dwa adresy kontrahenta: adres odbiorcy i adres odbioru -ten pierwszy przypisuje do kontrahenta a ten drugi wrzuciłem do uwag

Kontrhenci Bez nipów są oznaczane jako detaliczne
Trafiają się też litery "ruskie" -zrobilem translator tych liter do polskawych jako symbol podmotu -jak by pojawiały sie w przyszłości komunikaty że nie można znaleźć kontrahenta z rejestrem VAT i kontrahent miał znaki zapytanie to prosiłbym o plik zrodlowy i ew. ten log importu do dopisania kolejnylich liter do podmiany na polskie w akronimie.

Ustawiony jest import jako "usługi"


Prosze o info i ew. wskazowki ;)

