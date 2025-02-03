using System;
using System.IO.Ports;

namespace Prueba
{
    public partial class Form1 : Form
    {

        SerialPort outPutPorts = new SerialPort();
        String configString = "";
        private List<ComboBox> modeComboBoxes = new List<ComboBox>(); // Lista de combobox para los modos

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

            sendComandToMicrocontroller(txtOut.Text);


 

        }

        private void sendComandToMicrocontroller(string command)
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
            outPutPorts.WriteTimeout = 200;


            try
            {
                outPutPorts.Open();
                outPutPorts.WriteLine(command);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"No se pudo abrir el puerto {portNumber}: {ex.Message}");
            }

        }

        private void DetectMicrocontroller()
        {
            labelMicro.Text = $"Searching...";

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
                        tempPort.ReadTimeout = 3000; // 1 segundo de timeout
                        tempPort.WriteTimeout = 500;

                        tempPort.Open();
                        tempPort.WriteLine("WHO_ARE_YOU?");  // Preguntar al micro

                        string response = tempPort.ReadLine().Trim(); // Leer respuesta
                        configString = response;
                        string version = response.Split(";")[0];
                        tempPort.Close();

                        if (response != null) // Si coincide, asignar puerto
                        {
                            outPutPorts.PortName = port;
                            labelMicro.Text = $"Microcontrolador: {version} en {port}";
                            portsList.SelectedItem = port;

                            // Crear controles dinámicos con la configuración recibida
                            CreateDynamicControls(response);
                            break; // Dejar de buscar
                        }
                    }
                }
                catch (Exception)
                {
                    // Si no responde, ignoramos el puerto
                }

                labelMicro.Text = $"No PRISMA microcontroller found";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DetectMicrocontroller();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
        private void CreateDynamicControls(string config)
        {
            // Limpiar controles dinámicos anteriores
            foreach (var comboBox in modeComboBoxes)
            {
                this.Controls.Remove(comboBox);
            }
            modeComboBoxes.Clear();

            if (this.Controls.Contains(btnSend))
            {
                this.Controls.Remove(btnSend);
            }

            // Obtener la parte de los modos de la configuración (ejemplo: "MLLML")
            string[] configParts = config.Split(';');
            if (configParts.Length < 2)
            {
                MessageBox.Show("Configuración no válida.");
                return;
            }

            string modes = configParts[1]; // Segunda parte del string (modos)

            // Crear un ComboBox por cada botón
            int yOffset = 100; // Posición inicial en Y
            for (int i = 0; i < modes.Length; i++)
            {
                Label label = new Label
                {
                    Text = $"Botón {i}:",
                    Location = new System.Drawing.Point(20, yOffset),
                    AutoSize = true
                };
                this.Controls.Add(label);

                ComboBox comboBox = new ComboBox
                {
                    Location = new System.Drawing.Point(100, yOffset),
                    DropDownStyle = ComboBoxStyle.DropDownList
                };
                comboBox.Items.AddRange(new string[] { "Momentary (M)", "Latched (L)" });
                comboBox.SelectedIndex = modes[i] == 'M' ? 0 : 1; // Seleccionar el modo actual
                this.Controls.Add(comboBox);
                modeComboBoxes.Add(comboBox);

                yOffset += 30; // Aumentar la posición en Y para el siguiente control
            }

            // Crear el botón de enviar
            btnSend = new Button
            {
                Text = "Enviar Configuración",
                Location = new System.Drawing.Point(20, yOffset),
                AutoSize = true
            };
            btnSend.Click += new EventHandler(btnSend_Click); // Asignar evento
            this.Controls.Add(btnSend);


        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            // Construir el string de modos (ejemplo: "MLLML")
            string modes = "";
            foreach (var comboBox in modeComboBoxes)
            {
                modes += comboBox.SelectedIndex == 0 ? 'M' : 'L';
            }


            sendComandToMicrocontroller(modes);

            // Enviar el comando al microcontrolador
            //if (outPutPorts.IsOpen)
            //{
            //outPutPorts.WriteLine(modes);
            //MessageBox.Show("Configuración enviada correctamente.");
            //}
            //else
            //{
            //MessageBox.Show("El puerto no está abierto.");
            //}
        }
    }
}
