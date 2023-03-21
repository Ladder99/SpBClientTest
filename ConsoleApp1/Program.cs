using SparkplugNet.Core.Node;
using SparkplugNet.VersionB;
using SparkplugNet.VersionB.Data;

Console.WriteLine("node create");

var nodeMetrics = new List<Metric>
{
    new()
    {
        Name = "IAmNode", DataType = DataType.Boolean,
        BooleanValue = true
    }
};

var node = new SparkplugNode(nodeMetrics, null);

var nodeOptions = new SparkplugNodeOptions(
    brokerAddress: "139.59.208.136",
    port: 1883,
    userName: "user",
    clientId: Guid.NewGuid().ToString(),
    password: "password",
    useTls: false,
    scadaHostIdentifier: "scada",
    groupIdentifier: "WeProduce",
    edgeNodeIdentifier: "Data",
    reconnectInterval: TimeSpan.FromSeconds(30),
    webSocketParameters: null,
    proxyOptions: null,
    cancellationToken: new CancellationToken()
);

nodeOptions.AddSessionNumberToDataMessages = true;

// ReSharper disable once UnusedParameter.Local
node.DisconnectedAsync += async args =>
{
    Console.WriteLine("node disconnected");
};

// ReSharper disable once UnusedParameter.Local
node.NodeCommandReceivedAsync += async args =>
{
    Console.WriteLine("node command received");
};

node.StatusMessageReceivedAsync += async args =>
{
    Console.WriteLine("node status message received");
};

Console.WriteLine("node birth");

await node.Start(nodeOptions);
await node.PublishMetrics(nodeMetrics);

Console.WriteLine("device create");

var deviceMetrics = new List<Metric>
{
    new()
    {
        Name = "IAmDevice/Int", DataType = DataType.Int32,
        IntValue = 0
    },
    new()
    {
        Name="IAmDevice/DataSet", DataType = DataType.DataSet,
        DataSetValue = new DataSet(
            new Dictionary<string, DataType>
            {
                { "key1", DataType.Int32 },
                { "key2", DataType.Int32 }
            }
        )
    }
};

var row1 = new Row();
var row2 = new Row();

row1.Elements = new List<DataSetValue>
{
    new (DataType.Int32, 0),
    new (DataType.Int32, 0)
};
    
row2.Elements = new List<DataSetValue>
{
    new (DataType.Int32, 0),
    new (DataType.Int32, 0)
};

deviceMetrics[1].DataSetValue.Rows = new List<Row>
{
    row1, row2
};

await node.PublishDeviceBirthMessage(deviceMetrics, "AtTheEdge");

Console.WriteLine("make data forever");

var rowValueCounter = 0;

while (true)
{
    await Task.Delay(1000);
    deviceMetrics[0].IntValue += 1;

    deviceMetrics[1].DataSetValue.Rows[0].Elements[0].SetValue(DataType.Int32, ++rowValueCounter);
    deviceMetrics[1].DataSetValue.Rows[1].Elements[1].SetValue(DataType.Int32, ++rowValueCounter);
    
    await node.PublishDeviceData(deviceMetrics, "AtTheEdge");
}