﻿//Ho Zhen Yi S10267291 : Q1, Q4, Q7, Q8
//Pang Ai Jie Jennie S10268150 : Q2, Q3, Q5, Q6, Q9

//Dictionary list: (so we dont keep scrolling through the code LOL)
//Dictionary<string, string> FlightNumToGate
//Dictionary<string, string> airlinesDict
//Dictionary<string, BoardingGate> boardinggateDict
//Dictionary<string, Flight> flightsDict
//Dictionary<string, string> FlightAndSpecialCodeDict

using System;
using System.ComponentModel;
using Assignment;

//Q1 completed - Ho Zhen Yi S10267291
Dictionary<string, string> airlinesDict= new Dictionary<string, string>();
Dictionary<string, BoardingGate> boardinggateDict = new Dictionary<string, BoardingGate>();
string[] airlineLines = File.ReadAllLines("airlines.csv");
for (int i = 1; i< airlineLines.Length; i++ )
{
    string[] data = airlineLines[i].Trim().Split(',');
    string airlineName = data[0];
    string airlineCode = data[1];
    Airline a = new Airline(airlineName, airlineCode);
    airlinesDict.Add(a.Name, a.Code);
}
string[] boardinggateLines = File.ReadAllLines("boardinggates.csv");
for (int i = 1; i < boardinggateLines.Length; i++)
{
    string[] data = boardinggateLines[i].Trim().Split(",");
    string boardinggateName = data[0];
    bool ddjb = Convert.ToBoolean(data[1]);
    bool cfft = Convert.ToBoolean(data[2]);
    bool lwtt = Convert.ToBoolean(data[3]);
    BoardingGate b = new BoardingGate(boardinggateName, cfft, ddjb, lwtt);
    boardinggateDict.Add(b.GateName, b);
}

//Q2 completed - Pang Ai Jie Jennie S10268150
Dictionary<string, Flight> flightsDict = new Dictionary<string, Flight>();
Dictionary<string, string> FlightAndSpecialCodeDict = new Dictionary<string, string>();
string[] flightLines = File.ReadAllLines("flights.csv");
for (int i = 1; i < flightLines.Length; i++)
{
    string[] data = flightLines[i].Trim().Split(',');
    string flightNumber = data[0];
    string origin = data[1];
    string destination = data[2];
    DateTime expectedDate = Convert.ToDateTime(data[3]);
    string specialCode = data[4];
    Flight f;
    if (specialCode == "DDJB")
    {
        f = new DDJBFlight(flightNumber, origin, destination, expectedDate);
    }
    else if (specialCode == "CFFT")
    {
        f = new CFFTFlight(flightNumber, origin, destination, expectedDate);
    }
    else if (specialCode == "LWTT")
    {
        f = new LWTTFlight(flightNumber, origin, destination, expectedDate);
    }
    else
    {
        f = new NORMFlight(flightNumber, origin, destination, expectedDate);
    }
    flightsDict.Add(f.FlightNumber, f);
    if (specialCode == "DDJB" || specialCode == "CFFT" || specialCode == "LWTT")
    {
        FlightAndSpecialCodeDict[f.FlightNumber] = specialCode;
    }
    else
    {
        FlightAndSpecialCodeDict[f.FlightNumber] = "None";
    }
}

//Q3 completed - Pang Ai Jie Jennie S10268150
void ListAllFlights(Dictionary<string, string> airlinesDict, Dictionary<string, Flight> flightsDict)
{
    foreach (KeyValuePair<string, Flight> kvp in flightsDict)
    {
        foreach (KeyValuePair<string, string> keyvaluepair in airlinesDict)
        {
            if (kvp.Value.FlightNumber.Contains(keyvaluepair.Value))
            {
                string airlineName = keyvaluepair.Key;
                Console.WriteLine($"{kvp.Value.FlightNumber,-17}{airlineName,-24}{kvp.Value.Origin,-24}{kvp.Value.Destination,-24}{kvp.Value.ExpectedTime}");
            }
        }
    }
}

//Q4 completed - Ho Zhen Yi S10267291
void ListBoardingGates(Dictionary<string, BoardingGate> boardinggateDict)
{
    foreach (BoardingGate boardingGate in boardinggateDict.Values)
    {
        Console.WriteLine($"{boardingGate.GateName,-16} {boardingGate.SupportsDDJB,-23} {boardingGate.SupportsCFFT,-23} {boardingGate.SupportsLWTT}");
    }
}

//Q5 completed - Pang Ai Jie Jennie S10268150
Dictionary<string, string> FlightNumToGate = new Dictionary<string, string>();
//create dictionary to store flight number and boarding gate name
foreach (KeyValuePair<string, Flight> Kvp in flightsDict)
{
    FlightNumToGate.Add(Kvp.Value.FlightNumber, "Unassigned");
}
void AssignBoardingGateToFlight(Dictionary<string, Flight> flightsDict, Dictionary<string, BoardingGate> boardinggateDict, Dictionary<string, string> FlightAndSpecialCodeDict)
{
    while (true)
    {
        Console.WriteLine("Enter Flight Number:");
        string flightNum = Console.ReadLine().ToUpper().Replace(" ", "");
        Console.WriteLine("Enter Boarding Gate Name:");
        string boardingGateName = Console.ReadLine().ToUpper();
        bool flightExists = false;
        foreach (KeyValuePair<string, Flight> kvp in flightsDict)
        {
            if (kvp.Key.Replace(" ", "") == flightNum)
            {
                flightNum = kvp.Key;
                flightExists = true;
                break;
            }
        }
        if (flightExists && boardinggateDict.ContainsKey(boardingGateName))
        {
            //assign gate to flight
            foreach (KeyValuePair<string, string> KVp in FlightNumToGate)
            {
                if (KVp.Key == flightNum)
                {
                    if (boardinggateDict[boardingGateName].SupportsDDJB == true && FlightAndSpecialCodeDict[flightNum] == "DDJB")
                    {
                        FlightNumToGate[KVp.Key] = boardingGateName;
                    }
                    else if (boardinggateDict[boardingGateName].SupportsCFFT == true && FlightAndSpecialCodeDict[flightNum] == "CFFT")
                    {
                        FlightNumToGate[KVp.Key] = boardingGateName;
                    }
                    else if (boardinggateDict[boardingGateName].SupportsLWTT == true && FlightAndSpecialCodeDict[flightNum] == "LWTT")
                    {
                        FlightNumToGate[KVp.Key] = boardingGateName;
                    }
                    else if (FlightAndSpecialCodeDict[flightNum] == "None")
                    {
                        FlightNumToGate[KVp.Key] = boardingGateName;
                    }
                    else
                    {
                        Console.WriteLine("Boarding Gate does not support the flight's special request code. Please try again.");
                        break;
                    }
                }
            }

            //display basic info of flight
            Flight f = flightsDict[flightNum];
            BoardingGate b = boardinggateDict[boardingGateName];
            b.Flight = f;
            foreach (KeyValuePair<string, string> kvp in FlightAndSpecialCodeDict)
            {
                if (kvp.Key == f.FlightNumber)
                {
                    Console.WriteLine($"Flight Number: {f.FlightNumber}"
                        + $"\nOrigin: {f.Origin}"
                        + $"\nDestination: {f.Destination}"
                        + $"\nExpected Time: {f.ExpectedTime}"
                        + $"\nSpecial Request Code: {kvp.Value}"
                        + $"\nBoarding Gate Name: {b.GateName}"
                        + $"\nSupports DDJB: {b.SupportsDDJB}"
                        + $"\nSupports CFFT: {b.SupportsCFFT}"
                        + $"\nSupports LWTT: {b.SupportsLWTT}");
                    break;
                }
            }

            //update status of flight
            Console.WriteLine("Would you like to update the status of the flight? (Y/N)");
            string response = Console.ReadLine().ToUpper();
            if (response == "Y")
            {
                Console.WriteLine("1. Delayed" + "\n2. Boarding" + "\n3. On Time");
                Console.WriteLine("Please select the new status of the flight:");
                string newStatusOption = Console.ReadLine();
                if (newStatusOption == "1")
                {
                    f.Status = "Delayed";
                }
                else if (newStatusOption == "2")
                {
                    f.Status = "Boarding";
                }
                else if (newStatusOption == "3")
                {
                    f.Status = "On Time";
                }
                else
                {
                    Console.WriteLine("Invalid option. Please try again.");
                    continue;
                }
            }
            else if (response == "N")
            {
                break;
            }
            else
            {
                Console.WriteLine("Invalid option. Please try again.");
                continue;
            }
            Console.WriteLine($"Flight {f.FlightNumber} has been assigned to Boarding Gate {b.GateName}!");
            break;
        }
        else
        {
            Console.WriteLine("Invalid Flight Number or Boarding Gate Name. Please try again.");
            continue;
        }
    }
}

//Q6 - completed Pang Ai Jie Jennie S10268150
void CreateFlight(Dictionary<string, Flight> flightsDict)
{
    while (true)
    {
        Console.WriteLine("Enter Flight Number: ");
        string flightNumber = Console.ReadLine();
        Console.WriteLine("Enter Origin: ");
        string origin = Console.ReadLine();
        Console.WriteLine("Enter Destination: ");
        string destination = Console.ReadLine();
        Console.WriteLine("Enter Expected Departure/Arrival Time (dd/mm/yyyy hh:mm): ");
        DateTime expectedTime = Convert.ToDateTime(Console.ReadLine());
        Console.WriteLine("Enter Special Request Code (CFFT/DDJB/LWTT/None): ");
        string specialCode = Console.ReadLine();

        //create flight object & add to dictionary
        Flight fl;
        if (specialCode == "CFFT")
        {
            fl = new CFFTFlight(flightNumber, origin, destination, expectedTime);
        }
        else if (specialCode == "DDJB")
        {
            fl = new DDJBFlight(flightNumber, origin, destination, expectedTime);
        }
        else if (specialCode == "LWTT")
        {
            fl = new LWTTFlight(flightNumber, origin, destination, expectedTime);
        }
        else
        {
            fl = new NORMFlight(flightNumber, origin, destination, expectedTime);
        }
        flightsDict.Add(fl.FlightNumber, fl);

        //append new flight info to flights.csv
        if (specialCode == "None")
        {
            string[] lines = { $"{flightNumber},{origin},{destination},{expectedTime},{null}" };
            File.WriteAllLines("flights.csv", lines);
        }
        else
        {
            string[] lines = { $"{flightNumber},{origin},{destination},{expectedTime},{specialCode}" };
            File.WriteAllLines("flights.csv", lines);
        }
        Console.WriteLine($"Flight {fl.FlightNumber} has been added!");

        //prompt user to add another flight
        Console.WriteLine("Would you like to add another flight? (Y/N)");
        string response = Console.ReadLine().ToUpper();
        if (response == "Y")
        {
            continue;
        }
        else if (response == "N")
        {
            break;
        }
        else
        {
            Console.WriteLine("Invalid option. Please try again.");
            continue;
        }
    }
}

//List of Airlines for Changi Airport Terminal 5
void ListOfAirlines(Dictionary<string, string> airlinesDict)
{
    Console.WriteLine("=============================================" +
        "\nList of Airlines for Changi Airport Terminal 5" +
        "\n=============================================");
    Console.WriteLine($"{"Airline Code",-16}{"Airline Name"}");
    foreach (KeyValuePair<string, string> kvp in airlinesDict)
    {
        Console.WriteLine($"{kvp.Value,-16}{kvp.Key}");
    }
}

//Q7 - Ho Zhen Yi S10267291
void DisplayAirlineFlights(Dictionary<string, string> airlinesDict, Dictionary<string, Flight> flightsDict)
{
    Console.WriteLine("Enter Airline Name: ");
    string airlineName = Console.ReadLine();
    Console.WriteLine($"{"============================================="}" +
    $"{"\nList of Flights for {}"}" + //need to get airline name
    $"{"\n============================================="}");
    Console.WriteLine($"{"Flight Number", -16}{"Airline Name", -23}{"Origin", -23}{"Destination", -23}{"Expected Departure/Arrival Time"}");
}

//Q8 - Ho Zhen Yi S10267291
void ModifyFlightDetailed(Dictionary<string, Flight> flightsDict)
{
    ListOfAirlines(airlinesDict);
    DisplayAirlineFlights(airlinesDict, flightsDict);
    Console.WriteLine("");
}

//Q9 - completed Pang Ai Jie Jennie S10268150
void DisplayFlightSchedule(Dictionary<string, Flight> flightsDict, Dictionary<string, string> airlinesDict, Dictionary<string, string> FlightNumToGate)
{
    var sortedFlights = flightsDict.Values.ToList();
    sortedFlights.Sort();
    Console.WriteLine($"{"Flight Number",-16}{"Airline Name",-23}{"Origin",-23}{"Destination",-23}{"Expected Departure/Arrival Time",-36}{"Status",-16}{"Boarding Gate"}");
    foreach (KeyValuePair<string, Flight> kvp in flightsDict)
    {
        foreach (KeyValuePair<string, string> keyvaluepair in airlinesDict)
        {
            if (kvp.Value.FlightNumber.Contains(keyvaluepair.Value))
            {
                string airlineName = keyvaluepair.Key;
                Console.WriteLine($"{kvp.Value.FlightNumber,-16}{airlineName,-23}{kvp.Value.Origin,-23}{kvp.Value.Destination,-23}{kvp.Value.ExpectedTime, -36}{kvp.Value.Status, -16}{FlightNumToGate[kvp.Value.FlightNumber]}");
            }
        }
    }
}

//main loop
while (true)
{
    Console.WriteLine("\n\n\n\n\n=============================================" +
        "\nWelcome to Changi Airport Terminal 5" +
        "\n=============================================" +
        "\n1. List All Flights" +
        "\n2. List Boarding Gates" +
        "\n3. Assign a Boarding Gate to a Flight" +
        "\n4. Create Flight" +
        "\n5. Display Airline Flights" +
        "\n6. Modify Flight Details" +
        "\n7. Display Flight Schedule" +
        "\n0. Exit");
    Console.WriteLine("\n\nPlease select your option:");
    int option = Convert.ToInt32(Console.ReadLine());

    if (option == 1)
    {
        Console.WriteLine("=============================================" +
            "\nList of Flights for Changi Airport Terminal 5" +
            "\n=============================================");
        Console.WriteLine($"{"Flight Number", -16} {"Airline Name", -23} {"Origin", -23} {"Destination",-23} {"Expected Depareture/Arrival"}");
        ListAllFlights(airlinesDict, flightsDict);
    }
    else if (option == 2)
    {
        Console.WriteLine("=============================================" +
            "\nList of Boarding Gates for Changi Airport Terminal 5" +
            "\n=============================================");
        Console.WriteLine($"{"Gate Name",-17}{"DDJB",-24}{"CFFT",-24}{"LWTT"}");
        ListBoardingGates(boardinggateDict);
    }
    else if (option == 3)
    {
        Console.WriteLine("=============================================" +
                         "\nAssign a Boarding Gate to a Flight" +
                         "\n=============================================");
        AssignBoardingGateToFlight(flightsDict, boardinggateDict, FlightAndSpecialCodeDict);
    }
    else if (option == 4)
    {
        CreateFlight(flightsDict);
    }
    else if (option == 5)
    {
        ListOfAirlines(airlinesDict);
        DisplayAirlineFlights(airlinesDict, flightsDict);
    }
    else if (option == 6)
    {
        ModifyFlightDetailed(flightsDict);
    }
    else if (option == 7)
    {
        Console.WriteLine("=============================================" +
            "\nFlight Schedule for Changi Airport Terminal 5" +
            "\n=============================================");
        DisplayFlightSchedule(flightsDict, airlinesDict, FlightNumToGate);
    }
    else if (option == 0)
    {
        Console.WriteLine("Goodbye!");
        break;
    }
    else
    {
        Console.WriteLine("Invalid option. Please try again.");
    }
}