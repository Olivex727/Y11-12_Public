using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System.Linq;
using System.Text;
//Include Newtonsoft json -- When on VS

//===NOTES:
//
//Project progress: 46.75%
//+1.25% for each section in NPC
//+2% for the Top Ups
/*
| Module Library = 25% | Algorithims = 25% | Extras = 15% | Interface = 25% | File Management = 10% |
                                         ^
*/
//Current version: 0.8.3.1
//
//When on VS:
//- Complete setInterface()
//- Modify run() and print() with timers and buttons
//- Add file management system
//Required:
//- Banking system
//- Storage systems
//- Random events
//- Trade routes (can be included into the random events or into markets)
//After Base tasks complete (Further Ideas):
//- Crafting system
//- Production facilites (New NPCs tied to rescources)
//- Elections
//- Government NPCs
//- Interactive Map

namespace TradeGame
{
    class Program
    {
        public static List<Material> Materials = new List<Material>();

        public static List<Market> LocList = new List<Market>();

        public static List<NPC> Players = new List<NPC>();

        public static List<Facility> Locs = new List<Facility>();

        //Remove new Module(); for file management
        public static Module mod = new Module();

        public static string filename; //String Path, public

        public static void Main(string[] args)
        {
            //Creating new games and
            //storing new file paths on the transfer folder
            //will all be done by seperate launch program in VSMS

            //mod.setInterface('menu');
            //NewGameButton_Click(#){ newGame(NewBox.Text); }
            //LoadGameButton_Click(#){ loadGame(SaveList.Selected); }

            //loadGame();
            //if(time != 0){ createTurnOrder(); }
            //mod.runMod(Materials, LocList, Players); //Store lists to module class

            Console.WriteLine("Hello World!");
            Material Iron = new Material()
            {
                name = "Iron",
                type = "r",
                cost = 460.32,
                influx = 9,
                amount = 60,
                total = 10
            };
            Material Copper = new Material()
            {
                name = "Copper",
                type = "r",
                cost = 103.73,
                influx = 11,
                amount = 15,
                total = 16
            };
            Material Uranium = new Material()
            {
                name = "Uranium",
                type = "r",
                cost = 2374.19,
                influx = 4,
                amount = 3,
                total = 10
            };
            Materials.Add(Iron);
            Materials.Add(Copper);
            Materials.Add(Uranium);

            Material Base = new Material()
            {
                name = "Money",
                type = "c",
                cost = 1,
                influx = 0,
                amount = 10000,
                total = 12000,
                anchor = "Central"
            };
            Materials.Add(Base);

            Market Central = new Market()
            {
                name = "Central",
                type = "e",
                markup = 1.44,
                currency = Base
            };
            LocList.Add(Central);

            NPC Tenex = new NPC()
            {
                name = "Tenex",
                type = "c",
                intellegence = 72,
                location = "Central"
            };
            

            NPC Sciton = new NPC()
            {
                name = "Sciton",
                type = "c",
                intellegence = 95,
                location = "Central"
            };
            

            Facility Central1 = new Facility()
            {
                name = "Cen1s",
                type = "s",
                location = Central,
                mcost = 10.06,
                cost = 100,
                cap = 1000,
                mat = Uranium
            };
            Facility IronMine = new Facility()
            {
                name = "im-central",
                type = "p",
                location = Central,
                mcost = 49.67,
                cost = 78,
                cap = 200,
                mat = Iron
            };

            Locs.Add(Central1);
            Locs.Add(IronMine);
            Players.Add(Sciton);
            Players.Add(Tenex);

            foreach(NPC n in Players)
            {
                n.invent = new Dictionary<Material, int>();

                foreach(Material m in Materials){
                    n.invent.Add(m, 0);
                }
            }

            Tenex.manageInvent("add", Copper, 1);
            Tenex.manageInvent("add", Uranium, 7);
            Tenex.manageInvent("add", Base, 1000);
            Sciton.manageInvent("add", Iron, 70);
            Sciton.manageInvent("add", Base, 1000);

            IronMine.developProperty(Tenex);

            mod.increment(Materials, LocList, Players, Locs);

            mod.createTurnOrder();

            if(int.TryParse(Console.ReadLine(), out int x))
            {
                run(x);
            }
        }

        // Function, public void
        //Create new game file
        public static void newGame() { }

        // Function, public void
        //Saves game stats in designated files
        public static void saveGame() { }

        // Function, public void
        //Loads game stats in designated files
        public static void loadGame(string file = "") { }

        //Replace with timer (and button for players)
        public static void run(int period)
        {
            mod.per = period / 4;
            int x = 0;
            while (x < period)
            {
                x++;

                for(int i = 0; i < mod.order.Count() - 1; i++){
                    mod.takeTurn(mod.turn, mod.getClass(mod.turn)[0],mod.getClass(mod.turn)[1]);
                }
                foreach(NPC n in mod.Players)
                {
                    n.prevNet = n.netWorth();
                }
                mod.time += 0.25;

                if (x % (period / 10) == 0)
                {
                    Console.WriteLine("\n*** DAY: " + mod.time + " RUN: "+ x.ToString() + " ***\n");
                    mod.print();
                }
            }
        }


    }
    //Function Module, used to run everything not bound to the screen elements or file management
    //Requres inputs to help set up when first created
    //Module will be saved and loaded up
    public class Module
    {
        public double per { get; set; }

        public double time { get; set; } //Double, increments by 0.25, public
        //Length of time in days

        public int difficulty { get; set; } //Integer, public
        //Artificially marks up the intelegence of all NPCs

        public string turn { get; set; } //String, public
        //Determines who's turn it is (name)

        public bool playerturn { get; set; } //Bool, public
        //Checks if player is playing

        public Dictionary<int, string> order; //String[], public
        //an array of the order of turns

        public int ordern = 0;

        public int count = 0;

        public bool turnover { get; set; } //Bool, public
        //Checks if turn is complete

        public List<Material> Materials { get; set; }

        public List<Market> LocList { get; set; }

        public List<NPC> Players { get; set; }

        public List<Facility> Locs { get; set; }

        public Random rnd = new Random();

        public void increment(List<Material> m, List<Market> mr, List<NPC> p, List<Facility> l)
        {
            Materials = m;
            LocList = mr;
            Players = p;
            Locs = l;
        }

        //Replace with timer (and button for players)
        public void print()
        {
            Console.WriteLine(
                "--------------------------" +
                "\nMaterials (List): Type | Cost | Influx | Amount | Total | Inflation | Anchor |"
            );
            foreach (Material m in Materials)
            {
                int x = "Materials (List):".Length - (m.name + "("+ Math.Round(m.netWorth()).ToString() + ")").Length;
                string name = "";
                for (int i = 0; i < x; i++)
                {
                    name += " ";
                }
                x = 7 - m.influx.ToString().Length;
                string inf = "";
                for (int i = 0; i < x; i++)
                {
                    inf += " ";
                }
                x = 7 - m.amount.ToString().Length;
                string am = "";
                for (int i = 0; i < x; i++)
                {
                    am += " ";
                }
                x = 7 - m.cost.ToString().Length;
                string c = "";
                for (int i = 0; i < x; i++)
                {
                    c += " ";
                }
                Console.WriteLine
                (
                    (m.name + "(" + Math.Round(m.netWorth()).ToString() + ")") + name + " '" +
                    m.type + "'    " + c +
                    Math.Round(m.cost, 2).ToString() + inf +
                    m.influx.ToString() + "   " + am +
                    m.amount.ToString() + " " + am +
                    m.total.ToString() + "      " +
                    Math.Round(m.inflation, 2).ToString() + "     " +
                    m.anchor
                );
            }
            Console.WriteLine(
                "\n--------------------------" +
                "\nMarkets (List): Type | Cost | Currency | Anchor |"
            );
            foreach (Market m in LocList)
            {
                int x = "Markets (List):".Length - (m.name + "(" + Math.Round(m.netWorth(Locs)).ToString() + ")").Length;
                string name = "";
                for (int i = 0; i < x; i++)
                {
                    name += " ";
                }
                x = 7 - m.currency.name.Length;
                string a = "";
                for (int i = 0; i < x; i++)
                {
                    a += " ";
                }
                Console.WriteLine
                (
                    (m.name + "(" + Math.Round(m.netWorth(Locs)).ToString() + ")") + name + " '" +
                    m.type + "'    " +
                    Math.Round(m.markup, 2).ToString() + "     " +
                    m.currency.name + a + "   " +
                    m.anchor
                );

            }
            Console.WriteLine(
                "\n--------------------------" +
                "\nPlayers & NPCs (List): Type | Intellegence | Location | Sponsor |"
            );
            foreach (NPC m in Players)
            {
                int x = "Players & NPCs (List):".Length - (m.name + "(" + Math.Round(m.netWorth()).ToString() + ")").Length;
                string name = "";
                for (int i = 0; i < x; i++)
                {
                    name += " ";
                }
                x = 11 - m.location.Length;
                string l = "";
                for (int i = 0; i < x; i++)
                {
                    l += " ";
                }
                Console.WriteLine
                (
                    (m.name + "(" + Math.Round(m.netWorth()).ToString() + ")") + name + " '" +
                    m.type + "'         " +
                    m.intellegence.ToString() + l + "    " +
                    m.location  + "     " +
                    m.sponsor
                );
            }
            Console.WriteLine(
                "\n--------------------------" +
                "\nFacilites 'Locs' (List): Type | MCost | Cost | Cap | Location | Anchor |"
            );
            foreach(Facility m in Locs)
            {
                int x = "Facilites 'Locs' (List):".Length - (m.name + "(" + Math.Round(m.netWorth("worth")).ToString() + ")").Length;
                string name = "";
                for (int i = 0; i < x; i++)
                {
                    name += " ";
                }
                x = "MCost | ".Length - Math.Round(m.mcost, 2).ToString().Length;
                string mcost = "";
                for (int i = 0; i < x; i++)
                {
                    mcost += " ";
                }
                x = "Cost | ".Length - m.cost.ToString().Length;
                string cost = "";
                for (int i = 0; i < x; i++)
                {
                    cost += " ";
                }
                x = "Cap | ".Length - m.cap.ToString().Length;
                string cap = "";
                for (int i = 0; i < x; i++)
                {
                    cap += " ";
                }
                x = "Location | ".Length - m.location.name.Length;
                string l = "";
                for (int i = 0; i < x; i++)
                {
                    l += " ";
                }
                Console.WriteLine
                (
                    (m.name + "(" + Math.Round(m.netWorth("worth")).ToString() + ")") + name + " '" +
                    m.type + "'    " +
                    Math.Round(m.mcost, 2).ToString() + mcost +
                    m.cost.ToString() + cost +
                    m.cap.ToString() + cap +
                    m.location.name + l +
                    m.anchor
                );
            }
        }

        //Function, public void
        //Commences for every NPC, Market and Material run in a set of foreach loops
        //Makes the move for each object until over
        //if the type is player another function will be called
        public void takeTurn(string name, string classname, string type)
        {
            if (classname == "Material")
            {
                foreach (Material m in Materials)
                {
                    if (m.name == name)
                    {
                        //-----MATERIAL FLUX FORMULA-----
                        //Net amount: gain is positive (this is demand)
                        //double net = Math.Pow(m.amount, 2) / m.prevamount;

                        //Console.WriteLine(m.amount);
                        m.influx = rnd.Next((-1 * m.amount / 2) + 1, 1000);

                        //D/S = P
                        //m.inflation = net / m.amount;
                        if (m.prevamount != 0)
                        {
                            m.inflation = Convert.ToDouble(m.prevamount) / Convert.ToDouble(m.amount);
                        }
                        else
                        {
                            m.inflation = 1;
                        }



                        double x = -0.1;
                        double net;
                        //Add Currency fluctuations
                        if (type != "r"){
                            //Change/Test, x = ma.netWorth(Locs), no material
                            foreach (Market ma in LocList){if(m.anchor == ma.name){ x = ma.netWorth(Locs); }}
                            foreach (NPC ma in Players) { if (m.anchor == ma.name) { x = ma.netWorth(m); } }
                            if(m.anchor != "" && m.anchornet > 0)
                            {
                                net = x / m.anchornet;
                                //Calculate average
                                m.inflation = (m.cost * m.inflation + net) / (1 + m.cost);
                                //Console.WriteLine(x.ToString() + " | " + m.inflation.ToString());
                            }
                        }
                        else
                        {
                            foreach (Facility ma in Locs) { if (m == ma.mat && ma.type != "s") { x = ma.netWorth("worth"); } }
                            if (m.anchornet > 0)
                            {
                                net = x / m.anchornet;
                                //Calculate average
                                m.inflation = (m.cost * m.inflation + net )/ (1 + m.cost);
                                //Console.WriteLine(x.ToString() + " | " + m.inflation.ToString());
                            }
                        }

                        //Add Influxes, Inflations and previous amounts
                        m.cost = m.cost * (m.inflation);
                        m.prevamount = m.amount;
                        m.previnflation = m.inflation;
                        m.anchornet = x;
                        //Remove when adding Facilites
                        m.amount += m.influx;
                        m.total += m.influx;
                    }
                }
            }
            else if (classname == "Market")
            {
                foreach (Market m in LocList)
                {
                    if (m.name == name)
                    {
                        //
                        //-----ADD MARKET FLUX FORMULA HERE-----
                        //
                        double net = 0;
                        int x = 0;
                        //Take average of all facility production
                        foreach(Facility f in Locs)
                        {
                            if(f.location == m && f.developed)
                            {
                                net += f.netWorth("prod");
                                x++;
                            }
                        }
                        if(x > 0)
                        {
                            net = net / x;
                             //Console.WriteLine(net);
                        }
                        double p = 0;
                        foreach(Market ma in LocList)
                        {
                            if (m.anchor == ma.name)
                            {
                                p = ma.markup;
                            }
                        }

                        if(m.prevNet > 0)
                        {
                             double i = net / m.prevNet;
                             if(p == 0){ p = i; }
                             m.markup = (i+p)/2;
                        }
                        m.prevNet = net;
                    }
                }
            }
            else if (classname == "NPC" && type == "p")
            {
                playerturn = true;
            }
            else if (classname == "NPC" && type != "p")
            {
                foreach(NPC n in Players)
                {   
                    
                    if(n.name == name)
                    {
                        /*
                        foreach(string s in dv.Keys)
                        {
                            if(per % time == 0){
                                Console.WriteLine("\n"+s+", "+ dv[s][0].ToString()); 
                            }
                        }
                        //
                        if (!n.pub)
                        {
                            if (per % time == 0)
                            {
                                Console.WriteLine("\n" + n.name + ", split: " + dv[n.name][1].ToString() + ", keep: " + dv[n.name][2].ToString());
                            }
                        }
                        */
                        
                        Dictionary<string, double[]> dv = NPCturn(n);

                        //Compile into list
                        List<double> tally = new List<double>();
                        foreach (string s in dv.Keys){tally.Add(dv[s][0]);}
                        tally.Sort();
                        //foreach (double d in tally) { Console.WriteLine(d); }

                        //Make a choice on what to do
                        int r = rnd.Next(tally.Count * (n.intellegence / 100), tally.Count - 1);
                        double[] t = tally.ToArray();
                        //Console.WriteLine("{0}, {1}",r,tally.Count);
                        double num = t[r];
                        string choice = "";

                        foreach(string s in dv.Keys)
                        {
                            if(dv[s][0] == num){ choice = s; }
                        }

                        //Carry out descision
                        //RETRUN KEYS:
                        //Material_buysell
                        //Market_travel
                        //Facility_ud
                        //Facility_sell
                        //Facility_store
                        //NPC_sponsor
                        //NPC_public
                        string[] spc = choice.Split('_');
                        Material m = new Material();
                        Market mr = new Market();
                        Facility fa = new Facility();
                        NPC npc = new NPC();
                        foreach (Market ma in LocList) { if (n.location == ma.name) { mr = ma; } }
                        //Console.WriteLine(choice);
                        //foreach(Material s in n.invent.Keys){ Console.WriteLine(s.name + n.invent[s].ToString()); }
                        if(spc[1] == "buysell")
                        {
                            /*
                            foreach (Material ma in Materials) { if (spc[0] == ma.name) { m = ma; } }

                            //Add Materials to amounts, etc.
                            n.manageInvent("add", m, Convert.ToInt32(Math.Floor(dv[choice][1])));
                            m.amount -= Convert.ToInt32(Math.Floor(dv[choice][1]));

                            //Currency exchange
                            n.manageInvent("remove", mr.currency, Convert.ToInt32(Math.Floor(dv[choice][1] * m.cost / mr.currency.cost)));
                            mr.currency.amount += Convert.ToInt32(Math.Floor(dv[choice][1] * m.cost / mr.currency.cost));
                            */
                            
                        }
                        if (spc[1] == "travel")
                        {
                            Market mrnew = new Market();
                            foreach (Market ma in LocList) { if (spc[0] == ma.name) { mrnew = ma; } }
                            //Check if trade route is available
                            if(mr == mrnew || mr.travel.Contains(mrnew))
                            {
                                //INSERT RNG FUNCTION HERE

                                //Move NPC location to desired market
                                n.location = spc[0];
                            }
                        }
                        //Console.WriteLine(choice);
                        //Console.WriteLine(spc[0]);
                        foreach (Facility fac in Locs) { if (fac.name == spc[0]) { fa = fac; } }
                        if (spc[1] == "ud")
                        {
                            fa.developProperty(n);
                        }
                        if (spc[1] == "sell")
                        {
                            fa.anchor = null;
                            fa.developed = false;
                            n.manageInvent("add", mr.currency, Convert.ToInt32(fa.cost / mr.currency.cost));
                        }
                        if (spc[1] == "store")
                        {
                            if(dv[choice][1] + fa.units <= fa.cap || fa.units - dv[choice][1] >= 0)
                            {
                                fa.units -= Convert.ToInt32(dv[choice][1]);
                                n.invent[fa.mat] += Convert.ToInt32(dv[choice][1]);
                            }
                            else if(dv[choice][1] + fa.units > fa.cap)
                            {
                                n.invent[fa.mat] -= fa.cap - fa.units;
                                fa.units = fa.cap;
                            }
                            else if(fa.units - dv[choice][1] < 0)
                            {
                                n.invent[fa.mat] += fa.units;
                                fa.units = 0;
                            }
                        }
                        if (spc[1] == "sponsor")
                        {
                            n.sponsor = spc[0];
                        }
                        if (spc[1] == "public")
                        {
                            n.goPublic(Players, Convert.ToInt32(dv[choice][1]), Convert.ToInt32(dv[choice][0]));
                        }
                    }
                    
                }
                
                
                //
                //-----ADD NPC DESCISION ALGORITHIM HERE-----
                //
            }
            else if (classname == "Facility")
            {
                foreach (Facility m in Locs)
                {
                    if (m.name == name)
                    {
                        //
                        //-----ADD FACILITY FLUX FORMULA HERE-----
                        //
                        /*double x;
                        foreach(NPC n in Players)
                        {
                            if(n.prevNet > 0 && m.developed && m.anchor == n.name)
                            {
                                x = n.netWorth(m.mat);
                            }
                        }*/

                        m.cost = Convert.ToInt32(m.cost * m.location.markup); //* 1);//
                        //Console.WriteLine("{0}, {1}", m.cost, m.location.markup);
                        if(m.developed){
                            m.mcost = (m.mcost * m.mat.inflation);
                            //m.cost = Convert.ToInt32(m.cost * m.mat.inflation);
                            //Console.WriteLine("{0}, {1}, {2}", m.mcost, m.mat.inflation, m.location.markup);
                            //ADD CAP ADDITION FORMULA
                        }
                    }
                }
            }
            turn = changeTurn();
            if(!playerturn){
                turnover = true;
            }
        }

        public Dictionary<string, double[]> NPCturn(NPC n)
        {
            Dictionary<string, double[]> l = new Dictionary<string, double[]>();
            //Material Calculations
            double nw = 0;
            double[] marc = new double[2];
            foreach(Material m in Materials)
            {
                if(m.type != "c")
                {
                    foreach (Market ma in LocList)
                    {
                        if (n.location == ma.name && ma.type == "e")
                        {
                            //Main material calculation formula
                            //int x = -A + (A * (i+id) / Math.Pow(c, 1/2));
                            int A = m.amount;
                            double i = m.inflation;
                            double id = m.inflation - m.previnflation;

                            double x = -A + (A * (i + id) / Math.Pow(m.cost, 1 / 2));

                            //Check for max/min amounts possible
                            if (x > m.amount * 1 / 4) { x = m.amount * 1 / 4; }
                            if (-x > n.invent[m]) { x = -n.invent[m]; }
                            if ((x * m.cost) / ma.currency.cost > n.invent[ma.currency]) { x = (ma.currency.cost * n.invent[ma.currency]) / (m.cost); }
                            if ((-x * m.cost) / ma.currency.cost > ma.currency.amount * 1 / 4) { x = (ma.currency.amount * ma.currency.cost) / (4 * m.cost); }

                            double[] matc = new double[] { Math.Abs(x), x };
                            l.Add(m.name + "_buysell", matc);
                        }
                        if (n.location == ma.name)
                        {
                            nw = ma.netWorth(Locs, m);
                        }
                    }
                }
            }

            

            //Market & Facility Calculations
            foreach (Market ma in LocList)
            { 
                //Get total nw of all materials
                foreach (Material m in Materials)
                {
                    marc[0] = marc[0] + ma.netWorth(Locs, m) - nw;
                }
                //Get average of material net worth
                //marc[0] = marc[0] / Materials.Count;
                l.Add(ma.name+"_travel", marc);

                //Same material calculation but for warehouses
                foreach(Facility f in Locs)
                {
                    if(f.location == ma && n.location == ma.name){
                        double[] fnet = new double[2];
                        //
                        if(!f.developed)
                        {
                            //Console.WriteLine("{0}, {1}", f.name, (f.cap * f.cost - f.netWorth("worth")));
                            fnet[0] = (f.cap*f.cost - f.netWorth("worth"));
                            fnet[0] = fnet[0]/Math.Pow(10, Math.Floor(Math.Log10(Math.Abs(fnet[0]))) - 2);
                            //fnet[0] = fnet[0] / Math.Pow(10, Math.Log10(fnet[0]) - 3);
                        
                            l.Add(f.name+"_ud", fnet);
                        }
                        else if(f.developed)
                        {
                            //Console.WriteLine("{0}, {1}", f.name, (f.netWorth("worth") - 3 * f.netWorth("prod")));
                            fnet[0] = (f.netWorth("worth") - 3 * f.netWorth("prod"));
                            fnet[0] = fnet[0] / Math.Pow(10, Math.Floor(Math.Log10(Math.Abs(fnet[0]))) - 2);
                            l.Add(f.name + "_sell", fnet);
                        
                            if(f.type == "s")
                            {
                                //Main material calculation formula
                                //int x = -A + (A * (i+id) / Math.Pow(c, 1/2));
                                int A = f.mat.amount;
                                double i = f.mat.inflation;
                                double id = f.mat.inflation - f.mat.previnflation;

                                double x = -A + (A * (i + id) / Math.Pow(f.mat.cost, 1 / 2));

                                //Check for max/min amounts possible
                                if (x > f.cap * 1/4) { x = f.cap * 1 / 4; }
                                if (-x > n.invent[f.mat]) { x = -n.invent[f.mat]; }

                                fnet[0] = Math.Abs(x);
                                fnet[1] = x;
                                l.Add(f.name + "_store", fnet);
                            }
                        }
                    }
                    //Console.WriteLine(f.name + ": " + fnet[0].ToString() + ", " + fnet[1].ToString());
                }
                
            }

            double[] npc = new double[2];
            //NPC Calculations
            //Sponsors act as essentially a deed that if the NPC goes bankrupt, the Sponsor will get all the debt
            foreach(NPC m in Players)
            {
                if (m.name != n.name){
                    //Console.WriteLine(m.name + ", " + m.netWorth().ToString() + ", " + m.prevNet.ToString());
                    npc[0] = m.netWorth() - m.prevNet;
                    npc[0] = npc[0] / (1 + Math.Pow(10, Math.Floor(Math.Log10(Math.Abs(npc[0]))) - 2));
                    l.Add(m.name+"_sponsor", npc);
                }
            }

            //GoPublic & Others
            if(!n.pub){
                int mat = 0;
                int split = 1000;
            
                foreach(Material m in Materials){ mat += m.amount;}
                split = mat / Materials.Count;

                int keep = split / 2;
                keep = Convert.ToInt32(keep * n.prevNet / n.netWorth());

                double[] gp = new double[3];

                gp[0] = n.netWorth() / (split - keep);
                gp[1] = split;
                gp[2] = keep;

                l.Add(n.name+"_public", gp);
            }

            return l;
        }

        //Handles all the RNG in the game
        public void rng(string op)
        {
            //All RNG in the game goes here
        }

        //Function, public String[]
        //Returns the class and type of the named class member
        public string[] getClass(string name)
        {
            string[] att = new string[2];

            foreach (Material m in Materials)
            {
                if (m.name == name) { att[0] = "Material"; att[1] = m.type; }
            }
            foreach (Market m in LocList)
            {
                if (m.name == name) { att[0] = "Market"; att[1] = m.type; }
            }
            foreach (NPC m in Players)
            {
                if (m.name == name) { att[0] = "NPC"; att[1] = m.type; }
            }
            foreach (Facility m in Locs)
            {
                if (m.name == name) { att[0] = "Facility"; att[1] = m.type; }
            }
            return att;
        }

        //Function, public string[]
        //Returns an array of the order of turns
        public void createTurnOrder()
        {
            //Get total elements
            count = Players.Count + Materials.Count + LocList.Count + Locs.Count;
            order = new Dictionary<int, string>();
            int x = 0;

            foreach (NPC n in Players) { order.Add(x, n.name); x++; }
            foreach (Material n in Materials) { order.Add(x, n.name); x++; }
            foreach (Market n in LocList) { order.Add(x, n.name); x++; }
            foreach (Facility n in Locs) { order.Add(x, n.name); x++; }
        }

        //Function, public string
        //Returns a new name for the turn, based of a prescession of events, checks if time tick is over
        public string changeTurn()
        {
            string x = "";

            x = order[ordern];

            ordern++;
            if (ordern >= count) { ordern = 0; }

            return x;
        }

        //Function, public void
        //Sets the interface depending on who's turn it is
        //Setting is the type of interface needed e.g 'player', 'map'
        public void setInterface(string setting, NPC player = null) { }

    }
    public class Material
    //A rescource that is meant to have several attributes, stored on a json file
    {

        public string name { get; set; }//String

        public string type { get; set; }//String
        //'r', 'c', 'sv' or 'm'

        public double cost { get; set; }//Integer, constant
        //The base price of the Material

        public int influx { get; set; }//Integer, constant
        //The rate at which more of that material enters the economy

        public int amount { get; set; }//Integer, variable
        //The amount of that material, unsold

        public double inflation { get; set; }//Double, variable
        //The base rate at which material prices rise, affected by influx and amount available

        public int prevamount { get; set; }//Integer, variable
        //The amount of that material, unsold

        public double previnflation { get; set; }//Double, variable
        //The base rate at which material prices rise, affected by influx and amount available

        public int total { get; set; }//Integer, variable
        //The amount of that material in total

        public string anchor { get; set; } //String
        //The Company or Party's attachment to that Material value (Only for type 'c')

        public double anchornet = -0.1;
        //The previous net worth of the anchor NPC (Only for type 'c')

        public Dictionary<Material, int> craftable { get; set; }
        //A dictionary of how it is produced (Only for type 'm')

        //Function, public int
        //Calculates the total amount of money an NPC, Market or Material holds in standard units
        public double netWorth()
        {
            double net = cost * total;
            return net;
        }

    }
    public class Market
    {
        public string name { get; set; } //String

        public string type { get; set; } //String
        //'e', 'p'

        public double markup { get; set; } //Integer, variable
        //For markets, cost will help markup/markdown price of a material

        public string anchor = ""; //String
        //The anchor is a place that the location is tied to having their cost become affected by such

        public Material currency { get; set; }

        public List<Market> travel { get; set; }
        //The places you can travel to from that market

        public double prevNet { get; set; }

        //Function, public int
        //Calculates the total amount of money an NPC, Market or Material holds in standard units
        public double netWorth(List<Facility> fl, Material mat = null)
        {
            double net = 0;
            if (mat == null)
            {
                    //foreach (Material m in n.invent.Keys)
                    //{
                    //    //net += n.invent[m] * m.cost;
                    //}
                foreach (Facility f in fl)
                {
                    if (f.location.name == name)
                    {
                        net += f.cost;
                    }
                }
            }
            else
            {
                //net += n.invent[mat] * mat.cost;
                foreach (Facility f in fl)
                {
                    if (f.location.name == name && f.mat == mat)
                    {
                        net += f.cost;
                    }

                }
            }
            return net;
        }
    }
    public class NPC
    {
        public string name { get; set; } //String
        //Name of the NPC

        public string type { get; set; } //String
       //'p', 'c', 'i', 'pa', 'gov'

        public int intellegence { get; set; } //Integer
        //The way the NPC acts and makes descisions during a turn

        public Dictionary<Material, int> invent { get; set; } //Dictionary<Material,int>
        //All things owned by an NPC - Contains:
        //Rescources (Material) - No. of Materials
        //Currency (Material) - No. of locations, parties and companies
        //the integer value is the amount held by the NPC

        public string location { get; set; } //String
        //Current location or base of operations for NPCs

        public string sponsor { get; set; } //String array
        //2 feilds, Company and Party, unique value
        //Can only do such for public organisations

        public string anchor { get; set; } //ONLY TYPE 'p'

        public bool pub = false;

        public double prevNet { get; set; }

        //Function, public void
        //Function that has 2 values to manage, add item or remove one (and their location in 2D array)
        public void manageInvent(string operation, Material m = null, int amount = 0)
        {
            if (operation == "add") { invent[m] += amount; }
            if (operation == "remove") { invent[m] -= amount; }
            if (operation == "clear") { invent[m] = 0; }
            if (operation == "clearall") { foreach (Material i in invent.Keys) { invent[i] = 0; } }
        }

        //Function, public void
        //For each type, this will do certain things
        //- Companies will create a new Material
        //- Parties will join the election campaign, creating a new Material
        //money is the total net worth of the company/party
        //split only is needed for the company
        //NOTE: Add after 'goPublic', order.Add(count, m); count + 1;
        public Material goPublic(List<NPC> lnpc, int split = 1, int keep = 0)
        {
            string key = "";
            Material m = new Material();

            if (type == "p" || type == "i")
            {
                throw new Exception("Invalid input, type must be Company or Party");
            }
            if (type == "c" || type == "pa")
            {
                if (type == "c") { key = "Stock"; }
                if (type == "pa") { key = "Votes"; }

                m.name = name + "_mat";
                m.type = key;
                m.cost = netWorth() / split;
                m.amount = split - keep;
                m.total = split;
                m.influx = 0;
                m.anchor = name;

                foreach (NPC n in lnpc)
                {
                    if (n.name == name)
                    {
                        n.invent.Add(m, keep);
                    }
                    else
                    {
                        n.invent.Add(m, 0);
                    }
                }
            }
            pub = true;
            return m;
        }

        //Function, public int
        //Calculates the total amount of money an NPC, Market or Material holds in standard units
        public double netWorth(Material mat = null)
        {
            double net = 0;
            if (mat == null)
            {
                foreach (Material m in invent.Keys)
                {
                    net += invent[m] * m.cost;
                }

            }
            else
            {
                net += invent[mat] * mat.cost;

            }
            return net;
        }

        //public void liquidate(List<Facility> fl, List<NPC> nl, sting merger = "")
        //Create a list of all debtors - merger is top and only priority if given
        //Withdraw any Sponsors, Stock and Facilities (after moving Materials)
        //Give money to all debtors, then to govmt
        //After: Remove NPC from Controls, Players and Delete the NPC

    }
    //A facility is like an NPC but it is grounded in a specific location (Property)
    public class Facility
    {
        public string name { get; set; }

        public string type { get; set; }
        //Production - Makes craftable materials ('p') [districts only]
        //Collection - Produces raw materials ('c') [districts only]
        //Storage - Stores materials ('s')
        //Office - Allows companies to get sponsors and boosts intellegence ('o') [exchanges only]

        public bool developed = false;
        //Wether or not the Facility has been developed

        public int units { get; set; }
        //The material that the facility makes or stores

        public int cap { get; set; }
        //Amount of units produced (Prod, Coll, Off)
        //Total capacity of stores (Store)
        //Amount the intellegence is boosted (Off)

        public int cost { get; set; }
        //The cost to develop the property

        public double mcost { get; set; }
        //The cost to maintain the property

        public string anchor { get; set; }
        //Company tie

        public Material mat { get; set; }

        public Market location { get; set; }

        public void developProperty(NPC n)
        {
            anchor = n.name;
            developed = true;
            //mat = m;
            n.manageInvent("remove", location.currency, Convert.ToInt32(location.currency.cost * cost));
        }

        public double netWorth(string op)
        {
            double net = 0;
            if (op == "worth")
            {
                net += cost * location.currency.cost;
            }
            if (op == "prod")
            {
                net = (cap * mat.cost) / (mcost);
            }

            return net;
        }

    }
    //Institue class contains all 'above the system' NPCs

    public class Institute
    {

        public string name { get; set; }

        public string type { get; set; }
        //Government 'g': Regulates the economy of a particular NPC type
        //Bank 'b': Provides Loans to NPCs

        public Market head { get; set; }
        //The permanent location of the institute

        public string regulator { get; set; }
        //The NPC type that the Ins regulates (Govmts only)

        public double reserve { get; set; }
        //The amount of cost units owned by the bank

        public double intrest { get; set; }
        //The intrest rate is deterine

        //Manage the money reserve
        public void manageReserve(string op, int a, List<NPC> nl = null, NPC n = null)
        {
            if(op == "currency")
            {
                head.currency.amount += Convert.ToInt32(head.currency.cost / a);
            }
            if (op == "inject")
            {
                head.currency.amount += Convert.ToInt32(head.currency.cost / a);
            }
        }
    }
}
