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
            SuspendLayout();
            // 
            // portsList
            // 
            portsList.AllowDrop = true;
            portsList.FormattingEnabled = true;
            portsList.Location = new Point(87, 64);
            portsList.Name = "portsList";
            portsList.Size = new Size(121, 23);
            portsList.TabIndex = 0;
            portsList.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(474, 64);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(271, 307);
            textBox1.TabIndex = 1;
            // 
            // txtOut
            // 
            txtOut.Location = new Point(127, 191);
            txtOut.Name = "txtOut";
            txtOut.Size = new Size(208, 23);
            txtOut.TabIndex = 2;
            // 
            // button1
            // 
            button1.Location = new Point(133, 246);
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
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(labelMicro);
            Controls.Add(button1);
            Controls.Add(txtOut);
            Controls.Add(textBox1);
            Controls.Add(portsList);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
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
    }
}
