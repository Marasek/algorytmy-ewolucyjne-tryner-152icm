using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdgadnijTekst_20
{
    class Program
    {
        static void Main(string[] args)
        {
            StrGen pom;
            Console.WriteLine("Wprowadź tekst: ");
            //haslo - tekst do odgadnięcia
            string haslo = Console.ReadLine();
            //tworzymy obiekt klasy StrGen (generujemy losowe znaki o długości hasła).
            StrGen zagadka = new StrGen(haslo.Length);
            //obliczamy funkcję przystosowania dla zagadki i wyświetlamy wygenerowane dane
            int fitnes = zagadka.Wysw(haslo);
            // szukamy rozwiązania do momentu aż wszystkie znaki będą poprawne (fitnes = haslo.Length)
            while (fitnes < haslo.Length)
            {
                //tworzymy zmutowany obiekt
                pom = zagadka.Mutacja();
                //jeżeli funkcja przystosowania jest lepsza, zamieniamy go z oryginałem i wyświetlamy
                if (pom.Fitnes(haslo) > fitnes)
                {
                    zagadka = pom;
                    fitnes = zagadka.Wysw(haslo);
                }
            }
            Console.ReadKey();
        }
    }


    class StrGen
    {
        // z tych znaków będzie tworzone rozwiązanie
        static string ZbGen = " aąbcćdeęfghijklłmnoópqrsśtuvwxyzźżABCĆDEFGHIJKLMNOÓPQRSŚTUVWXYZŹŻ!.";
        // obiekt do generowania liczb losowych
        public static Random r = new Random(System.DateTime.Now.Millisecond);
        // w danych będzie prechowywany tekst do obróbki
        List<char> dane = new List<char>();
        // konstruktor generuje losowy ciąg znaków o podanej długości
        public StrGen(int dl)
        {
            for (int i = 0; i < dl; i++)
                dane.Add(ZbGen[r.Next(ZbGen.Length)]);
        }

        // konstruktor wczytuje podany ciąg naków 
        public StrGen(List<char> dane)
        {
            this.dane.InsertRange(0, dane);
        }


        // tworzymy nowy obiekt który różni się jednym znakiem (losowa pozycja, losowy znak)
        public StrGen Mutacja()
        {
            StrGen pom = new StrGen(dane);
            pom.dane[r.Next(dane.Count)] = ZbGen[r.Next(ZbGen.Length)];
            return pom;
        }

        // wyświetlenie ciągu znaków i funkcji przystosowania (zwraca funkcję prystosowania - dla wygody)
        public int Wysw(string oryginal)
        {
            int pom;
            foreach (var item in dane) Console.Write(item);
            pom = Fitnes(oryginal);
            Console.WriteLine("  {0}", pom);
            return pom;
        }
        // funkcja przystosowania podaje ile znaków pasuje do oryginału
        public int Fitnes(string oryginal)
        {
            int pom = 0, i = 0;
            foreach (var item in dane) if (item == oryginal[i++]) pom++;
            return pom;
        }
    }
}
