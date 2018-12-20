using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json.Linq;
using GAF;
using GAF.Operators;
using GAF.Extensions;
using System.Web.UI.HtmlControls;

public partial class _Default : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public class globals
    {
        public static int chromosomeLength;
        public static string[] iata;
        public static string date;

        public static double totalPrice;
        public static double totalDistance;
        public static double totalChanges;
        public static double weight1;
        public static double weight2;
        public static double weight3;
        public static List<string> orderOfDestinations = new List<string>();
        public static string order;
        public static bool errorOutput = false;

    }
    public void ErrorMessage()
    {

        string errMsg = "flight data could not be found please try changing the desired dates";
        HttpContext.Current.Response.Write("<script>alert('" + errMsg + "')</script>");
    }

    protected void btnOptimise_OnClick(object sender, EventArgs e)
    {

        System.Diagnostics.Debug.WriteLine(HiddenValue.Value);

        string s = HiddenValue.Value;

        globals.iata = s.Split(',').ToArray();

        globals.chromosomeLength = Int32.Parse(HiddenValueChromSize.Value);

        globals.date = HiddenValueDate.Value;

        globals.weight1 = Double.Parse(HiddenValueW1.Value);
        globals.weight2 = Double.Parse(HiddenValueW2.Value);
        globals.weight3 = Double.Parse(HiddenValueW3.Value);

        System.Diagnostics.Debug.WriteLine(globals.iata);

        System.Diagnostics.Debug.WriteLine(globals.chromosomeLength);
        System.Diagnostics.Debug.WriteLine(globals.date);

        geneticAlgorithm(sender, e, totalPrice, totalDistance, totalChanges, orderOfDests);

    }

    public static void geneticAlgorithm(object sender, EventArgs e, Label totalPrice, Label totalDistance, Label totalChanges, Label orderOfDests)
    {

        var population = new Population();

        for (var p = 0; p < globals.chromosomeLength; p++)
        {

            var chromosome = new Chromosome();
            for (var g = 0; g < globals.chromosomeLength; g++)
            {
                chromosome.Genes.Add(new Gene(g));
            }
            chromosome.Genes.ShuffleFast();
            population.Solutions.Add(chromosome);
        }

        var elite = new Elite(8);

        var crossover = new Crossover(0.8)
        {
            CrossoverType = CrossoverType.DoublePointOrdered
        };

        var mutate = new SwapMutate(0.04);

        var ga = new GeneticAlgorithm(population, objectiveValue);

        ga.OnRunComplete += ga_OnRunComplete;

        ga.Operators.Add(elite);
        ga.Operators.Add(crossover);
        ga.Operators.Add(mutate);

        ga.Run(Terminate);

        globals.order = String.Join(",", globals.orderOfDestinations.ToArray());

        totalPrice.Text = Convert.ToString(globals.totalPrice);
        totalDistance.Text = Convert.ToString(globals.totalDistance);
        totalChanges.Text = Convert.ToString(globals.totalChanges);
        orderOfDests.Text = Convert.ToString(globals.order);

        if (globals.errorOutput == true)
        {
            _Default form1 = new _Default();

            form1.ErrorMessage();

        }

    }

    public static void ga_OnRunComplete(object sender, GaEventArgs e)
    {
        double price = 0;
        double distance = 0;
        double changes = 0;
        string origin = null;
        string destination = null;

        var fittest = e.Population.GetTop(1)[0];
        foreach (var gene in fittest.Genes)
        {

            destination = globals.iata[(int)gene.RealValue];
            System.Diagnostics.Debug.WriteLine(globals.iata[(int)gene.RealValue]);
            globals.orderOfDestinations.Add(globals.iata[(int)gene.RealValue]);

            if (origin != null)
            {

                using (var webClient = new System.Net.WebClient())
                {
                    var json = webClient.DownloadString("http://api.travelpayouts.com/v2/prices/month-matrix?latest?currency=usd&origin=" + origin + "&destination=" + destination + "&beginning_of_period=" + globals.date + "&period_type=month&page=1&limit=30&show_to_affiliates=fALSE&sorting=price&trip_class=0&token=6ef8913be158c79f08b3ba1098cb2f07");

                    JToken flightData = JObject.Parse(json);

                    var json3 = webClient.DownloadString("http://api.travelpayouts.com/v2/prices/latest?currency=usd&origin=" + origin + "&destination=" + destination + "&period_type=year&page=1&limit=30&show_to_affiliates=true&sorting=price&trip_class=0&token=6ef8913be158c79f08b3ba1098cb2f07");

                    JToken flightData3 = JObject.Parse(json3);

                    var json2 = webClient.DownloadString("http://api.travelpayouts.com/v1/prices/cheap?currency=USD&origin=" + origin + "&destination=" + destination + "&token=6ef8913be158c79f08b3ba1098cb2f07");

                    JToken flightData2 = JObject.Parse(json2);

                    if (flightData.SelectToken("data[0].distance") != null)
                    {
                        distance = distance + (int)flightData.SelectToken("data[0].distance");
                    }
                    else if (flightData3.SelectToken("data[0].distance") != null)
                    {
                        distance = distance + (int)flightData3.SelectToken("data[0].distance");

                    }
                    else
                    {
                        globals.errorOutput = true;
                    }

                    if (flightData.SelectToken("data[0].number_of_changes") != null)
                    {
                        changes = changes + (int)flightData.SelectToken("data[0].number_of_changes");
                    }
                    else if (flightData3.SelectToken("data[0].number_of_changes") != null)
                    {
                        changes = changes + (int)flightData3.SelectToken("data[0].number_of_changes");

                    }
                    else
                    {
                        globals.errorOutput = true;
                    }

                    if (flightData2.SelectToken("data." + destination + ".0.price") != null)
                    {

                        price = price + (int)flightData2.SelectToken("data." + destination + ".0.price");

                    }
                    else if (flightData3.SelectToken("data[0].value") != null)
                    {
                        price = price + (int)flightData3.SelectToken("data[0].value");
                    }
                    else if (flightData.SelectToken("data[0].value") != null)
                    {
                        price = price + (int)flightData.SelectToken("data[0].value");
                    }
                    else
                    {
                        globals.errorOutput = true;
                    }

                }

            }

            origin = destination;
        }

        globals.totalPrice = price;
        globals.totalDistance = distance;
        globals.totalChanges = changes;

        System.Diagnostics.Debug.WriteLine(globals.totalPrice);
        System.Diagnostics.Debug.WriteLine(globals.totalDistance);
        System.Diagnostics.Debug.WriteLine(globals.totalChanges);

    }

    private static double objectiveValue(Chromosome chromosome)
    {

        double[] weights = new double[3];
        double price = 0;
        double distance = 0;
        double changes = 0;
        double objValue = 0;

        weights[0] = globals.weight1;
        weights[1] = globals.weight2;
        weights[2] = globals.weight3;

        string origin = null;
        string destination = null;

        foreach (var gene in chromosome.Genes)
        {

            destination = globals.iata[(int)gene.RealValue];

            if (origin != null)
            {
                using (var webClient = new System.Net.WebClient())
                {

                    var json = webClient.DownloadString("http://api.travelpayouts.com/v2/prices/month-matrix?latest?currency=usd&origin=" + origin + "&destination=" + destination + "&beginning_of_period=" + globals.date + "&period_type=month&page=1&limit=30&show_to_affiliates=fALSE&sorting=price&trip_class=0&token=6ef8913be158c79f08b3ba1098cb2f07");

                    JToken flightData = JObject.Parse(json);

                    var json3 = webClient.DownloadString("http://api.travelpayouts.com/v2/prices/latest?currency=usd&origin=" + origin + "&destination=" + destination + "&period_type=year&page=1&limit=30&show_to_affiliates=true&sorting=price&trip_class=0&token=6ef8913be158c79f08b3ba1098cb2f07");

                    JToken flightData3 = JObject.Parse(json3);

                    var json2 = webClient.DownloadString("http://api.travelpayouts.com/v1/prices/cheap?currency=USD&origin=" + origin + "&destination=" + destination + "&token=6ef8913be158c79f08b3ba1098cb2f07");

                    JToken flightData2 = JObject.Parse(json2);

                    if (flightData.SelectToken("data[0].distance") != null)
                    {
                        distance = distance + (int)flightData.SelectToken("data[0].distance");
                    }
                    else if (flightData3.SelectToken("data[0].distance") != null)
                    {
                        distance = distance + (int)flightData3.SelectToken("data[0].distance");

                    }
                    else
                    {
                        globals.errorOutput = true;
                    }

                    if (flightData.SelectToken("data[0].number_of_changes") != null)
                    {
                        changes = changes + (int)flightData.SelectToken("data[0].number_of_changes");
                    }
                    else if (flightData3.SelectToken("data[0].number_of_changes") != null)
                    {
                        changes = changes + (int)flightData3.SelectToken("data[0].number_of_changes");

                    }
                    else
                    {
                        globals.errorOutput = true;
                    }

                    if (flightData2.SelectToken("data." + destination + ".0.price") != null)
                    {

                        price = price + (int)flightData2.SelectToken("data." + destination + ".0.price");

                    }
                    else if (flightData3.SelectToken("data[0].value") != null)
                    {
                        price = price + (int)flightData3.SelectToken("data[0].value");
                    }
                    else if (flightData.SelectToken("data[0].value") != null)
                    {
                        price = price + (int)flightData.SelectToken("data[0].value");
                    }
                    else
                    {
                        globals.errorOutput = true;
                    }

                }

            }
            origin = destination;
        }

        price = (price * 0.01) * weights[0];
        distance = (distance * 0.01) * weights[1];
        changes = changes * weights[2];

        objValue = price + distance + changes;

        return 1 - objValue / 10000;

    }

    private static bool Terminate(Population population, int currentGeneration, long currentEvaluation)
    {

        return currentGeneration > (4 * globals.chromosomeLength);

    }

}