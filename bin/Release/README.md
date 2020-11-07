Bolt2Optima (c) MARM.pl Sp. z o.o.

Konwerter danych z systemu Bolt (plik .csv) do Optima (.xml) dla Księgowości.


Wybrac identyfikatory ksiegowosci i nadawcy -te dane musza byc zgodne.
-Aby dowiedzieć się jak i gdzie skonfigurować OPTIME do importu -kliknij dwukrotnie myszką na polu IDKSI podświetlone czerwonymi literami Jako identyfikator księgowości

Plik zrodlowy wybieramy po wcisnieciu OK lub dwa razy klikniemy w górne okno programu lub Drag&Drop -przeniesiemy plik csv na okno programu.



Kontrahenci dodawani są tylko z danymi w polach Optimy: NAZWA i Kraj jako Polska oraz ew. kod i miejscowość jeśli została taka wartość wpisana w oknie programu.
Kontrahent ma oznaczenie liczonego od cen brutto.
Kontrahenci Bez NIPów są oznaczane jako detaliczne
czasem pojawiaja sie NIPy -stad opcja usuniecia "-" z numerów NIP

Po konwersji otwarte zostanie okno skąd był wczytywany plik do konwersji, w tym folderze będzie plik z nazwa na koncu -OPTIMA i rozszerzeniem .xml -to plik przekonwertowany dla Optimy oraz utworzone zostaje archiwum .zip zawierajace plik źródłowy i wynikowy ;) -oryginalnych tu nie usuwam.

Podczas importu:
DATA_WYSTAWIENIA/SPRZEDAŻY i TERMIN PŁATNOŚCI jako Data w pliku importu
   Data_przejazdu (są rozbieżności min. 20-30minut, więc gdy zaczepi o północ...) jest dopisywana do pola Opis tak jak i dane adresów podjazdu i adresu docelowego.

Nie przypisuję pól hash źródła i optima ma kłopoty z identyfikacją więc przy ponownych i kolejnych importach:
-gdy drugi raz puścimy import do optimy w rejestrze wskocza ponownie te same operacje itd.
-pewnie też powinny Tworzyć się płatności
Jak zajdzie potrzeba to powyższe poprawię w miarę możliwości, ale wstecz nie zadziała -nie znam algorytmu i nie są opublikowane schematy definicji generowania hash Optimy ;(


Trafiają się też litery "ruskie" -zrobilem translator tych liter do polskawych jako symbol podmotu -jak by pojawiały sie w przyszłości komunikaty że nie można znaleźć kontrahenta z rejestrem VAT i kontrahent miał znaki zapytanie to prosiłbym o plik zrodlowy i ew. ten log importu do dopisania kolejnylich liter do podmiany na polskie w akronimie.


Prosze o info i ew. wskazowki ;)
W przypadku błędów proszę o plik źródłowy do analizy i najlepiej screenshot wysłany na e-mail b2ok@marm.pl


PS. Wersja bezpłatna wysyła logi statystyczne i analityczne.


