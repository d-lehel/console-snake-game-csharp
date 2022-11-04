using System;
using System.Collections.Generic;
using System.Threading;


// utoljara szerkesztettem: 2020 / okt / 15
// eszlelt hibak, etel melett elmegy/kanyarodik etel fele eltunik ?!??!?!?!

namespace Snake
{
    class Program
    {
        class Jatek
        {

            Megjelenites megjelenit = new Megjelenites();

            public int kigyo_meret = 0;
            public bool kigyo_elet = true;
            public char irany = 'j';
            public int sebesseg = 100;
            public string kigyo_szine = "feher";

            public struct koordinata
            {
                public int x;
                public int y;
            }

            Random r = new Random();


            public koordinata elelem_pozicio = new koordinata();


            public Queue<koordinata> kigyo_teste = new Queue<koordinata>(); // teljes teste

            public koordinata kigyo_testresz = new koordinata(); // kelle e? valamelyiket torolnom kell
            public koordinata kigyo_megjelenik = new koordinata(); // ahol meg kell jelenjen
            public koordinata kigyo_eltunik = new koordinata(); // ahonnan el kell tunjon

            public koordinata seged_valtozo = new koordinata(); // kell e?

            public void kigyo_gyorsul()
            {
                sebesseg -= 2;
            }
            public void kigyo_novekszik()
            {
                kigyo_testresz.x = kigyo_megjelenik.x;
                kigyo_testresz.y = kigyo_megjelenik.y;
                kigyo_teste.Enqueue(kigyo_testresz);
                kigyo_meret++;
            }

            public void elelem_general()
            {
                do
                {
                    elelem_pozicio.y = r.Next(0, 25);
                    elelem_pozicio.x = r.Next(0, 40);
                }
                while (test_helyzet()); // nem a testenel probalja megjeleniteni

                megjelenit.rajzol(elelem_pozicio.x, elelem_pozicio.y, "voros");

            }

            public bool test_helyzet()
            {
                foreach (koordinata e in kigyo_teste)
                {
                    seged_valtozo = e;
                    if (seged_valtozo.x == elelem_pozicio.x && seged_valtozo.y == elelem_pozicio.y)
                    {
                        return true;
                    }
                }
                return false;
            }

            public void kezodo_pozicio()
            {
                this.kigyo_teste.Clear(); // teljes testet torolom

                // letrehozom az alapertelmezett testet / kezdo koordinata

                seged_valtozo.x = 20;
                seged_valtozo.y = 15;

                kigyo_megjelenik.x = 20;
                kigyo_megjelenik.y = 15;

                kigyo_teste.Enqueue(kigyo_megjelenik);
                kigyo_teste.Enqueue(kigyo_megjelenik);
                kigyo_teste.Enqueue(kigyo_megjelenik);
                kigyo_teste.Enqueue(kigyo_megjelenik);
                kigyo_teste.Enqueue(kigyo_megjelenik);
            }

            public bool kigyo_testhelyzet_ellenorziz() ///// ittt a baj
            {

                foreach (koordinata e in kigyo_teste)
                {
                    seged_valtozo = e;
                    if (seged_valtozo.x == kigyo_megjelenik.x && seged_valtozo.y == kigyo_megjelenik.y)
                    {
                        return false;
                    }
                }               

                return true;
            }

            public bool fal_ellenoriz() // palya szelet ellenorzom
            {

                if (kigyo_megjelenik.x == -1 || kigyo_megjelenik.y == -1 || kigyo_megjelenik.x == 40 || kigyo_megjelenik.y == 25)
                    return false;
                else return true;

            }
            public void kigyo_mozgas()
            {
                switch (irany)
                {
                    case 'b':
                        {
                            kigyo_megjelenik.x--;
                            break;
                        }
                    case 'j':
                        {
                            kigyo_megjelenik.x++;
                            break;
                        }
                    case 'f':
                        {
                            kigyo_megjelenik.y--;
                            break;
                        }
                    case 'l':
                        {
                            kigyo_megjelenik.y++;
                            break;
                        }
                }               
            }

            public void kigyo_vegso_mozgas()
            {
                kigyo_teste.Enqueue(this.kigyo_megjelenik);
                this.kigyo_eltunik = kigyo_teste.Dequeue();
            }
            public void kigyo_mozgasirany()
            {
                manualis(); // modositja amennyiben van bevitel
                switch (irany)
                {
                    case 'b':
                        {
                            irany = 'b';
                            break;
                        }
                    case 'j':
                        {
                            irany = 'j';
                            break;
                        }
                    case 'f':
                        {
                            irany = 'f';
                            break;
                        }
                    case 'l':
                        {
                            irany = 'l';
                            break;
                        }
                }

            }

            public void manualis()
            {

                Thread.Sleep(sebesseg); // jatek gyorsasaga

                if (Console.KeyAvailable) // amennyiben van leutott billentyu
                {
                    ConsoleKeyInfo key = Console.ReadKey();
                    switch (key.Key)
                    {
                        case ConsoleKey.LeftArrow:
                            {
                                if (irany != 'b' && irany != 'j')
                                {
                                    irany= 'b';
                                }
                                break;

                            }
                        case ConsoleKey.RightArrow:
                            {
                                if (irany != 'b' && irany != 'j')
                                {
                                    irany = 'j';
                                }
                                break;
                            }
                        case ConsoleKey.UpArrow:
                            {
                                if (irany != 'f' && irany != 'l')
                                {
                                    irany = 'f';
                                }
                                break;
                            }

                        case ConsoleKey.DownArrow:
                            {
                                if (irany != 'f' && irany != 'l')
                                {
                                    irany = 'l';
                                }
                                break;
                            }
                    }
                }
                

            }
            public void kigyo_megjelenit()
            {

                megjelenit.rajzol(kigyo_megjelenik.x, kigyo_megjelenik.y, kigyo_szine); // megjelenitem a fejet
                megjelenit.rajzol(kigyo_eltunik.x, kigyo_eltunik.y, "fekete"); // eltuntetem a farkat
            }

            public void meghal()
            {

                kigyo_elet = false;
                Console.Beep();
                for (int i = 0; i < 2000; i++)
                {
                    elelem_general();
                }
                Thread.Sleep(1000);

            }
        }
        class Megjelenites
        {

            public void rajzol(int x, int y, string szin)
            // fontos!!! 0 = (0,0+1), 1 = (2,2+1)
            {
                x *= 2;
                switch (szin)
                {
                    case "voros":
                        {
                            // x#0 pixel
                            Console.SetCursorPosition(x, y);
                            Console.BackgroundColor = ConsoleColor.DarkRed;
                            Console.Write(" ");

                            // x#1 pixel;
                            Console.SetCursorPosition(x + 1, y);
                            Console.Write(" ");
                            Console.BackgroundColor = ConsoleColor.Black;
                            break;
                        }
                    case "fekete":
                        {
                            Console.SetCursorPosition(x, y);
                            Console.Write(" ");

                            Console.SetCursorPosition(x + 1, y);
                            Console.Write(" ");
                            break;
                        }
                    case "zold":
                        {
                            Console.SetCursorPosition(x, y);
                            Console.BackgroundColor = ConsoleColor.DarkGreen;
                            Console.Write(" ");
                            Console.SetCursorPosition(x + 1, y);
                            Console.Write(" ");
                            Console.BackgroundColor = ConsoleColor.Black;
                            break;
                        }
                    case "feher":
                        {
                            Console.SetCursorPosition(x, y);
                            Console.BackgroundColor = ConsoleColor.DarkGray;
                            Console.Write(" ");
                            Console.SetCursorPosition(x + 1, y);
                            Console.Write(" ");
                            Console.BackgroundColor = ConsoleColor.Black;
                            break;
                        }
                    case "kek":
                        {
                            Console.SetCursorPosition(x, y);
                            Console.BackgroundColor = ConsoleColor.DarkBlue;
                            Console.Write(" ");
                            Console.SetCursorPosition(x + 1, y);
                            Console.Write(" ");
                            Console.BackgroundColor = ConsoleColor.Black;
                            break;
                        }
                    case "sarga":
                        {
                            Console.SetCursorPosition(x, y);
                            Console.BackgroundColor = ConsoleColor.DarkYellow;
                            Console.Write(" ");
                            Console.SetCursorPosition(x + 1, y);
                            Console.Write(" ");
                            Console.BackgroundColor = ConsoleColor.Black;
                            break;
                        }

                }
            }
            public void kezdokepernyo()
            {
                Console.Title = "Snakes (c) Lehel";
                Console.SetWindowSize(80, 25);
                System.Console.CursorVisible = false;
                Console.SetCursorPosition(0, 6);
                Console.ForegroundColor = ConsoleColor.Green;

                Console.WriteLine("                   _________              __                  ");
                Console.WriteLine("                  /   _____/ ____ _____  |  | __ ____   ______");
                Console.WriteLine("                  \\_____  \\ /    \\\\__  \\ |  |/ // __ \\ /  ___/");
                Console.WriteLine("                  /        \\   |  \\/ __ \\|    <\\  ___/ \\___ \\ ");
                Console.WriteLine("                 /_______  /___|  (____  /__|_ \\\\ ___ >____  >");
                Console.WriteLine("                         \\/     \\/     \\/     \\/    \\/     \\/ \n");
                Console.Write("                     Nyomj egy gombot az inditashoz!!!");

                Console.ReadKey();
                Console.Clear();
            }

            public void pontszam(int meret)
            {
                Console.SetCursorPosition(5, 2);
                Console.Write("Pontszam: {0}", meret);
            }

            public void rekord(int rekord)
            {
                Console.SetCursorPosition(64, 2);
                Console.Write("Rekord: {0}", rekord);
            }


        }




        static void Main(string[] args)
        {
            int rekord=0;

            do // fo jatek ciklus meghat/ujraindit vegtelensegig
            {
                Jatek jatek = new Jatek();
                Megjelenites megjelenit = new Megjelenites();

                megjelenit.kezdokepernyo();
                jatek.kezodo_pozicio();
                jatek.elelem_general();

                megjelenit.pontszam(jatek.kigyo_meret);
                megjelenit.rekord(rekord);

                do // jatek ciklus halalig
                {
                    jatek.kigyo_mozgasirany();
                    jatek.kigyo_mozgas();


                    if (jatek.fal_ellenoriz() && jatek.kigyo_testhelyzet_ellenorziz())  //  valami hiba van
                    {
                        jatek.kigyo_vegso_mozgas();
                        jatek.kigyo_megjelenit();

                        if (jatek.elelem_pozicio.x == jatek.kigyo_megjelenik.x && jatek.elelem_pozicio.y == jatek.kigyo_megjelenik.y)
                        {
                            jatek.kigyo_novekszik();
                            jatek.kigyo_gyorsul();

                            if (jatek.kigyo_meret == 10)
                            {
                                jatek.kigyo_szine = "sarga";
                            }
                            else if (jatek.kigyo_meret == 20)
                            {
                                jatek.kigyo_szine = "zold";
                            }
                            else if (jatek.kigyo_meret == 30)
                            {
                                jatek.kigyo_szine = "kek";
                            }
                            else if (jatek.kigyo_meret == 40)
                            {
                                jatek.kigyo_szine = "voros";
                            }

                            megjelenit.pontszam(jatek.kigyo_meret);
                            megjelenit.rekord(rekord);

                            jatek.elelem_general();
                        }
                        else
                        {
                        }
                    }
                    else
                    {
                        if (jatek.kigyo_meret > rekord)
                        {
                            rekord = jatek.kigyo_meret;
                        }
                        else { }

                        jatek.meghal();
                    }
                }
                while (jatek.kigyo_elet);

                Console.Clear();
            }
            while (true);
        }
    }
}


