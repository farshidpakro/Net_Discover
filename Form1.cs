using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace Net_Discover;

public partial class Form1 : Form
{
    private Button button1;
    private Button button2;
    private Button button3;
    private Button button4;
    private RichTextBox richTextBox1;

    public Form1()
    {
        InitializeComponent();
        this.Size = new System.Drawing.Size(1200, 700);
        this.StartPosition = FormStartPosition.CenterScreen;
        this.Name ="test";
        button1 = new Button();
        this.Controls.Add(button1);
        button1.Click += new EventHandler(button1_Click);
        button1.Text = "Network Discover";
        button1.Location = new Point(70, 70);
        button1.Size = new Size(500, 100);
        button1.Location = new Point(40, 50);

        button2 = new Button();
        this.Controls.Add(button2);
        button2.Click += new EventHandler(button2_Click);
        button2.Text = "Show All IPs";
        button2.Location = new Point(70, 70);
        button2.Size = new Size(500, 100);
        button2.Location = new Point(40, 160);

        button3 = new Button();
        this.Controls.Add(button3);
        button3.Click += new EventHandler(button3_Click);
        button3.Text = "Show All MacAddress";
        button3.Location = new Point(70, 70);
        button3.Size = new Size(500, 100);
        button3.Location = new Point(40, 270);

        button4 = new Button();
        this.Controls.Add(button4);
        button4.Click += new EventHandler(button4_Click);
        button4.Text = "Show All Open Port";
        button4.Location = new Point(70, 70);
        button4.Size = new Size(500, 100);
        button4.Location = new Point(40, 380);

        richTextBox1 = new RichTextBox();
        this.Controls.Add(richTextBox1);

        richTextBox1.Location = new Point(70, 70);
        richTextBox1.Size = new Size(500, 440);
        richTextBox1.Location = new Point(600, 50);


    }
    private void button1_Click(object sender, EventArgs e)
    {
        richTextBox1.Text = "";
        NetworkInterface[] networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();

        foreach (NetworkInterface networkInterface in networkInterfaces)
        {
            richTextBox1.Text +=
            "Interface Name: " + networkInterface.Name +
            "\n Interface Description: " + networkInterface.Description +
            "\n Interface Type: " + networkInterface.NetworkInterfaceType +
            "\n Interface Speed: " + networkInterface.Speed +
            "\n Interface MAC Address: " + networkInterface.GetPhysicalAddress() +
            "\n Is Interface Connected: " + networkInterface.OperationalStatus +
            "\n ---------------- \n";

        }

    }
    private void button2_Click(object sender, EventArgs e)
    {
        richTextBox1.Text = "";

        NetworkInterface[] networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();

        foreach (NetworkInterface networkInterface in networkInterfaces)
        {
            if (networkInterface.OperationalStatus == OperationalStatus.Up)
            {
                IPInterfaceProperties ipProperties = networkInterface.GetIPProperties();

                foreach (UnicastIPAddressInformation ip in ipProperties.UnicastAddresses)
                {
                    if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        richTextBox1.Text += "Interface: " + networkInterface.Name +
                         "\n IP Address: " + ip.Address.ToString() +
                         "\n ---------------- \n";
                    }
                }
            }
        }
    }
    private void button3_Click(object sender, EventArgs e)
    {
        richTextBox1.Text = "";
        NetworkInterface[] networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();

        foreach (NetworkInterface networkInterface in networkInterfaces)
        {
            PhysicalAddress macAddress = networkInterface.GetPhysicalAddress();
            richTextBox1.Text += "Interface Name: " + networkInterface.Name +
           "\n MAC Address: " + macAddress.ToString() +
           "\n ---------------- \n";
        }
    }
    private void button4_Click(object sender, EventArgs e)
    {
        richTextBox1.Text = "";
        List<string> list = new List<string>();

        
        NetworkInterface[] networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();

        foreach (NetworkInterface networkInterface in networkInterfaces)
        {
            if (networkInterface.OperationalStatus == OperationalStatus.Up)
            {
                IPInterfaceProperties ipProperties = networkInterface.GetIPProperties();

                foreach (UnicastIPAddressInformation ip in ipProperties.UnicastAddresses)
                {
                    if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {

                        list.Add(ip.Address.ToString());

                    }
                }
            }
        }
        list.RemoveAll(ip => ip == "127.0.0.1");
        string[] ipAddresses = list.ToArray();
         foreach (string ip in ipAddresses)
        {
            for (int port = 1; port <= 65535; port++)
            {
                try
                {
                    using (TcpClient client = new TcpClient())
                    {
                        client.ReceiveTimeout = 1000; // Set a timeout for the connection attempt
                        client.Connect(ip, port);
                        richTextBox1.Text += $"Port {port} is open on {ip}";
                    }
                }
                catch (SocketException ex)
                {
                    if (ex.SocketErrorCode == SocketError.ConnectionRefused)
                    {
                        // Port is actively refused
                    }
                    else
                    {
                        // Other socket exceptions
                    }
                }
            }
        }
    }
}
