# ASPLabb2
# Vad du ska göra

### Sätta grunderna

- [X]  Skapa ett nytt projekt i VS och skapa fyra modeller → Lärare, Student, Klass (ex. 3A), Kurs (ex. Matematik).
- [X]  Anpassa relationen mellan tabellerna → till exempel (en lärare har en eller flera kurser)
- [X]  Bygga DB till modeller och fylla på data i alla tabeller med hjälp av Entity Framework.
- [X]  Du behöver en meny för för varje funktion och när vi testar program funktionalitet
    
    Tips: Du kan bygga den väldigt enkelt med exempelvis en Switch och ett användar-input som tar en siffra för varje alternativ. Eller skapa en [asp.net](http://asp.net) core app med menyer.
    

### Funktionalitet i systemet

Dessa ska finnas som olika funktioner där en av dessa körs genom att välja ett alternativ i menyn.

**OBS!** Alla frågor mot databasen ska göras via Entity Framework. 

- [X]  Hämta alla lärare som undervisar i “programmering 1”
- [X]  Hämta alla elever med deras lärare, skriv ut både elevernas namn och namnet på alla lärare de har
- [X]  Hämta alla elever som läser “programmering 1” och skriv ut deras namn samt vilka lärare de har i den kursen
- [X]  Editera ett ämne från “programmering 2” till “OOP”
- [X]  Uppdatera en elevs lärare i “programmering1” från Reidar till Tobias.
