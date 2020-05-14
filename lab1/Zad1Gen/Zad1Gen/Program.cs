using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Program
{
    static void Main(string[] args)
    {
        //tworzymy populację czeroelementową (takie założenie w zadaniu)
        OperGenet popul = new OperGenet(4);

        //wyświetlamy ją
        popul.Wysw();
        //3 razy tworzymy nową populację i oglądamy wyniki tego działania
        for (int i = 0; i < 3; i++)
        {
            popul.NowaPop();
            Console.WriteLine();
            popul.Wysw();
        }
        Console.ReadKey();
    }
}

// Jest to element podstawowy  na którym będą prowadzone operacje genetyczne
class Osobnik : IComparable
{
    int[] Chromosom = new int[10];
    public static Random r = new Random(System.DateTime.Now.Millisecond);
    static int podz = 0;
    //tworzymy osobnika
    public Osobnik()
    {
        for (int i = 0; i < 10; i++)
        {
            Chromosom[i] = r.Next(100);
        }
    }
    //mutacja wybieramy losowo miejsce i losujemy wartość tyle razy ile podano (ile)
    public void Mutacja(int ile)
    {
        for (int i = 0; i < ile; i++)
        {
            Chromosom[r.Next(10)] = r.Next(100);
        }
    }

    //funkcja przystosowania 
    public int Fitness()
    {
        int sum = 0;
        int zn = 1;
        foreach (var item in Chromosom)
        {
            sum += zn * item;
            zn = -zn;
        }
        return Math.Abs(sum);
    }

    //wyświetlenie osobnika (chrmosomy plus funkcja fitness
    public void Wysw()
    {
        foreach (var item in Chromosom)
        {
            Console.Write("{0,2} ", item);
        }
        Console.WriteLine("  {0,4}", Fitness());
    }
    // tworzenie nowego osobnika poprzez krzyżowanie dwóch innych (punkt krzyżowania wybrany losowo)
    public static Osobnik operator +(Osobnik o1, Osobnik o2)
    {
        podz = r.Next(10);
        Osobnik pom = new Osobnik();
        for (int i = 0; i < podz; i++) pom.Chromosom[i] = o1.Chromosom[i];
        for (int i = podz; i < 10; i++) pom.Chromosom[i] = o2.Chromosom[i];
        return pom;
    }
    // tworzenie nowego osobnika poprzez krzyżowanie dwóch innych (punkt krzyżowania ten sam co w +) tylko odwrotnie niż w +
    public static Osobnik operator -(Osobnik o1, Osobnik o2)
    {
        Osobnik pom = new Osobnik();
        for (int i = 0; i < podz; i++) pom.Chromosom[i] = o2.Chromosom[i];
        for (int i = podz; i < 10; i++) pom.Chromosom[i] = o1.Chromosom[i];
        return pom;
    }
    //metoda potrzebna do sortowania populacji względem fitness
    public int CompareTo(object ob)
    {
        return (ob as Osobnik).Fitness() - Fitness();
    }
}


//tworzymy listę osobników (ilość jako paranetr w konstruktorze)
class OperGenet : List<Osobnik>
{
    public static Random l = new Random(System.DateTime.Now.Millisecond);
    public double[] przedz;
    public OperGenet(int ile)
    {
        for (int i = 0; i < ile; i++)
        {
            Add(new Osobnik());
        }
        Sort();
        OblPrzedz();
    }

    //oblicz przeedział potrzebny do losowania nowych osobników (ruletka)
    public void OblPrzedz()
    {
        przedz = new double[Count];
        double W = 0;
        for (int i = 0; i < Count; i++) W = (przedz[i] = this[i].Fitness() + W);
        for (int i = 0; i < Count; i++) przedz[i] = przedz[i] / W;
    }

    //mając przedział możemy powiedzieć który element będzie wybrany do krzyżowania
    public int nrEl(double w)
    {
        for (int i = 0; i < Count; i++)
            if (przedz[i] > w) return i;
        return -1;
    }


    // tworzenie nowej populacji (dodajemy tyle samo nowych elementów sortujemy i odrzucamy najgorsze)
    public void NowaPop()
    {
        int rozm = Count;
        double i, j;
        do
        {
            i = l.NextDouble();
            j = l.NextDouble();
            Add(this[nrEl(i)] + this[nrEl(j)]);
            Add(this[nrEl(i)] - this[nrEl(j)]);
        } while (Count < 2 * rozm);

        //wprowadziłem dwie mutacje jednokrotne
        for (int k = 0; k < 2; k++)
        {
            this[l.Next(Count)].Mutacja(1);
        }
        Sort();
        do
        {
            RemoveAt(Count - 1);
        } while (Count > rozm);
        OblPrzedz();
    }

    //wyświetlenie populacji
    public void Wysw()
    {
        foreach (var item in this)
        {
            item.Wysw();
        }
    }
}

