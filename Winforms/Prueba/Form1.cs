using System;
using System.IO.Ports;

namespace Prueba
{
    public partial class Form1 : Form
    {

        SerialPort outPutPorts = new SerialPort();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DetectMicrocontroller();
            initialize();

        }

        private void initialize()
        {
            outPutPorts.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
        }

        public void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;

            try
            {
                string indata = sp.ReadExisting().Trim(); // Leer datos y limpiar espacios
                this.Invoke(new Action(() => { textBox1.AppendText(indata + Environment.NewLine); }));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"No se pudo leer el puerto {sp.PortName}: {ex.Message}");
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string portNumber = (string)portsList.SelectedItem;
            if (string.IsNullOrEmpty(portNumber)) return;

            if (outPutPorts.IsOpen)
            {
                outPutPorts.Close();
            }

            // Configurar el puerto correctamente
            outPutPorts.PortName = portNumber;
            outPutPorts.BaudRate = 9600;      // Asegurar que coincida con Serial.begin(9600)
            outPutPorts.Parity = Parity.None;
            outPutPorts.DataBits = 8;
            outPutPorts.StopBits = StopBits.One;
            outPutPorts.Handshake = Handshake.None;
            outPutPorts.DtrEnable = true;
            outPutPorts.RtsEnable = true;

            try
            {
                outPutPorts.Open();
                outPutPorts.WriteLine(txtOut.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"No se pudo abrir el puerto {portNumber}: {ex.Message}");
            }

        }

        private void DetectMicrocontroller()
        {
            var ports = SerialPort.GetPortNames();
            portsList.Items.Clear();
            portsList.Items.AddRange(ports);

            foreach (var port in ports)
            {
                try
                {
                    using (SerialPort tempPort = new SerialPort(port, 9600, Parity.None, 8, StopBits.One))
                    {
                        tempPort.DtrEnable = true;
                        tempPort.RtsEnable = true;
                        tempPort.ReadTimeout = 1000; // 1 segundo de timeout
                        tempPort.WriteTimeout = 500;

                        tempPort.Open();
                        tempPort.WriteLine("WHO_ARE_YOU?");  // Preguntar al micro

                        string response = tempPort.ReadLine().Trim(); // Leer respuesta
                        tempPort.Close();

                        if (response != null) // Si coincide, asignar puerto
                        {
                            outPutPorts.PortName = port;
                            labelMicro.Text = $"Microcontrolador: {response} en {port}";
                            portsList.SelectedItem = port;
                            break; // Dejar de buscar
                        }
                    }
                }
                catch (Exception)
                {
                    // Si no responde, ignoramos el puerto
                }
            }
        }

    }
}
