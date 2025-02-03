namespace Prueba
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            portsList = new ComboBox();
            textBox1 = new TextBox();
            txtOut = new TextBox();
            button1 = new Button();
            labelMicro = new Label();
            contextMenuStrip1 = new ContextMenuStrip(components);
            button2 = new Button();
            groupBox1 = new GroupBox();
            btnSend = new Button();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // portsList
            // 
            portsList.AllowDrop = true;
            portsList.FormattingEnabled = true;
            portsList.Location = new Point(598, 7);
            portsList.Name = "portsList";
            portsList.Size = new Size(121, 23);
            portsList.TabIndex = 0;
            portsList.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(268, 22);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(663, 378);
            textBox1.TabIndex = 1;
            // 
            // txtOut
            // 
            txtOut.Location = new Point(19, 22);
            txtOut.Name = "txtOut";
            txtOut.Size = new Size(208, 23);
            txtOut.TabIndex = 2;
            // 
            // button1
            // 
            button1.Location = new Point(19, 51);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 3;
            button1.Text = "Enviar";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // labelMicro
            // 
            labelMicro.AutoSize = true;
            labelMicro.Location = new Point(89, 15);
            labelMicro.Name = "labelMicro";
            labelMicro.Size = new Size(38, 15);
            labelMicro.TabIndex = 4;
            labelMicro.Text = "label1";
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(61, 4);
            // 
            // button2
            // 
            button2.Location = new Point(32, 12);
            button2.Name = "button2";
            button2.Size = new Size(51, 23);
            button2.TabIndex = 5;
            button2.Text = "Reload";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(txtOut);
            groupBox1.Controls.Add(button1);
            groupBox1.Controls.Add(textBox1);
            groupBox1.Location = new Point(68, 292);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(986, 406);
            groupBox1.TabIndex = 6;
            groupBox1.TabStop = false;
            groupBox1.Text = "Debug";
            groupBox1.Enter += groupBox1_Enter;
            // 
            // btnSend
            // 
            btnSend.Location = new Point(87, 254);
            btnSend.Name = "btnSend";
            btnSend.Size = new Size(75, 23);
            btnSend.TabIndex = 7;
            btnSend.Text = "Enviar";
            btnSend.UseVisualStyleBackColor = true;
            btnSend.Click += btnSend_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1222, 779);
            Controls.Add(btnSend);
            Controls.Add(groupBox1);
            Controls.Add(button2);
            Controls.Add(labelMicro);
            Controls.Add(portsList);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ComboBox portsList;
        private TextBox textBox1;
        private TextBox txtOut;
        private Button button1;
        private Label labelMicro;
        private ContextMenuStrip contextMenuStrip1;
        private Button button2;
        private GroupBox groupBox1;
        private Button btnSend;
    }
}
