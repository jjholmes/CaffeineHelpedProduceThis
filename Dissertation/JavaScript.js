var iata = [];
var uniqIata = [];
var outboundDate = "";
var serialisedIata = "";
var map = AmCharts.makeChart("mapdiv", {
    type: "map",
    theme: "dark",
    projection: "mercator",
    panEventsEnabled: true,
    backgroundColor: "#F7F7F7",
    backgroundAlpha: 1,
    zoomControl: {
        zoomControlEnabled: true
    },
    dataProvider: {
        map: "worldHigh",
        getAreasFromMap: true,
        areas: [
        ]
    },
    areasSettings: {
        autoZoom: true,
        color: "#000000",
        colorSolid: "#84ADE9",
        selectedColor: "#84ADE9",
        outlineColor: "#B3B3B3",
        rollOverColor: "#538AD6",
        rollOverOutlineColor: "#ADADAD"

    },
    "listeners": [{
        "event": "clickMapObject",
        "method": function (event) {

            map.selectedObject = map.dataProvider;
            event.mapObject.showAsSelected = !event.mapObject.showAsSelected;
            map.returnInitialColor(event.mapObject);

            var countries = [];
            for (var i in map.dataProvider.areas) {
                var area = map.dataProvider.areas[i];
                if (area.showAsSelected) {
                    countries.push(area.title);
                    switch (area.title) {

                        case 'United Kingdom':
                            iata.push("LON");
                            break;

                        case 'France':
                            iata.push("PAR");
                            break;

                        case 'Germany':
                            iata.push("BER");
                            break;

                        case 'Canada':
                            iata.push("YOW");
                            break;

                        case 'Japan':
                            iata.push("TYO");
                            break;

                        case 'United States':
                            iata.push("WAS");
                            break;

                        case 'Australia':
                            iata.push("CBR");
                            break;

                        case 'Spain':
                            iata.push("MAD");
                            break;

                        case 'Portugal':
                            iata.push("LIS");
                            break;

                        case 'Ireland':
                            iata.push("DUB");
                            break;

                        case 'Iceland':
                            iata.push("REK");
                            break;

                        case 'Italy':
                            iata.push("ROM");
                            break;

                        case 'Belgium':
                            iata.push("BRU");
                            break;

                        case 'Switzerland':
                            iata.push("BRN");
                            break;

                        case 'Netherlands':
                            iata.push("AMS");
                            break;

                        case 'Norway':
                            iata.push("OSL");
                            break;

                        case 'Sweden':
                            iata.push("STO");
                            break;

                        case 'Denmark':
                            iata.push("CPH");
                            break;
                        default:
                            alert("Unfortunately this country " + area.title + " is not currently supported!");

                    }

                    uniqIata = iata.reduce(function (a, b) {
                        if (a.indexOf(b) < 0) a.push(b);
                        return a;
                    }, []);

                    console.log(uniqIata);
                    console.log(iata);
                }
            }
            document.getElementById("destinationHolder").innerHTML = countries;
            serialisedIata = uniqIata.toString();
        }
    }],
    "export": {
        "enabled": true
    }

});

document.getElementById("outbound").addEventListener('change', function () {

    outboundDate = document.getElementById("outbound").value;
});

document.getElementById('btnPlanRoute').addEventListener('click', function planRoute() {

    var weight1 = parseFloat(document.getElementById('txtWeight1').value);
    var weight2 = parseFloat(document.getElementById('txtWeight2').value);
    var weight3 = parseFloat(document.getElementById('txtWeight3').value);
    var weightValidator = (weight1 + weight2 + weight3);
    var dt = new Date(outboundDate);

    console.log(uniqIata.length);

    console.log(dt.getDate());

    console.log(serialisedIata);
    console.log(weightValidator);
    if (uniqIata.length < 3) {

            alert("Number of Destinations is invalid (min 3)");

    } else if (weightValidator !== 1) {

            alert("The weights for the algorithm of Price, Distance and changes must have the sum of 1")


    } else {

            document.getElementById('HiddenValue').value = serialisedIata;
            document.getElementById('HiddenValueChromSize').value = uniqIata.length;
            document.getElementById('HiddenValueDate').value = outboundDate;

            document.getElementById('HiddenValueW1').value = document.getElementById('txtWeight1').value;
            document.getElementById('HiddenValueW2').value = document.getElementById('txtWeight2').value;
            document.getElementById('HiddenValueW3').value = document.getElementById('txtWeight3').value;

            console.log(document.getElementById('HiddenValueW1').value);
            console.log(document.getElementById('HiddenValueW2').value);
            console.log(document.getElementById('HiddenValueW3').value);

            document.getElementById('HiddenValue').style.visibility = 'visible';
            document.getElementById('HiddenValueChromSize').style.visibility = 'visible';
            document.getElementById('HiddenValueDate').style.visibility = 'visible';
    }
});