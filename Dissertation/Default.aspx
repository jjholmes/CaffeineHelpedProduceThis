<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>
    <!DOCTYPE html>
    <html xmlns="http://www.w3.org/1999/xhtml">

    <head runat="server">
        <title></title>
        <link href="bootstrap.css" rel="stylesheet" />
    </head>

    <body>
        <form id="form1" runat="server">
            <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true"></asp:ScriptManager>
            <div>
                <nav class="navbar navbar-expand-lg navbar-dark bg-primary">
                    <a class="navbar-brand" href="#">Travel Planner</a>
                    <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarColor01" aria-controls="navbarColor01" aria-expanded="false" aria-label="Toggle navigation" style="">
                        <span class="navbar-toggler-icon"></span>
                    </button>
                    <div class="collapse navbar-collapse" id="navbarColor01">
                        <ul class="navbar-nav mr-auto">
                            <li class="nav-item active">
                                <a class="nav-link" href="#Home">Home 
                                   <span class="sr-only">(current)</span>
                                </a>
                            </li>
                        </ul>
                    </div>
                </nav>
            </div>
            <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.7.1/jquery.min.js" type="text/javascript"></script>
            <script src="https://www.amcharts.com/lib/3/ammap.js" type="text/javascript"></script>
            <script src="https://www.amcharts.com/lib/3/maps/js/worldHigh.js" type="text/javascript"></script>
            <script src="https://www.amcharts.com/lib/3/themes/dark.js" type="text/javascript"></script>

            <div id="mapdiv" style="width: 1250px; height: 700px;"></div>
            <div id="chartdiv"></div>
            <div class="summary">
                <h5>Currently selected destinations:</h5>
                <p id="destinationHolder"></p>
            </div>

            <h5><span>Travel Start date (Must be the first day of specified month)</span></h5>
            <input type="date" id="outbound" />

            <p></p>


            <span class="badge badge-pill badge-warning">The larger the weights the higher prioritization on the variable in the algorithm</span>
            <span class="badge badge-pill badge-danger">Please input 3 decimal values for each of the weights, the sum of the weights must be equal to 1</span>

            <p></p>

            <div class="form-group has-success">
                <span>Price</span>
                <input id="txtWeight1" type="text" />


                <span>Distance</span>
                <input id="txtWeight2" type="text" />
                <span>Number of changes</span>
                <input id="txtWeight3" type="text" />
                <input class="btn btn-primary" value="Confirm destinations" type="button" id="btnPlanRoute" />
                <asp:Button id="btnOpt" runat="server" Text="Optimise Route" OnClick="btnOptimise_OnClick" class="btn btn-primary" />
                <p></p>



                <p></p>

                <div>
                    <h5>Total Price of trip:</h5>
                    <asp:Label ID="totalPrice" runat="server" />

                    <h5>Total Distance:</h5>
                    <asp:Label ID="totalDistance" runat="server" />

                    <h5>Total Changes:</h5>
                    <asp:Label ID="totalChanges" runat="server" />

                    <h5>Order of Travel:</h5>
                    <asp:Label ID="orderOfDests" runat="server" />

                </div>
                <input type="hidden" id="HiddenValue" runat="server" />
                <input type="hidden" id="HiddenValueChromSize" runat="server" />
                <input type="hidden" id="HiddenValueDate" runat="server" />
                <input type="hidden" id="HiddenValueW1" runat="server" />
                <input type="hidden" id="HiddenValueW2" runat="server" />
                <input type="hidden" id="HiddenValueW3" runat="server" />
            </div>
            <script src="JavaScript.js"></script>



        </form>
    </body>

    </html>